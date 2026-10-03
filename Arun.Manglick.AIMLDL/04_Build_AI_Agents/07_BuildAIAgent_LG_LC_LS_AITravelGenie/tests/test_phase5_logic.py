"""Unit tests for Phase 5 Memory & Conversation logic — no LLM calls."""
import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

pass_count = 0
fail_count = 0


def check(name, condition):
    global pass_count, fail_count
    if condition:
        pass_count += 1
        print(f"  [PASS] {name}", flush=True)
    else:
        fail_count += 1
        print(f"  [FAIL] {name}", flush=True)


# ========== IMPORTS ==========
print("=" * 70, flush=True)
print("  PHASE 5: Memory & Conversation — Logic Tests", flush=True)
print("=" * 70, flush=True)
print(flush=True)

from src.supervisor import (
    _diff_requests,
    _compute_cascade,
    _describe_changes,
    _keyword_fallback_modification,
    human_review,
    after_human_review,
    parse_intent,
    supervisor_router,
    aggregate_results,
    ALL_AGENT_FIELDS,
    CASCADE_RULES,
)
from src.state import TravelPlanState, TravelRequest

check("All Phase 5 functions imported", True)

# ========== State Changes ==========
print(flush=True)
print("--- State Definition ---", flush=True)

check("TravelPlanState has previous_travel_request", "previous_travel_request" in TravelPlanState.__annotations__)
check("TravelPlanState has invalidated_agents", "invalidated_agents" in TravelPlanState.__annotations__)
plan_status_type = TravelPlanState.__annotations__["plan_status"]
check("plan_status includes 'revising'", "revising" in str(plan_status_type))

# ========== _diff_requests ==========
print(flush=True)
print("--- _diff_requests ---", flush=True)

base = {
    "destination": "Maldives", "origin": "New York",
    "start_date": "2025-06-10", "end_date": "2025-06-17",
    "budget_usd": 5000.0, "travelers": 2,
    "preferences": ["beach", "spa"], "special_requirements": "none",
}

# No change
check("No change -> empty set", _diff_requests(base, dict(base)) == set())

# Single field changes
check("Destination change", _diff_requests(base, {**base, "destination": "Bali"}) == {"destination"})
check("Origin change", _diff_requests(base, {**base, "origin": "Chicago"}) == {"origin"})
check("Start date change", _diff_requests(base, {**base, "start_date": "2025-04-10"}) == {"start_date"})
check("End date change", _diff_requests(base, {**base, "end_date": "2025-04-17"}) == {"end_date"})
check("Budget change", _diff_requests(base, {**base, "budget_usd": 3000.0}) == {"budget_usd"})
check("Travelers change", _diff_requests(base, {**base, "travelers": 4}) == {"travelers"})
check("Preferences change", _diff_requests(base, {**base, "preferences": ["adventure"]}) == {"preferences"})
check("Requirements change", _diff_requests(base, {**base, "special_requirements": "wheelchair"}) == {"special_requirements"})

# Multiple field changes
check("Dest + dates", _diff_requests(base, {**base, "destination": "Bali", "start_date": "2025-04-10"}) == {"destination", "start_date"})
check("Budget + prefs", _diff_requests(base, {**base, "budget_usd": 3000, "preferences": ["luxury"]}) == {"budget_usd", "preferences"})

# Preferences order doesn't matter (set comparison)
check("Same prefs different order", _diff_requests(base, {**base, "preferences": ["spa", "beach"]}) == set())

# ========== _compute_cascade ==========
print(flush=True)
print("--- _compute_cascade ---", flush=True)

# Destination -> all 5
cascade = _compute_cascade({"destination"})
check("Destination cascades to all 5 agents", len(cascade) == 5)
check("Cascade order preserved", cascade == sorted(cascade, key=lambda f: ALL_AGENT_FIELDS.index(f)))

# Origin -> flight, hotel, budget
cascade = _compute_cascade({"origin"})
check("Origin cascades to flight, hotel, budget",
      set(cascade) == {"flight_options", "hotel_options", "budget_summary"})

# Dates -> flight, hotel, itinerary, budget
cascade = _compute_cascade({"start_date"})
check("Start date cascades to 4 agents", set(cascade) == {"flight_options", "hotel_options", "itinerary", "budget_summary"})
cascade = _compute_cascade({"end_date"})
check("End date cascades to 4 agents", set(cascade) == {"flight_options", "hotel_options", "itinerary", "budget_summary"})
cascade = _compute_cascade({"start_date", "end_date"})
check("Both dates cascades to 4 agents", set(cascade) == {"flight_options", "hotel_options", "itinerary", "budget_summary"})

# Budget -> hotel, budget
cascade = _compute_cascade({"budget_usd"})
check("Budget cascades to hotel + budget", set(cascade) == {"hotel_options", "budget_summary"})

# Travelers -> flight, hotel, budget
cascade = _compute_cascade({"travelers"})
check("Travelers cascades to flight, hotel, budget", set(cascade) == {"flight_options", "hotel_options", "budget_summary"})

# Preferences -> hotel, itinerary, budget
cascade = _compute_cascade({"preferences"})
check("Preferences cascades to hotel, itinerary, budget",
      set(cascade) == {"hotel_options", "itinerary", "budget_summary"})

# Requirements -> itinerary, budget
cascade = _compute_cascade({"special_requirements"})
check("Requirements cascades to itinerary + budget", set(cascade) == {"itinerary", "budget_summary"})

# Combined: dest + budget -> all 5 (union)
cascade = _compute_cascade({"destination", "budget_usd"})
check("Dest + budget cascades to all 5", len(cascade) == 5)

# Empty change
cascade = _compute_cascade(set())
check("Empty change -> empty cascade", cascade == [])

# ========== _describe_changes ==========
print(flush=True)
print("--- _describe_changes ---", flush=True)

desc = _describe_changes({"destination"})
check("Single change description", "destination" in desc.lower())

desc = _describe_changes({"start_date", "end_date"})
check("Multi change description", "date" in desc.lower())

# ========== _keyword_fallback_modification ==========
print(flush=True)
print("--- Keyword fallback ---", flush=True)

mod = _keyword_fallback_modification(base, "Find cheaper hotels")
check("'cheaper' lowers budget", mod["budget_usd"] < base["budget_usd"])

mod = _keyword_fallback_modification(base, "Change destination to Bali")
check("Detects Bali destination", mod["destination"] == "Bali")

mod = _keyword_fallback_modification(base, "Change dates to 2025-04-10 to 2025-04-17")
check("Detects date pattern", mod["start_date"] == "2025-04-10" and mod["end_date"] == "2025-04-17")

mod = _keyword_fallback_modification(base, "Make it more luxurious")
check("'luxury' increases budget", mod["budget_usd"] > base["budget_usd"])

mod = _keyword_fallback_modification(base, "No changes")
check("No keywords -> no changes", mod == base)

# ========== after_human_review ==========
print(flush=True)
print("--- after_human_review routing ---", flush=True)

check("Complete -> __end__", after_human_review({"plan_status": "complete"}) == "__end__")
check("Revising -> intent_parser", after_human_review({"plan_status": "revising"}) == "intent_parser")
check("Reviewing -> intent_parser", after_human_review({"plan_status": "reviewing"}) == "intent_parser")

# ========== supervisor_router unchanged ==========
print(flush=True)
print("--- supervisor_router (unchanged) ---", flush=True)

s = {"destination_research": None, "flight_options": None, "hotel_options": None, "itinerary": None, "budget_summary": None}
check("All None -> destination_research", supervisor_router(s) == "destination_research")

s["destination_research"] = {"name": "X"}
s["flight_options"] = {"options": []}
check("Dest+flight filled, hotel None -> hotel_search", supervisor_router(s) == "hotel_search")

s["hotel_options"] = {"options": []}
s["itinerary"] = {"days": []}
s["budget_summary"] = {"total": 0}
check("All filled -> aggregator", supervisor_router(s) == "aggregator")

# Selective: only hotel+budget None (modification scenario)
s2 = {
    "destination_research": {"name": "X"},
    "flight_options": {"options": []},
    "hotel_options": None,
    "itinerary": {"days": []},
    "budget_summary": None,
}
check("Hotel+budget None -> hotel_search (selective)", supervisor_router(s2) == "hotel_search")

s2["hotel_options"] = {"options": []}
check("Only budget None -> budget_currency", supervisor_router(s2) == "budget_currency")

# ========== Graph compilation with MemorySaver ==========
print(flush=True)
print("--- Graph compilation ---", flush=True)

from langgraph.graph import StateGraph, START, END
from langgraph.checkpoint.memory import MemorySaver

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

mem = MemorySaver()
graph = builder.compile(checkpointer=mem)
check("Graph compiled with 8 nodes + MemorySaver", True)

nodes = list(graph.get_graph().nodes.keys())
check("human_review node present", "human_review" in nodes)
check("8 nodes total (including __start__ and __end__)", len(nodes) == 10)

# ========== SUMMARY ==========
print(flush=True)
print("=" * 70, flush=True)
print(f"  PHASE 5 LOGIC TESTS: {pass_count} PASS, {fail_count} FAIL", flush=True)
print("=" * 70, flush=True)

if fail_count > 0:
    sys.exit(1)
