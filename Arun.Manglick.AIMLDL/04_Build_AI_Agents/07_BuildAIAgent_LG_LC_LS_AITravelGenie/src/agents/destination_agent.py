from langchain_core.messages import AIMessage, SystemMessage
from langsmith import traceable

from src.prompts.destination_prompt import DESTINATION_RESEARCH_PROMPT
from src.state import TravelPlanState
from src.tools.search_tools import get_destination_info, web_search

TOOLS = [web_search, get_destination_info]

MOCK_DESTINATIONS = {
    "bali": {
        "name": "Bali, Indonesia",
        "overview": "Tropical paradise known for stunning beaches, ancient temples, lush rice terraces, and vibrant culture.",
        "best_time_to_visit": "April to October (dry season)",
        "top_attractions": [
            "Uluwatu Temple - cliffside temple with sunset views",
            "Tegallalang Rice Terraces - iconic terraced landscapes",
            "Seminyak Beach - upscale beach area with great dining",
            "Sacred Monkey Forest Sanctuary - nature and culture",
            "Mount Batur - sunrise trekking volcano",
        ],
        "visa_requirements": "Visa-free for US citizens (up to 30 days)",
        "currency": "Indonesian Rupiah (IDR)",
        "language": "Bahasa Indonesia (English widely spoken in tourist areas)",
        "safety_rating": "Generally safe for tourists",
        "average_daily_cost_usd": 80,
        "suitability_score": 9,
    },
    "maldives": {
        "name": "Maldives",
        "overview": "Luxury island paradise with crystal-clear waters, overwater villas, and world-class snorkeling and diving.",
        "best_time_to_visit": "November to April (dry season)",
        "top_attractions": [
            "Male Fish Market - local culture experience",
            "Banana Reef - premier snorkeling and diving spot",
            "Vaadhoo Island - bioluminescent beach",
            "Maafushi Island - budget-friendly local island",
            "Hulhumale Beach - swimming and water sports",
        ],
        "visa_requirements": "Visa on arrival for US citizens (30 days free)",
        "currency": "Maldivian Rufiyaa (MVR)",
        "language": "Dhivehi (English widely spoken in resorts)",
        "safety_rating": "Very safe for tourists",
        "average_daily_cost_usd": 200,
        "suitability_score": 10,
    },
}


@traceable(run_type="chain", name="destination_react_agent", tags=["react", "destination"])
def _run_react_agent(destination: str, preferences: list[str]) -> str:
    """Run the ReAct agent with tools to research a destination. Falls back to mock on failure."""
    try:
        from src.utils.config import get_llm

        llm = get_llm()
        llm_with_tools = llm.bind_tools(TOOLS)

        from langgraph.graph import StateGraph, START, END
        from langgraph.prebuilt import ToolNode
        from typing import TypedDict, Annotated
        from langchain_core.messages import BaseMessage
        from langgraph.graph.message import add_messages

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

        pref_text = ", ".join(preferences) if preferences else "general sightseeing"
        user_msg = (
            f"Research the destination: {destination}\n"
            f"Traveler preferences: {pref_text}\n\n"
            f"Use get_destination_info to get structured data, and web_search for "
            f"current attractions, safety info, and travel tips. "
            f"Then provide a comprehensive destination research report."
        )

        result = react_graph.invoke({
            "messages": [
                SystemMessage(content=DESTINATION_RESEARCH_PROMPT),
                {"role": "user", "content": user_msg},
            ]
        })

        return result["messages"][-1].content

    except Exception as e:
        return f"FALLBACK_TO_MOCK: {e}"


@traceable(run_type="chain", name="destination_research_agent", tags=["agent", "destination"], metadata={"phase": "research"})
def destination_research_agent(state: TravelPlanState) -> dict:
    """Destination Research Agent - uses LLM with tools, falls back to mock data."""
    destination = state["travel_request"]["destination"]
    preferences = state["travel_request"].get("preferences", [])

    agent_response = _run_react_agent(destination, preferences)

    if agent_response.startswith("FALLBACK_TO_MOCK"):
        dest_key = destination.lower()
        data = MOCK_DESTINATIONS.get(dest_key, {
            "name": destination,
            "overview": f"Information about {destination} (mock fallback - LLM unavailable).",
            "best_time_to_visit": "Varies by season",
            "top_attractions": [f"Popular attractions in {destination}"],
            "visa_requirements": "Check with local embassy",
            "currency": "Local currency",
            "language": "Local language",
            "safety_rating": "Check travel advisories",
            "average_daily_cost_usd": 100,
            "suitability_score": 7,
        })
        return {
            "destination_research": data,
            "current_agent": "destination_research",
            "plan_status": "researching",
            "messages": [
                AIMessage(
                    content=f"[Mock fallback] Destination research for {data['name']}. "
                    f"Suitability: {data['suitability_score']}/10."
                )
            ],
        }

    research_data = {
        "name": destination,
        "overview": agent_response,
        "best_time_to_visit": "See research report above",
        "top_attractions": ["See research report for details"],
        "visa_requirements": "See research report",
        "currency": "See research report",
        "language": "See research report",
        "safety_rating": "See research report",
        "average_daily_cost_usd": 100,
        "suitability_score": 8,
        "llm_research": True,
    }

    summary = agent_response[:200] + "..." if len(agent_response) > 200 else agent_response

    return {
        "destination_research": research_data,
        "current_agent": "destination_research",
        "plan_status": "researching",
        "messages": [
            AIMessage(content=f"Destination research complete for {destination}.\n\n{agent_response}")
        ],
    }
