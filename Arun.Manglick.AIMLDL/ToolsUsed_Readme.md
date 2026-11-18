# AI/ML Tools Catalog — 46 Tools Across 13 Categories

---

## AI/ML Frameworks (7)

| Tool | Description | Used In |
|------|-------------|---------|
| **CrewAI** | Open-source Python framework for building and orchestrating multi-agent AI systems with modular agent design | 01_CrewAI |
| **LangChain** | LLM framework providing tool abstraction (`@tool`), message types, provider integration, and ToolNode for agent building | 03_LangGraph, 07_TravelGenie |
| **LangGraph** | Graph-based agent orchestration engine with StateGraph, conditional routing, checkpointing, and interrupt/resume for stateful workflows | 03_LangGraph, 07_TravelGenie |
| **Claude API (Anthropic)** | Cloud LLM provider powering agent reasoning, tool calling, and intent parsing via the Anthropic SDK | 04_MultiAgent, 07_TravelGenie |
| **OpenAI API** | Cloud LLM provider used for agent reasoning and as the LLM backend for Graphiti MCP Server operations | 01_CrewAI, 02_Graphiti |
| **Ollama** | Platform for running LLMs locally (LLaMA, Mistral, Gemma, Phi) with no cloud dependency, accessible at localhost:11434 | 01_CrewAI, 03_LangGraph, 07_TravelGenie |
| **ChatOllama** | LangChain integration for Ollama that wraps local models as a ChatModel with bind_tools() support | 03_LangGraph |

## Search & Retrieval (4)

| Tool | Description | Used In |
|------|-------------|---------|
| **Tavily** | AI-optimized web search API designed for LLM agents, used as an alternative search provider for destination research | 07_TravelGenie |
| **DuckDuckGo Search** | Free, no-API-key web search via langchain-community's DuckDuckGoSearchResults for destination research | 07_TravelGenie |
| **Linkup** | AI search engine optimized for LLMs and agents, offering fast and accurate results through the LinkupSearchTool and SDK | 01_CrewAI |
| **Pinecone** | Cloud-hosted vector database used for RAG (Retrieval-Augmented Generation) pipelines and semantic search | 06_N8N |

## Memory & Knowledge Graphs (3)

| Tool | Description | Used In |
|------|-------------|---------|
| **Graphiti (Zep)** | Open-source graph engine for building temporally-aware knowledge graphs that give AI agents persistent memory and semantic reasoning | 02_Graphiti, 01_CrewAI |
| **Neo4j** | Native graph database (v5.26+) used as the storage backend for Graphiti knowledge graphs, browsable at localhost:7474 | 02_Graphiti |
| **MemorySaver** | LangGraph's in-memory checkpointer for session persistence, enabling multi-turn conversations via thread_id | 03_LangGraph, 07_TravelGenie |

## MCP — Model Context Protocol (3)

| Tool | Description | Used In |
|------|-------------|---------|
| **FastMCP** | Python framework that simplifies creating MCP servers, handling networking and protocol details so you just define tool functions | 01_CrewAI |
| **Atlassian MCP Server** | Hosted MCP server connecting AI agents to Confluence and JIRA via OAuth for publishing docs and creating tickets | 09_CustomAgents |
| **GitHub MCP Server** | MCP endpoint enabling AI agents to interact with GitHub repos, issues, PRs, and code search via the MCP protocol | 09_CustomAgents |

## Observability & Evaluation (2)

| Tool | Description | Used In |
|------|-------------|---------|
| **LangSmith** | Distributed tracing and evaluation platform with `@traceable` decorator, custom evaluators, and dashboard filtering | 03_LangGraph, 07_TravelGenie |
| **Opik (Comet)** | Open-source LLM evaluation platform for monitoring agent thought processes, tool usage, and task execution in real-time | 01_CrewAI |

## Web UI Frameworks (2)

| Tool | Description | Used In |
|------|-------------|---------|
| **Gradio** | Python web UI framework for building interactive ML demos with chat interfaces, streaming, tabbed results, and animated backgrounds | 07_TravelGenie |
| **Astro 5** | Modern static site generator and SSR framework that outputs either static files or a Node.js server entry point | 01_Portfolio |

## Automation & Workflow (3)

| Tool | Description | Used In |
|------|-------------|---------|
| **n8n** | Visual workflow automation platform for building AI agent pipelines with webhooks, data transforms, emails, and integrations | 06_N8N |
| **Airtable** | Cloud-based database/spreadsheet used as a data source and destination within n8n automation workflows | 06_N8N |
| **Temporal** | Durable workflow engine wrapping backend operations with automatic retry policies, task queues, and a web UI | 02_URLReducer |

## Backend Frameworks (7)

| Tool | Description | Used In |
|------|-------------|---------|
| **FastAPI** | Async Python web framework for building REST APIs with automatic validation, CORS middleware, and OpenAPI docs | 02_URLReducer |
| **SQLAlchemy 2.0** | Python ORM with async session support; all database queries run non-blocking via the asyncpg driver | 02_URLReducer |
| **Alembic** | Lightweight database migration tool for SQLAlchemy, managing versioned schema changes | 02_URLReducer |
| **Pydantic v2** | Data validation library for request/response schemas and environment-variable settings management | 02_URLReducer |
| **Uvicorn** | Lightning-fast ASGI server that runs FastAPI applications with hot-reload support for development | 02_URLReducer |
| **Spring Boot** | Java web framework powering the Student CRUD REST API with JPA, EntityManager, and auto-configured beans | 09_CustomAgents |
| **httpx** | Modern async HTTP client for Python used for making API calls to Claude and external services | 04_MultiAgent |

## Frontend (5)

| Tool | Description | Used In |
|------|-------------|---------|
| **React 18** | JavaScript UI framework for building the single-page application with component-based architecture | 02_URLReducer |
| **Vite 5** | Fast frontend build tool with Hot Module Replacement (HMR) for instant development feedback | 02_URLReducer |
| **React Router v6** | Client-side routing library for navigating between pages in the React SPA | 02_URLReducer |
| **Recharts** | Declarative React charting library used for per-URL click analytics bar charts | 02_URLReducer |
| **ipywidgets** | Interactive HTML widgets for Jupyter notebooks providing text inputs, buttons, and chat output displays | 03_LangGraph |

## Databases (3)

| Tool | Description | Used In |
|------|-------------|---------|
| **PostgreSQL 16** | Primary relational database running as a Docker service, shared between the app and Temporal server | 02_URLReducer |
| **MySQL** | Relational database backing the Spring Boot Student CRUD service on localhost:3306 | 09_CustomAgents |
| **asyncpg** | High-performance async PostgreSQL driver used by SQLAlchemy for non-blocking DB queries | 02_URLReducer |

## Infrastructure & DevOps (3)

| Tool | Description | Used In |
|------|-------------|---------|
| **Docker / Docker Compose** | Containerization platform orchestrating multi-service stacks (DB, Temporal, backend, worker, frontend) | 01_Portfolio, 02_URLReducer |
| **Nginx** | Web server acting as reverse proxy or static file server with caching, SSL termination, gzip, and SPA routing | 01_Portfolio, 02_URLReducer |
| **ngrok** | Secure tunneling service creating public HTTPS URLs from local servers for demos and webhook testing | 01_Portfolio, 02_URLReducer |

## Development Tools (6)

| Tool | Description | Used In |
|------|-------------|---------|
| **Claude Code** | Anthropic's agentic AI coding assistant that designed, scaffolded, and iteratively developed entire projects across phases | 01_Portfolio, 02_URLReducer, 00_Skills |
| **VS Code Copilot Custom Agents** | Custom agent definitions in .agent.md files for orchestrated code review, Confluence publishing, JIRA ticketing, and PR creation | 09_CustomAgents |
| **Jupyter Notebook** | Interactive computing environment for running Python code cells with persisted outputs and inline visualizations | 01_CrewAI, 03_LangGraph, 07_TravelGenie |
| **uv** | Fast Python package manager handling dependency resolution, lock files, and virtual environment management | 03_LangGraph, 07_TravelGenie |
| **Maven (Wrapper)** | Java build tool managing dependencies and builds for the Spring Boot application via mvnw | 09_CustomAgents |
| **python-dotenv** | Utility that loads environment variables from .env files into the application's environment | 01_CrewAI, 03_LangGraph, 07_TravelGenie |

## Visualization (2)

| Tool | Description | Used In |
|------|-------------|---------|
| **matplotlib** | Python plotting library used for generating budget pie charts and data visualizations in agent outputs | 07_TravelGenie |
| **Mermaid** | Diagram-as-code renderer for creating flowcharts and architecture diagrams from text via draw_mermaid_png() | 03_LangGraph |
