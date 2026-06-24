# Decision 004: Containerization — Multi-Stage Docker Build

- **Choice**: Two-stage Dockerfile: `node:22-alpine` (builder) → `nginx:1.27-alpine` (runtime)
- **Reasoning**: Builder stage contains Node.js and dev dependencies (~300+ MB). Final image has only Nginx and static files (~50 MB). No Node.js in production container reduces attack surface. Alpine variants minimize image size.
- **Alternatives Considered**: Single-stage with Node.js serving (larger image, unnecessary runtime), pre-built dist committed to repo (clutters git history), Caddy instead of Nginx (less ecosystem support).
- **Timestamp**: 2025-01-15
- **Phase**: Phase 1 — Static Portfolio Setup (deferred until Docker Desktop available)
- **Verification**: `docker build` completes. Final image under 60 MB. Container serves site on port 80.
