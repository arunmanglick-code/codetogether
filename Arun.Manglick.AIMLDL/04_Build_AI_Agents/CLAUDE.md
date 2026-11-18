# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This is a learning and portfolio monorepo for building AI agents. Each numbered subdirectory is an independent project exploring a different framework, pattern, or agent architecture. Projects are not interdependent — they share a git history but have separate dependencies, virtual environments, and configuration.

## Project Map

| Directory | Framework | What It Builds |
|-----------|-----------|----------------|
| `01_AgentBuild_Using_CrewAI` | CrewAI + Ollama/OpenAI | Multi-agent orchestration with tools, MCP servers, and Graphiti/Neo4j memory |
| `02_Graphiti` | Graphiti (Zep) | Cloned Graphiti repo for reference — knowledge graph engine |
| `03_BuildAgents_LangGraph` | LangGraph + Ollama | Progressive notebook series: state graphs → conditional routing → tools → memory → LangSmith |
| `04_BuildMultiAgentSystem` | Raw Claude API (httpx) | Planner/Orchestrator/Worker/Tool-Using agents coordinated without a framework |
| `06_BuildAIAgents_N8N` | N8N (no-code) | Workflow JSON exports — RAG, webhooks, AI agents in N8N |
| `07_BuildAIAgent_LG_LC_LS_AITravelGenie` | LangChain + LangGraph + LangSmith | Production-grade 5-agent travel planner with Gradio UI, human-in-the-loop, cascade re-planning |
| `08_BuildRAGAgent_LangFamily` | (new, in progress) | RAG agent using the LangChain/LangGraph family |

## Common Commands

Each project manages its own dependencies via `uv` (preferred) or `pip`. Always `cd` into the project directory first.

```powershell
# Install dependencies (most projects)
uv sync

# Run Jupyter notebooks (projects 01, 03, 07)
uv run jupyter notebook

# Run the Travel Genie Gradio app (project 07)
uv run python -m src.app        # → http://localhost:7860

# Unit tests — project 07
uv run python tests/test_all_phases.py       # 55 tests, no LLM needed
uv run python tests/test_phase5_logic.py     # 46 tests, no LLM needed

# Integration tests — project 07 (requires ANTHROPIC_API_KEY)
uv run python tests/test_phase5_multiturn.py
uv run python tests/test_full_pipeline.py
```

## Architecture Patterns Across Projects

**ReAct sub-graphs (projects 03, 07):** LLM calls tools in a loop — `should_continue` checks for `tool_calls` on the last message. Each agent is a self-contained `StateGraph` with its own tool set, nested inside an outer supervisor graph.

**State-driven routing (project 07):** A flat `TypedDict` state has `dict | None` fields per agent. The supervisor checks which fields are `None` and routes to the corresponding agent — no LLM needed for routing decisions.

**Cascade invalidation (project 07):** When a user modifies a travel request, deterministic rules (not LLM) decide which agents must re-run based on which fields changed.

**Mock fallback (project 07):** Every agent has a primary path (LLM + tools) and a fallback path (direct tool invocation with mock data) so the system works without API keys.

## Environment & Configuration

- **Python 3.12+** (project 03 uses 3.14)
- **Package manager:** `uv` with `pyproject.toml` per project
- **LLM providers:** Anthropic Claude (primary), Ollama local models (fallback), OpenAI (some projects)
- **API keys** go in `.env` files per project — never commit them
- **LangSmith tracing** is opt-in via `LANGSMITH_TRACING=true` in `.env`

## Key Design Decisions

- `@traceable` decorators go on agent node functions and ReAct helpers, NOT on `@tool` functions (LangChain auto-traces tools when LangSmith is enabled)
- Tools use hash-seeded deterministic mock generation (MD5 of route string) so results are reproducible without external APIs
- Human-in-the-loop uses LangGraph's `interrupt()` / `Command(resume=)` pattern, not the older `input()` approach
- Budget-aware agent chaining: later agents (hotel, budget) read earlier agent outputs from shared state
