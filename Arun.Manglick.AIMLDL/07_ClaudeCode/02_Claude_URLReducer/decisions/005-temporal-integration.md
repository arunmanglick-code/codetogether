# Decision 005: Integrate Temporal for Durable Workflows

**Date:** 2026-07-02  
**Status:** Accepted

## Context
Click recording uses fire-and-forget BackgroundTasks — data is silently lost on server crash. DB writes have no automatic retry on transient failures.

## Decision
- Wrap ALL 5 backend operations in Temporal workflows (both reads and writes per user request)
- Use `temporalio/auto-setup` Docker image for local Temporal server
- Add `click_id` UUID column to `clicks` table for idempotency on Temporal retries
- Capture `clicked_at` timestamp at request time (not server_default) for retry accuracy
- Click recording uses `start_workflow` (fire-and-forget) to maintain redirect latency
- Write retries: 5 attempts, 1s→30s exponential backoff
- Read retries: 3 attempts, 500ms→10s exponential backoff
- Single task queue: `url-reducer`
- Worker runs as separate Docker service with same backend image

## Consequences
- All DB operations are durable and automatically retried
- Click data is never lost, even across process restarts
- Adds ~5-20ms latency to read operations (Temporal scheduling overhead)
- Adds 2 new Docker services (temporal, temporal-worker)
- Existing service layer code unchanged — only routers and infrastructure modified
