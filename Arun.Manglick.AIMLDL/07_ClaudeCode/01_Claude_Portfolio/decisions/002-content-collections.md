# Decision 002: Content Model — Astro Content Collections

- **Choice**: Astro content collections with Zod schemas and markdown files
- **Reasoning**: Provides build-time schema validation (catches missing fields before deploy), TypeScript autocompletion in components, and markdown body for rich project descriptions. Clean migration path to Phase 2: `getCollection()` calls swap to `fetch()` API calls without changing component interfaces.
- **Alternatives Considered**: Raw JSON files (no validation, no markdown body), headless CMS like Contentful (adds external dependency, overkill for 20 projects), GitHub API at build time (rate limits, complex caching).
- **Timestamp**: 2025-01-15
- **Phase**: Phase 1 — Static Portfolio Setup
- **Verification**: `astro sync` validates all content files against schemas. Build fails on invalid frontmatter.
