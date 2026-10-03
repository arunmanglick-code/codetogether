import json

from langchain_core.messages import AIMessage, SystemMessage
from langsmith import traceable

from src.prompts.activity_prompt import ACTIVITY_ITINERARY_PROMPT
from src.state import TravelPlanState
from src.tools.activity_tools import search_activities, get_weather_forecast

TOOLS = [search_activities, get_weather_forecast]


@traceable(run_type="chain", name="activity_react_agent", tags=["react", "activity"])
def _run_react_agent(destination: str, preferences: list[str], start_date: str, num_days: int) -> str:
    """Run the ReAct agent with activity tools. Falls back to mock on failure."""
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

        pref_text = ", ".join(preferences) if preferences else "general sightseeing"
        user_msg = (
            f"Create a {num_days}-day itinerary for {destination}.\n"
            f"Traveler preferences: {pref_text}\n"
            f"Start date: {start_date}\n\n"
            f"Use get_weather_forecast to check weather conditions for the trip dates, "
            f"then use search_activities to find available activities matching the traveler's preferences. "
            f"Create a day-by-day itinerary with morning, afternoon, and evening activities. "
            f"Consider weather when planning — schedule indoor activities on rainy days. "
            f"For each activity, include the name and estimated cost per person."
        )

        result = react_graph.invoke({
            "messages": [
                SystemMessage(content=ACTIVITY_ITINERARY_PROMPT),
                {"role": "user", "content": user_msg},
            ]
        })

        return result["messages"][-1].content

    except Exception as e:
        return f"FALLBACK_TO_MOCK: {e}"


def _generate_mock_itinerary(destination: str, preferences: list[str], start_date: str, num_days: int) -> dict:
    """Generate a mock itinerary using tools directly."""
    activities_raw = search_activities.invoke({
        "destination": destination,
        "preferences": ", ".join(preferences) if preferences else "",
        "num_days": num_days,
    })
    activities_data = json.loads(activities_raw)

    weather_raw = get_weather_forecast.invoke({
        "destination": destination,
        "start_date": start_date,
        "num_days": num_days,
    })
    weather_data = json.loads(weather_raw)

    all_activities = []
    for cat_items in activities_data["activities_by_type"].values():
        all_activities.extend(cat_items)

    days = []
    act_idx = 0
    themes = ["Exploration", "Culture & History", "Adventure", "Local Experience", "Relaxation",
              "Nature & Discovery", "Food & Markets", "Scenic Tour", "Beach & Water", "Farewell"]

    for day_num in range(num_days):
        weather = weather_data["forecast"][day_num] if day_num < len(weather_data["forecast"]) else {
            "description": "Pleasant", "temperature_c": 25, "outdoor_friendly": True
        }

        def pick_activity():
            nonlocal act_idx
            if act_idx < len(all_activities):
                a = all_activities[act_idx]
                act_idx += 1
                return {"activity": a["name"], "cost": a["cost"]}
            return {"activity": f"Free time in {destination}", "cost": 0}

        day_theme = themes[day_num % len(themes)]
        if day_num == 0:
            day_theme = "Arrival & Exploration"
        elif day_num == num_days - 1:
            day_theme = "Departure"

        weather_str = f"{weather['description']}, {weather['temperature_c']}C"

        days.append({
            "day": day_num + 1,
            "theme": day_theme,
            "morning": pick_activity(),
            "afternoon": pick_activity(),
            "evening": pick_activity(),
            "weather": weather_str,
        })

    total_activity_cost = sum(
        d["morning"]["cost"] + d["afternoon"]["cost"] + d["evening"]["cost"]
        for d in days
    )

    return {
        "days": days,
        "total_days": num_days,
        "total_activity_cost": total_activity_cost,
        "estimated_food_cost": num_days * 50,
        "source": "mock_fallback",
    }


@traceable(run_type="chain", name="activity_itinerary_agent", tags=["agent", "activity"], metadata={"phase": "planning"})
def activity_itinerary_agent(state: TravelPlanState) -> dict:
    """Activity/Itinerary Agent - uses LLM with tools, falls back to dynamic mock data."""
    destination = state["travel_request"]["destination"]
    preferences = state["travel_request"].get("preferences", [])
    start_date = state["travel_request"]["start_date"]
    end_date = state["travel_request"]["end_date"]

    try:
        from datetime import datetime
        num_days = (datetime.strptime(end_date, "%Y-%m-%d") - datetime.strptime(start_date, "%Y-%m-%d")).days
    except (ValueError, TypeError):
        num_days = 5

    agent_response = _run_react_agent(destination, preferences, start_date, num_days)

    if agent_response.startswith("FALLBACK_TO_MOCK"):
        itinerary = _generate_mock_itinerary(destination, preferences, start_date, num_days)
        days = itinerary["days"]
        return {
            "itinerary": itinerary,
            "current_agent": "activity_itinerary",
            "plan_status": "planning",
            "messages": [
                AIMessage(
                    content=f"[Mock fallback] Created {len(days)}-day itinerary for {destination}. "
                    f"Total activity cost: ${itinerary['total_activity_cost']}."
                )
            ],
        }

    itinerary = _generate_mock_itinerary(destination, preferences, start_date, num_days)
    itinerary["llm_analysis"] = agent_response
    itinerary["source"] = "llm_react"

    return {
        "itinerary": itinerary,
        "current_agent": "activity_itinerary",
        "plan_status": "planning",
        "messages": [
            AIMessage(content=f"Itinerary planning complete for {destination} ({num_days} days).\n\n{agent_response}")
        ],
    }
