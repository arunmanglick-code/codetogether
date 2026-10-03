from langchain_core.messages import AIMessage, SystemMessage
from langsmith import traceable

from src.prompts.flight_prompt import FLIGHT_SEARCH_PROMPT
from src.state import TravelPlanState
from src.tools.flight_tools import search_flights, get_airport_code

TOOLS = [search_flights, get_airport_code]


@traceable(run_type="chain", name="flight_react_agent", tags=["react", "flight"])
def _run_react_agent(origin: str, destination: str, travelers: int) -> str:
    """Run the ReAct agent with flight tools. Falls back to mock on failure."""
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
            f"Find flight options from {origin} to {destination} for {travelers} traveler(s).\n\n"
            f"Use get_airport_code to resolve city names to IATA codes, then use search_flights "
            f"to find available flights. Present the top 3 options ranked by best value "
            f"(balancing price, duration, and airline quality)."
        )

        result = react_graph.invoke({
            "messages": [
                SystemMessage(content=FLIGHT_SEARCH_PROMPT),
                {"role": "user", "content": user_msg},
            ]
        })

        return result["messages"][-1].content

    except Exception as e:
        return f"FALLBACK_TO_MOCK: {e}"


def _generate_mock_flights(origin: str, destination: str, travelers: int) -> dict:
    """Generate mock flight data using the dynamic tool directly."""
    import json
    raw = search_flights.invoke({"origin": origin, "destination": destination, "travelers": travelers})
    data = json.loads(raw)
    flights = data["flights"]
    return {
        "options": flights,
        "recommended": flights[0]["airline"] if flights else "N/A",
        "cheapest": min(flights, key=lambda f: f["total_price"])["airline"] if flights else "N/A",
        "source": "mock_fallback",
    }


@traceable(run_type="chain", name="flight_search_agent", tags=["agent", "flight"], metadata={"phase": "planning"})
def flight_search_agent(state: TravelPlanState) -> dict:
    """Flight Search Agent - uses LLM with tools, falls back to dynamic mock data."""
    origin = state["travel_request"]["origin"]
    destination = state["travel_request"]["destination"]
    travelers = state["travel_request"]["travelers"]

    agent_response = _run_react_agent(origin, destination, travelers)

    if agent_response.startswith("FALLBACK_TO_MOCK"):
        flight_data = _generate_mock_flights(origin, destination, travelers)
        options = flight_data["options"]
        cheapest = min(options, key=lambda f: f["total_price"]) if options else {}
        return {
            "flight_options": flight_data,
            "current_agent": "flight_search",
            "plan_status": "planning",
            "messages": [
                AIMessage(
                    content=f"[Mock fallback] Found {len(options)} flight options. "
                    f"Recommended: {options[0]['airline']} at ${options[0]['total_price']} total. "
                    f"Cheapest: {cheapest.get('airline', 'N/A')} at ${cheapest.get('total_price', 0)} total."
                    if options else "[Mock fallback] No flights found."
                )
            ],
        }

    import json
    try:
        raw = search_flights.invoke({"origin": origin, "destination": destination, "travelers": travelers})
        structured = json.loads(raw)
        flights = structured["flights"]
    except Exception:
        flights = []

    flight_data = {
        "options": flights,
        "recommended": flights[0]["airline"] if flights else "N/A",
        "cheapest": min(flights, key=lambda f: f["total_price"])["airline"] if flights else "N/A",
        "llm_analysis": agent_response,
        "source": "llm_react",
    }

    return {
        "flight_options": flight_data,
        "current_agent": "flight_search",
        "plan_status": "planning",
        "messages": [
            AIMessage(content=f"Flight search complete for {origin} to {destination}.\n\n{agent_response}")
        ],
    }
