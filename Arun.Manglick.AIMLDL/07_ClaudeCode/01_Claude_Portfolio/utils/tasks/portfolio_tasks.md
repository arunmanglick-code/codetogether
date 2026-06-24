# Portfolio Website Tasks

## Phase 1: Static Portfolio Site

### Setup
- [x] Scaffold Astro 5 project (package.json, astro.config.mjs, tsconfig.json)
- [x] Install and configure Tailwind CSS with dark mode
- [x] Configure Inter font via @fontsource/inter
- [x] Create .gitignore for node_modules, dist, .astro

### Content
- [x] Define content collection schemas (categories + projects)
- [x] Create 14 category markdown files
- [x] Create 18 featured project markdown files
- [x] Create skills.json with 6 technology domains

### Components
- [x] BaseLayout and PageLayout
- [x] Header with navigation and dark mode toggle
- [x] Footer with copyright and GitHub link
- [x] Hero section
- [x] About section
- [x] SkillsGrid and skill badges
- [x] ProjectCard with featured badge and tech stack
- [x] CategoryFilter navigation
- [x] ProjectGrid responsive layout
- [x] FeaturedProjects section
- [x] ContactSection
- [x] TechStack badge component

### Pages
- [x] Home page (index.astro)
- [x] Projects listing (/projects/)
- [x] Category filter pages (/categories/[slug])
- [x] Project detail pages (/project/[slug])
- [x] Custom 404 page

### Infrastructure (Deferred — Docker Desktop not installed)
- [ ] Write Dockerfile (multi-stage: node builder → nginx runtime)
- [ ] Write nginx.conf with gzip, caching, security headers
- [ ] Build and test Docker container
- [ ] Configure and test ngrok tunnel
- [ ] Cross-browser responsive testing

### Documentation
- [x] Create decisions/ folder with 5 decision logs
- [x] Update portfolio_tasks.md
- [ ] Update CLAUDE.md with final project structure

---

## Phase 2: Backend Introduction
- [ ] Switch Astro to SSR mode with @astrojs/node adapter
- [ ] Build Node.js API service (Express/Fastify)
- [ ] Integrate GitHub API via Octokit with caching
- [ ] Update Nginx for reverse proxy
- [ ] Create docker-compose.yml for multi-service setup
- [ ] Replace getCollection() with API fetch() calls

---

## Phase 3: Database Integration
- [ ] Add PostgreSQL container to docker-compose
- [ ] Define database schema (categories, projects, technologies)
- [ ] Set up Prisma/Drizzle ORM
- [ ] Seed database from Phase 1 markdown files
- [ ] Implement CRUD operations
- [ ] Add admin interface

---

## Phase 4: Enhancements
- [ ] Add CI/CD pipeline (GitHub Actions)
- [ ] Add monitoring/logging
- [ ] Lighthouse performance optimization
- [ ] Add Pagefind search
- [ ] Add blog content collection
- [ ] Add resume/CV page
