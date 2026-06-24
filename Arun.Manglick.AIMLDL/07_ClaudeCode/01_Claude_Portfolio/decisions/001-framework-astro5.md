# Decision 001: Frontend Framework — Astro 5

- **Choice**: Astro 5 (static site generator with SSR capability)
- **Reasoning**: Zero-JavaScript-by-default output keeps the portfolio fast. SSG-to-SSR is a one-line config change (`output: 'static'` → `output: 'server'`), making Phase 2 backend integration seamless. Content collections provide type-safe markdown handling. Astro's island architecture allows adding interactivity only where needed.
- **Alternatives Considered**: Next.js (heavier, full React runtime shipped), Gatsby (slower builds, complex data layer), Hugo (Go templating less flexible).
- **Timestamp**: 2025-01-15
- **Phase**: Phase 1 — Static Portfolio Setup
- **Verification**: `npm run build` produces static HTML in `/dist`. Dev server runs on localhost:4321.
