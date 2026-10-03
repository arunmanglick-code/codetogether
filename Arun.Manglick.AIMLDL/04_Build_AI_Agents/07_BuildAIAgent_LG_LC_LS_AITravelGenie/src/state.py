from typing import Annotated, Literal, TypedDict

from langchain_core.messages import BaseMessage
from langgraph.graph.message import add_messages


class TravelRequest(TypedDict):
    destination: str
    origin: str
    start_date: str
    end_date: str
    budget_usd: float
    travelers: int
    preferences: list[str]
    special_requirements: str


class TravelPlanState(TypedDict):
    messages: Annotated[list[BaseMessage], add_messages]
    travel_request: TravelRequest
    current_agent: str
    destination_research: dict | None
    flight_options: dict | None
    hotel_options: dict | None
    itinerary: dict | None
    budget_summary: dict | None
    plan_status: Literal[
        "gathering_info",
        "researching",
        "planning",
        "reviewing",
        "revising",
        "complete",
    ]
    human_feedback: str | None
    iteration_count: int
    previous_travel_request: TravelRequest | None
    invalidated_agents: list[str]
