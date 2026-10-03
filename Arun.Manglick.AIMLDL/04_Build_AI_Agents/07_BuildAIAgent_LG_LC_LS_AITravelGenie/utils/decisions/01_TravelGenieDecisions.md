# AI Travel Genie — Decisions Log

This document tracks all architectural, design, and implementation decisions made during planning and development of the AI Travel Genie project.

---

## D01: LLM Provider — Ollama (ChatOllama) as Primary

**Decision**: Use Ollama with `ChatOllama` as the primary LLM provider.

**Rationale**: Consistent with sibling projects in the repository. Free, runs locally, no API key required for basic usage. Privacy-friendly — no data leaves the machine.

**Trade-off**: Local models have weaker tool-calling ability than GPT-4o or Claude. If tool-calling quality is poor with the default model, we will document which Ollama models work best (e.g., `llama3.1`, `mistral`, `qwen2.5`).

**Fallback**: OpenAI (`ChatOpenAI` with GPT-4o) can be configured via `.env` as a fallback if Ollama tool-calling proves unreliable.

---

## D02: Mock APIs for Flight, Hotel, and Activity Data

**Decision**: Use hardcoded but realistic mock APIs instead of real third-party APIs for flights, hotels, and activities.

**Rationale**: Real APIs (Amadeus, Booking.com, Skyscanner) require paid subscriptions, complex auth, and rate limits. Hardcoded mocks provide deterministic, debuggable results while still demonstrating the multi-agent architecture. Data uses real airline names, real hotel chains, and plausible prices seeded by route/destination.

**Trade-off**: Not suitable for production. But the architecture supports swapping in real APIs later — tools are defined as LangChain `@tool` functions with a clean interface.

---

## D03: Sequential Phase Execution

**Decision**: Implement all 7 phases sequentially, each reviewed before proceeding to the next.

**Rationale**: Each phase builds on the prior. Sequential execution allows catching issues early (e.g., state design problems in Phase 1 before building 5 real agents on top). The user reviews and tests each notebook before the next phase begins.

**Trade-off**: Slower than implementing everything at once, but significantly reduces rework risk.

---

## D04: One Notebook Per Phase

**Decision**: Each phase produces one Jupyter notebook (`notebooks/01_*.ipynb` through `07_*.ipynb`) that exercises the new functionality.

**Rationale**: Matches the convention in sibling projects (e.g., `03_BuildAgents_LangGraph/01_Fundamental_langGraphAgent.ipynb`). Notebooks provide interactive, visual, step-by-step demonstrations ideal for learning and presentation.

**Trade-off**: Notebooks are harder to test in CI than Python scripts. Tests in `tests/` directory supplement notebooks for automated validation.

---

## D05: Shared `src/` Module Structure

**Decision**: Production code lives in `src/` with sub-packages (`agents/`, `tools/`, `prompts/`, `utils/`). Notebooks import from `src/`.

**Rationale**: Keeps notebooks clean (orchestration and demos only) while making code reusable and testable. Follows the project structure defined in the spec (Section 8).

**Trade-off**: Requires adding `src/` to the Python path in notebooks (e.g., `sys.path.insert`).

---

## D06: State Design — Flat TypedDict with Optional Fields

**Decision**: Use a flat `TravelPlanState` TypedDict where agent result fields (`destination_research`, `flight_options`, etc.) are `dict | None`. Supervisor routing checks for `None` to decide the next agent.

**Rationale**: Simplest approach that maps directly to the spec's conditional routing rules. LangGraph's `add_messages` reducer handles message accumulation. No need for nested state machines at this stage.

**Trade-off**: Selective re-invocation (Phase 5) will need change-tracking logic added to state. We'll extend the state then rather than over-engineering now.

---

## D07: Tavily for Web Search

**Decision**: Use Tavily Search as the web search tool for the Destination Research Agent.

**Rationale**: Tavily provides a purpose-built search API for AI agents with structured results. LangChain has first-class `TavilySearchResults` integration. Free tier available.

**Fallback**: DuckDuckGo search (`DuckDuckGoSearchResults`) as a zero-cost alternative if Tavily API key is unavailable.

---

## D08: Weather and Currency — Mock with Real API Option

**Decision**: Start with mock implementations for weather forecasts and currency conversion. Provide configuration to swap in real APIs (OpenWeatherMap, ExchangeRate-API).

**Rationale**: Reduces external dependencies and API key requirements during development. Mock data is deterministic and easier to test. Architecture supports real APIs via the same tool interface.

---

## D09: Human-in-the-Loop via `interrupt_before`

**Decision**: Use LangGraph's `interrupt_before` on the human review node for the approval gate.

**Rationale**: Native LangGraph pattern. Pauses graph execution, presents the aggregated plan, and waits for user input (approve/revise). Integrates cleanly with checkpointing for session persistence.

---

## D10: Package Management — `uv` with `pyproject.toml`

**Decision**: Use `uv` as the package manager with `pyproject.toml` for dependency specification.

**Rationale**: Consistent with sibling project (`01_AgentBuild_Using_CrewAI/pyproject.toml`). `uv` is fast and handles virtual environments automatically.

---

## D11: Graph Visualization — `draw_mermaid_png()`

**Decision**: Use LangGraph's built-in `graph.get_graph().draw_mermaid_png()` for architecture visualization.

**Rationale**: Zero additional dependencies. Produces clear, accurate diagrams of the actual compiled graph. Supplements the hand-crafted Mermaid diagram in `utils/images/travel_genie_design.mmd`.

---

## D12: Python Version — 3.14

**Decision**: Target Python 3.14 (matching the user's installed version).

**Rationale**: User's environment has Python 3.14 installed (visible from sibling notebook outputs). Note: LangChain shows Pydantic V1 compatibility warnings on 3.14 — these are non-blocking.

---

## D13: LLM Provider Changed — Claude (Anthropic) as Primary (Phase 2)

**Decision**: Switch from Ollama (`ChatOllama`) to Claude (`ChatAnthropic`) as the primary LLM provider.

**Rationale**: User has a Claude license and prefers the Claude model family. Claude has superior tool-calling capabilities compared to local Ollama models, which is critical for the ReAct agent pattern in Phase 2+.

**Trade-off**: Requires an Anthropic API key and incurs API costs. A mock fallback is provided for when the API is unreachable, so the code remains runnable without a valid key.

**Impact**: Updated `src/utils/config.py` to use `ChatAnthropic`. Added `langchain-anthropic` to `pyproject.toml`. Ollama remains available as a fallback option via config.

---

## D14: Web Search — DuckDuckGo Instead of Tavily (Phase 2)

**Decision**: Use DuckDuckGo search (`DuckDuckGoSearchResults`) instead of Tavily for the Destination Research Agent's web search tool.

**Rationale**: User does not have a Tavily API key. DuckDuckGo is free, requires no API key, and is available immediately via `langchain-community`.

**Trade-off**: DuckDuckGo results are less structured than Tavily's AI-optimized search results. Sufficient for destination research demos. Can be swapped to Tavily later by setting `TAVILY_API_KEY` in `.env`.

---

## D15: Mock Fallback for All Real Agents (Phase 2+)

**Decision**: Every real agent implementation includes a fallback to mock data if the LLM API call fails.

**Rationale**: Ensures the graph always completes, even during API outages or key issues. Makes development and testing smoother.

---

## D16: Dynamic Mock Generation Over Expanded Hardcoded Data (Phase 3)

**Decision**: Use hash-seeded dynamic generation for flight and hotel mock APIs rather than expanding hardcoded data to cover more destinations.

**Rationale**: User chose "Dynamic generation" when asked. This approach generates plausible data for ANY destination using route-based logic: hash seeding produces deterministic results per route/destination while varying realistically. Region-based pricing multipliers (e.g., Southeast Asia 0.55x, Indian Ocean 1.30x) ensure price realism.

**Trade-off**: Generated data won't match real-world flight routes exactly (e.g., layover hubs are region-based approximations). But it scales to unlimited destinations without manual data entry.

**Impact**: `src/tools/flight_tools.py` covers 50+ airports, 14 regions, real airline names. `src/tools/hotel_tools.py` uses 3 hotel tiers (luxury/upscale/midrange) with region-aware pricing.

---

## D17: Hotel Budget Awareness From Prior Agent Results (Phase 3)

**Decision**: The hotel agent subtracts the cheapest flight's total cost from the overall budget to derive a realistic per-night hotel budget.

**Rationale**: In a sequential pipeline, the hotel agent runs after the flight agent. Using flight cost data prevents the hotel agent from recommending options that would blow the total budget. Formula: `budget_per_night = (budget - flight_cost) / nights / travelers * 0.5` (50% of remaining budget allocated to accommodation, rest for activities/food/transport).

**Trade-off**: Couples the hotel agent's behavior to the flight agent's output format. Acceptable since both are defined in the same project.

---

## D18: ReAct Pattern for All Tool-Using Agents (Phase 2-3)

**Decision**: All tool-using agents (destination, flight, hotel) use the same ReAct sub-graph pattern: internal `StateGraph` with `AgentState`, `agent_node`, `should_continue`, and `ToolNode`.

**Rationale**: Consistent architecture across all agents. Each sub-graph handles its own reasoning loop independently. The outer supervisor graph sees each agent as a single node that takes state in and returns updated state.

**Trade-off**: Some code duplication in the sub-graph setup. Could be extracted into a factory function, but keeping it explicit per agent makes each agent self-contained and easier to customize.

---

## D19: Climate-Aware Weather Forecasting (Phase 4)

**Decision**: Use a climate-profile-based weather forecast system rather than calling a real weather API.

**Rationale**: Real weather APIs (OpenWeatherMap) require API keys and only forecast 5-7 days ahead. For trip planning months in advance, historical climate profiles are more useful. The system categorizes destinations into 3 climate types (tropical, mediterranean, temperate) with dry/wet season profiles. Month-based season detection selects appropriate weather patterns.

**Trade-off**: Not real-time weather data. But for vacation planning (typically booked weeks/months ahead), climate-based forecasts are more accurate than attempting to predict exact weather.

---

## D20: Region-Based Cost Estimation for Food and Transport (Phase 4)

**Decision**: The budget tool estimates food and local transport costs using region-based daily rates rather than destination-specific data.

**Rationale**: Per-destination food/transport cost data would require maintaining a large database. Region-based estimates (e.g., Southeast Asia $30/day/person food, Europe $60/day/person) provide reasonable approximations. The rates are derived from travel cost indexes and adjusted by region.

**Trade-off**: Less precise than destination-specific data. A budget trip to Tokyo vs. rural Japan would have very different costs. Sufficient for trip planning estimates; the LLM analysis can add nuance.

---

## D21: Structured Mock Data Alongside LLM Analysis (Phase 4)

**Decision**: Both the activity and budget agents produce structured data from tools (for the aggregator) AND capture the LLM's narrative analysis. The structured data drives the plan output; the LLM analysis provides context and recommendations.

**Rationale**: The aggregator needs consistent field names and data types to format the final plan. The LLM's free-form analysis is valuable but unpredictable in structure. By running tools for structured output and keeping the LLM response as supplementary context, both needs are met.

---

## D22: `interrupt()` Over `interrupt_before` for Human-in-the-Loop (Phase 5)

**Decision**: Use the `interrupt()` function from `langgraph.types` (available in langgraph 1.2.12) rather than the `interrupt_before` compile parameter.

**Rationale**: `interrupt()` is the modern LangGraph pattern. It allows the `human_review` node to both present data to the user AND receive the response in a single function, with `Command(resume=...)` for resumption. The older `interrupt_before` pattern requires sending state updates via `graph.invoke({"messages": [...]})` which is less clean for this use case.

**Trade-off**: Requires langgraph >= 0.2.28 (we have 1.2.12). The `interrupt()` function re-executes the node on resume, so the node must be idempotent up to the interrupt call.

---

## D23: Deterministic Cascade Rules Over LLM-Determined Invalidation (Phase 5)

**Decision**: The cascade from field changes to agent invalidation is hardcoded in `_compute_cascade()`, not determined by the LLM.

**Rationale**: The LLM only determines which fields of TravelRequest changed (by producing a merged request). The mapping from "destination changed" → "re-run all agents" is a deterministic business rule. This prevents the LLM from accidentally skipping or over-including agents in the re-run set.

**Trade-off**: Less flexible than an LLM-driven approach (e.g., "find cheaper flights" can't skip destination research without a destination change). But predictable and testable — the cascade rules are unit-tested independently.

---

## D24: `parse_intent` Nulls Fields Directly (Phase 5)

**Decision**: `parse_intent` returns the nulled output fields directly in its return dict (e.g., `"hotel_options": None`) rather than using a separate `apply_invalidation` node.

**Rationale**: LangGraph TypedDict state uses overwrite semantics for fields without reducers. Returning `None` for a `dict | None` field sets it to None. The existing `supervisor_router` then naturally routes to the agent whose field is None. This keeps the graph topology simpler — no extra node needed.

---

## D25: `@traceable` on Agent Nodes + ReAct Helpers, Not Tools (Phase 6)

**Decision**: Add `@traceable` decorators to agent node functions and their internal `_run_react_agent` helpers (10 decorators across 5 agents + 4 on supervisor functions). Do NOT decorate `@tool` functions.

**Rationale**: LangChain `@tool` functions are automatically traced when `LANGSMITH_TRACING=true` — adding `@traceable` would create duplicate spans. The agent node functions and ReAct helpers are plain Python functions that LangGraph does not auto-trace with custom metadata. `@traceable` adds agent-specific tags (`["agent", "flight"]`, `["react", "destination"]`) and metadata (`{"phase": "planning"}`) that enable filtering and grouping in the LangSmith dashboard.

**Trade-off**: Slightly more verbose agent files. But the observability payoff is significant — you can filter traces by agent type, phase, or ReAct vs. orchestration.

---

## D26: Custom Evaluators Over LangSmith Auto-Eval (Phase 6)

**Decision**: Define 4 custom evaluator functions (`budget_accuracy`, `destination_correctness`, `itinerary_completeness`, `agent_output_completeness`) rather than relying on LangSmith's built-in LLM-based evaluation.

**Rationale**: Domain-specific quality checks produce deterministic, interpretable scores. `budget_accuracy` verifies component costs sum to total. `itinerary_completeness` checks day count and AM/PM/EVE slot structure. These are structural correctness checks that don't need LLM judgment — a rule-based evaluator is faster, cheaper, and more reliable.

**Trade-off**: Cannot evaluate subjective quality (e.g., "are the hotel recommendations culturally appropriate?"). LLM-based evaluation could supplement these checks for subjective criteria in Phase 7.

---

## D27: Both Gradio App + Notebook for Phase 7 UI (Phase 7)

**Decision**: Build both a standalone Gradio web app (`src/app.py`) and a demonstration notebook (`notebooks/07_polish_presentation.ipynb`).

**Rationale**: User chose "Both" when asked. Gradio provides a polished, shareable demo with streaming progress, tabbed results, and human-in-the-loop review — ideal for presentations. The notebook demonstrates the formatter APIs and streaming internals — ideal for learning and development.

**Trade-off**: More code to maintain (app.py + notebook). But they share the same `formatters.py` module and `graph_builder.py`, so the duplication is minimal — only the UI layer differs.

---

## D28: Centralized `graph_builder.py` Over Inline Graph Construction (Phase 7)

**Decision**: Extract the 20+ lines of graph wiring (nodes, edges, conditional edges, checkpointer) into `src/graph_builder.py` with a single `build_graph(checkpointer, include_human_review)` function.

**Rationale**: Six notebooks, four test files, and the Gradio app all construct the same graph. A single entry point reduces duplication and ensures consistency. The `include_human_review` parameter handles the two graph variants (full HIL graph vs. eval/batch graph without interrupt).

**Trade-off**: Adds a level of indirection. But the function is short, well-typed, and follows the same pattern all consumers were already using inline.

---

## Future Decisions

- **D29**: LangSmith evaluation scoring thresholds (to be calibrated after initial eval runs)
