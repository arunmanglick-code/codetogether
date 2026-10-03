# AI Travel CoPilot - Multi-Agent Vacation Planner
## Full Implementation Prompt (LangChain + LangGraph + LangSmith)

## 0. INSTRUCTIONS FOR CLAUDE

Before writing implementation code, create a **phase-wise development plan** for the AI Travel CoPilot project. Use the phases in Section 5 as the starting point, refine them where necessary, and present the plan before beginning development.

For every phase, include:

- Phase objective and scope
- User-facing capabilities delivered
- Technical tasks broken down into actionable steps
- Files, modules, notebooks, or interfaces to create or modify
- Dependencies and prerequisites from earlier phases
- Expected deliverables
- Tests and validation checks
- Definition of done and exit criteria
- Risks, assumptions, and fallback options
kee
The plan must:

1. Follow a logical dependency order from foundation to production-ready presentation.
2. Identify which work can be done in parallel and which work must be sequential.
3. Keep each phase independently testable and runnable where possible.
4. Include milestones for graph routing, real tools, memory, human review, observability, error handling, and streaming.
5. Map every success criterion in Section 10 to one or more phases and validation checks.
6. End with a consolidated milestone timeline, dependency summary, and recommended implementation order.

Do not start implementation until the phase-wise plan has been presented and its assumptions are clear.

---

## 1. PROJECT VISION

Build **AI Travel CoPilot** - a multi-agent vacation planning system where specialized AI agents collaborate through a LangGraph supervisor architecture to deliver end-to-end personalized travel plans. The system takes a user's vacation request (destination, dates, budget, preferences, group size) and orchestrates multiple domain-specific agents that research, plan, and assemble a comprehensive travel package. 

**Key differentiators:**
- Multi-agent collaboration via LangGraph's state graph with supervisor routing
- Real tool integrations (web search, weather APIs, currency conversion)
- Conversational memory across planning sessions (LangGraph checkpointing)
- Full observability and tracing via LangSmith
- Human-in-the-loop approval gates at critical planning decisions
- Streaming output so users see agents working in real-time

---

## 2. AGENT ARCHITECTURE

### 2.1 Supervisor Agent (Orchestrator)
- **Role**: Central coordinator that receives user requests, decomposes them into sub-tasks, routes to specialist agents, aggregates results, and handles follow-up questions
- **Routing logic**: Uses conditional edges to dispatch to the right specialist based on what information is still needed
- **Responsibilities**:
  - Parse and validate user travel intent
  - Determine which specialist agents to invoke and in what order
  - Aggregate partial results into a cohesive travel plan
  - Handle clarification questions back to the user
  - Decide when the plan is complete

### 2.2 Destination Research Agent
- **Role**: Researches and recommends destinations based on user preferences
- **Tools**:
  - `web_search` - Search for destination information, attractions, safety advisories
  - `get_destination_info` - Retrieve structured data about a destination (visa requirements, language, currency, best time to visit)
- **Output**: Destination profile with key facts, top attractions, travel advisories, and a suitability score

### 2.3 Flight Search Agent
- **Role**: Finds and compares flight options
- **Tools**:
  - `search_flights` - Search flights by origin, destination, dates, passengers, class
  - `get_airport_code` - Resolve city names to IATA airport codes
- **Output**: Top 3 flight options with prices, durations, layovers, and airline ratings

### 2.4 Hotel & Accommodation Agent
- **Role**: Finds suitable accommodation options
- **Tools**:
  - `search_hotels` - Search hotels by destination, dates, guests, budget range, star rating
  - `get_hotel_reviews` - Fetch review summaries for a specific hotel
- **Output**: Top 3 hotel recommendations with prices, ratings, amenities, and location proximity to attractions

### 2.5 Activity & Itinerary Agent
- **Role**: Plans day-by-day activities and sightseeing
- **Tools**:
  - `search_activities` - Search tours, activities, and experiences at destination
  - `get_weather_forecast` - Get weather forecast for the travel dates (to plan outdoor vs indoor activities)
- **Output**: Day-by-day itinerary with morning/afternoon/evening activities, estimated costs, and weather-appropriate suggestions

### 2.6 Budget & Currency Agent
- **Role**: Manages overall trip budget, currency conversions, and cost summaries
- **Tools**:
  - `convert_currency` - Real-time currency conversion
  - `calculate_budget` - Aggregate costs from all agents and compare against user budget
- **Output**: Itemized budget breakdown, currency conversion table, budget status (under/over/on-target), and savings tips

---

## 3. LANGGRAPH STATE DESIGN

```python
from typing import TypedDict, Annotated, Literal
from langgraph.graph.message import add_messages
from langchain_core.messages import BaseMessage

class TravelRequest(TypedDict):
    destination: str           # Target destination (can be "undecided")
    origin: str                # Departure city
    start_date: str            # Trip start date (YYYY-MM-DD)
    end_date: str              # Trip end date (YYYY-MM-DD)
    budget_usd: float          # Total budget in USD
    travelers: int             # Number of travelers
    preferences: list[str]     # e.g. ["beach", "adventure", "family-friendly", "luxury"]
    special_requirements: str  # Dietary, accessibility, etc.

class TravelPlanState(TypedDict):
    messages: Annotated[list[BaseMessage], add_messages]
    travel_request: TravelRequest
    current_agent: str
    destination_research: dict | None
    flight_options: dict | None
    hotel_options: dict | None
    itinerary: dict | None
    budget_summary: dict | None
    plan_status: Literal["gathering_info", "researching", "planning", "reviewing", "complete"]
    human_feedback: str | None
    iteration_count: int
```

---

## 4. LANGGRAPH WORKFLOW

```
                         +------------------+
                         |      START       |
                         +--------+---------+
                                  |
                                  v
                     +------------+------------+
                     |   Intent Parser Node    |
                     |  (Parse user request)   |
                     +------------+------------+
                                  |
                                  v
                     +------------+------------+
                     |   Supervisor Router     |
                     |  (Decide next agent)    |
                     +--+-----+-----+-----+---+
                        |     |     |     |
            +-----------+  +--+  +--+  +--+-----------+
            v              v     v     v              v
    +-------+------+ +----+--+ ++---+ ++---+  +------+-------+
    | Destination  | |Flight | |Hotel| |Act.|  |   Budget     |
    | Research     | |Search | |Search|Itin.|  |   & Currency |
    +--------------+ +-------+ +-----+ +----+  +--------------+
            |              |     |     |              |
            +-----------+--+--+--+--+--+-----------+--+
                        |
                        v
               +--------+---------+
               | Result Aggregator|
               +--------+---------+
                        |
                        v
               +--------+---------+
               |  Human Review    |
               |  (Approval Gate) |
               +--------+---------+
                        |
                  +-----+-----+
                  |           |
                  v           v
            +---------+  +--------+
            | Revise  |  |Complete|
            | (loop)  |  | (END)  |
            +---------+  +--------+
```

**Conditional routing rules in Supervisor:**
1. If `destination_research` is None and destination is "undecided" -> route to Destination Research Agent
2. If `destination_research` is set but `flight_options` is None -> route to Flight Search Agent
3. If `flight_options` is set but `hotel_options` is None -> route to Hotel & Accommodation Agent
4. If `hotel_options` is set but `itinerary` is None -> route to Activity & Itinerary Agent
5. If `itinerary` is set but `budget_summary` is None -> route to Budget & Currency Agent
6. If all sections complete -> route to Result Aggregator
7. After human feedback with revision requests -> route back to relevant specialist

---

## 5. PHASED IMPLEMENTATION PLAN

The following seven phases define the recommended development sequence. Use this outline to produce the detailed phase-wise development plan requested in Section 0. Do not merge phases unless the dependency and validation impact is explicitly explained.

Each phase plan must use this structure:

```text
Phase N: <name>
Objective:
Capabilities:
Technical tasks:
Files and interfaces:
Dependencies:
Deliverables:
Tests and validation:
Definition of done:
Risks and fallback options:
```

### Phase 1: Foundation (Notebook 01)
**"Build the Skeleton Graph"**

- Set up project structure, dependencies (`pyproject.toml` with `uv`)
- Define `TravelPlanState` TypedDict with all fields
- Build the LangGraph `StateGraph` with:
  - Intent Parser node (extracts structured `TravelRequest` from natural language)
  - Supervisor node (placeholder routing logic)
  - Stub nodes for each specialist agent (return mock data)
  - Result Aggregator node
- Wire all edges including conditional routing from Supervisor
- Visualize the graph with `draw_mermaid_png()`
- Test with a sample request: *"Plan a 5-day beach vacation in Bali for 2 people, budget $3000, departing from New York"*
- **LLM**: Use Claude via `ChatAnthropic` (`langchain-anthropic`) with model `claude-sonnet-4-6` for the supervisor and intent parser
- **Deliverable**: A running graph that routes through all agents with mock responses

### Phase 2: Tools & First Real Agent (Notebook 02)
**"Destination Research Agent with Real Tools"**

- Implement `web_search` tool using Tavily or DuckDuckGo search
- Implement `get_destination_info` tool (structured web scraping or API)
- Build the Destination Research Agent with:
  - System prompt for travel research expertise
  - Bound tools
  - LangGraph ToolNode for tool execution
  - ReAct loop (agent reasons, calls tool, reasons again)
- Replace the stub Destination Research node with the real agent sub-graph
- Test: Ask about multiple destinations and verify research quality
- **Deliverable**: Destination agent that actually researches and returns structured findings

### Phase 3: Flight & Hotel Agents (Notebook 03)
**"Search and Compare Travel Options"**

- Implement flight search tool (mock API simulating Amadeus/Skyscanner responses with realistic data)
- Implement hotel search tool (mock API simulating Booking.com responses)
- Implement `get_airport_code` tool
- Implement `get_hotel_reviews` tool
- Build Flight Search Agent and Hotel Agent as sub-graphs with their tools
- Add price comparison and ranking logic within each agent
- Replace stub nodes with real agent sub-graphs
- Test: Full flow from destination research -> flight search -> hotel search
- **Deliverable**: Three agents producing coordinated, realistic travel data

### Phase 4: Itinerary & Budget Agents (Notebook 04)
**"Complete the Planning Pipeline"**

- Implement `search_activities` tool (returns tours, attractions, restaurants)
- Implement `get_weather_forecast` tool (using OpenWeatherMap API or mock)
- Implement `convert_currency` tool (using exchange rate API or mock)
- Implement `calculate_budget` tool (aggregates all costs)
- Build Activity & Itinerary Agent that creates day-by-day plans
- Build Budget & Currency Agent that produces itemized breakdowns
- Wire all agents together in the full graph
- Test: Complete end-to-end flow producing a full travel plan
- **Deliverable**: All 5 specialist agents producing a complete, integrated travel plan

### Phase 5: Memory & Conversation (Notebook 05)
**"Remember and Refine"**

- Add `MemorySaver` checkpointer to the compiled graph
- Implement `thread_id` based conversation memory
- Support multi-turn refinement:
  - User: *"Plan a trip to Bali"* -> Full plan generated
  - User: *"Actually, make it Thailand instead"* -> Supervisor re-routes only to affected agents
  - User: *"Can you find cheaper hotels?"* -> Only Hotel agent re-runs
- Implement selective agent re-invocation (don't redo work that hasn't changed)
- Add human-in-the-loop approval node with `interrupt_before`
- Test: Multi-turn conversation with plan revisions
- **Deliverable**: Stateful, conversational travel planning with selective re-planning

### Phase 6: LangSmith Observability (Notebook 06)
**"Trace, Debug, and Evaluate"**

- Configure LangSmith tracing for the full graph
- Add `@traceable` decorators to key functions
- Set up custom metadata tags per agent (agent_name, tool_name, phase)
- Create a LangSmith dataset with 5-10 diverse travel planning scenarios
- Build evaluation criteria:
  - Does the plan match the budget?
  - Are dates and destination correct?
  - Is the itinerary weather-appropriate?
  - Are all requested preferences addressed?
- Run evaluations and analyze traces
- **Deliverable**: Full observability dashboard showing agent interactions, tool calls, latencies, and quality scores

### Phase 7: Polish & Presentation (Notebook 07)
**"Production-Ready Output"**

- Build a rich output formatter that produces:
  - Trip summary card
  - Day-by-day itinerary with times and costs
  - Flight comparison table
  - Hotel comparison table  
  - Budget pie chart breakdown
  - Packing suggestions based on weather
- Add streaming output so users see partial results as agents work
- Add error handling and graceful fallbacks (agent timeout, tool failure)
- Build an interactive Jupyter widgets interface (or Gradio/Streamlit UI)
- Create a comprehensive README with architecture diagrams
- **Deliverable**: Polished, demo-ready multi-agent travel planner

---

## 6. TECHNOLOGY STACK

| Component | Technology | Purpose |
|-----------|-----------|---------|
| Orchestration | LangGraph (StateGraph, conditional edges, sub-graphs) | Multi-agent workflow |
| LLM Framework | LangChain (prompts, tools, output parsers) | Agent building blocks |
| LLM Provider | Ollama (ChatOllama) with fallback to OpenAI | Chat completions |
| Observability | LangSmith (@traceable, datasets, evaluations) | Tracing & evaluation |
| Web Search | Tavily Search / DuckDuckGo | Destination research |
| Weather | OpenWeatherMap API (or mock) | Itinerary planning |
| Currency | ExchangeRate API (or mock) | Budget calculations |
| Memory | LangGraph MemorySaver (checkpointing) | Conversation state |
| UI | Jupyter ipywidgets / Gradio (optional) | User interaction |
| Package Mgmt | uv + pyproject.toml | Dependencies |
| Python | 3.12+ | Runtime |

---

## 7. SAMPLE INTERACTION FLOW

```
User: "I want to plan a 7-day romantic getaway to the Maldives for 2 people. 
       Budget is $5000. We love snorkeling and spa treatments. 
       Flying from Chicago in March 2025."

[Intent Parser] -> Extracts structured TravelRequest
[Supervisor]    -> Routes to Destination Research (Maldives is specified)
[Destination]   -> Researches Maldives: visa-free for US, best islands, 
                   March weather (dry season, perfect!), safety info
[Supervisor]    -> Routes to Flight Search
[Flights]       -> Finds CHI->MLE options: United via Dubai ($1,200pp), 
                   Emirates direct ($1,500pp), Singapore Air via SIN ($1,100pp)
[Supervisor]    -> Routes to Hotel Search  
[Hotels]        -> Finds: Overwater villa at Centara ($280/night), 
                   Beach bungalow at Adaaran ($180/night), 
                   Budget guesthouse on Maafushi ($90/night)
[Supervisor]    -> Routes to Activity Planner
[Activities]    -> Plans 7 days: snorkeling tours, sunset cruises, 
                   spa days, dolphin watching, local island hopping
[Supervisor]    -> Routes to Budget Agent
[Budget]        -> Total: $4,850 (flights $2,200 + hotel $1,260 + 
                   activities $890 + food $500) - WITHIN BUDGET!
[Aggregator]    -> Assembles complete travel plan
[Human Review]  -> Presents plan for approval

User: "Looks great but can we find a slightly cheaper hotel?"
[Supervisor]    -> Re-routes ONLY to Hotel Agent with lower budget
[Hotels]        -> Finds alternatives with $150/night cap
[Budget]        -> Recalculates: Now $4,200 - more savings!
[Aggregator]    -> Updated plan presented

User: "Can we move the trip to April 10-17 instead?"
[Supervisor]    -> Updates travel dates and re-runs Flight, Hotel, Activity, and Budget agents
[Flights]       -> Searches availability and prices for April 10-17
[Hotels]        -> Checks room availability and seasonal rates for the new dates
[Activities]    -> Adjusts the itinerary and weather-dependent activities
[Budget]        -> Recalculates the total for the updated dates
[Aggregator]    -> Presents the revised plan for approval

User: "Make it a 5-day trip instead of 7 days."
[Supervisor]    -> Updates the duration and routes to Hotel, Activity, and Budget agents
[Hotels]        -> Reprices the stay for 5 days
[Activities]    -> Condenses the itinerary into 5 days while keeping snorkeling and spa treatments
[Budget]        -> Recalculates lodging, activities, food, and the trip total
[Aggregator]    -> Presents the shorter itinerary and updated budget

User: "We have one more traveler joining us."
[Supervisor]    -> Updates travelers from 2 to 3 and re-runs Flight, Hotel, and Budget agents
[Flights]       -> Searches options and prices for 3 passengers
[Hotels]        -> Finds rooms or villas that accommodate 3 guests
[Budget]        -> Recalculates per-person and total costs
[Aggregator]    -> Presents the updated plan for 3 travelers

User: "Actually, change the destination to Bali."
[Supervisor]    -> Re-runs Destination Research and routes the new destination through all dependent agents
[Destination]   -> Researches Bali attractions, travel advisories, weather, and suitability
[Flights]       -> Searches Chicago-to-Bali flight options for the selected dates
[Hotels]        -> Finds accommodation matching the budget and preferences in Bali
[Activities]    -> Creates a new snorkeling, spa, and sightseeing itinerary
[Budget]        -> Recalculates all costs for Bali
[Aggregator]    -> Presents the replacement travel plan for approval

User: "Keep the Maldives, but make the trip more adventurous and add scuba diving."
[Supervisor]    -> Preserves completed destination, flight, and hotel research and re-runs Activity and Budget agents
[Activities]    -> Replaces low-intensity activities with scuba diving, island hopping, and other adventures
[Budget]        -> Adds the new activity costs and checks the total against the budget
[Aggregator]    -> Presents the revised adventure-focused itinerary

User: "Can you find a nonstop flight, even if it costs a little more?"
[Supervisor]    -> Re-runs only the Flight Search Agent with nonstop preferred and a flexible price constraint
[Flights]       -> Searches and ranks nonstop options, showing the price difference and travel time saved
[Budget]        -> Recalculates the total with the selected flight
[Aggregator]    -> Presents the updated flight comparison and trip total

User: "Increase the budget to $6000 and upgrade us to a luxury resort."
[Supervisor]    -> Updates the budget and routes to Hotel and Budget agents
[Hotels]        -> Searches luxury resorts and overwater villas up to the new budget
[Budget]        -> Recalculates the package with the upgraded accommodation
[Aggregator]    -> Presents the upgraded plan and confirms it remains within budget

User: "One of us is vegetarian. Please update the meal recommendations."
[Supervisor]    -> Preserves the travel plan and re-runs the relevant Activity and Budget planning steps
[Activities]    -> Adds vegetarian-friendly restaurants and meal options to each day
[Budget]        -> Updates estimated meal costs if needed
[Aggregator]    -> Presents the plan with dietary requirements included
```

---

## 8. PROJECT STRUCTURE

```
07_BuildAIAgent_LG_LC_LS_AITravelGenie/
├── utils/
│   └── prompt/
│       └── 01_planprompt.md              # This prompt document
├── notebooks/
│   ├── 01_foundation_skeleton_graph.ipynb
│   ├── 02_destination_agent_tools.ipynb
│   ├── 03_flight_hotel_agents.ipynb
│   ├── 04_itinerary_budget_agents.ipynb
│   ├── 05_memory_conversation.ipynb
│   ├── 06_langsmith_observability.ipynb
│   └── 07_polish_presentation.ipynb
├── src/
│   ├── __init__.py
│   ├── state.py                          # TravelPlanState definition
│   ├── supervisor.py                     # Supervisor agent & routing logic
│   ├── agents/
│   │   ├── __init__.py
│   │   ├── destination_agent.py
│   │   ├── flight_agent.py
│   │   ├── hotel_agent.py
│   │   ├── activity_agent.py
│   │   └── budget_agent.py
│   ├── tools/
│   │   ├── __init__.py
│   │   ├── search_tools.py               # web_search, get_destination_info
│   │   ├── flight_tools.py               # search_flights, get_airport_code
│   │   ├── hotel_tools.py                # search_hotels, get_hotel_reviews
│   │   ├── activity_tools.py             # search_activities, get_weather_forecast
│   │   └── budget_tools.py               # convert_currency, calculate_budget
│   ├── prompts/
│   │   ├── supervisor_prompt.py
│   │   ├── destination_prompt.py
│   │   ├── flight_prompt.py
│   │   ├── hotel_prompt.py
│   │   ├── activity_prompt.py
│   │   └── budget_prompt.py
│   └── utils/
│       ├── formatters.py                 # Output formatting
│       └── config.py                     # Environment and model config
├── tests/
│   ├── test_state.py
│   ├── test_tools.py
│   └── test_graph.py
├── images/                               # Architecture diagrams
├── .env.example
├── .gitignore
├── pyproject.toml
└── README.md
```

---

## 9. KEY LANGGRAPH PATTERNS TO DEMONSTRATE

1. **Supervisor pattern** - Central agent routing to specialists via conditional edges
2. **Sub-graphs** - Each specialist agent as its own compiled graph (ReAct loop with ToolNode)
3. **State reducers** - `add_messages` for conversation history, custom reducers for plan sections
4. **Conditional edges** - Dynamic routing based on what's been completed
5. **Human-in-the-loop** - `interrupt_before` for approval gates
6. **Checkpointing** - `MemorySaver` for multi-turn conversations
7. **Streaming** - Stream agent outputs as they work
8. **Error boundaries** - Graceful handling of tool failures and timeouts
9. **Parallel execution** - Fan-out to independent agents (flights and hotels can run simultaneously)
10. **State persistence** - Thread-based conversation management

---

## 10. SUCCESS CRITERIA

- [ ] All 5 specialist agents produce domain-appropriate outputs
- [ ] Supervisor correctly routes based on plan completion state
- [ ] Multi-turn conversation works (modify destination, adjust budget, re-plan)
- [ ] Human-in-the-loop approval gate functions correctly
- [ ] LangSmith traces show full agent interaction chain
- [ ] Budget calculations are accurate and currency conversions work
- [ ] Day-by-day itinerary accounts for weather and preferences
- [ ] Graph visualization clearly shows the multi-agent architecture
- [ ] Error handling covers tool failures without crashing the graph
- [ ] Output is well-formatted and presentation-ready
