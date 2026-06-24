# Decision 005: Web Server — Nginx

- **Choice**: Nginx 1.27 Alpine as static file server and future reverse proxy
- **Reasoning**: Production-grade performance for static files. Gzip compression and cache headers reduce load times. `try_files` handles Astro's clean URL structure. In Phase 2, same Nginx instance becomes a reverse proxy to the Astro SSR server and backend API — no new infrastructure needed.
- **Alternatives Considered**: Caddy (simpler config but less control), `serve` npm package (not production-grade), direct Node.js serving (wastes resources for static files).
- **Timestamp**: 2025-01-15
- **Phase**: Phase 1 — Static Portfolio Setup
- **Verification**: `nginx -t` validates config. Site accessible on port 80 inside container. `/health` returns 200.
