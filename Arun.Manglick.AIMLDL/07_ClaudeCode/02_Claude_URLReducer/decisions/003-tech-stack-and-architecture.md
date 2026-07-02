# Decision 003: Tech Stack and Architecture

**Date:** 2026-07-02  
**Status:** Accepted

## Context
Need to choose the technology stack for the URL shortener application. Requirements include REST API backend, simple frontend UI, local Postgres via Docker Compose, click analytics, no auth, and no vanity URLs.

## Decision
- **Backend:** Python FastAPI (async) with SQLAlchemy 2.0 async + asyncpg, Alembic for migrations
- **Frontend:** React 18 SPA with Vite build tooling, Recharts for analytics charts
- **Database:** PostgreSQL 16 via Docker Compose
- **Architecture:** Service layer pattern — routers handle HTTP, services handle business logic
- **Short codes:** 7-character base62 (62^7 ≈ 3.5T combinations), generated with `secrets.choice()`
- **Redirects:** 307 Temporary Redirect (not 301) to prevent browser caching
- **Click tracking:** Background task in redirect handler for minimal latency
- **Denormalized click_count** on `urls` table for fast list queries

## Consequences
- Async throughout means all DB operations must use async sessions.
- Denormalized click_count requires atomic increment on every click.
- 307 redirects mean browsers will hit the server on every visit (enables analytics but adds load).
