# AI Travel Genie — Implementation Plan

## Context

Build **AI Travel Genie**, a multi-agent vacation planner using LangChain + LangGraph + LangSmith. Five specialist agents (Destination, Flight, Hotel, Activity, Budget) are orchestrated by a LangGraph supervisor to produce end-to-end travel plans. The project is greenfield — only design docs exist today in `utils/prompt/` and `utils/images/`.

Source specification: `utils/prompt/01_planprompt.md`

**Conventions** (from sibling projects): Python 3.14, `uv` + `pyproject.toml`, Jupyter notebooks, LangGraph `StateGraph` with `TypedDict`, Ollama (`ChatOllama`).

**Implementation pace**: Phase-by-phase. Each phase produces a notebook + `src/` modules. Review and test before proceeding to the next.

---

## Phase 1: Foundation — Skeleton Graph
**Notebook**: `notebooks/01_foundation_skeleton_graph.ipynb`

**Objective**: Set up project structure, define `TravelPlanState`, build the full `StateGraph` with stub agents returning mock data, and verify end-to-end routing.

**Files to create**:
- `pyproject.toml` — deps: `langgraph`, `langchain`, `langchain-ollama`, `langchain-community`, `langsmith`, `python-dotenv`, `tavily-python`, `ipywidgets`
- `.env.example`, `.gitignore`
- `src/__init__.py`, `src/state.py` — `TravelRequest` + `TravelPlanState` TypedDicts
- `src/supervisor.py` — intent parser node, supervisor router (conditional edges), result aggregator node
- `src/utils/__init__.py`, `src/utils/config.py` — LLM init helper, env loading
- `src/agents/__init__.py` + 5 stub agent files (`destination_agent.py`, `flight_agent.py`, `hotel_agent.py`, `activity_agent.py`, `budget_agent.py`) — each returns realistic mock data
- `src/prompts/` — system prompts for supervisor and each specialist (6 files)
- `src/tools/__init__.py`
- `notebooks/01_foundation_skeleton_graph.ipynb`

**Key details**:
- Intent parser uses `ChatOllama` to extract structured `TravelRequest` from natural language
- Supervisor routing: checks which state fields are `None`, routes to the next needed agent
- Stubs return hardcoded but realistic data (real airline names, plausible prices, real attractions)
- Graph: START → Intent Parser → Supervisor → [agents in sequence] → Aggregator → END
- Visualize with `graph.get_graph().draw_mermaid_png()`

**Test**: Run with *"Plan a 5-day beach vacation in Bali for 2 people, budget $3000, departing from New York"*

**Done when**: Graph routes through all 5 stubs in correct order, visualization renders, mock plan is complete.

---

## Phase 2: Destination Research Agent with Real Tools
**Notebook**: `notebooks/02_destination_agent_tools.ipynb`

**Objective**: Replace the stub destination agent with a real LLM-powered agent using Tavily web search and a ReAct loop via `ToolNode`.

**Files to create/modify**:
- Create `src/tools/search_tools.py` — `web_search` (Tavily wrapper), `get_destination_info` (structured data tool)
- Rewrite `src/agents/destination_agent.py` — LangGraph sub-graph with ReAct loop, tools bound to `ChatOllama`, `ToolNode` for execution
- Modify `src/supervisor.py` — wire sub-graph in place of stub
- Create `notebooks/02_destination_agent_tools.ipynb`

**Key details**:
- ReAct pattern: agent reasons → calls tool → observes result → reasons again → final answer
- `ToolNode` handles tool execution within the sub-graph
- Structured output: destination profile with attractions, visa info, weather, safety, suitability score

**Depends on**: Phase 1. Tavily API key in `.env`.

**Test**: Research 3 different destinations (Paris, Bali, lesser-known location). Verify tool calls visible and output structured.

**Done when**: Agent researches real destinations and returns structured findings via tool calls.

---

## Phase 3: Flight & Hotel Agents
**Notebook**: `notebooks/03_flight_hotel_agents.ipynb`

**Objective**: Build Flight Search and Hotel agents with realistic mock APIs and tool-based reasoning.

**Files to create/modify**:
- Create `src/tools/flight_tools.py` — `search_flights` (mock API, realistic data seeded by route), `get_airport_code` (city → IATA lookup)
- Create `src/tools/hotel_tools.py` — `search_hotels` (mock API, filtered by budget/stars/dates), `get_hotel_reviews` (summary reviews)
- Rewrite `src/agents/flight_agent.py` — sub-graph with ReAct loop, ranks top 3 flights
- Rewrite `src/agents/hotel_agent.py` — sub-graph with ReAct loop, ranks top 3 hotels
- Modify `src/supervisor.py` — wire both sub-graphs
- Create `notebooks/03_flight_hotel_agents.ipynb`

**Key details**:
- Mock flight data: real airlines (United, Emirates, Singapore Air), plausible prices varying by route, realistic durations and layover counts
- Mock hotel data: real hotel chains, star ratings, amenities, proximity scores
- Each agent produces a ranked comparison (top 3)

**Depends on**: Phase 2.

**Test**: Full flow: destination research → flight search → hotel search. Verify ranking logic and budget filtering.

**Done when**: 3-agent pipeline produces coordinated, realistic travel data.

---

## Phase 4: Itinerary & Budget Agents — Complete Pipeline
**Notebook**: `notebooks/04_itinerary_budget_agents.ipynb`

**Objective**: Complete the 5-agent pipeline with Activity/Itinerary and Budget/Currency agents.

**Files to create/modify**:
- Create `src/tools/activity_tools.py` — `search_activities` (tours, attractions, restaurants), `get_weather_forecast` (OpenWeatherMap API or mock)
- Create `src/tools/budget_tools.py` — `convert_currency` (exchange rate API or mock), `calculate_budget` (aggregates all costs)
- Rewrite `src/agents/activity_agent.py` — day-by-day itinerary with morning/afternoon/evening slots, weather-appropriate
- Rewrite `src/agents/budget_agent.py` — itemized breakdown, budget status (under/over/on-target), savings tips
- Modify `src/supervisor.py` — wire remaining sub-graphs
- Create `notebooks/04_itinerary_budget_agents.ipynb`

**Depends on**: Phase 3.

**Test**: Full end-to-end with the Maldives sample request from Section 7 of the spec. Verify budget math, weather-appropriate activities, currency conversion.

**Done when**: All 5 agents produce an integrated, complete travel plan. Budget calculation is accurate.

---

## Phase 5: Memory & Conversation
**Notebook**: `notebooks/05_memory_conversation.ipynb`

**Objective**: Add `MemorySaver` checkpointing for multi-turn conversations, selective agent re-invocation, and human-in-the-loop approval gates.

**Files to modify**:
- `src/supervisor.py` — add human review node, selective routing logic (detect changed fields, re-run only affected agents), `interrupt_before` configuration
- `src/state.py` — add change-tracking fields if needed
- Create `notebooks/05_memory_conversation.ipynb`

**Key details**:
- `MemorySaver` checkpointer on compiled graph
- `config={"configurable": {"thread_id": "..."}}`
- Selective re-invocation rules:
  - Destination change → cascades to all 5 agents
  - Date change → Flight + Hotel + Activity + Budget
  - Budget/hotel preference change → Hotel + Budget
  - Activity preference change → Activity + Budget
- Human review: `interrupt_before=["human_review"]`, user can approve or revise with feedback

**Depends on**: Phase 4.

**Test**: Multi-turn conversation flow from Section 7:
1. Initial Maldives request → full plan
2. "Find cheaper hotels" → only Hotel + Budget re-run
3. "Change dates to April" → Flight + Hotel + Activity + Budget re-run
4. "Change destination to Bali" → all agents re-run
5. Human approval gate blocks and resumes correctly

**Done when**: 5+ turn conversation with selective re-planning works. Thread state persists.

---

## Phase 6: LangSmith Observability
**Notebook**: `notebooks/06_langsmith_observability.ipynb`

**Objective**: Full LangSmith tracing with custom metadata, evaluation datasets, and quality scoring.

**Files to modify**:
- Add `@traceable` decorators to functions in `src/agents/*.py` and `src/tools/*.py`
- Add custom metadata (`run_name`, `tags`) per agent
- Create `tests/eval_dataset.json` — 5-10 diverse travel scenarios
- Create `notebooks/06_langsmith_observability.ipynb`

**Key details**:
- Env vars: `LANGSMITH_TRACING=true`, `LANGSMITH_API_KEY`, `LANGSMITH_PROJECT="ai-travel-genie"`
- Evaluation criteria: budget accuracy, date/destination correctness, weather-appropriate itinerary, preference coverage

**Depends on**: Phase 5. LangSmith API key.

**Test**: Run evaluation suite, verify traces visible in LangSmith dashboard with correct metadata.

**Done when**: Full traces with agent/tool metadata visible. Evaluation runs and produces quality scores.

---

## Phase 7: Polish & Presentation
**Notebook**: `notebooks/07_polish_presentation.ipynb`

**Objective**: Production-quality output, streaming, error handling, optional UI, README.

**Files to create/modify**:
- Create `src/utils/formatters.py` — trip summary card, itinerary table, flight/hotel comparison tables, budget breakdown, packing suggestions
- Modify `src/supervisor.py` — streaming via `graph.stream()`, error handling (tool failure fallbacks, agent timeouts)
- Create `notebooks/07_polish_presentation.ipynb` — interactive UI with Jupyter widgets or Gradio
- Create `README.md` — overview, architecture diagram, setup, usage

**Depends on**: Phase 6.

**Test**: Formatted output is clean and demo-ready. Streaming shows real-time agent progress. Graph survives tool failures gracefully.

**Done when**: Polished, demo-ready travel planner with rich output and error resilience.

---

## Success Criteria → Phase Mapping

| Criterion | Phase |
|-----------|-------|
| All 5 agents produce domain-appropriate outputs | P1 stubs, P2-P4 real |
| Supervisor routes correctly based on completion state | P1 |
| Multi-turn conversation works | P5 |
| Human-in-the-loop approval gate | P5 |
| LangSmith traces show full chain | P6 |
| Budget calculations accurate, currency conversion works | P4 |
| Itinerary accounts for weather and preferences | P4 |
| Graph visualization shows architecture | P1 |
| Error handling covers tool failures | P7 |
| Output well-formatted and presentation-ready | P7 |

---

## Implementation Order

All phases are **sequential** — each builds on the prior:

```
P1 (Foundation) → P2 (Destination) → P3 (Flight+Hotel) → P4 (Activity+Budget)
→ P5 (Memory) → P6 (LangSmith) → P7 (Polish)
```

Within each phase: create tools first, then agents, then wire into graph, then build notebook.

---

## Verification (per phase)

After each phase:
1. Run `uv sync` to confirm dependencies
2. Run the phase's notebook end-to-end
3. Verify graph visualization renders
4. Confirm expected output structure and correctness
5. Move to next phase only after review
