"""Comprehensive end-to-end test for all phases (1-4) of AI Travel Genie."""
import json
import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

pass_count = 0
fail_count = 0


def safe(s):
    return s.encode("ascii", "replace").decode("ascii") if isinstance(s, str) else str(s)


def check(name, condition):
    global pass_count, fail_count
    if condition:
        pass_count += 1
        print(f"  [PASS] {name}")
    else:
        fail_count += 1
        print(f"  [FAIL] {name}")


# ========== PHASE 1: Foundation ==========
print("=" * 70)
print("  PHASE 1: Foundation - State, Graph, Routing")
print("=" * 70)
print()

from src.state import TravelPlanState, TravelRequest
check("TravelPlanState and TravelRequest imported", True)

from src.utils.config import get_llm, ANTHROPIC_MODEL, LLM_PROVIDER
check(f"Config loaded: provider={LLM_PROVIDER}, model={ANTHROPIC_MODEL}", True)

from src.agents.destination_agent import destination_research_agent
from src.agents.flight_agent import flight_search_agent
from src.agents.hotel_agent import hotel_search_agent
from src.agents.activity_agent import activity_itinerary_agent
from src.agents.budget_agent import budget_currency_agent
check("All 5 agents imported", True)

from src.supervisor import parse_intent, supervisor_router, aggregate_results
check("Supervisor functions imported", True)

from langgraph.graph import StateGraph, START, END
from langchain_core.messages import HumanMessage

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
check("Graph compiled (7 nodes)", True)

# Routing logic
s = {"destination_research": None, "flight_options": None, "hotel_options": None, "itinerary": None, "budget_summary": None}
check("Route: None -> destination_research", supervisor_router(s) == "destination_research")
s["destination_research"] = {"name": "X"}
check("Route: dest filled -> flight_search", supervisor_router(s) == "flight_search")
s["flight_options"] = {"options": []}
check("Route: flights filled -> hotel_search", supervisor_router(s) == "hotel_search")
s["hotel_options"] = {"options": []}
check("Route: hotels filled -> activity_itinerary", supervisor_router(s) == "activity_itinerary")
s["itinerary"] = {"days": []}
check("Route: itinerary filled -> budget_currency", supervisor_router(s) == "budget_currency")
s["budget_summary"] = {}
check("Route: all filled -> aggregator", supervisor_router(s) == "aggregator")

# ========== PHASE 2: Search Tools ==========
print()
print("=" * 70)
print("  PHASE 2: Destination Research Tools")
print("=" * 70)
print()

from src.tools.search_tools import get_destination_info, web_search

for dest in ["Bali", "Maldives", "Paris", "Tokyo"]:
    r = get_destination_info.invoke(dest)
    check(f"get_destination_info({dest}) found", "No pre-loaded info" not in r)

r = get_destination_info.invoke("Timbuktu")
check("Unknown dest suggests web_search", "No pre-loaded info" in r)

try:
    r = web_search.invoke("Bali travel 2025")
    check(f"DuckDuckGo web_search ({len(r)} chars)", len(r) > 50)
except Exception as e:
    check(f"DuckDuckGo web_search (error: {e})", False)

# ========== PHASE 3: Flight & Hotel Tools ==========
print()
print("=" * 70)
print("  PHASE 3: Flight & Hotel Tools")
print("=" * 70)
print()

from src.tools.flight_tools import search_flights, get_airport_code
from src.tools.hotel_tools import search_hotels, get_hotel_reviews

# Airport codes
airports = {"New York": "JFK", "Bali": "DPS", "Paris": "CDG", "Tokyo": "NRT",
            "Maldives": "MLE", "Santorini": "JTR", "Cancun": "CUN", "Dubai": "DXB"}
for city, expected in airports.items():
    r = json.loads(get_airport_code.invoke(city))
    check(f"Airport {city} -> {expected}", r["code"] == expected)

r = json.loads(get_airport_code.invoke("Timbuktu"))
check("Unknown airport returns estimate", "code" in r)

# Flight search - multiple routes
routes = [
    ("New York", "Bali", 2), ("Chicago", "Maldives", 2),
    ("San Francisco", "Santorini", 2), ("Miami", "Cancun", 4),
    ("London", "Tokyo", 1),
]
for origin, dest, pax in routes:
    r = json.loads(search_flights.invoke({"origin": origin, "destination": dest, "travelers": pax}))
    flights = r["flights"]
    has_3 = len(flights) == 3
    prices_ok = all(f["price_per_person"] > 0 for f in flights)
    totals_ok = all(f["total_price"] == f["price_per_person"] * pax for f in flights)
    ranked = all(
        (flights[i]["rating"] > flights[i+1]["rating"]) or
        (flights[i]["rating"] == flights[i+1]["rating"] and flights[i]["price_per_person"] <= flights[i+1]["price_per_person"])
        for i in range(len(flights)-1)
    )
    check(f"Flights {origin}->{dest} ({pax}pax): 3 results, ranked, totals correct",
          has_3 and prices_ok and totals_ok and ranked)

# Determinism
r1 = json.loads(search_flights.invoke({"origin": "New York", "destination": "Bali", "travelers": 2}))
r2 = json.loads(search_flights.invoke({"origin": "New York", "destination": "Bali", "travelers": 2}))
check("Flight search deterministic", r1["flights"][0]["price_per_person"] == r2["flights"][0]["price_per_person"])

# Hotel search
hotel_dests = [("Bali", 5, 150), ("Paris", 4, 250), ("Santorini", 7, 0), ("Cancun", 5, 100), ("Tokyo", 3, 200)]
for dest, nights, budget in hotel_dests:
    r = json.loads(search_hotels.invoke({"destination": dest, "nights": nights, "budget_per_night": budget}))
    hotels = r["hotels"]
    has_3 = len(hotels) == 3
    nights_ok = all(h["nights"] == nights for h in hotels)
    totals_ok = all(h["total_price"] == h["price_per_night"] * nights for h in hotels)
    ranked = all(
        (hotels[i]["guest_rating"] > hotels[i+1]["guest_rating"]) or
        (hotels[i]["guest_rating"] == hotels[i+1]["guest_rating"] and hotels[i]["price_per_night"] <= hotels[i+1]["price_per_night"])
        for i in range(len(hotels)-1)
    )
    check(f"Hotels {dest} ({nights}n): 3 results, ranked, totals correct",
          has_3 and nights_ok and totals_ok and ranked)

# Hotel reviews
r = json.loads(get_hotel_reviews.invoke({"hotel_name": "Four Seasons", "destination": "Bali"}))
check("Hotel reviews: rating + 3 reviews", r.get("overall_rating", 0) > 0 and len(r.get("sample_reviews", [])) == 3)

# ========== PHASE 4: Activity & Budget Tools ==========
print()
print("=" * 70)
print("  PHASE 4: Activity & Budget Tools")
print("=" * 70)
print()

from src.tools.activity_tools import search_activities, get_weather_forecast
from src.tools.budget_tools import convert_currency, calculate_budget

# Activities - known
for dest, prefs in [("Bali", "beach,culture"), ("Maldives", "snorkeling,spa"), ("Paris", "culture,food"), ("Tokyo", "food,adventure")]:
    r = json.loads(search_activities.invoke({"destination": dest, "preferences": prefs, "num_days": 5}))
    total = r.get("total_unique_activities", 0)
    check(f"Activities {dest}: {total} activities", total > 5)

# Activities - dynamic
r = json.loads(search_activities.invoke({"destination": "Reykjavik", "preferences": "nature", "num_days": 5}))
check(f"Activities Reykjavik (dynamic): {r.get('total_unique_activities', 0)} acts", r.get("total_unique_activities", 0) > 0)

# Weather
weather_tests = [
    ("Bali", "2025-03-15", "tropical"), ("Paris", "2025-06-15", "temperate"),
    ("Santorini", "2025-07-10", "mediterranean"), ("Tokyo", "2025-04-10", "temperate"),
]
for dest, date, exp_climate in weather_tests:
    r = json.loads(get_weather_forecast.invoke({"destination": dest, "start_date": date, "num_days": 5}))
    check(f"Weather {dest}: climate={r['climate']}, {len(r['forecast'])} days",
          r["climate"] == exp_climate and len(r["forecast"]) == 5)

# Currency conversion
currencies = [("Bali", "IDR", 15800), ("Tokyo", "JPY", 149.0), ("Paris", "EUR", 0.92),
              ("Maldives", "MVR", 15.42), ("Cancun", "MXN", 17.2)]
for dest, exp_code, exp_rate in currencies:
    r = json.loads(convert_currency.invoke({"amount_usd": 1000, "destination": dest}))
    check(f"Currency {dest}: {exp_code} @ {exp_rate}",
          r["currency_code"] == exp_code and abs(r["exchange_rate"] - exp_rate) < 0.01)

# Budget calculator
budget_tests = [
    {"budget_usd": 3000, "travelers": 2, "num_days": 5, "flight_cost": 1900, "hotel_cost": 460, "activity_cost": 380, "destination": "Bali"},
    {"budget_usd": 8000, "travelers": 2, "num_days": 7, "flight_cost": 2500, "hotel_cost": 3000, "activity_cost": 900, "destination": "Maldives"},
    {"budget_usd": 5000, "travelers": 2, "num_days": 7, "flight_cost": 1200, "hotel_cost": 1500, "activity_cost": 500, "destination": "Santorini"},
]
for bt in budget_tests:
    r = json.loads(calculate_budget.invoke(bt))
    components = ["flights", "accommodation", "activities", "food_and_dining", "local_transport", "miscellaneous"]
    comp_sum = sum(r["breakdown"].get(k, 0) for k in components)
    math_ok = abs(comp_sum - r["total_usd"]) < 1
    total = r["total_usd"]
    bud = r["budget_usd"]
    if total <= bud * 0.95:
        exp_status = "WITHIN BUDGET"
    elif total <= bud * 1.05:
        exp_status = "ON TARGET"
    else:
        exp_status = "OVER BUDGET"
    status_ok = r["budget_status"] == exp_status
    conv = r["currency_conversion"]
    fx_ok = abs(conv["total_local_currency"] - total * conv["exchange_rate"]) < 1
    tips_ok = (r["budget_status"] != "OVER BUDGET") or len(r.get("savings_tips", [])) > 0
    check(f"Budget {bt['destination']}: math OK, status={r['budget_status']}, fx OK, tips OK",
          math_ok and status_ok and fx_ok and tips_ok)

# ========== SUMMARY ==========
print()
print("=" * 70)
print(f"  ALL TOOL & LOGIC TESTS: {pass_count} PASS, {fail_count} FAIL")
print("=" * 70)

if fail_count > 0:
    sys.exit(1)
