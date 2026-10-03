from langgraph.checkpoint.memory import MemorySaver
from langgraph.graph import END, START, StateGraph

from src.state import TravelPlanState


def build_graph(checkpointer=None, include_human_review: bool = True):
    """Build and compile the AI Travel Genie graph.

    Args:
        checkpointer: A LangGraph checkpointer (e.g. MemorySaver()).
            Auto-created when include_human_review is True and none is provided.
        include_human_review: If True, include the human_review node with
            interrupt(). If False, wire aggregator -> END (for eval/batch).

    Returns:
        Compiled StateGraph.
    """
    from src.agents.activity_agent import activity_itinerary_agent
    from src.agents.budget_agent import budget_currency_agent
    from src.agents.destination_agent import destination_research_agent
    from src.agents.flight_agent import flight_search_agent
    from src.agents.hotel_agent import hotel_search_agent
    from src.supervisor import (
        after_human_review,
        aggregate_results,
        human_review,
        parse_intent,
        supervisor_router,
    )

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

    if include_human_review:
        builder.add_node("human_review", human_review)
        builder.add_edge("aggregator", "human_review")
        builder.add_conditional_edges("human_review", after_human_review)
        if checkpointer is None:
            checkpointer = MemorySaver()
    else:
        builder.add_edge("aggregator", END)

    return builder.compile(checkpointer=checkpointer)
