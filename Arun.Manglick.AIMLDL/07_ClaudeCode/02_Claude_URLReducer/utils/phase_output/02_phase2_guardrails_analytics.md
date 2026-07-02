# Phase 2 Output: Backend Guardrails + Analytics

**Date:** 2026-07-02  
**Status:** Complete

## Files Created
- `backend/app/utils/validators.py` — URL safety validator (SSRF prevention: blocks private IPs, non-http(s) schemes, oversized URLs)
- `backend/app/middleware/error_handler.py` — Global exception handlers for validation errors (422) and unhandled exceptions (500)
- `backend/app/middleware/rate_limiter.py` — In-memory sliding-window rate limiter per client IP (default 60 req/min, returns 429 + Retry-After)

## Files Modified
- `backend/app/main.py` — Registered RateLimiterMiddleware, validation_exception_handler, generic_exception_handler
- `backend/app/schemas/url.py` — Replaced basic scheme check with full `is_safe_url()` validator (SSRF protection)

## Guardrails Summary
| Guardrail | Implementation | Location |
|-----------|---------------|----------|
| URL validation | Pydantic HttpUrl + is_safe_url() | schemas/url.py, utils/validators.py |
| SSRF prevention | Block private/reserved IP ranges | utils/validators.py |
| Rate limiting | Sliding window per IP, 60 req/min | middleware/rate_limiter.py |
| Error handling | Consistent JSON error responses | middleware/error_handler.py |
| Short code validation | Regex `^[0-9a-zA-Z]{6,8}$` | routers/redirect.py |
| SQL injection prevention | SQLAlchemy ORM (parameterized queries) | services/url_service.py |
| Safe redirects | 307 Temporary Redirect | routers/redirect.py |
| Click recording | Background task (non-blocking) | routers/redirect.py |

## Analytics
- Click tracking already implemented in Phase 1 (background task in redirect handler)
- Stats endpoint (`GET /api/urls/{code}/stats`) already implemented in Phase 1
- Click schema (`schemas/click.py`) already created in Phase 1
