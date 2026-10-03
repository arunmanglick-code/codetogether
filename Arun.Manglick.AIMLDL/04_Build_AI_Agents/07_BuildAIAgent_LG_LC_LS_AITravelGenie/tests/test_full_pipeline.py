"""Full LLM-powered pipeline tests — runs the complete graph with Claude."""
import json
import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))


def safe(s):
    return s.encode("ascii", "replace").decode("ascii") if isinstance(s, str) else str(s)


def flush(*args, **kwargs):
    print(*args, **kwargs, flush=True)


pass_count = 0
fail_count = 0


def check(name, condition):
    global pass_count, fail_count
    if condition:
        pass_count += 1
        flush(f"  [PASS] {name}")
    else:
        fail_count += 1
        flush(f"  [FAIL] {name}")


from langchain_core.messages import HumanMessage
from langgraph.graph import StateGraph, START, END
from src.state import TravelPlanState
from src.supervisor import parse_intent, supervisor_router, aggregate_results
from src.agents.destination_agent import destination_research_agent
from src.agents.flight_agent import flight_search_agent
from src.agents.hotel_agent import hotel_search_agent
from src.agents.activity_agent import activity_itinerary_agent
from src.agents.budget_agent import budget_currency_agent

builder = StateGraph(TravelPlanState)
builder.add_node("intent_parser", parse_intent)
builder.add_node("destination_research", destination_research_agent)
builder.add_node("flight_search", flight_search_agent)
builder.add_node("hotel_search", hotel_search_agent)
builder.add_node("activity_itinerary", activity_itinerary_agent)
builder.add_node("budget_currency", budget_currency_agent)
builder.add_node("aggregator", aggregate_results)
builder.add_edge(START, "intent_parser")
builder.add_conditional_edges("intent_parser", supervisor_router)
builder.add_conditional_edges("destination_research", supervisor_router)
builder.add_conditional_edges("flight_search", supervisor_router)
builder.add_conditional_edges("hotel_search", supervisor_router)
builder.add_conditional_edges("activity_itinerary", supervisor_router)
builder.add_conditional_edges("budget_currency", supervisor_router)
builder.add_edge("aggregator", END)
graph = builder.compile()

# ========== TEST SCENARIOS ==========
scenarios = [
    {
        "name": "Bali Beach Vacation",
        "prompt": "Plan a 5-day beach vacation in Bali for 2 people, budget $3000, departing from New York. Dates: 2025-03-15 to 2025-03-20.",
        "expected_dest": "Bali",
        "expected_days": 5,
        "budget": 3000,
        "travelers": 2,
    },
    {
        "name": "Santorini Romantic Getaway",
        "prompt": "Plan a 7-day romantic getaway to Santorini for 2 people, budget $5000, departing from San Francisco. Dates: 2025-06-10 to 2025-06-17. Preferences: romantic, food, culture.",
        "expected_dest": "Santorini",
        "expected_days": 7,
        "budget": 5000,
        "travelers": 2,
    },
]

for scenario in scenarios:
    flush("=" * 70)
    flush(f"  PIPELINE TEST: {scenario['name']}")
    flush("=" * 70)
    flush()

    # Single invoke — captures final state and is faster than stream + invoke
    flush("  Running graph.invoke()...")
    final = graph.invoke({"messages": [HumanMessage(content=scenario["prompt"])]})
    flush("  Graph completed.")
    flush()

    # 1. Intent parsing
    req = final.get("travel_request", {})
    check(f"Intent parsed destination: {req.get('destination', 'N/A')}",
          req.get("destination", "").lower() in scenario["expected_dest"].lower() or
          scenario["expected_dest"].lower() in req.get("destination", "").lower())

    # 2. Destination research
    dest = final.get("destination_research", {})
    check("Destination research populated", dest is not None and len(dest) > 0)
    source = "LLM" if dest.get("llm_research") else "Mock"
    flush(f"    (source: {source})")

    # 3. Flight options
    flights = final.get("flight_options", {})
    options = flights.get("options", [])
    check(f"Flight options: {len(options)} results", len(options) == 3)
    check("Flight source", flights.get("source") in ("llm_react", "mock_fallback"))
    if options:
        ranked = all(
            (options[i]["rating"] > options[i+1]["rating"]) or
            (options[i]["rating"] == options[i+1]["rating"] and
             options[i]["price_per_person"] <= options[i+1]["price_per_person"])
            for i in range(len(options)-1)
        )
        check("Flight ranking correct", ranked)
        totals_ok = all(
            f["total_price"] == f["price_per_person"] * scenario["travelers"]
            for f in options
        )
        check("Flight total prices correct", totals_ok)

    # 4. Hotel options
    hotels = final.get("hotel_options", {})
    h_options = hotels.get("options", [])
    check(f"Hotel options: {len(h_options)} results", len(h_options) == 3)
    if h_options:
        ranked_h = all(
            (h_options[i]["guest_rating"] > h_options[i+1]["guest_rating"]) or
            (h_options[i]["guest_rating"] == h_options[i+1]["guest_rating"] and
             h_options[i]["price_per_night"] <= h_options[i+1]["price_per_night"])
            for i in range(len(h_options)-1)
        )
        check("Hotel ranking correct", ranked_h)
        nights = hotels.get("nights", 0)
        totals_ok_h = all(h["total_price"] == h["price_per_night"] * nights for h in h_options)
        check(f"Hotel totals correct ({nights} nights)", totals_ok_h)

    # 5. Itinerary
    itin = final.get("itinerary", {})
    days = itin.get("days", [])
    check(f"Itinerary: {len(days)} days", len(days) == scenario["expected_days"])
    if days:
        all_have_slots = all(
            "morning" in d and "afternoon" in d and "evening" in d and "weather" in d
            for d in days
        )
        check("All days have AM/PM/EVE slots + weather", all_have_slots)
        computed_act = sum(d["morning"]["cost"] + d["afternoon"]["cost"] + d["evening"]["cost"] for d in days)
        reported_act = itin.get("total_activity_cost", -1)
        check(f"Activity cost matches: computed={computed_act}, reported={reported_act}", computed_act == reported_act)

    # 6. Budget
    budget = final.get("budget_summary", {})
    check("Budget summary populated", budget is not None and "total_usd" in budget)
    if budget:
        components = ["flights", "accommodation", "activities", "food_and_dining", "local_transport", "miscellaneous"]
        comp_sum = sum(budget.get(k, 0) for k in components)
        check(f"Budget math: sum={comp_sum}, total={budget.get('total_usd', 0)}", abs(comp_sum - budget.get("total_usd", 0)) < 1)

        total = budget.get("total_usd", 0)
        bud = budget.get("budget_usd", 0)
        if total <= bud * 0.95:
            exp = "WITHIN BUDGET"
        elif total <= bud * 1.05:
            exp = "ON TARGET"
        else:
            exp = "OVER BUDGET"
        check(f"Budget status: {budget.get('budget_status')} (expected {exp})", budget.get("budget_status") == exp)

        conv = budget.get("currency_conversion", {})
        if conv and conv.get("exchange_rate", 0) > 0:
            expected_local = round(total * conv["exchange_rate"], 2)
            check(f"Currency conversion correct ({conv.get('currency_code', '')})",
                  abs(expected_local - conv.get("total_local_currency", 0)) < 1)

        if budget.get("budget_status") == "OVER BUDGET":
            check("Savings tips provided", len(budget.get("savings_tips", [])) > 0)

    # 7. Final plan
    check("Plan status: complete", final.get("plan_status") == "complete")
    last_msg = final["messages"][-1].content
    check("Final aggregated plan generated", len(last_msg) > 200)
    flush(f"    (plan length: {len(last_msg)} chars)")

    flush()

# ========== SUMMARY ==========
flush("=" * 70)
flush(f"  FULL PIPELINE TESTS: {pass_count} PASS, {fail_count} FAIL")
flush("=" * 70)

if fail_count > 0:
    sys.exit(1)
