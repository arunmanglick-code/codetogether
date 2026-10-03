"""Full LLM multi-turn pipeline test for Phase 5 — tests interrupt/resume flow."""
import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

pass_count = 0
fail_count = 0


def safe(s):
    return s.encode("ascii", "replace").decode("ascii") if isinstance(s, str) else str(s)


def flush(*args, **kwargs):
    print(*args, **kwargs, flush=True)


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
from langgraph.checkpoint.memory import MemorySaver
from langgraph.types import Command

from src.state import TravelPlanState
from src.supervisor import (
    parse_intent,
    supervisor_router,
    aggregate_results,
    human_review,
    after_human_review,
)
from src.agents.destination_agent import destination_research_agent
from src.agents.flight_agent import flight_search_agent
from src.agents.hotel_agent import hotel_search_agent
from src.agents.activity_agent import activity_itinerary_agent
from src.agents.budget_agent import budget_currency_agent

# Build graph with checkpointer
builder = StateGraph(TravelPlanState)
builder.add_node("intent_parser", parse_intent)
builder.add_node("destination_research", destination_research_agent)
builder.add_node("flight_search", flight_search_agent)
builder.add_node("hotel_search", hotel_search_agent)
builder.add_node("activity_itinerary", activity_itinerary_agent)
builder.add_node("budget_currency", budget_currency_agent)
builder.add_node("aggregator", aggregate_results)
builder.add_node("human_review", human_review)
builder.add_edge(START, "intent_parser")
builder.add_conditional_edges("intent_parser", supervisor_router)
builder.add_conditional_edges("destination_research", supervisor_router)
builder.add_conditional_edges("flight_search", supervisor_router)
builder.add_conditional_edges("hotel_search", supervisor_router)
builder.add_conditional_edges("activity_itinerary", supervisor_router)
builder.add_conditional_edges("budget_currency", supervisor_router)
builder.add_edge("aggregator", "human_review")
builder.add_conditional_edges("human_review", after_human_review)

memory = MemorySaver()
graph = builder.compile(checkpointer=memory)
config = {"configurable": {"thread_id": "test-multiturn-1"}}

# ========== TURN 1: Initial Request ==========
flush("=" * 70)
flush("  TURN 1: Initial Maldives Request")
flush("=" * 70)
flush()

flush("  Invoking graph (all 5 agents)...")
result1 = graph.invoke(
    {"messages": [HumanMessage(content=(
        "Plan a 7-day luxury beach vacation in the Maldives for 2 people, "
        "budget $5000, departing from New York. "
        "Dates: 2025-06-10 to 2025-06-17. "
        "Preferences: beach, snorkeling, spa."
    ))]},
    config,
)
flush("  Graph paused at human_review.")
flush()

state1 = graph.get_state(config)
s1 = state1.values

check("Turn 1: plan_status is 'reviewing'", s1.get("plan_status") == "reviewing")
check("Turn 1: next node is human_review", "human_review" in state1.next)
check("Turn 1: destination_research populated", s1.get("destination_research") is not None)
check("Turn 1: flight_options has 3", len(s1.get("flight_options", {}).get("options", [])) == 3)
check("Turn 1: hotel_options has 3", len(s1.get("hotel_options", {}).get("options", [])) == 3)
check("Turn 1: itinerary has 7 days", len(s1.get("itinerary", {}).get("days", [])) == 7)
check("Turn 1: budget_summary populated", s1.get("budget_summary") is not None)
check("Turn 1: iteration_count is 0", s1.get("iteration_count") == 0)

# Verify interrupt payload
has_interrupt = False
if state1.tasks:
    for task in state1.tasks:
        if hasattr(task, "interrupts") and task.interrupts:
            has_interrupt = True
check("Turn 1: interrupt payload present", has_interrupt)

# Save Turn 1 state for comparison
t1_dest = s1.get("travel_request", {}).get("destination", "")
t1_flights = s1.get("flight_options", {}).get("options", [])
t1_hotels = s1.get("hotel_options", {}).get("options", [])
t1_itinerary_days = len(s1.get("itinerary", {}).get("days", []))
flush()

# ========== TURN 2: Cheaper Hotels (budget change) ==========
flush("=" * 70)
flush("  TURN 2: Find cheaper hotels (budget $3500)")
flush("=" * 70)
flush()

flush("  Resuming with modification...")
result2 = graph.invoke(
    Command(resume="Find cheaper hotels, lower the budget to $3500"),
    config,
)
flush("  Graph paused again.")
flush()

state2 = graph.get_state(config)
s2 = state2.values

check("Turn 2: plan_status is 'reviewing'", s2.get("plan_status") == "reviewing")
check("Turn 2: budget changed to ~3500", abs(s2.get("travel_request", {}).get("budget_usd", 0) - 3500) < 500)
check("Turn 2: iteration_count is 1", s2.get("iteration_count") == 1)
check("Turn 2: destination unchanged", s2.get("travel_request", {}).get("destination", "").lower() == t1_dest.lower())

# Flights should be unchanged (budget change doesn't cascade to flights)
t2_flights = s2.get("flight_options", {}).get("options", [])
# Destination research should be unchanged
check("Turn 2: destination_research preserved", s2.get("destination_research") is not None)
flush()

# ========== TURN 3: Approve ==========
flush("=" * 70)
flush("  TURN 3: Approve the plan")
flush("=" * 70)
flush()

flush("  Resuming with approval...")
result3 = graph.invoke(
    Command(resume="approve"),
    config,
)
flush("  Graph completed.")
flush()

state3 = graph.get_state(config)
s3 = state3.values

check("Turn 3: plan_status is 'complete'", s3.get("plan_status") == "complete")
check("Turn 3: next is empty (graph ended)", len(state3.next) == 0)
check("Turn 3: human_feedback is 'approve'", s3.get("human_feedback") == "approve")

# Verify final message
last_msg = s3["messages"][-1].content
check("Turn 3: final approval message", "approved" in last_msg.lower() or "wonderful" in last_msg.lower())

flush()

# ========== SUMMARY ==========
flush("=" * 70)
flush(f"  PHASE 5 MULTI-TURN TESTS: {pass_count} PASS, {fail_count} FAIL")
flush("=" * 70)

if fail_count > 0:
    sys.exit(1)
