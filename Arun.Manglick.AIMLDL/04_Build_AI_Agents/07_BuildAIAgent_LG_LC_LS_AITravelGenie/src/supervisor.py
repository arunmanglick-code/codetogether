import json
from typing import Literal

from langchain_core.messages import AIMessage, HumanMessage
from langsmith import traceable
from langgraph.types import interrupt

from src.prompts.supervisor_prompt import (
    AGGREGATOR_PROMPT,
    INTENT_PARSER_PROMPT,
    MODIFICATION_PARSER_PROMPT,
)
from src.state import TravelPlanState


# ---------------------------------------------------------------------------
# Cascade rules: which agent output fields to null when a request field changes
# ---------------------------------------------------------------------------

CASCADE_RULES: dict[str, list[str]] = {
    "destination": [
        "destination_research",
        "flight_options",
        "hotel_options",
        "itinerary",
        "budget_summary",
    ],
    "origin": ["flight_options", "hotel_options", "budget_summary"],
    "start_date": ["flight_options", "hotel_options", "itinerary", "budget_summary"],
    "end_date": ["flight_options", "hotel_options", "itinerary", "budget_summary"],
    "budget_usd": ["hotel_options", "budget_summary"],
    "travelers": ["flight_options", "hotel_options", "budget_summary"],
    "preferences": ["hotel_options", "itinerary", "budget_summary"],
    "special_requirements": ["itinerary", "budget_summary"],
}

ALL_AGENT_FIELDS = [
    "destination_research",
    "flight_options",
    "hotel_options",
    "itinerary",
    "budget_summary",
]


# ---------------------------------------------------------------------------
# Helper functions for modification detection
# ---------------------------------------------------------------------------


def _diff_requests(old: dict, new: dict) -> set[str]:
    """Compare two TravelRequest dicts and return which fields changed."""
    changed = set()
    for key in old:
        old_val = old.get(key)
        new_val = new.get(key)
        if isinstance(old_val, list) and isinstance(new_val, list):
            if set(old_val) != set(new_val):
                changed.add(key)
        elif old_val != new_val:
            changed.add(key)
    return changed


def _compute_cascade(changed_fields: set[str]) -> list[str]:
    """Given a set of changed TravelRequest fields, return agent output fields to invalidate."""
    invalidated = set()
    for field in changed_fields:
        invalidated.update(CASCADE_RULES.get(field, []))
    return sorted(invalidated, key=lambda f: ALL_AGENT_FIELDS.index(f))


def _describe_changes(changed_fields: set[str]) -> str:
    """Produce a human-readable summary of what changed."""
    descriptions = {
        "destination": "destination",
        "origin": "departure city",
        "start_date": "start date",
        "end_date": "end date",
        "budget_usd": "budget",
        "travelers": "number of travelers",
        "preferences": "preferences",
        "special_requirements": "special requirements",
    }
    parts = [descriptions.get(f, f) for f in sorted(changed_fields)]
    if len(parts) == 1:
        return f"Updating {parts[0]}."
    return f"Updating {', '.join(parts[:-1])} and {parts[-1]}."


@traceable(run_type="chain", name="parse_modification", tags=["supervisor", "modification"])
def _parse_modification(existing_request: dict, user_text: str) -> dict:
    """Use LLM to merge user's modification into existing TravelRequest."""
    try:
        from src.utils.config import get_llm

        llm = get_llm()
        prompt = MODIFICATION_PARSER_PROMPT.format(
            existing_request=json.dumps(existing_request, indent=2),
            user_message=user_text,
        )
        response = llm.invoke(
            [
                {"role": "system", "content": prompt},
                {"role": "user", "content": user_text},
            ]
        )
        content = response.content.strip()
        if content.startswith("```"):
            content = content.split("```")[1]
            if content.startswith("json"):
                content = content[4:]
        parsed = json.loads(content)

        return {
            "destination": parsed.get("destination", existing_request["destination"]),
            "origin": parsed.get("origin", existing_request["origin"]),
            "start_date": parsed.get("start_date", existing_request["start_date"]),
            "end_date": parsed.get("end_date", existing_request["end_date"]),
            "budget_usd": float(
                parsed.get("budget_usd", existing_request["budget_usd"])
            ),
            "travelers": int(
                parsed.get("travelers", existing_request["travelers"])
            ),
            "preferences": parsed.get(
                "preferences", existing_request["preferences"]
            ),
            "special_requirements": parsed.get(
                "special_requirements", existing_request["special_requirements"]
            ),
        }
    except Exception:
        return _keyword_fallback_modification(existing_request, user_text)


def _keyword_fallback_modification(existing_request: dict, user_text: str) -> dict:
    """Keyword-based fallback when LLM modification parsing fails."""
    updated = dict(existing_request)
    text_lower = user_text.lower()

    if "cheaper" in text_lower or "less expensive" in text_lower:
        updated["budget_usd"] = existing_request["budget_usd"] * 0.7
    if "more expensive" in text_lower or "luxur" in text_lower:
        updated["budget_usd"] = existing_request["budget_usd"] * 1.5

    import re

    date_match = re.findall(r"\d{4}-\d{2}-\d{2}", user_text)
    if len(date_match) >= 2:
        updated["start_date"] = date_match[0]
        updated["end_date"] = date_match[1]
    elif len(date_match) == 1:
        updated["start_date"] = date_match[0]

    known_destinations = [
        "Bali",
        "Paris",
        "Tokyo",
        "Maldives",
        "Santorini",
        "Cancun",
        "Dubai",
        "London",
        "Rome",
        "Sydney",
        "Bangkok",
        "Phuket",
        "Barcelona",
        "Hawaii",
        "Reykjavik",
    ]
    for dest in known_destinations:
        if dest.lower() in text_lower:
            updated["destination"] = dest
            break

    return updated


# ---------------------------------------------------------------------------
# Graph nodes
# ---------------------------------------------------------------------------


@traceable(run_type="chain", name="parse_intent", tags=["supervisor", "intent"], metadata={"phase": "parsing"})
def parse_intent(state: TravelPlanState) -> dict:
    """Extract structured TravelRequest from the user's message.

    On first turn: parse from scratch.
    On modification turn: merge changes into existing request and selectively
    invalidate agent outputs via cascade rules.
    """
    last_message = state["messages"][-1]
    user_text = (
        last_message.content if hasattr(last_message, "content") else str(last_message)
    )

    existing_request = state.get("travel_request")
    is_modification = state.get("plan_status") in ("complete", "reviewing", "revising")

    if is_modification and existing_request:
        # --- MODIFICATION PATH ---
        previous_request = dict(existing_request)
        new_request = _parse_modification(existing_request, user_text)
        changed_fields = _diff_requests(previous_request, new_request)

        if not changed_fields:
            changed_fields = {"preferences"}

        invalidated = _compute_cascade(changed_fields)
        change_desc = _describe_changes(changed_fields)

        result = {
            "travel_request": new_request,
            "previous_travel_request": previous_request,
            "invalidated_agents": invalidated,
            "plan_status": "revising",
            "iteration_count": state.get("iteration_count", 0) + 1,
            "messages": [
                AIMessage(
                    content=f"Modifying your plan: {change_desc} "
                    f"Re-running {len(invalidated)} agent(s): "
                    f"{', '.join(invalidated)}."
                )
            ],
        }
        for field in invalidated:
            result[field] = None
        return result

    # --- NEW REQUEST PATH ---
    try:
        from src.utils.config import get_llm

        llm = get_llm()
        response = llm.invoke(
            [
                {"role": "system", "content": INTENT_PARSER_PROMPT},
                {"role": "user", "content": user_text},
            ]
        )
        content = response.content.strip()
        if content.startswith("```"):
            content = content.split("```")[1]
            if content.startswith("json"):
                content = content[4:]
        parsed = json.loads(content)
    except Exception:
        parsed = {
            "destination": "Bali",
            "origin": "New York",
            "start_date": "2025-03-15",
            "end_date": "2025-03-20",
            "budget_usd": 3000,
            "travelers": 2,
            "preferences": ["beach", "culture"],
            "special_requirements": "none",
        }

    travel_request = {
        "destination": parsed.get("destination", "undecided"),
        "origin": parsed.get("origin", "unknown"),
        "start_date": parsed.get("start_date", "flexible"),
        "end_date": parsed.get("end_date", "flexible"),
        "budget_usd": float(parsed.get("budget_usd", 0)),
        "travelers": int(parsed.get("travelers", 1)),
        "preferences": parsed.get("preferences", []),
        "special_requirements": parsed.get("special_requirements", "none"),
    }

    result = {
        "travel_request": travel_request,
        "previous_travel_request": None,
        "invalidated_agents": ALL_AGENT_FIELDS[:],
        "plan_status": "gathering_info",
        "iteration_count": 0,
        "messages": [
            AIMessage(
                content=f"Understood! Planning a trip to {travel_request['destination']} "
                f"for {travel_request['travelers']} traveler(s), "
                f"departing from {travel_request['origin']}. "
                f"Dates: {travel_request['start_date']} to {travel_request['end_date']}. "
                f"Budget: ${travel_request['budget_usd']}."
            )
        ],
    }
    for field in ALL_AGENT_FIELDS:
        result[field] = None
    return result


def supervisor_router(
    state: TravelPlanState,
) -> Literal[
    "destination_research",
    "flight_search",
    "hotel_search",
    "activity_itinerary",
    "budget_currency",
    "aggregator",
]:
    """Route to the next agent based on which plan sections are still missing."""
    if state.get("destination_research") is None:
        return "destination_research"
    if state.get("flight_options") is None:
        return "flight_search"
    if state.get("hotel_options") is None:
        return "hotel_search"
    if state.get("itinerary") is None:
        return "activity_itinerary"
    if state.get("budget_summary") is None:
        return "budget_currency"
    return "aggregator"


@traceable(run_type="chain", name="aggregate_results", tags=["supervisor", "aggregator"], metadata={"phase": "aggregation"})
def aggregate_results(state: TravelPlanState) -> dict:
    """Combine all agent outputs into a final travel plan summary."""
    req = state["travel_request"]
    dest = state.get("destination_research", {})
    flights = state.get("flight_options", {})
    hotels = state.get("hotel_options", {})
    itinerary = state.get("itinerary", {})
    budget = state.get("budget_summary", {})

    iteration = state.get("iteration_count", 0)
    revision_note = ""
    if iteration > 0:
        revision_note = f"\n  (Revision #{iteration})\n"

    plan_lines = [
        f"{'='*60}",
        f"  AI TRAVEL GENIE - YOUR TRAVEL PLAN",
        f"{'='*60}",
        revision_note,
        f"DESTINATION: {dest.get('name', req['destination'])}",
        f"DATES: {req['start_date']} to {req['end_date']}",
        f"TRAVELERS: {req['travelers']}",
        f"BUDGET: ${req['budget_usd']}",
        "",
        f"--- DESTINATION OVERVIEW ---",
        f"{dest.get('overview', 'N/A')}",
        f"Best time to visit: {dest.get('best_time_to_visit', 'N/A')}",
        f"Visa: {dest.get('visa_requirements', 'N/A')}",
        f"Suitability score: {dest.get('suitability_score', 'N/A')}/10",
        "",
        f"--- TOP FLIGHT OPTIONS ---",
    ]

    for i, flight in enumerate(flights.get("options", []), 1):
        plan_lines.append(
            f"  {i}. {flight['airline']} ({flight['route']}) - "
            f"${flight.get('total_price', flight['price_per_person'])} total | "
            f"{flight['duration']} | {flight['layovers']} stop(s)"
        )

    plan_lines.append("")
    plan_lines.append("--- HOTEL OPTIONS ---")
    for i, hotel in enumerate(hotels.get("options", []), 1):
        plan_lines.append(
            f"  {i}. {hotel['name']} ({hotel['star_rating']}*) - "
            f"${hotel.get('total_price', hotel['price_per_night'])}/stay | "
            f"Rating: {hotel['guest_rating']}/10 | {hotel['location']}"
        )

    plan_lines.append("")
    plan_lines.append("--- DAY-BY-DAY ITINERARY ---")
    for day in itinerary.get("days", []):
        plan_lines.append(f"  Day {day['day']}: {day['theme']} ({day['weather']})")
        plan_lines.append(
            f"    AM: {day['morning']['activity']} (${day['morning']['cost']})"
        )
        plan_lines.append(
            f"    PM: {day['afternoon']['activity']} (${day['afternoon']['cost']})"
        )
        plan_lines.append(
            f"    EVE: {day['evening']['activity']} (${day['evening']['cost']})"
        )

    plan_lines.append("")
    plan_lines.append("--- BUDGET SUMMARY ---")
    plan_lines.append(f"  Flights:        ${budget.get('flights', 0):>8}")
    plan_lines.append(f"  Accommodation:  ${budget.get('accommodation', 0):>8}")
    plan_lines.append(f"  Activities:     ${budget.get('activities', 0):>8}")
    plan_lines.append(f"  Food & Dining:  ${budget.get('food_and_dining', 0):>8}")
    plan_lines.append(f"  Transport:      ${budget.get('local_transport', 0):>8}")
    plan_lines.append(f"  Miscellaneous:  ${budget.get('miscellaneous', 0):>8}")
    plan_lines.append(f"  {'-'*30}")
    plan_lines.append(f"  TOTAL:          ${budget.get('total_usd', 0):>8}")
    plan_lines.append(f"  BUDGET:         ${budget.get('budget_usd', 0):>8}")
    plan_lines.append(f"  STATUS:         {budget.get('budget_status', 'N/A')}")

    conv = budget.get("currency_conversion", {})
    if conv:
        plan_lines.append(
            f"  Local currency: {conv.get('total_local_currency', 0):,.0f} "
            f"{conv.get('currency_code', '')}"
        )

    plan_lines.append("")
    plan_lines.append(f"{'='*60}")

    plan_text = "\n".join(plan_lines)

    return {
        "plan_status": "reviewing",
        "messages": [AIMessage(content=plan_text)],
    }


@traceable(run_type="chain", name="human_review", tags=["supervisor", "review"], metadata={"phase": "review"})
def human_review(state: TravelPlanState) -> dict:
    """Pause for human review. The user can approve the plan or request modifications."""
    plan_message = state["messages"][-1].content

    feedback = interrupt(
        {
            "plan": plan_message,
            "prompt": (
                "Review the travel plan above. "
                "Reply 'approve' to finalize, or describe what changes you'd like."
            ),
        }
    )

    feedback_str = str(feedback).strip()
    feedback_lower = feedback_str.lower()

    if feedback_lower in (
        "approve",
        "approved",
        "looks good",
        "yes",
        "ok",
        "accept",
        "lgtm",
        "done",
    ):
        return {
            "human_feedback": feedback_str,
            "plan_status": "complete",
            "messages": [
                AIMessage(
                    content="Travel plan approved! Have a wonderful trip!"
                )
            ],
        }

    return {
        "human_feedback": feedback_str,
        "plan_status": "revising",
        "messages": [HumanMessage(content=feedback_str)],
    }


def after_human_review(
    state: TravelPlanState,
) -> Literal["__end__", "intent_parser"]:
    """Route after human review: end if approved, back to intent_parser if revising."""
    if state.get("plan_status") == "complete":
        return "__end__"
    return "intent_parser"
