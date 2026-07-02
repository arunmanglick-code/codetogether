# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

URL Reducer — a full-stack URL shortening service with click analytics. No auth, no vanity URLs. Open access.

## Architecture

- **Backend:** Python FastAPI (async) — `backend/app/`
- **Frontend:** React 18 SPA with Vite — `frontend/src/`
- **Database:** PostgreSQL 16 via Docker Compose
- **ORM:** SQLAlchemy 2.0 async with asyncpg driver
- **Migrations:** Alembic (`backend/alembic/`)
- **Workflow Engine:** Temporal (temporalio SDK) — `backend/app/temporal/`

The backend follows a service-layer pattern: `routers/` → Temporal workflows → activities → `services/` → DB. Routers no longer call service functions directly; all 5 operations are wrapped in Temporal workflows with automatic retry policies.

Short codes are 7-character base62 strings generated with `secrets.choice()`. Redirects use 307 (not 301) to enable click tracking. Click recording uses `start_workflow` (fire-and-forget) for minimal redirect latency with durable delivery. `click_count` on the `urls` table is denormalized and incremented atomically. Click idempotency is ensured via a `click_id` UUID column with a UNIQUE constraint.

### Temporal Integration

- **5 workflows** wrap all backend operations (reads + writes)
- **Write retry policy:** 5 attempts, 1s→30s exponential backoff
- **Read retry policy:** 3 attempts, 500ms→10s exponential backoff
- **Task queue:** `url-reducer`
- **Worker:** Runs as a separate Docker service (`temporal-worker`) using the same backend image
- **Client:** Singleton connected via FastAPI lifespan, used by routers
- **Temporal server:** `temporalio/auto-setup` sharing Postgres with the app, Web UI on port 8233

## Development Commands

```bash
# Start all services (dev mode with hot-reload)
docker compose -f docker-compose.yml -f docker-compose.dev.yml up --build

# Start all services (production — includes Temporal server + worker)
docker compose up --build

# Start just DB + Temporal (for local backend dev)
docker compose up db temporal

# Backend only (requires local Postgres)
cd backend
pip install -r requirements.txt
alembic upgrade head
uvicorn app.main:app --reload

# Frontend only
cd frontend
npm install
npm run dev

# Run backend tests
cd backend
pytest

# Run a single test
cd backend
pytest tests/test_shortcode.py -v
```

## API Endpoints

| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/shorten` | Create shortened URL |
| GET | `/api/urls` | List recent URLs |
| GET | `/api/urls/{code}/stats` | Analytics for a URL |
| GET | `/{short_code}` | Redirect (307) + record click |
| GET | `/health` | Health check |

## Key Files

- `backend/app/main.py` — FastAPI app factory with CORS, rate limiter, error handlers, Temporal lifespan
- `backend/app/services/url_service.py` — Core business logic (unchanged, called by activities)
- `backend/app/temporal/workflows.py` — 5 workflow definitions with retry policies
- `backend/app/temporal/activities.py` — 5 activity functions (one per service operation)
- `backend/app/temporal/worker.py` — Worker entrypoint (`python -m app.temporal.worker`)
- `backend/app/temporal/client.py` — Temporal client singleton + lifespan
- `backend/app/temporal/dataclasses.py` — Input/output dataclasses for all workflows
- `backend/app/middleware/rate_limiter.py` — In-memory sliding-window rate limiter (60 req/min per IP)
- `backend/app/utils/validators.py` — URL safety validation (SSRF prevention, scheme checking)
- `frontend/src/pages/HomePage.jsx` — Shorten form + URL list
- `frontend/src/pages/AnalyticsPage.jsx` — Click analytics with Recharts bar chart
- `docker-compose.yml` / `docker-compose.dev.yml` — Service orchestration (5 services: db, temporal, backend, temporal-worker, frontend)

## Environment Variables

See `.env.example` for all variables. Key ones: `DATABASE_URL`, `BASE_URL`, `CORS_ORIGINS`, `VITE_API_BASE_URL`, `TEMPORAL_SERVER_URL`, `TEMPORAL_NAMESPACE`, `TEMPORAL_TASK_QUEUE`.
