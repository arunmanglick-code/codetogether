# URL Reducer — Implementation Plan

## Context

Build a URL shortener application from scratch. The project directory currently has only scaffolding (CLAUDE.md, decisions/, utils/). The user wants a full-stack app with analytics, dockerized services, and guardrails — but no auth and no vanity URLs.

**Confirmed requirements:**
- Backend: Python FastAPI REST API
- Frontend: React SPA (Vite)
- Database: PostgreSQL via Docker Compose
- Analytics: Click counts, timestamps, referrer tracking
- Vanity URLs: No (random codes only)
- Auth: No (open access)

---

## Architecture

```
02_Claude_URLReducer/
├── docker-compose.yml          # prod: Postgres + backend + frontend
├── docker-compose.dev.yml      # dev overrides (hot-reload, volume mounts)
├── .env.example
├── .gitignore
├── backend/
│   ├── Dockerfile / Dockerfile.dev
│   ├── requirements.txt
│   ├── alembic.ini + alembic/
│   └── app/
│       ├── main.py             # FastAPI app factory
│       ├── config.py           # pydantic-settings
│       ├── database.py         # async SQLAlchemy engine
│       ├── models/             # url.py, click.py (ORM)
│       ├── schemas/            # url.py, click.py (Pydantic)
│       ├── routers/            # urls.py, redirect.py
│       ├── services/           # url_service.py, shortcode.py
│       ├── middleware/         # error_handler.py, rate_limiter.py
│       └── utils/              # validators.py
├── frontend/
│   ├── Dockerfile / Dockerfile.dev
│   ├── package.json
│   ├── vite.config.js
│   └── src/
│       ├── api/client.js
│       ├── components/         # ShortenForm, ShortenedResult, UrlList, ClicksChart, Header, Footer
│       ├── pages/              # HomePage, AnalyticsPage
│       └── hooks/              # useShorten, useAnalytics
└── decisions/ + utils/         # (existing scaffolding)
```

## Database Schema

**Table `urls`:** id (BIGINT PK), short_code (VARCHAR(8) UNIQUE INDEX), original_url (TEXT), created_at (TIMESTAMPTZ), is_active (BOOLEAN), click_count (INTEGER — denormalized for fast reads)

**Table `clicks`:** id (BIGINT PK), url_id (BIGINT FK→urls.id INDEX), clicked_at (TIMESTAMPTZ INDEX), referrer (VARCHAR(2048)), user_agent (VARCHAR(1024)), ip_address (VARCHAR(45))

ORM: SQLAlchemy 2.0 async with asyncpg driver. Migrations: Alembic.

## API Endpoints

| Method | Path | Purpose | Response |
|--------|------|---------|----------|
| POST | `/api/shorten` | Create shortened URL | 201: `{short_code, short_url, original_url}` |
| GET | `/api/urls` | List recent URLs | 200: array of `{short_code, original_url, created_at, click_count}` |
| GET | `/api/urls/{code}/stats` | Analytics for a URL | 200: `{short_code, original_url, click_count, clicks: [...]}` |
| GET | `/{short_code}` | Redirect to original | 307 redirect (click logged via BackgroundTask) |
| GET | `/health` | Health check | 200: `{status: "ok"}` |

## Guardrails

- **URL validation:** Pydantic `HttpUrl` + custom validator blocking non-http(s) schemes and private IP ranges (SSRF prevention). Max 2048 chars.
- **Short code validation:** Strict regex `^[0-9a-zA-Z]{6,8}$` on path params before DB lookup.
- **Rate limiting:** In-memory sliding-window per client IP (default 60 req/min). Returns 429 + Retry-After.
- **Error handling:** Global middleware producing consistent `{error, status_code, message, details}` JSON. Never leaks stack traces.
- **SQL injection prevention:** All queries via SQLAlchemy ORM (parameterized).
- **CORS:** Explicit allowed origins from env var, no wildcards in production.
- **307 (not 301) redirect:** Prevents browser caching, allows deactivation and repeated click tracking.

## Key Design Decisions

1. **Async throughout** — FastAPI async endpoints + SQLAlchemy async engine + asyncpg
2. **Denormalized click_count** on `urls` — avoids JOIN/subquery for the list endpoint; incremented atomically
3. **Background task for click recording** — redirect handler sends 307 first, writes click record after
4. **Service layer pattern** — business logic in `services/`, HTTP concerns in `routers/`
5. **Base62 short codes (7 chars)** — 62^7 ≈ 3.5 trillion combinations, collision-safe with 5 retries
6. **Recharts** for frontend analytics chart (lightweight, React-native)

---

## Implementation Phases

### Phase 1: Backend Core + Database
Create the FastAPI app, SQLAlchemy models, Alembic migrations, config, shortcode generator, URL service, and core routers (`POST /api/shorten`, `GET /{code}`, `GET /health`). Add `.gitignore` and `.env.example`.

### Phase 2: Backend Guardrails + Analytics
Add click tracking (background task in redirect handler), analytics endpoint (`GET /api/urls/{code}/stats`), error handling middleware, rate limiter middleware, URL validators.

### Phase 3: Docker Compose Integration
Create Dockerfiles (prod + dev) for backend and frontend placeholder. Create `docker-compose.yml` and `docker-compose.dev.yml`. Backend runs Alembic migrations on startup.

### Phase 4: Frontend — Core UI
Scaffold React+Vite app. Build ShortenForm, ShortenedResult, UrlList components. API client with fetch wrapper. HomePage composing the components.

### Phase 5: Frontend — Analytics Dashboard
Add React Router (`/` and `/analytics/:shortCode`). Build AnalyticsPage with AnalyticsDashboard, ClicksChart (Recharts), and clicks table. useAnalytics hook.

### Phase 6: Production Docker + Polish
Add nginx.conf for frontend (API proxy + SPA fallback). Multi-stage frontend Dockerfile. Update CLAUDE.md with final architecture and commands.

---

## Dependencies

**Backend (`requirements.txt`):** fastapi, uvicorn[standard], sqlalchemy[asyncio], asyncpg, alembic, pydantic-settings, python-dotenv, httpx, pytest, pytest-asyncio

**Frontend (`package.json`):** react, react-dom, react-router-dom, recharts | devDeps: @vitejs/plugin-react, vite, eslint
