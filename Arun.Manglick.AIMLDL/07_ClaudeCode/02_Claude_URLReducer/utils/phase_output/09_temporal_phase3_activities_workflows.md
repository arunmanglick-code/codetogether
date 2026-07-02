# Phase Output: Temporal Phase 3 — Activities + Workflows

**Date:** 2026-07-02  
**Status:** Complete

## Changes Made

1. **`backend/app/temporal/activities.py`** — 5 activity functions:
   - `create_short_url_activity` — generates short code, inserts URL, returns output
   - `get_url_by_code_activity` — looks up active URL by short code
   - `list_urls_activity` — paginated list of active URLs
   - `record_click_activity` — inserts click with click_id, atomic click_count increment, IntegrityError-safe dedup
   - `get_url_with_clicks_activity` — URL with all click details (uses selectin relationship)

2. **`backend/app/temporal/workflows.py`** — 5 workflow definitions:
   - `CreateShortUrlWorkflow` — write retry policy (5 attempts, 1s→30s)
   - `GetUrlByCodeWorkflow` — read retry policy (3 attempts, 500ms→10s)
   - `ListUrlsWorkflow` — read retry policy
   - `RecordClickWorkflow` — write retry policy
   - `GetUrlWithClicksWorkflow` — read retry policy
   - All activities: 10s start-to-close timeout

## Design Notes
- Activities use `async_session()` directly (not FastAPI's `get_db` dependency)
- `record_click_activity` catches `IntegrityError` on duplicate `click_id` → returns `recorded=False` (safe dedup)
- Workflow imports use `workflow.unsafe.imports_passed_through()` for determinism
- `clicked_at` is parsed from ISO string (captured at request time for retry accuracy)
