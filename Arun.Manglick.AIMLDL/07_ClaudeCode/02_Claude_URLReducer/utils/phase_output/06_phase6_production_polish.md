# Phase 6 Output: Production Docker + Polish

**Date:** 2026-07-02  
**Status:** Complete

## Files Created
- `frontend/nginx.conf` — Nginx config for production: proxies `/api/` and short code paths to backend, SPA fallback for client routes

## Files Updated
- `CLAUDE.md` — Updated with full architecture, development commands, API endpoints, key files, and environment variables

## Production Stack
When running `docker compose up --build`:
- **Postgres** on port 5432 (with healthcheck)
- **Backend** on port 8000 (runs Alembic migrations on startup, then uvicorn)
- **Frontend** on port 3000 (nginx serves React build, proxies API + short code requests to backend)
