import json

from langchain_core.messages import AIMessage, SystemMessage
from langsmith import traceable

from src.prompts.budget_prompt import BUDGET_PROMPT
from src.state import TravelPlanState
from src.tools.budget_tools import convert_currency, calculate_budget

TOOLS = [convert_currency, calculate_budget]


@traceable(run_type="chain", name="budget_react_agent", tags=["react", "budget"])
def _run_react_agent(
    destination: str, budget_usd: float, travelers: int, num_days: int,
    flight_cost: float, hotel_cost: float, activity_cost: float,
) -> str:
    """Run the ReAct agent with budget tools. Falls back to mock on failure."""
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

        user_msg = (
            f"Create a budget analysis for a trip to {destination}.\n"
            f"Budget: ${budget_usd} USD for {travelers} traveler(s), {num_days} days.\n"
            f"Known costs: Flights ${flight_cost}, Hotels ${hotel_cost}, Activities ${activity_cost}.\n\n"
            f"Use calculate_budget to get a full breakdown including food, transport, and misc estimates. "
            f"Then use convert_currency to show the total in the local currency. "
            f"Provide a clear summary with the budget status and savings tips if over budget."
        )

        result = react_graph.invoke({
            "messages": [
                SystemMessage(content=BUDGET_PROMPT),
                {"role": "user", "content": user_msg},
            ]
        })

        return result["messages"][-1].content

    except Exception as e:
        return f"FALLBACK_TO_MOCK: {e}"


def _generate_mock_budget(
    destination: str, budget_usd: float, travelers: int, num_days: int,
    flight_cost: float, hotel_cost: float, activity_cost: float,
) -> dict:
    """Generate a mock budget using tools directly."""
    budget_raw = calculate_budget.invoke({
        "budget_usd": budget_usd,
        "travelers": travelers,
        "num_days": num_days,
        "flight_cost": flight_cost,
        "hotel_cost": hotel_cost,
        "activity_cost": activity_cost,
        "destination": destination,
    })
    return json.loads(budget_raw)


@traceable(run_type="chain", name="budget_currency_agent", tags=["agent", "budget"], metadata={"phase": "planning"})
def budget_currency_agent(state: TravelPlanState) -> dict:
    """Budget/Currency Agent - uses LLM with tools, falls back to dynamic mock data."""
    destination = state["travel_request"]["destination"]
    budget_usd = state["travel_request"]["budget_usd"]
    travelers = state["travel_request"]["travelers"]

    itinerary = state.get("itinerary") or {}
    num_days = itinerary.get("total_days", 5)

    flights = state.get("flight_options") or {}
    flight_cost = 0.0
    if flights.get("options"):
        cheapest = min(flights["options"], key=lambda f: f.get("total_price", 0))
        flight_cost = float(cheapest["total_price"])

    hotels = state.get("hotel_options") or {}
    hotel_cost = 0.0
    if hotels.get("options"):
        hotel_cost = float(hotels["options"][0].get("total_price", 0))

    activity_cost = float(itinerary.get("total_activity_cost", 0)) * travelers

    agent_response = _run_react_agent(
        destination, budget_usd, travelers, num_days,
        flight_cost, hotel_cost, activity_cost,
    )

    if agent_response.startswith("FALLBACK_TO_MOCK"):
        budget_data = _generate_mock_budget(
            destination, budget_usd, travelers, num_days,
            flight_cost, hotel_cost, activity_cost,
        )
        budget_summary = {
            "flights": budget_data["breakdown"]["flights"],
            "accommodation": budget_data["breakdown"]["accommodation"],
            "activities": budget_data["breakdown"]["activities"],
            "food_and_dining": budget_data["breakdown"]["food_and_dining"],
            "local_transport": budget_data["breakdown"]["local_transport"],
            "miscellaneous": budget_data["breakdown"]["miscellaneous"],
            "total_usd": budget_data["total_usd"],
            "budget_usd": budget_data["budget_usd"],
            "budget_status": budget_data["budget_status"],
            "remaining_or_over": budget_data["remaining_or_over"],
            "currency_conversion": budget_data["currency_conversion"],
            "per_person_cost": budget_data["per_person_cost"],
            "daily_spending_allowance": budget_data["daily_spending"],
            "savings_tips": budget_data["savings_tips"],
            "source": "mock_fallback",
        }

        return {
            "budget_summary": budget_summary,
            "current_agent": "budget_currency",
            "plan_status": "reviewing",
            "messages": [
                AIMessage(
                    content=f"[Mock fallback] Budget analysis: ${budget_data['total_usd']} total | "
                    f"Budget: ${budget_usd} | Status: {budget_data['budget_status']}. "
                    f"Per person: ${budget_data['per_person_cost']}."
                )
            ],
        }

    budget_data = _generate_mock_budget(
        destination, budget_usd, travelers, num_days,
        flight_cost, hotel_cost, activity_cost,
    )

    budget_summary = {
        "flights": budget_data["breakdown"]["flights"],
        "accommodation": budget_data["breakdown"]["accommodation"],
        "activities": budget_data["breakdown"]["activities"],
        "food_and_dining": budget_data["breakdown"]["food_and_dining"],
        "local_transport": budget_data["breakdown"]["local_transport"],
        "miscellaneous": budget_data["breakdown"]["miscellaneous"],
        "total_usd": budget_data["total_usd"],
        "budget_usd": budget_data["budget_usd"],
        "budget_status": budget_data["budget_status"],
        "remaining_or_over": budget_data["remaining_or_over"],
        "currency_conversion": budget_data["currency_conversion"],
        "per_person_cost": budget_data["per_person_cost"],
        "daily_spending_allowance": budget_data["daily_spending"],
        "savings_tips": budget_data["savings_tips"],
        "llm_analysis": agent_response,
        "source": "llm_react",
    }

    return {
        "budget_summary": budget_summary,
        "current_agent": "budget_currency",
        "plan_status": "reviewing",
        "messages": [
            AIMessage(content=f"Budget analysis complete for {destination}.\n\n{agent_response}")
        ],
    }
