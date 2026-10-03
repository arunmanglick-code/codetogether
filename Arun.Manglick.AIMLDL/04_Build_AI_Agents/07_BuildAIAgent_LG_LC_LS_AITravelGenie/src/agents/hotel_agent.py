from langchain_core.messages import AIMessage, SystemMessage
from langsmith import traceable

from src.prompts.hotel_prompt import HOTEL_SEARCH_PROMPT
from src.state import TravelPlanState
from src.tools.hotel_tools import search_hotels, get_hotel_reviews

TOOLS = [search_hotels, get_hotel_reviews]


@traceable(run_type="chain", name="hotel_react_agent", tags=["react", "hotel"])
def _run_react_agent(destination: str, nights: int, budget_per_night: float) -> str:
    """Run the ReAct agent with hotel tools. Falls back to mock on failure."""
    try:
        from typing import Annotated, TypedDict

        from langchain_core.messages import BaseMessage
        from langgraph.graph import END, START, StateGraph
        from langgraph.graph.message import add_messages
        from langgraph.prebuilt import ToolNode

        from src.utils.config import get_llm

        llm = get_llm()
        llm_with_tools = llm.bind_tools(TOOLS)

        class AgentState(TypedDict):
            messages: Annotated[list[BaseMessage], add_messages]

        def agent_node(state: AgentState) -> dict:
            response = llm_with_tools.invoke(state["messages"])
            return {"messages": [response]}

        def should_continue(state: AgentState) -> str:
            last = state["messages"][-1]
            if hasattr(last, "tool_calls") and last.tool_calls:
                return "tools"
            return "end"

        tool_node = ToolNode(TOOLS)

        builder = StateGraph(AgentState)
        builder.add_node("agent", agent_node)
        builder.add_node("tools", tool_node)
        builder.add_edge(START, "agent")
        builder.add_conditional_edges("agent", should_continue, {"tools": "tools", "end": END})
        builder.add_edge("tools", "agent")

        react_graph = builder.compile()

        budget_note = f" Budget is approximately ${budget_per_night:.0f}/night." if budget_per_night > 0 else ""
        user_msg = (
            f"Find hotel options in {destination} for {nights} night(s).{budget_note}\n\n"
            f"Use search_hotels to find available hotels, then use get_hotel_reviews "
            f"to check reviews for the top options. Present the top 3 hotels ranked by "
            f"best overall value (balancing price, rating, amenities, and location)."
        )

        result = react_graph.invoke({
            "messages": [
                SystemMessage(content=HOTEL_SEARCH_PROMPT),
                {"role": "user", "content": user_msg},
            ]
        })

        return result["messages"][-1].content

    except Exception as e:
        return f"FALLBACK_TO_MOCK: {e}"


def _generate_mock_hotels(destination: str, nights: int, budget_per_night: float) -> dict:
    """Generate mock hotel data using the dynamic tool directly."""
    import json
    raw = search_hotels.invoke({
        "destination": destination,
        "nights": nights,
        "budget_per_night": budget_per_night,
    })
    data = json.loads(raw)
    hotels = data["hotels"]
    return {
        "options": hotels,
        "nights": nights,
        "recommended": hotels[0]["name"] if hotels else "N/A",
        "source": "mock_fallback",
    }


@traceable(run_type="chain", name="hotel_search_agent", tags=["agent", "hotel"], metadata={"phase": "planning"})
def hotel_search_agent(state: TravelPlanState) -> dict:
    """Hotel Search Agent - uses LLM with tools, falls back to dynamic mock data."""
    destination = state["travel_request"]["destination"]
    budget_usd = state["travel_request"]["budget_usd"]
    travelers = state["travel_request"]["travelers"]

    start = state["travel_request"]["start_date"]
    end = state["travel_request"]["end_date"]
    try:
        from datetime import datetime
        nights = (datetime.strptime(end, "%Y-%m-%d") - datetime.strptime(start, "%Y-%m-%d")).days
    except (ValueError, TypeError):
        nights = 5

    flight_cost = 0
    flight_opts = state.get("flight_options", {})
    if flight_opts and flight_opts.get("options"):
        cheapest_flight = min(flight_opts["options"], key=lambda f: f.get("total_price", 0))
        flight_cost = cheapest_flight.get("total_price", 0)

    remaining_budget = max(0, budget_usd - flight_cost)
    budget_per_night = remaining_budget / max(nights, 1) / max(travelers, 1) * 0.5

    agent_response = _run_react_agent(destination, nights, budget_per_night)

    if agent_response.startswith("FALLBACK_TO_MOCK"):
        hotel_data = _generate_mock_hotels(destination, nights, budget_per_night)
        options = hotel_data["options"]
        return {
            "hotel_options": hotel_data,
            "current_agent": "hotel_search",
            "plan_status": "planning",
            "messages": [
                AIMessage(
                    content=f"[Mock fallback] Found {len(options)} hotel options for {nights} nights. "
                    f"Recommended: {options[0]['name']} ({options[0]['star_rating']}*) "
                    f"at ${options[0]['total_price']} total (${options[0]['price_per_night']}/night)."
                    if options else "[Mock fallback] No hotels found."
                )
            ],
        }

    import json
    try:
        raw = search_hotels.invoke({
            "destination": destination,
            "nights": nights,
            "budget_per_night": budget_per_night,
        })
        structured = json.loads(raw)
        hotels = structured["hotels"]
    except Exception:
        hotels = []

    hotel_data = {
        "options": hotels,
        "nights": nights,
        "recommended": hotels[0]["name"] if hotels else "N/A",
        "llm_analysis": agent_response,
        "source": "llm_react",
    }

    return {
        "hotel_options": hotel_data,
        "current_agent": "hotel_search",
        "plan_status": "planning",
        "messages": [
            AIMessage(content=f"Hotel search complete for {destination} ({nights} nights).\n\n{agent_response}")
        ],
    }
