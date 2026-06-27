# Portfolio Website — Phase-Wise Implementation Plan

## Context

Build a portfolio website to showcase GitHub projects from [codetogether](https://github.com/arunmanglick-code/codetogether). The repo contains 14+ project categories (AI/ML, Java/Spring Boot, React, Node.js, AWS Lambda, Terraform, Docker, CI/CD, etc.) with 70+ sub-projects. The site uses **Astro 5 + Tailwind CSS + Nginx + ngrok**, deployed in Docker. Phase 1 is a static site with no backend; later phases add API, database, and CI/CD.

**Key decisions from user:**
- Start with **15-20 featured projects**, add the rest incrementally
- Use **Tailwind CSS** (not vanilla CSS)
- **Docker Desktop not installed yet** — Phase 1 focuses on local dev; Docker containerization is a follow-up step

---

## Phase 1: Static Portfolio Site

### Step 1 — Scaffold Astro 5 project
- Run `npm create astro@latest` with minimal template, TypeScript strict
- Install Tailwind: `npx astro add tailwind`
- Configure `astro.config.mjs` for `output: 'static'`, set `outDir: './dist'`
- Verify: `npm run dev` serves on localhost:4321

### Step 2 — Project structure & content model
Create this directory layout inside `src/`:

```
src/
├── content/
│   ├── config.ts          # Astro content collection schemas (Zod)
│   ├── categories/        # 14 markdown files (one per category)
│   └── projects/          # 15-20 featured project markdown files
├── layouts/
│   ├── BaseLayout.astro   # HTML head, meta, global styles, footer
│   └── PageLayout.astro   # Wraps BaseLayout + nav + content area
├── components/
│   ├── global/            # Header, Footer, ThemeToggle, SEOHead
│   ├── home/              # Hero, About, SkillsGrid, FeaturedProjects, ContactSection
│   └── projects/          # ProjectCard, CategoryFilter, ProjectGrid, TechStack
├── pages/
│   ├── index.astro        # Home page
│   ├── projects/index.astro        # All projects + category filter
│   ├── categories/[slug].astro     # Category-filtered view
│   └── project/[slug].astro        # Project detail page
├── styles/
│   └── global.css         # Tailwind directives + custom properties for theming
├── data/
│   └── skills.json        # Technology taxonomy
└── utils/
    └── constants.ts       # Site metadata, social links
```

**Content collection schemas** (`src/content/config.ts`):
- `categories` collection: title, slug, description, icon, color, displayOrder
- `projects` collection: title, slug, category (ref), description, technologies[], repoPath, githubUrl, featured, status, dateAdded, highlights[]

### Step 3 — Author content files
- Write 14 category markdown files (AI/ML, Java, React, Node.js, Angular, Lambda, Terraform, Docker, CI/CD, Datadog, Grafana, Test Automation, AWS Certs, Legacy .NET)
- Write 15-20 featured project markdown files prioritizing high-impact projects:
  - CrewAI Agents, Graphiti Knowledge Graphs, LangGraph Agents
  - Spring Boot REST/Security, Apache Camel
  - React apps, Node.js AWS Accelerators
  - Lambda DynamoDB, S3-to-Kinesis
  - Terraform modules, Cypress testing
- Create `src/data/skills.json` with skills grouped by domain

### Step 4 — Build components
Components use Tailwind utility classes. Key components:

| Component | Purpose |
|-----------|---------|
| `Header.astro` | Site nav with links to Home, Projects, dark/light toggle |
| `Footer.astro` | Copyright, GitHub link |
| `Hero.astro` | Full-width intro: name, title, tagline, CTA to /projects |
| `About.astro` | 2-3 paragraph professional summary |
| `SkillsGrid.astro` | Skills grouped by domain (Frontend, Backend, Cloud, AI/ML) |
| `ProjectCard.astro` | Card: title, description (2-line clamp), tech badges, category badge, GitHub link |
| `CategoryFilter.astro` | 14 category link buttons + "All" (no JS — uses page navigation) |
| `ProjectGrid.astro` | Responsive grid: 3 cols desktop, 2 tablet, 1 mobile |
| `FeaturedProjects.astro` | Shows projects with `featured: true` on home page |
| `ContactSection.astro` | GitHub, LinkedIn, email links |
| `ThemeToggle.astro` | Light/dark mode via CSS custom properties + localStorage |

### Step 5 — Build pages
- **Home** (`/`): Hero + About + SkillsGrid + FeaturedProjects + Contact
- **Projects** (`/projects/`): CategoryFilter + ProjectGrid (all projects)
- **Category** (`/categories/[slug]`): CategoryFilter (active state) + filtered ProjectGrid
- **Project detail** (`/project/[slug]`): Breadcrumb, title, tech stack, full description, highlights, related projects
- **404**: Custom styled error page

### Step 6 — Tailwind configuration
- Configure `tailwind.config.mjs` with custom color palette, dark mode (`class` strategy)
- Add Inter font via `@fontsource/inter` (self-hosted, no Google Fonts dependency)
- CSS custom properties in `global.css` for theme colors that Tailwind references
- Responsive design: mobile-first with `sm:`, `md:`, `lg:` breakpoints

### Step 7 — Docker + Nginx (when Docker Desktop is installed)
- **Dockerfile** (multi-stage): `node:22-alpine` builder → `nginx:1.27-alpine` runtime
- **nginx.conf**: serve from `/usr/share/nginx/html`, gzip, cache `/_assets/` with immutable, security headers, `/health` endpoint, `try_files` for Astro's static output
- Build: `docker build -f docker/Dockerfile -t portfolio:latest .`
- Run: `docker run -p 8080:80 portfolio:latest`
- Until Docker is ready: use `npm run preview` for local production testing

### Step 8 — ngrok tunnel
- `ngrok http 8080 --host-header=localhost:8080` (or port 4321 for dev mode)
- Note: free tier has ephemeral URLs and interstitial page

### Step 9 — Decision logs & task tracking
- Create `decisions/` folder with 5 initial decision logs:
  1. `001-framework-astro5.md` — Astro 5 over Next.js/Gatsby/Hugo
  2. `002-content-collections.md` — Astro collections over raw JSON/headless CMS
  3. `003-tailwind-css.md` — Tailwind over vanilla CSS
  4. `004-docker-multistage.md` — Multi-stage build rationale
  5. `005-nginx-config.md` — Nginx over alternatives
- Update `portfolio_tasks.md` with actionable, checkable tasks

### Phase 1 Risks
| Risk | Mitigation |
|------|------------|
| Docker Desktop unavailable | Use `npm run preview` for local testing; Docker step is independent and can be done later |
| Content authoring for 15-20 projects takes time | Start with 5 highest-impact projects, iterate |
| Astro 5 + Tailwind integration issues | Both are officially supported; use `npx astro add tailwind` |

### Phase 1 Verification
- `npm run build` succeeds with zero errors
- `npm run preview` serves the site locally
- All category pages render with correct filtered projects
- Project detail pages show full metadata
- Site is responsive at 375px, 768px, 1440px widths
- Light/dark toggle works and persists
- (When Docker ready) Container serves site on port 8080
- (When Docker ready) ngrok produces working public URL

---

## Phase 2: Backend Introduction (High-Level)

**Goal:** Replace static content collections with a live API; switch Astro to SSR mode.

- Switch `astro.config.mjs` to `output: 'server'` + `@astrojs/node` adapter
- Build Node.js API (Express/Fastify): `GET /api/categories`, `GET /api/projects`, `GET /api/projects/:slug`
- Integrate GitHub API via Octokit (fetch READMEs, commit dates) with 5-min cache
- Update Nginx: reverse proxy `/api/` to backend:3000, `/` to astro:4321
- Containerize backend; use `docker-compose.yml` for multi-service orchestration
- Replace `getCollection()` calls in pages with `fetch()` to API — component props unchanged

**Risk:** GitHub API rate limits (60/hr unauth, 5000/hr with token) — mitigated by server-side caching.

---

## Phase 3: Database Integration (High-Level)

**Goal:** Persist project metadata in PostgreSQL; enable CRUD via admin interface.

- Add `postgres:16-alpine` container to docker-compose
- Schema: `categories`, `projects`, `technologies`, `project_technologies` (join), `project_highlights`
- Use Prisma or Drizzle ORM with TypeScript
- Seed database from Phase 1 markdown files (migration script)
- Backend reads from DB instead of GitHub API (GitHub API becomes a sync source)
- Add admin routes for CRUD operations

---

## Phase 4: Enhancements (High-Level)

- **CI/CD**: GitHub Actions for lint/build on PR, Docker image push on merge to main
- **Monitoring**: Structured Nginx logs, `/api/health` endpoint, optional Prometheus metrics
- **Performance**: Lighthouse 90+ target, Astro `<Image>` for WebP/AVIF, Pagefind for client-side search
- **Features**: Blog section (content collection), resume/CV page, privacy-friendly analytics (Plausible/Umami)

---

## Execution Order (Phase 1)

Steps 1-2 first (scaffolding + structure), then Step 3 (content) and Step 4 (components) in parallel, then Step 5 (pages), Step 6 (Tailwind polish), Step 9 (docs). Steps 7-8 (Docker/ngrok) when Docker Desktop is available.

**Critical files to create/modify:**
- `astro.config.mjs`, `tailwind.config.mjs`, `tsconfig.json`, `package.json` (new from scaffold)
- `src/content/config.ts` (collection schemas)
- `src/content/categories/*.md` (14 files)
- `src/content/projects/*.md` (15-20 files)
- `src/layouts/*.astro` (2-3 files)
- `src/components/**/*.astro` (~12 files)
- `src/pages/**/*.astro` (5-6 files)
- `docker/Dockerfile`, `docker/nginx.conf` (deferred)
- `decisions/*.md` (5 files), `portfolio_tasks.md`
- Update `CLAUDE.md` after scaffolding
