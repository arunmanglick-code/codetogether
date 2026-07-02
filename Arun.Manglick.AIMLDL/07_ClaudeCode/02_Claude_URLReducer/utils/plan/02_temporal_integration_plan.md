# Temporal Integration Plan — URL Reducer

## Context

The URL Reducer application is fully functional (FastAPI + React + Postgres via Docker Compose), but has a reliability gap: click recording uses fire-and-forget `BackgroundTasks` — if the server crashes after sending the 307 redirect but before the background task writes the click, data is silently lost. DB write operations (`create_short_url`, `record_click`) also have no automatic retry on transient failures.

Integrating Temporal makes all backend operations durable — writes are retried automatically with configurable policies, and click recording is guaranteed even across process restarts.

**Confirmed scope:** ALL backend operations (reads + writes) wrapped in Temporal workflows. Real-time per-click analytics via Temporal. No cleanup workflow. `temporalio/auto-setup` for local dev.

---

## Architecture Change

**Before:** FastAPI routers → service functions → direct DB calls
**After:** FastAPI routers → Temporal client → Temporal server → worker → activities (DB calls)

```
Request → FastAPI Router → Temporal Client
                              ↓
                        Temporal Server (gRPC :7233)
                              ↓
                        Worker Process → Activity → DB
                              ↓
                        Result → back to Router → Response
```

For click recording, `start_workflow` (fire-and-forget) replaces `execute_workflow` to keep redirect latency minimal.

## New Docker Services

| Service | Image | Port | Purpose |
|---------|-------|------|---------|
| `temporal` | `temporalio/auto-setup:latest` | 7233, 8233 | Temporal server (shares Postgres with app), Web UI on 8233 |
| `temporal-worker` | Same backend Dockerfile | — | Runs `python -m app.temporal.worker` |

## New File Structure

```
backend/app/temporal/
├── __init__.py
├── dataclasses.py    # Input/output dataclasses for all workflows
├── activities.py     # 5 activity functions (one per service function)
├── workflows.py      # 5 workflow definitions with retry policies
├── client.py         # Temporal client singleton + FastAPI lifespan
└── worker.py         # Worker entrypoint (separate Docker service)
```

## Workflow-to-Operation Mapping

| Operation | Workflow | Activity | Type | Retry Policy |
|-----------|----------|----------|------|-------------|
| Create short URL | `CreateShortUrlWorkflow` | `create_short_url_activity` | Write | 5 attempts, 1s→30s backoff |
| Get URL by code | `GetUrlByCodeWorkflow` | `get_url_by_code_activity` | Read | 3 attempts, 500ms→10s |
| List URLs | `ListUrlsWorkflow` | `list_urls_activity` | Read | 3 attempts, 500ms→10s |
| Record click | `RecordClickWorkflow` | `record_click_activity` | Write | 5 attempts, 1s→30s |
| Get URL with clicks | `GetUrlWithClicksWorkflow` | `get_url_with_clicks_activity` | Read | 3 attempts, 500ms→10s |

All activities: `start_to_close_timeout=10s`. Single task queue: `url-reducer`.

## Idempotency Design (Critical)

**Problem:** Temporal retries could duplicate clicks — the `clicks` table has no unique constraint beyond auto-increment `id`.

**Solution:**
1. Add `click_id` column (`String(36)`, UNIQUE, nullable) to `clicks` table via Alembic migration 003.
2. Router generates `click_id = str(uuid.uuid4())` at request time, passes it through workflow input.
3. Activity inserts with `click_id`. On retry, `IntegrityError` from UNIQUE constraint is caught → return `recorded=False` (safe dedup).
4. `clicked_at` timestamp is captured at request time and passed through (not `server_default`), preserving accuracy on retries.

## Dataclass Design

Each workflow uses a typed input/output dataclass pair to ensure type safety across the Temporal boundary:

| Dataclass | Fields |
|-----------|--------|
| `CreateShortUrlInput` | `original_url` |
| `CreateShortUrlOutput` | `short_code`, `original_url`, `short_url`, `created_at` |
| `GetUrlByCodeInput` | `short_code` |
| `GetUrlByCodeOutput` | `id`, `short_code`, `original_url`, `is_active` |
| `ListUrlsInput` | `skip`, `limit` |
| `ListUrlsOutput` | `urls` (list of dicts) |
| `RecordClickInput` | `short_code`, `click_id`, `clicked_at`, `referrer`, `user_agent`, `ip_address` |
| `RecordClickOutput` | `recorded` (bool) |
| `GetUrlWithClicksInput` | `short_code` |
| `GetUrlWithClicksOutput` | `url` (dict with nested clicks) |

## Workflow ID Patterns

| Operation | Pattern | Purpose |
|-----------|---------|---------|
| Create URL | `create-url-{uuid}` | Unique per creation request |
| Get URL | `get-url-{short_code}-{uuid}` | Unique per redirect lookup |
| List URLs | `list-urls-{uuid}` | Unique per list request |
| Record Click | `click-{click_id}` | click_id IS the idempotency key |
| Get Stats | `stats-{short_code}-{uuid}` | Unique per stats request |

## Files to Modify

| File | Change |
|------|--------|
| `backend/requirements.txt` | Add `temporalio>=1.7.0` |
| `backend/app/config.py` | Add `TEMPORAL_SERVER_URL`, `TEMPORAL_NAMESPACE`, `TEMPORAL_TASK_QUEUE` |
| `backend/app/main.py` | Add lifespan context manager for Temporal client connection |
| `backend/app/models/click.py` | Add `click_id` column (String(36), unique, nullable) |
| `backend/app/routers/urls.py` | Replace 3 service calls with `client.execute_workflow()`. Remove `db` dependency. |
| `backend/app/routers/redirect.py` | Replace `BackgroundTasks` + direct DB call with `execute_workflow` (lookup) + `start_workflow` (click). Remove `db` dependency, remove `_record_click_background`. |
| `docker-compose.yml` | Add `temporal` + `temporal-worker` services. Add Temporal env vars to `backend`. |
| `docker-compose.dev.yml` | Add `temporal-worker` dev override with volume mount. |
| `.env.example` | Add 3 Temporal env vars |
| `CLAUDE.md` | Update architecture, commands, key files |

**Unchanged:** `database.py`, `services/url_service.py`, `services/shortcode.py`, `models/url.py`, `schemas/*`, `middleware/*`, `utils/*`, all frontend files, both Dockerfiles.

---

## Implementation Phases

### Phase 1: Infrastructure + Config
- Add `temporalio>=1.7.0` to `requirements.txt`
- Add 3 Temporal settings to `config.py`: `TEMPORAL_SERVER_URL` (default `temporal:7233`), `TEMPORAL_NAMESPACE` (default `default`), `TEMPORAL_TASK_QUEUE` (default `url-reducer`)
- Create `backend/app/temporal/__init__.py`
- Create `backend/app/temporal/dataclasses.py` (all 5 input/output pairs)
- Add `temporal` service to `docker-compose.yml` with healthcheck (`tctl cluster health`), ports 7233 + 8233, Postgres connection vars
- Add `temporal-worker` service to `docker-compose.yml` using same backend image, command `python -m app.temporal.worker`
- Add `temporal-worker` dev override to `docker-compose.dev.yml` with volume mount
- Add Temporal vars to `.env.example`
- Add Temporal env vars (`TEMPORAL_SERVER_URL`, `TEMPORAL_NAMESPACE`, `TEMPORAL_TASK_QUEUE`) to backend service in `docker-compose.yml`
- Backend `depends_on` updated to wait for `temporal: service_healthy`

**Verify:** `docker compose up db temporal` — Temporal server starts and is healthy on port 7233.

### Phase 2: Database Migration
- Add `click_id` column to `models/click.py`: `String(36)`, unique, nullable
- Create `alembic/versions/003_add_click_id_to_clicks.py` — `ADD COLUMN click_id` + unique index `ix_clicks_click_id`
- Nullable so existing clicks (pre-Temporal) remain valid

**Verify:** `alembic upgrade head` — column exists with unique index. Existing clicks unaffected.

### Phase 3: Activities + Workflows
- Create `backend/app/temporal/activities.py` — 5 `@activity.defn` functions, each uses `async_session()` directly (not FastAPI `get_db` dependency)
  - `create_short_url_activity` — collision-retry loop, returns `CreateShortUrlOutput`
  - `get_url_by_code_activity` — returns `GetUrlByCodeOutput | None`
  - `list_urls_activity` — paginated, returns `ListUrlsOutput` with list of dicts
  - `record_click_activity` — inserts click with `click_id`, catches `IntegrityError` for dedup, atomically increments `click_count`
  - `get_url_with_clicks_activity` — returns URL with nested clicks via `selectin` relationship
- Create `backend/app/temporal/workflows.py` — 5 `@workflow.defn` classes
  - Imports use `workflow.unsafe.imports_passed_through()` for determinism
  - Write workflows: `WRITE_RETRY_POLICY` (5 attempts, 1s initial, 30s max, 2.0 backoff)
  - Read workflows: `READ_RETRY_POLICY` (3 attempts, 500ms initial, 10s max, 2.0 backoff)
  - All use `start_to_close_timeout=10s`

**Verify:** Import check — `python -c "from app.temporal.workflows import *"` succeeds.

### Phase 4: Worker + Client
- Create `backend/app/temporal/worker.py` — `asyncio.run(main())` entrypoint, registers all 5 workflows + 5 activities, listens on `url-reducer` queue
- Create `backend/app/temporal/client.py` — singleton `_client` with `get_temporal_client()`, `close_temporal_client()`, and `temporal_lifespan()` async context manager
- Update `backend/app/main.py` — add `@asynccontextmanager async def lifespan(app)` wrapping `temporal_lifespan()`, pass to `FastAPI(lifespan=lifespan)`

**Verify:** `docker compose up db temporal temporal-worker backend` — worker logs show "Listening on queue: url-reducer", backend logs show "Connected to Temporal".

### Phase 5: Wire Routers
- Rewrite `backend/app/routers/urls.py`:
  - `POST /api/shorten` → `client.execute_workflow(CreateShortUrlWorkflow.run, ..., id=f"create-url-{uuid}")`
  - `GET /api/urls` → `client.execute_workflow(ListUrlsWorkflow.run, ..., id=f"list-urls-{uuid}")`
  - `GET /api/urls/{code}/stats` → `client.execute_workflow(GetUrlWithClicksWorkflow.run, ..., id=f"stats-{code}-{uuid}")`
  - Remove all `Depends(get_db)`, `AsyncSession`, direct service imports
- Rewrite `backend/app/routers/redirect.py`:
  - URL lookup: `client.execute_workflow(GetUrlByCodeWorkflow.run, ...)` — awaits result
  - Click recording: `client.start_workflow(RecordClickWorkflow.run, ...)` — fire-and-forget
  - Generate `click_id = str(uuid.uuid4())` and `clicked_at = datetime.now(timezone.utc).isoformat()` at request time
  - Remove `BackgroundTasks`, `Depends(get_db)`, `_record_click_background` helper

**Verify:** Full end-to-end:
1. `POST /api/shorten` → creates URL via Temporal
2. `GET /{code}` → 307 redirect, `RecordClickWorkflow` started (visible in Temporal Web UI)
3. `GET /api/urls` → lists URLs with click counts
4. `GET /api/urls/{code}/stats` → shows click details
5. Temporal Web UI at `http://localhost:8233` — workflows visible and completed

### Phase 6: Documentation + Cleanup
- Update `CLAUDE.md` with new architecture, Temporal subsection, updated commands and key files
- Save phase outputs to `utils/phase_output/` (07–12)
- Write this plan to `utils/plan/02_temporal_integration_plan.md`

---

## Verification

```bash
# Start full stack
docker compose up --build

# Test endpoints
curl http://localhost:8000/health
curl -X POST http://localhost:8000/api/shorten \
  -H "Content-Type: application/json" \
  -d '{"url":"https://example.com"}'
curl -I http://localhost:8000/{short_code}
curl http://localhost:8000/api/urls
curl http://localhost:8000/api/urls/{short_code}/stats

# Verify Temporal workflows in Web UI
# Open http://localhost:8233 in browser
```

## Dependencies

**New (`requirements.txt`):** `temporalio>=1.7.0`

**Docker images:** `temporalio/auto-setup:latest` (Temporal server with auto-provisioned namespace)

## Key Design Decisions

1. **All 5 operations wrapped** — not just writes. Reads get retry policies too (3 attempts vs 5 for writes), making the app resilient to transient DB connection issues.
2. **Fire-and-forget click recording** — `start_workflow` instead of `execute_workflow` keeps redirect latency the same as before (no round-trip through Temporal for the click write).
3. **Idempotency via `click_id` UUID** — Temporal retries won't duplicate clicks. The `click_id` is generated once at request time and carried through the entire workflow.
4. **Timestamp at request time** — `clicked_at` captured in the router (not DB `server_default`), so retries record the original click time, not the retry time.
5. **Activities use `async_session()` directly** — not FastAPI's `Depends(get_db)`, since activities run in the worker process, not inside a FastAPI request lifecycle.
6. **`workflow.unsafe.imports_passed_through()`** — required for importing activity references in workflow files without breaking Temporal's determinism sandbox.
7. **Service layer unchanged** — the existing `url_service.py` functions are still valid and could be used for testing or local dev without Temporal. Activities duplicate some logic to avoid coupling to the service layer's `AsyncSession` dependency pattern.
8. **Shared Postgres** — Temporal server reuses the app's Postgres instance (separate databases), simplifying the dev setup.
