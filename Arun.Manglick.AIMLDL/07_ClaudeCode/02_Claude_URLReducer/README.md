# URL Reducer

> A full-stack URL shortening service with click analytics — built end-to-end using **Claude Code** as the agentic AI development assistant.

[![Python](https://img.shields.io/badge/Python-3.12-blue?logo=python)](https://www.python.org/)
[![FastAPI](https://img.shields.io/badge/FastAPI-async-009688?logo=fastapi)](https://fastapi.tiangolo.com/)
[![React](https://img.shields.io/badge/React-18-61DAFB?logo=react)](https://react.dev/)
[![Temporal](https://img.shields.io/badge/Temporal-workflow-000?logo=temporal)](https://temporal.io/)
[![Docker](https://img.shields.io/badge/Docker-compose-2496ED?logo=docker)](https://www.docker.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)](https://www.postgresql.org/)

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Tech Stack](#tech-stack)
3. [Architecture](#architecture)
4. [Installation Guide](#installation-guide)
5. [Running the Project](#running-the-project)
6. [API Reference](#api-reference)
7. [Folder Structure](#folder-structure)
8. [Environment Variables](#environment-variables)
9. [Contributing & Future Enhancements](#contributing--future-enhancements)

---

## Project Overview

**URL Reducer** is an open-access URL shortening service. Paste a long URL, get a 7-character short code, and share it anywhere. Every redirect is tracked — timestamps, click counts, and per-URL analytics are available instantly via the React dashboard.

No authentication, no vanity URLs — open access by design.

### What it does

- Shortens any valid HTTP/HTTPS URL to a compact `/{code}` slug
- Redirects via **307 Temporary Redirect** (not 301) so every click is always recorded — browsers never cache the destination
- Tracks each unique click with a UUID `click_id` for idempotent, replay-safe recording
- Displays per-URL analytics with a Recharts bar chart
- Enforces a **60 req/min per-IP** sliding-window rate limit
- Validates URLs against SSRF attack vectors before shortening

### How Claude Code is integrated

This project was designed, scaffolded, and iteratively developed using **Claude Code** (`claude.ai/code`) — an agentic AI coding assistant. Claude drove the entire development lifecycle:

| Claude Capability | How it was used |
|---|---|
| **Persistent context (`CLAUDE.md`)** | A `CLAUDE.md` file at the root gives Claude durable project memory — architecture rules, key file paths, design constraints, and dev commands — loaded at the start of every session |
| **Agentic multi-phase workflows** | Development was broken into 13 documented phases (`utils/phase_output/`); Claude autonomously planned and implemented each phase end-to-end |
| **Architecture Decision Records** | Claude logged every significant design choice in `decisions/` as structured ADR documents |
| **Temporal workflow wiring** | Claude integrated a durable workflow engine (Temporal) across the full stack — Docker Compose services, FastAPI lifespan hooks, and typed activity dataclasses |
| **Iterative prompting** | Prompt files in `utils/prompt/` capture the exact instructions given to Claude per phase, making the AI-assisted process fully reproducible |

---

## Tech Stack

| Technology | Version | Role |
|---|---|---|
| **Claude Code** | — | Agentic AI assistant that designed, implemented, and iterated the entire codebase across 13 phases |
| **Python** | 3.12 | Primary backend language |
| **FastAPI** | 0.115+ | Async REST API framework — routing, validation, CORS middleware, and error handling |
| **SQLAlchemy** | 2.0 async | ORM with async session support; all DB queries are non-blocking |
| **asyncpg** | 0.29+ | High-performance async PostgreSQL driver used by SQLAlchemy |
| **Alembic** | 1.13+ | Database schema migration management (3 versioned migrations) |
| **Pydantic v2** | 2.0+ | Request/response validation and environment-variable settings management |
| **Temporal** | latest | Durable workflow engine — wraps all 5 backend operations with automatic retry policies |
| **temporalio SDK** | 1.7+ | Python SDK for defining Temporal workflows, activities, and the worker |
| **PostgreSQL** | 16 | Primary relational database; runs as a Docker service shared with Temporal |
| **React** | 18.3 | Frontend SPA framework |
| **Vite** | 5.4 | Fast frontend build tool with Hot Module Replacement (HMR) for development |
| **React Router** | v6 | Client-side routing between the Home page and Analytics page |
| **Recharts** | 2.12 | Declarative charting library for the per-URL click analytics bar chart |
| **Node.js** | 18+ | JavaScript runtime required to build and develop the React frontend |
| **npm** | 9+ | Package manager for all frontend dependencies |
| **Docker** | 24+ | Containerises all 6 services for consistent local and production environments |
| **Docker Compose** | v2 | Orchestrates the multi-service stack (db, temporal, temporal-ui, backend, temporal-worker, frontend) |
| **Nginx** | alpine | Serves the production Vite build inside the frontend container; handles SPA client-side routing |
| **Uvicorn** | 0.30+ | ASGI server that runs the FastAPI application |
| **ngrok** | latest | Tunnels the local server to a public HTTPS URL for demos and external webhook testing |

---

## Architecture

```
                    +-------------+
                    |   Browser   |
                    +------+------+
                           | HTTP
              +------------v------------+
              |   React 18 SPA (Vite)   |  :3000 prod / :5173 dev
              +------------+------------+
                           | REST API
              +------------v------------+
              |    FastAPI Backend      |  :8000
              |  Rate Limiter MW        |
              |  CORS MW                |
              |  Error Handler MW       |
              |  Routers -> Temporal    |
              +------------+------------+
                           | Temporal SDK
         +-----------------v------------------+
         |         Temporal Server             |  :7233 (Web UI :8233)
         |  Workflows + Activities + Retries   |
         +-----------------+------------------+
                           |
         +-----------------v------------------+
         |         Temporal Worker             |
         |  (same image, separate container)   |
         +-----------------+------------------+
                           | SQLAlchemy async
         +-----------------v------------------+
         |         PostgreSQL 16               |  :5432
         +------------------------------------+
```

**Request flow:** Router -> `execute_workflow` (Temporal client) -> Worker picks up task -> Activity calls service layer -> SQLAlchemy async DB query.

### Temporal Workflows

| Workflow | Trigger | Retry Policy |
|---|---|---|
| `CreateShortUrlWorkflow` | `POST /api/shorten` | 5 attempts, 1s -> 30s exponential |
| `GetUrlByCodeWorkflow` | `GET /{short_code}` | 3 attempts, 500ms -> 10s exponential |
| `GetUrlWithClicksWorkflow` | `GET /api/urls/{code}/stats` | 3 attempts, 500ms -> 10s exponential |
| `ListUrlsWorkflow` | `GET /api/urls` | 3 attempts, 500ms -> 10s exponential |
| `RecordClickWorkflow` | Fire-and-forget on redirect | 5 attempts, 1s -> 30s exponential |

---

## Installation Guide

### Prerequisites

| Tool | Minimum Version | Verify |
|---|---|---|
| Python | 3.11+ | `python --version` |
| Node.js | 18+ | `node --version` |
| npm | 9+ | `npm --version` |
| Docker Desktop | 24+ | `docker --version` |
| Docker Compose | v2 (bundled) | `docker compose version` |
| Git | any | `git --version` |

---

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd 02_Claude_URLReducer
```

---

### 2. Install Node.js

Node.js is required to build and develop the React frontend.

**Recommended — via nvm (Node Version Manager):**

```bash
# macOS / Linux
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.0/install.sh | bash

# Windows: download nvm-windows from https://github.com/coreybutler/nvm-windows/releases

# Install and use Node 20 LTS
nvm install 20
nvm use 20

# Verify
node --version   # v20.x.x
npm --version    # 10.x.x
```

**Or download directly:** [https://nodejs.org/en/download](https://nodejs.org/en/download)

---

### 3. Install Frontend Dependencies

```bash
cd frontend
npm install
cd ..
```

Installs React 18, Vite 5, React Router v6, and Recharts from `frontend/package.json`.

---

### 4. Install Python & Backend Dependencies (local dev only)

Required only if you want to run the backend **outside** Docker:

```bash
# Create and activate a virtual environment
python -m venv .venv

# Windows
.venv\Scripts\activate

# macOS / Linux
source .venv/bin/activate

# Install all backend dependencies
pip install -r backend/requirements.txt
```

Key packages: `fastapi`, `uvicorn`, `sqlalchemy[asyncio]`, `asyncpg`, `alembic`, `pydantic`, `temporalio`, `pytest`.

---

### 5. Install Docker Desktop

Docker runs all 6 services (PostgreSQL, Temporal, Temporal UI, backend, Temporal worker, frontend Nginx) in isolated containers.

1. Download from [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
2. Follow the installer for your OS
3. Start Docker Desktop and wait for the engine to be ready
4. Verify:

```bash
docker --version          # Docker version 24.x.x
docker compose version    # Docker Compose version v2.x.x
```

---

### 6. Configure Nginx (handled automatically via Docker)

Nginx is pre-configured inside the frontend Docker image. The `frontend/nginx.conf` handles:

- Serving the static Vite production build from `/usr/share/nginx/html`
- Falling back all unknown routes to `index.html` for React Router's client-side routing
- No reverse-proxy setup is required — the frontend calls the backend API directly via `VITE_API_BASE_URL`

No manual Nginx installation is needed. To inspect the config:

```bash
cat frontend/nginx.conf
```

---

### 7. Configure Environment Variables

```bash
# Copy the example file
cp .env.example .env
```

Edit `.env` with your values. The defaults work out of the box for local Docker Compose:

```dotenv
POSTGRES_PASSWORD=urlreducer
BASE_URL=http://localhost:8000
VITE_API_BASE_URL=http://localhost:8000
```

See [Environment Variables](#environment-variables) for the full list.

---

### 8. Install ngrok (Optional — for public access)

ngrok creates a secure public HTTPS tunnel to your local server, useful for sharing demos or testing external webhooks.

```bash
# Install via npm
npm install -g ngrok

# OR download the binary from https://ngrok.com/download and add to PATH
```

**One-time authentication (free ngrok account required):**

```bash
ngrok config add-authtoken <YOUR_NGROK_AUTHTOKEN>
```

Get your token at [https://dashboard.ngrok.com/get-started/your-authtoken](https://dashboard.ngrok.com/get-started/your-authtoken).

---

## Running the Project

### Development Mode (hot-reload enabled)

```bash
docker compose -f docker-compose.yml -f docker-compose.dev.yml up --build
```

The `docker-compose.dev.yml` overlay mounts local source directories into containers, enabling live reload for both the FastAPI backend (Uvicorn `--reload`) and the React frontend (Vite HMR).

| Service | URL |
|---|---|
| Frontend (Vite HMR) | http://localhost:5173 |
| Backend API | http://localhost:8000 |
| Swagger / OpenAPI Docs | http://localhost:8000/docs |
| Temporal Web UI | http://localhost:8233 |

---

### Production Mode

```bash
docker compose up --build
```

Builds optimised images. The frontend is compiled with `vite build` and served by Nginx.

| Service | URL |
|---|---|
| Frontend (Nginx) | http://localhost:3000 |
| Backend API | http://localhost:8000 |
| Temporal Web UI | http://localhost:8233 |

---

### Start Only the Database + Temporal (for local backend dev)

```bash
docker compose up db temporal
```

Then in separate terminals:

```bash
# Terminal 1 — run migrations and start the API
cd backend
alembic upgrade head
uvicorn app.main:app --reload

# Terminal 2 — start the Temporal worker
cd backend
python -m app.temporal.worker
```

---

### Frontend Only (Vite dev server)

```bash
cd frontend
npm run dev
```

Preview the production build locally (no Docker needed):

```bash
npm run build
npm run preview
```

---

### Run Backend Tests

```bash
cd backend
pytest

# Single file, verbose output
pytest tests/test_shortcode.py -v
```

---

### Expose Locally via ngrok

After starting the project (any mode above), open a new terminal:

```bash
# Tunnel the backend API
ngrok http 8000

# Tunnel the frontend (production mode)
ngrok http 3000
```

ngrok prints a public `https://<id>.ngrok-free.app` URL. Update your `.env`:

```dotenv
BASE_URL=https://<id>.ngrok-free.app
VITE_API_BASE_URL=https://<id>.ngrok-free.app
```

Then restart services so shortened URLs resolve correctly through the public tunnel:

```bash
docker compose up --build
```

---

### Stop All Services

```bash
# Stop and remove containers + networks
docker compose down

# Also remove the database volume (deletes all data)
docker compose down -v
```

---

## API Reference

| Method | Path | Description |
|---|---|---|
| `POST` | `/api/shorten` | Shorten a URL; body: `{ "original_url": "..." }` |
| `GET` | `/api/urls` | List recent shortened URLs with click counts |
| `GET` | `/api/urls/{code}/stats` | Detailed click analytics for a short code |
| `GET` | `/{short_code}` | 307 redirect to original URL + records a click |
| `GET` | `/health` | Health check — returns `{ "status": "ok" }` |

**Rate limit:** 60 requests per minute per IP (in-memory sliding window).

**Short code format:** 7-character base62 string (`[A-Za-z0-9]`) — ~3.5 trillion unique combinations.

**Example — shorten a URL:**

```bash
curl -X POST http://localhost:8000/api/shorten \
  -H "Content-Type: application/json" \
  -d '{"original_url": "https://example.com/very/long/path"}'
```

```json
{
  "short_code": "aB3xY7z",
  "short_url": "http://localhost:8000/aB3xY7z",
  "original_url": "https://example.com/very/long/path",
  "click_count": 0
}
```

---

## Folder Structure

```
02_Claude_URLReducer/
|
+-- CLAUDE.md                        # Claude Code project context — loaded every session
+-- docker-compose.yml               # Production service orchestration (6 services)
+-- docker-compose.dev.yml           # Dev overrides (hot-reload, volume mounts)
+-- .env.example                     # Environment variable template
|
+-- backend/
|   +-- Dockerfile                   # Production image
|   +-- Dockerfile.dev               # Dev image (with --reload)
|   +-- requirements.txt             # Python dependencies
|   +-- alembic.ini                  # Alembic migration configuration
|   +-- alembic/
|   |   +-- versions/                # DB migration scripts (001 -> 003)
|   +-- app/
|       +-- main.py                  # FastAPI app factory (CORS, rate limiter, Temporal lifespan)
|       +-- config.py                # Settings via pydantic-settings + .env
|       +-- database.py              # Async SQLAlchemy engine + session factory
|       +-- models/                  # SQLAlchemy ORM models (Url, Click)
|       +-- schemas/                 # Pydantic request/response schemas
|       +-- routers/                 # HTTP route handlers (urls.py, redirect.py)
|       +-- services/                # Core business logic (url_service.py, shortcode.py)
|       +-- middleware/              # Rate limiter + global error handler middleware
|       +-- temporal/
|       |   +-- workflows.py         # 5 workflow definitions with retry policies
|       |   +-- activities.py        # 5 activity functions (one per service operation)
|       |   +-- worker.py            # Temporal worker entrypoint
|       |   +-- client.py            # Temporal client singleton + FastAPI lifespan
|       |   +-- dataclasses.py       # Typed input/output dataclasses for all workflows
|       +-- utils/
|           +-- validators.py        # URL safety validation (SSRF prevention, scheme check)
|   +-- tests/
|       +-- test_shortcode.py        # Unit tests for short-code generation
|
+-- frontend/
|   +-- Dockerfile                   # Production image (Nginx serving Vite build)
|   +-- Dockerfile.dev               # Dev image (Vite HMR server)
|   +-- nginx.conf                   # Nginx config — SPA routing fallback
|   +-- vite.config.js               # Vite build configuration
|   +-- package.json                 # Node.js dependencies and npm scripts
|   +-- src/
|       +-- App.jsx                  # Root component + React Router setup
|       +-- api/
|       |   +-- client.js            # Fetch-based API client pointing to backend
|       +-- components/              # Reusable UI components
|       |   +-- ShortenForm.jsx      # URL input form
|       |   +-- ShortenedResult.jsx  # Displays the newly created short URL
|       |   +-- UrlList.jsx          # Recent URLs table with click counts
|       |   +-- AnalyticsDashboard.jsx
|       |   +-- ClicksChart.jsx      # Recharts bar chart for click history
|       |   +-- Header.jsx
|       |   +-- Footer.jsx
|       +-- hooks/
|       |   +-- useShorten.js        # Custom hook: shorten form state and submission
|       |   +-- useAnalytics.js      # Custom hook: analytics data fetching
|       +-- pages/
|           +-- HomePage.jsx         # Shorten form + recent URL list
|           +-- AnalyticsPage.jsx    # Per-URL click analytics view
|
+-- decisions/                       # Architecture Decision Records (ADRs)
|   +-- 001-create-decision-log.md
|   +-- 002-create-utils-folder-structure.md
|   +-- 003-tech-stack-and-architecture.md
|   +-- 004-implementation-complete.md
|   +-- 005-temporal-integration.md
|
+-- utils/
    +-- plan/                        # Implementation plans used to guide Claude
    |   +-- 01_implementation_plan.md
    |   +-- 02_temporal_integration_plan.md
    +-- prompt/                      # Claude prompts per development phase
    |   +-- 01_plan_prompt.md
    |   +-- 02_plan_prompt_add_temporal.md
    +-- phase_output/                # Claude session output logs (13 phases)
```

---

## Environment Variables

Create a `.env` file in the project root (copy from `.env.example`):

```dotenv
# -- Database ------------------------------------------------------------------
POSTGRES_PASSWORD=urlreducer

# -- Backend -------------------------------------------------------------------
DATABASE_URL=postgresql+asyncpg://urlreducer:urlreducer@db:5432/urlreducer
BASE_URL=http://localhost:8000
CORS_ORIGINS=["http://localhost:3000","http://localhost:5173"]

# -- Frontend (Vite build-time) ------------------------------------------------
VITE_API_BASE_URL=http://localhost:8000

# -- Temporal ------------------------------------------------------------------
TEMPORAL_SERVER_URL=temporal:7233
TEMPORAL_NAMESPACE=default
TEMPORAL_TASK_QUEUE=url-reducer
```

> **Docker note:** `TEMPORAL_SERVER_URL` uses the Compose service name `temporal` inside containers. For local dev against a Dockerised Temporal, change it to `localhost:7233`.

> **ngrok note:** Set `BASE_URL` and `VITE_API_BASE_URL` to your public ngrok tunnel URL so shortened links resolve correctly from outside your machine.

---

## Contributing & Future Enhancements

### Contributing

1. **Fork** the repository and create a feature branch:

   ```bash
   git checkout -b feat/my-feature
   ```

2. Follow the **service-layer pattern**: `routers/` -> Temporal workflow -> activity -> `services/` -> DB. Do not call service functions directly from routers.

3. Add a new **Architecture Decision Record** in `decisions/` for any significant design choice:

   ```
   decisions/00N-short-description.md
   ```

4. Run the test suite before opening a PR:

   ```bash
   cd backend
   pytest
   ```

5. Keep **`CLAUDE.md`** up to date if the architecture changes — it is the source of truth for all Claude Code sessions.

6. Open a PR with a clear description of the change and its motivation.

---

### Roadmap / Future Enhancements

| Feature | Notes |
|---|---|
| **Custom vanity slugs** | Let users choose their own short code instead of random 7-char generation |
| **Link expiry (TTL)** | Add an optional expiry date per link; a Temporal cron workflow cleans up expired URLs |
| **QR code generation** | Return a QR code image alongside the short URL on creation |
| **User authentication** | JWT-based auth so users can manage, edit, and delete their own links |
| **Geo analytics** | Record country and city from `X-Forwarded-For` headers at click time |
| **Referrer tracking** | Capture the HTTP `Referer` header per click for traffic source analysis |
| **Aggregate dashboard** | Time-series charts across all URLs — total clicks per day/week/month |
| **Redis caching** | Cache hot short-code -> original URL lookups to reduce DB load for viral links |
| **Tailwind CSS** | Replace module CSS with utility-first Tailwind for easier UI customisation |
| **Kubernetes manifests** | Helm chart for deploying the full stack to a Kubernetes cluster |
| **CI/CD pipeline** | GitHub Actions: lint -> test -> build Docker images -> push to registry |
| **Rate limit persistence** | Move the in-memory sliding-window rate limiter to Redis for multi-instance deployments |

---

## Acknowledgements

This project was built entirely through an agentic workflow with **Claude Code** (`claude.ai/code`). The `CLAUDE.md` file, `decisions/` ADRs, `utils/plan/`, `utils/prompt/`, and `utils/phase_output/` directories together form a complete record of the AI-assisted development process — fully reproducible from the prompts alone.
