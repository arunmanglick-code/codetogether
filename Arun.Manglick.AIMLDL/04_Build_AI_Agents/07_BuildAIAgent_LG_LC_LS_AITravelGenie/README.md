# AI Travel Genie

A production-grade, multi-agent vacation planning system built with **LangChain**, **LangGraph**, and **LangSmith**. Five specialist AI agents — each powered by Claude and equipped with domain-specific tools — collaborate under a supervisor to research destinations, find flights and hotels, create weather-aware day-by-day itineraries, and analyze budgets with currency conversion. The entire pipeline runs as a LangGraph `StateGraph` with human-in-the-loop approval, checkpointed memory for multi-turn conversations, and full LangSmith observability.

---

## Table of Contents

- [Architecture](#architecture)
- [How It Works — The AI Harness](#how-it-works--the-ai-harness)
- [Agent Deep Dive](#agent-deep-dive)
- [Tool Reference](#tool-reference)
- [Design Complexity](#design-complexity)
- [Why LangChain + LangGraph + LangSmith](#why-langchain--langgraph--langsmith)
- [Project Structure](#project-structure)
- [Setup & Installation](#setup--installation)
- [How to run](#usage)
- [Testing & Evaluation](#testing--evaluation)
- [Development Phases](#development-phases)
- [Tech Stack](#tech-stack)

---

## Architecture

```
User Message
     |
     v
+------------------+
|  intent_parser   |  ── LLM extracts TravelRequest (destination, dates, budget, travelers, preferences)
+--------+---------+
         |
+--------v---------+
| supervisor_router|  ── Conditional edges: checks which agent outputs are None
+--------+---------+
         |
         +--------- destination_research is None? ─── Yes ──> Destination Research Agent
         |                                                          |
         +--------- flight_options is None? ──────── Yes ──> Flight Search Agent
         |                                                          |
         +--------- hotel_options is None? ──────── Yes ──> Hotel Search Agent
         |                                                          |
         +--------- itinerary is None? ──────────── Yes ──> Activity/Itinerary Agent
         |                                                          |
         +--------- budget_summary is None? ─────── Yes ──> Budget/Currency Agent
         |                                                          |
         +--------- all populated ───────────────── Yes ──> Aggregator
                                                                    |
                                                           +--------v---------+
                                                           |   human_review   |
                                                           |  interrupt() /   |
                                                           | Command(resume=) |
                                                           +--------+---------+
                                                                    |
                                                          approve?--+--modify?
                                                           |                 |
                                                      +---v---+    +--------v---------+
                                                      |  END  |    |  intent_parser   |
                                                      +-------+    | (selective re-run |
                                                                   |  via cascade)     |
                                                                   +------------------+
```

Each specialist agent internally runs its own **ReAct sub-graph** — an inner `StateGraph` where the LLM reasons, calls tools, observes results, and reasons again until it has a complete answer. The outer supervisor graph sees each agent as a single node.

---

## How It Works — The AI Harness

### 1. State-Driven Orchestration

The system's brain is `TravelPlanState`, a flat `TypedDict` with `Annotated[list[BaseMessage], add_messages]` for message accumulation and `dict | None` fields for each agent's output:

```python
class TravelPlanState(TypedDict):
    messages: Annotated[list[BaseMessage], add_messages]
    travel_request: TravelRequest          # Parsed user intent
    destination_research: dict | None      # None = agent hasn't run yet
    flight_options: dict | None
    hotel_options: dict | None
    itinerary: dict | None
    budget_summary: dict | None
    plan_status: Literal["gathering_info", "researching", "planning", "reviewing", "revising", "complete"]
    human_feedback: str | None
    iteration_count: int
    previous_travel_request: TravelRequest | None
    invalidated_agents: list[str]
```

The `supervisor_router` function checks each `None` field sequentially to decide which agent runs next. When all fields are populated, it routes to the `aggregator`. This simple, deterministic routing eliminates the need for an LLM to make routing decisions — the graph topology handles it.

### 2. ReAct Agent Pattern

Every tool-using agent follows the same internal ReAct pattern:

```
[System Prompt] → LLM → has tool_calls? → Yes → ToolNode → LLM → ...
                                         → No  → Return result
```

Each agent builds its own `StateGraph` with `AgentState`, binds tools via `llm.bind_tools()`, and uses `ToolNode` from `langgraph.prebuilt` for tool execution. The `should_continue` function checks for `tool_calls` on the last message to decide whether to loop.

### 3. Human-in-the-Loop

After the aggregator assembles the complete plan, the `human_review` node uses LangGraph's modern `interrupt()` function (from `langgraph.types`):

```python
feedback = interrupt({"plan": plan_message, "prompt": "Review the plan..."})
```

The graph pauses, presents the plan to the user, and waits. The user can:
- **Approve** → graph ends with `plan_status: "complete"`
- **Modify** → feedback becomes a `HumanMessage`, graph routes back to `intent_parser`

Resumption uses `Command(resume=user_feedback)` — the node re-executes up to the `interrupt()` call, which returns the user's response.

### 4. Selective Re-Planning (Cascade Rules)

When the user modifies their request (e.g., "change the destination to Bali"), the system doesn't re-run all agents. Instead:

1. **`_parse_modification()`** — LLM merges the user's change into the existing `TravelRequest`
2. **`_diff_requests()`** — compares old vs. new request to find changed fields
3. **`_compute_cascade()`** — deterministic rules map changed fields to invalidated agents:

| Changed Field | Agents Invalidated |
|--------------|-------------------|
| `destination` | All 5 agents |
| `origin` | flight, hotel, budget |
| `start_date` / `end_date` | flight, hotel, itinerary, budget |
| `budget_usd` | hotel, budget |
| `travelers` | flight, hotel, budget |
| `preferences` | hotel, itinerary, budget |
| `special_requirements` | itinerary, budget |

4. **`parse_intent`** nulls the invalidated fields → `supervisor_router` naturally re-routes to those agents.

### 5. Checkpointed Memory

`MemorySaver` from `langgraph.checkpoint.memory` provides session persistence. Each conversation thread gets a unique ID, enabling multi-turn planning — the user can plan a trip, approve it, then start a new session or modify the same plan in a continued conversation.

### 6. Mock Fallback Architecture

Every agent has a dual-path design:
- **Primary**: LLM + tools via the ReAct sub-graph
- **Fallback**: Direct tool invocation with mock data when the LLM API is unavailable

This means the graph always completes, even during API outages, making the system resilient for demos and development.

---

## Agent Deep Dive

### Destination Research Agent
- **Tools**: `web_search` (DuckDuckGo), `get_destination_info` (structured DB)
- **Behavior**: Searches the web for current travel info (attractions, safety, visa requirements) and cross-references with a structured info database covering 4 major destinations (Bali, Maldives, Paris, Tokyo) with visa/currency/safety/emergency details
- **Output**: `destination_research` dict with name, overview, best time to visit, top attractions, visa, currency, suitability score (1-10)

### Flight Search Agent
- **Tools**: `search_flights`, `get_airport_code`
- **Behavior**: Resolves city names to IATA codes from a 50+ airport database, generates route-specific flight options using hash-seeded dynamic generation with region-based pricing, real airline names, and realistic durations
- **Output**: `flight_options` dict with 3 options sorted by rating then price, each with airline, flight number, route, departure/arrival times, duration, layovers, per-person and total prices

### Hotel Search Agent
- **Tools**: `search_hotels`, `get_hotel_reviews`
- **Budget Awareness**: Subtracts the cheapest flight cost from the total budget, then allocates 50% of the remaining per-night budget to accommodation — preventing hotel recommendations that would blow the overall budget
- **Output**: `hotel_options` dict with 3 hotels across tiers (luxury/upscale/midrange), each with name, star rating, location, price, amenities, guest rating, and proximity to attractions

### Activity/Itinerary Agent
- **Tools**: `search_activities`, `get_weather_forecast`
- **Behavior**: Fetches destination-specific activities (cultural, adventure, relaxation, food categories for 4 major destinations, generic templates for others), checks weather forecast using climate-profile-based forecasting (tropical/mediterranean/temperate × dry/wet season), then assigns morning/afternoon/evening activities to each day with weather-aware scheduling
- **Output**: `itinerary` dict with day-by-day plan (theme, 3 time slots with activities and costs, weather) and total activity cost

### Budget/Currency Agent
- **Tools**: `calculate_budget`, `convert_currency`
- **Behavior**: Aggregates known costs (flights, hotels, activities), estimates food and transport using region-based daily rates (14 regions covered), adds 5% miscellaneous, computes budget status (WITHIN BUDGET / ON TARGET / OVER BUDGET with ±5% tolerance), converts total to local currency, generates savings tips when over budget
- **Output**: `budget_summary` dict with 6-category breakdown, total, per-person cost, daily spending allowance, currency conversion, and savings tips

---

## Tool Reference

### Search Tools (`src/tools/search_tools.py`)

| Tool | Parameters | Description |
|------|-----------|-------------|
| `web_search` | `query: str` | Wraps `DuckDuckGoSearchResults` from `langchain-community`. Returns up to 5 search results about travel destinations, safety advisories, visa requirements. Falls back gracefully if DuckDuckGo is unavailable. |
| `get_destination_info` | `destination: str` | Returns structured travel data (visa, currency, language, safety, best season, timezone, electricity, emergency numbers) from a curated database of major destinations. Fuzzy-matches destination names. |

### Flight Tools (`src/tools/flight_tools.py`)

| Tool | Parameters | Description |
|------|-----------|-------------|
| `search_flights` | `origin: str, destination: str, travelers: int` | Generates 3 flight options via hash-seeded dynamic generation. Uses a 50+ airport IATA code database, 14-region classification, region-pair base prices (e.g., North America→Southeast Asia: $950), region-specific airlines with quality ratings and price multipliers, realistic duration ranges, and hub-based layover logic. Results are deterministic per route (MD5 hash seeding) and sorted by rating then price. |
| `get_airport_code` | `city: str` | Resolves city names to IATA codes. Covers 50+ cities with fuzzy matching (e.g., "NYC" → JFK, "Hawaii" → HNL). Returns estimated code for unknown cities. |

**Technical Detail**: Flight pricing uses `base_price × airline_price_multiplier × (1 + hash_variation)` where `hash_variation` ranges ±15%. Duration is interpolated within region-pair ranges (e.g., North America→Europe: 7-9h). Layover hubs are selected per destination region (e.g., DXB/SIN/DOH for Indian Ocean).

### Hotel Tools (`src/tools/hotel_tools.py`)

| Tool | Parameters | Description |
|------|-----------|-------------|
| `search_hotels` | `destination: str, nights: int, budget_per_night: float` | Generates 3 hotels across price tiers using 12 real hotel chains in 4 tiers (luxury/upscale/midrange/boutique). Pricing uses region-based multipliers (e.g., Southeast Asia 0.55×, Indian Ocean 1.30×) applied to tier base prices (5★: $350, 4★: $180, 3★: $95/night). Location names are destination-specific (16 destinations with real neighborhood names). |
| `get_hotel_reviews` | `hotel_name: str, destination: str` | Generates 3 guest reviews with hash-seeded ratings and tier-appropriate templates (luxury: "Exceptional luxury experience..."; midrange: "Good value for money..."). Returns overall rating aggregated from individual reviews. |

### Activity Tools (`src/tools/activity_tools.py`)

| Tool | Parameters | Description |
|------|-----------|-------------|
| `search_activities` | `destination: str, preferences: str, num_days: int` | Returns activities organized by type (cultural, adventure, relaxation, food). Covers 4 destinations with hand-crafted activities (Bali: 18, Maldives: 14, Paris: 14, Tokyo: 15) with real attraction names, realistic costs, and durations. Unknown destinations get customized generic activities. Preference mapping boosts relevant categories (e.g., "beach" → adventure + relaxation). |
| `get_weather_forecast` | `destination: str, start_date: str, num_days: int` | Climate-profile-based forecasting. Maps 30+ destinations to 3 climate types (tropical/mediterranean/temperate), determines dry/wet season from travel month, then returns daily forecasts with description, temperature (°C/°F), rain probability, and outdoor-friendly boolean. Hash seeding adds ±2°C temperature variation for realism. |

### Budget Tools (`src/tools/budget_tools.py`)

| Tool | Parameters | Description |
|------|-----------|-------------|
| `calculate_budget` | `budget_usd, travelers, num_days, flight_cost, hotel_cost, activity_cost, destination` | Comprehensive budget calculator. Estimates food ($25-70/person/day) and transport ($8-20/person/day) by region, adds 5% misc. Computes budget status with ±5% tolerance band. Currency conversion uses a 30+ destination database with exchange rates. Generates contextual savings tips when over budget. |
| `convert_currency` | `amount_usd: float, destination: str` | Converts USD to local currency. Covers 30+ destinations/countries with realistic exchange rates (IDR 15,800, JPY 149, EUR 0.92, etc.). Fuzzy-matches destination names. Returns formatted local amount with currency symbol. |

---

## Design Complexity

### Multi-Level Graph Composition
The system uses nested `StateGraph` instances — an outer supervisor graph coordinates 5 inner ReAct sub-graphs. Each sub-graph manages its own tool-calling loop independently, while the outer graph handles sequencing, routing, and state management.

### Deterministic Mock Generation
Rather than hardcoding data for a fixed set of destinations, the system uses **hash-seeded dynamic generation**. Route strings like `"JFK-DPS-0"` are MD5-hashed to produce deterministic but varying prices, durations, and ratings. This means:
- Results are **reproducible** (same input → same output)
- Results are **realistic** (region-based pricing multipliers, real airline/hotel names)
- Results scale to **any destination** without manual data entry

### Budget-Aware Agent Chaining
Agents are not independent — they read each other's outputs:
- **Hotel agent** reads `flight_options` to subtract flight cost from the budget before searching hotels
- **Budget agent** reads all prior agent outputs (flights, hotels, activities) to compute the total
- **Activity agent** reads `travel_request` dates to determine weather season

### Cascade-Based Selective Invalidation
The modification system uses deterministic cascade rules rather than asking the LLM which agents to re-run. This is a deliberate design choice — the LLM determines *what changed* (by merging the modification into the existing request), but the *consequences* of that change (which agents to invalidate) are hardcoded business rules. This makes the system predictable and testable.

### Rich Output Pipeline
The `formatters.py` module transforms raw agent outputs into:
- Markdown summary cards with trip details
- Flight and hotel comparison tables
- Day-by-day itinerary tables with weather
- Budget breakdown tables with savings tips
- Budget pie charts (matplotlib)
- Weather-aware packing suggestions

### Animated Gradio UI
The web app features CSS-animated travel-themed backgrounds (flying airplane, sailing ship, surfer, waves) using SVG data URIs and CSS keyframe animations. The chat interface streams agent progress in real-time, and results are displayed in tabbed output panels. Dynamic edit prompts are generated from the actual trip details after the first plan.

---

## Why LangChain + LangGraph + LangSmith

### LangChain — The Tool & LLM Abstraction Layer

| Feature | How We Use It |
|---------|---------------|
| `@tool` decorator | All 10 tools are defined as `@tool` functions with docstrings that become the tool schema for the LLM |
| `ChatAnthropic` | Primary LLM provider (Claude) with `bind_tools()` for tool-calling |
| `BaseMessage` types | `HumanMessage`, `AIMessage` for typed message passing throughout the graph |
| `DuckDuckGoSearchResults` | Real web search via `langchain-community` — no API key required |
| `ToolNode` | Prebuilt node from `langgraph.prebuilt` that executes tool calls and returns results |
| Provider flexibility | `ChatOllama` available as a local fallback via `config.py` — swap providers by changing one env var |

### LangGraph — The Orchestration Engine

| Feature | How We Use It |
|---------|---------------|
| `StateGraph` | Both the outer supervisor graph and inner ReAct sub-graphs use `StateGraph` with `TypedDict` state |
| Conditional edges | `supervisor_router` returns the next node name based on state — LangGraph routes accordingly |
| `add_messages` reducer | Annotated message list that accumulates messages across nodes instead of overwriting |
| `interrupt()` | Modern human-in-the-loop — pauses the graph, presents data, waits for user input |
| `Command(resume=)` | Resumes graph execution after interrupt with the user's response |
| `MemorySaver` | In-memory checkpointing for multi-turn conversations — each thread_id preserves full state |
| `stream_mode="updates"` | Real-time streaming — the Gradio UI shows which agent is working as the graph executes |
| Graph compilation | `builder.compile(checkpointer=...)` produces a runnable graph with automatic state management |

### LangSmith — Observability & Evaluation

| Feature | How We Use It |
|---------|---------------|
| `LANGSMITH_TRACING=true` | Automatic tracing of all LangChain/LangGraph operations when enabled |
| `@traceable` decorator | 14 decorators across agent nodes, ReAct helpers, and supervisor functions with custom tags (`["agent", "flight"]`, `["react", "destination"]`) and metadata (`{"phase": "planning"}`) |
| Agent-level filtering | Tags enable filtering traces by agent type, phase, or execution path in the LangSmith dashboard |
| Evaluation dataset | 7 diverse scenarios (Bali beach, Maldives luxury, Paris culture, Tokyo food, Santorini romantic, Cancun family, Dubai luxury) in `tests/eval_dataset.json` |
| Custom evaluators | 4 domain-specific evaluators: `budget_accuracy` (cost math), `destination_correctness`, `itinerary_completeness` (day count + AM/PM/EVE slots), `agent_output_completeness` |

**Key Design Choice**: `@traceable` is added to agent node functions and ReAct helpers (plain Python), NOT to `@tool` functions — because LangChain `@tool` functions are already auto-traced when `LANGSMITH_TRACING=true`. Adding `@traceable` to tools would create duplicate spans.

---

## Project Structure

```
src/
  state.py              # TravelPlanState and TravelRequest TypedDict definitions
  supervisor.py         # Intent parsing, routing, aggregation, human review, cascade rules
  graph_builder.py      # Centralized build_graph() — single entry point for all consumers
  app.py                # Gradio web application with streaming, tabs, animated background
  agents/
    destination_agent.py  # ReAct sub-graph with web search + destination info tools
    flight_agent.py       # ReAct sub-graph with flight search + airport code tools
    hotel_agent.py        # ReAct sub-graph with hotel search + review tools
    activity_agent.py     # ReAct sub-graph with activity search + weather tools
    budget_agent.py       # ReAct sub-graph with budget calculator + currency tools
  tools/
    search_tools.py       # web_search (DuckDuckGo), get_destination_info
    flight_tools.py       # search_flights (50+ airports, 14 regions), get_airport_code
    hotel_tools.py        # search_hotels (12 chains, 16 locations), get_hotel_reviews
    activity_tools.py     # search_activities (60+ activities), get_weather_forecast
    budget_tools.py       # calculate_budget (14-region estimates), convert_currency (30+ currencies)
  prompts/
    supervisor_prompt.py  # Intent parser, modification parser, aggregator prompts
    destination_prompt.py # Destination research system prompt
    flight_prompt.py      # Flight search system prompt
    hotel_prompt.py       # Hotel search system prompt
    activity_prompt.py    # Activity/itinerary system prompt
    budget_prompt.py      # Budget analysis system prompt
  utils/
    config.py             # LLM provider configuration (Anthropic / Ollama)
    formatters.py         # Markdown tables, budget pie chart, packing suggestions
notebooks/
  01_foundation_skeleton_graph.ipynb    # Phase 1: Skeleton graph with stub agents
  02_destination_agent_tools.ipynb      # Phase 2: Real destination research with tools
  03_flight_hotel_agents.ipynb          # Phase 3: Flight and hotel agents with mock APIs
  04_itinerary_budget_agents.ipynb      # Phase 4: Activity/itinerary and budget agents
  05_memory_conversation.ipynb          # Phase 5: Multi-turn, human-in-the-loop, cascade
  06_langsmith_observability.ipynb      # Phase 6: LangSmith tracing and evaluation
  07_polish_presentation.ipynb          # Phase 7: Formatters, streaming, final demo
tests/
  test_all_phases.py        # 55 tests: imports, routing, search, flights, hotels, activities, weather, budget
  test_phase5_logic.py      # 46 tests: diff, cascade, describe, keyword fallback, routing, compilation
  test_phase5_multiturn.py  # 17 tests: 3-turn LLM integration (plan → modify → approve)
  test_full_pipeline.py     # Full LLM pipeline: 2 scenarios with 15+ checks each
  eval_dataset.json         # 7 evaluation scenarios for LangSmith
utils/
  decisions/                # 28 architectural decision records (D01–D28)
  plan/                     # Implementation plan
  prompt/                   # Project specification
```

---

## Setup & Installation

### Prerequisites

- **Python 3.12+**
- **[uv](https://docs.astral.sh/uv/)** package manager
- **Anthropic API key** (for Claude LLM)

### Install

```bash
cd 07_BuildAIAgent_LG_LC_LS_AITravelGenie

# Install all dependencies
uv sync

# Create your environment file
cp .env.example .env
# Edit .env and add your API keys
```

### Environment Variables

```bash
# Required — powers all agent reasoning and tool calling
ANTHROPIC_API_KEY=your_anthropic_api_key_here

# Optional — enables LangSmith tracing and evaluation
LANGSMITH_TRACING=true
LANGSMITH_ENDPOINT=https://api.smith.langchain.com
LANGSMITH_API_KEY=your_langsmith_api_key_here
LANGSMITH_PROJECT=your_langsmith_project_here

# Optional — local LLM fallback
OLLAMA_MODEL=llama3.1

# Optional — alternative search provider
TAVILY_API_KEY=your_tavily_api_key_here
```

---

## Usage (How to run)

### Gradio Web App

```bash
uv run python -m src.app
# Open http://localhost:7860
```

The web app provides:
- **Travel Copilot** (left panel): Chat-based interface with streaming agent progress
- **Quick Form** (right panel): Structured form for trip details
- **Example Prompts**: Pre-built trip planning prompts to get started
- **Edit Plan Prompts**: Dynamically generated modification suggestions based on your actual trip details
- **Tabbed Results**: Summary, flights, hotels, itinerary, budget breakdown, budget chart, packing list
- **Animated Background**: CSS-animated travel scene (flying airplane, sailing ship, surfer, ocean waves)

### Notebooks

```bash
uv run jupyter notebook notebooks/
```

Start with `01_foundation_skeleton_graph.ipynb` and progress through each phase. Each notebook is self-contained and demonstrates the incremental development of the system.

### Programmatic

```python
from src.graph_builder import build_graph
from langchain_core.messages import HumanMessage

# Without human review (for batch/eval)
graph = build_graph(include_human_review=False)
result = graph.invoke({
    "messages": [HumanMessage(content=(
        "Plan a 5-day trip to Bali for 2, budget $3000, from NYC. "
        "Dates: 2025-03-15 to 2025-03-20. Preferences: beach, culture."
    ))]
})

# With human review (for interactive use)
from langgraph.checkpoint.memory import MemorySaver
from langgraph.types import Command

graph = build_graph(checkpointer=MemorySaver(), include_human_review=True)
config = {"configurable": {"thread_id": "my-session"}}

# Initial plan
for chunk in graph.stream(
    {"messages": [HumanMessage(content="Plan a trip to Bali...")]},
    config, stream_mode="updates"
):
    print(list(chunk.keys())[0])  # Shows agent progress

# Resume after interrupt
for chunk in graph.stream(
    Command(resume="approve"),
    config, stream_mode="updates"
):
    print(list(chunk.keys())[0])
```

---

## Testing & Evaluation

### Unit Tests (118+ tests, no LLM required)

```bash
# All phases — imports, routing, tools, mock data
uv run python tests/test_all_phases.py          # 55 tests

# Phase 5 logic — diff, cascade, keyword fallback, routing
uv run python tests/test_phase5_logic.py         # 46 tests
```

### Integration Tests (requires Anthropic API key)

```bash
# Multi-turn conversation: plan → modify → approve
uv run python tests/test_phase5_multiturn.py     # 17 tests (3-turn interrupt/resume)

# Full pipeline: 2 scenarios with 15+ checks each
uv run python tests/test_full_pipeline.py
```

### LangSmith Evaluation

```bash
# Requires LANGSMITH_API_KEY
# Run evaluation against 7 diverse scenarios
uv run jupyter notebook notebooks/06_langsmith_observability.ipynb
```

The evaluation dataset (`tests/eval_dataset.json`) covers Bali beach, Maldives luxury, Paris culture, Tokyo food, Santorini romantic, Cancun family, and Dubai luxury — testing across different budgets ($3,000–$10,000), group sizes (1–4), trip lengths (5–10 days), and preference profiles.

---

## Development Phases

| Phase | Focus | Key Deliverable | Notebook |
|-------|-------|-----------------|----------|
| 1 | **Foundation** | Skeleton `StateGraph` with stub agents, `TravelPlanState`, `supervisor_router` | 01 |
| 2 | **Destination Research** | First real agent with ReAct sub-graph, DuckDuckGo web search, structured destination info | 02 |
| 3 | **Flights & Hotels** | Dynamic mock generation with hash-seeded data, 50+ airports, 14 regions, budget-aware hotel search | 03 |
| 4 | **Activities & Budget** | Weather-aware itinerary, climate-profile forecasting, budget calculator with currency conversion | 04 |
| 5 | **Memory & Conversation** | `MemorySaver` checkpointing, `interrupt()`/`Command(resume=)`, cascade-based selective re-planning | 05 |
| 6 | **Observability** | `@traceable` on 14 functions, LangSmith tracing, 4 custom evaluators, 7-scenario eval dataset | 06 |
| 7 | **Polish & Presentation** | Gradio web app with streaming, rich Markdown formatters, budget charts, packing suggestions, animated UI | 07 |

---

## Tech Stack

| Layer | Technology | Role |
|-------|-----------|------|
| **Orchestration** | LangGraph `StateGraph` | Graph-based agent coordination with conditional routing, checkpointing, and interrupt/resume |
| **LLM Framework** | LangChain | Tool abstraction (`@tool`), message types, `ToolNode`, provider integration |
| **Observability** | LangSmith | Distributed tracing, evaluation datasets, custom evaluators, dashboard filtering |
| **Primary LLM** | Claude (Anthropic) | Agent reasoning, tool calling, intent parsing, modification merging |
| **Fallback LLM** | Ollama (local) | Optional local LLM for development without API keys |
| **Web Search** | DuckDuckGo (`langchain-community`) | Free, no-API-key destination research |
| **Web UI** | Gradio | Interactive chat, streaming, tabbed results, animated CSS backgrounds |
| **Visualization** | matplotlib | Budget pie charts |
| **Package Manager** | uv + pyproject.toml | Fast dependency management |
| **Language** | Python 3.12+ | Type hints, `TypedDict`, `Annotated`, union types (`dict \| None`) |
