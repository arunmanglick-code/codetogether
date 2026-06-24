# Decision 003: Styling — Tailwind CSS

- **Choice**: Tailwind CSS with class-based dark mode strategy
- **Reasoning**: Utility-first approach speeds up UI development. Official Astro integration (`@astrojs/tailwind`). Dark mode via `class` strategy allows user-controlled toggle with localStorage persistence. Widely adopted — easier for future contributors.
- **Alternatives Considered**: Vanilla CSS with custom properties (full control but slower iteration), CSS modules (good scoping but no utility system), styled-components (requires React runtime).
- **Timestamp**: 2025-01-15
- **Phase**: Phase 1 — Static Portfolio Setup
- **Verification**: Tailwind classes render correctly in dev and production. Dark mode toggle switches theme.
