# Phase Output: Temporal Phase 5 — Wire Routers

**Date:** 2026-07-02  
**Status:** Complete

## Changes Made

1. **`backend/app/routers/urls.py`** — Rewrote all 3 endpoints:
   - `POST /api/shorten` → `execute_workflow(CreateShortUrlWorkflow)`
   - `GET /api/urls` → `execute_workflow(ListUrlsWorkflow)`
   - `GET /api/urls/{code}/stats` → `execute_workflow(GetUrlWithClicksWorkflow)`
   - Removed `Depends(get_db)`, `AsyncSession`, all direct service imports

2. **`backend/app/routers/redirect.py`** — Rewrote redirect endpoint:
   - URL lookup: `execute_workflow(GetUrlByCodeWorkflow)` — waits for result
   - Click recording: `start_workflow(RecordClickWorkflow)` — fire-and-forget
   - Generates `click_id` UUID and captures `clicked_at` at request time
   - Removed `BackgroundTasks`, `Depends(get_db)`, `_record_click_background`, all direct service imports

## Workflow ID Patterns
- `create-url-{uuid}` — unique per creation
- `list-urls-{uuid}` — unique per list request
- `stats-{short_code}-{uuid}` — unique per stats request
- `get-url-{short_code}-{uuid}` — unique per redirect lookup
- `click-{click_id}` — click_id IS the idempotency key
