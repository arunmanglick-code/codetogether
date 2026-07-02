# Plan: Install Skills for Security, Reliability & Robustness

## Context

Your Astro 5 + Nginx + Docker portfolio project currently has **2 skills installed** (`find-skills`, `github-issues`). The project has a solid local dev setup but gaps in: testing, CI/CD, accessibility, web performance, and security hardening. This plan recommends high-value skills from the open skills ecosystem to close those gaps.

## Current State

- **Installed skills:** `find-skills` (vercel-labs/skills), `github-issues` (custom project skill)
- **Present:** TypeScript strict mode, Nginx security headers, post-edit code review hook, Docker multi-stage build
- **Missing:** No linting, no tests, no CI/CD pipeline, no accessibility checks, no dependency auditing

---

## Recommended Skills to Install

### Tier 1: High Priority (Directly addresses security, reliability, robustness)

| # | Skill | Source | Installs | Why |
|---|-------|--------|----------|-----|
| 1 | **webapp-testing** | `anthropics/skills` | 105.7K | Web app testing patterns — your project has zero tests |
| 2 | **tdd** | `mattpocock/skills` | 318.9K | Test-driven development guidance from Matt Pocock |
| 3 | **github-actions-docs** | `xixu-me/skills` | 254.1K | GitHub Actions reference — you have no CI/CD pipeline |
| 4 | **verification-before-completion** | `obra/superpowers` | 124.8K | Forces verification that changes actually work before marking done |
| 5 | **accessibility** | `addyosmani/web-quality-skills` | 32.4K | Web accessibility best practices (Addy Osmani / Google) |
| 6 | **skill-vetter** | `useai-pro/openclaw-skills-security` | 20K | Audits skills themselves for security before you install them |

**Install commands:**
```bash
npx skills add anthropics/skills@webapp-testing -g -y
npx skills add mattpocock/skills@tdd -g -y
npx skills add xixu-me/skills@github-actions-docs -g -y
npx skills add obra/superpowers@verification-before-completion -g -y
npx skills add addyosmani/web-quality-skills@accessibility -g -y
npx skills add useai-pro/openclaw-skills-security@skill-vetter -g -y
```

### Tier 2: Recommended (Improves quality and developer experience)

| # | Skill | Source | Installs | Why |
|---|-------|--------|----------|-----|
| 7 | **astro** | `astrolicious/agent-skills` | 9.4K | Astro-specific patterns and best practices (below 10K installs — newer skill, evaluate after install) |
| 8 | **grill-me** | `mattpocock/skills` | 410.7K | Adversarial code review — finds issues your review hook might miss |
| 9 | **improve-codebase-architecture** | `mattpocock/skills` | 337.8K | Architecture improvement suggestions |
| 10 | **diagnose** | `mattpocock/skills` | 230.7K | Systematic debugging methodology |
| 11 | **systematic-debugging** | `obra/superpowers` | 163.6K | Structured approach to finding and fixing bugs |
| 12 | **dependency-upgrade** | `wshobson/agents` | 8K | Safe dependency upgrade workflows (lower install count — community skill, vet before relying on it) |

**Install commands:**
```bash
npx skills add astrolicious/agent-skills@astro -g -y
npx skills add mattpocock/skills@grill-me -g -y
npx skills add mattpocock/skills@improve-codebase-architecture -g -y
npx skills add mattpocock/skills@diagnose -g -y
npx skills add obra/superpowers@systematic-debugging -g -y
npx skills add wshobson/agents@dependency-upgrade -g -y
```

### Tier 3: Nice to Have (Design quality and stack-specific)

| # | Skill | Source | Installs | Why |
|---|-------|--------|----------|-----|
| 13 | **tailwind-design-system** | `wshobson/agents` | 51.4K | Tailwind design system patterns (you use Tailwind CSS) |
| 14 | **web-design-guidelines** | `vercel-labs/agent-skills` | 423.1K | General web design best practices |
| 15 | **docker-expert** | `sickn33/antigravity-awesome-skills` | 21.6K | Docker best practices (you have Docker in your stack) |
| 16 | **fixing-accessibility** | `ibelick/ui-skills` | 13.8K | Practical accessibility fixes for UI components |
| 17 | **github-actions-templates** | `wshobson/agents` | 12.2K | Ready-made CI/CD templates for GitHub Actions |

**Install commands:**
```bash
npx skills add wshobson/agents@tailwind-design-system -g -y
npx skills add vercel-labs/agent-skills@web-design-guidelines -g -y
npx skills add sickn33/antigravity-awesome-skills@docker-expert -g -y
npx skills add ibelick/ui-skills@fixing-accessibility -g -y
npx skills add wshobson/agents@github-actions-templates -g -y
```

---

## Skills NOT Recommended (Searched but filtered out)

| Skill | Reason Skipped |
|-------|----------------|
| Firebase/Golang/Flutter security skills | Wrong stack |
| Nginx C module skills | Too low-level for this project |
| Azure/Microsoft skills | No Azure deployment planned |
| Generic "code quality" skills (<1K installs) | Too few installs, unverified quality |
| `openclaw-secure-linux-cloud` (249.5K) | Linux server hardening — targets bare-metal/VM Linux deployments, not applicable to a Dockerized static site behind Nginx |

---

## Quick-Reference Links

All skills can be browsed at their `skills.sh` pages:

1. https://skills.sh/anthropics/skills/webapp-testing
2. https://skills.sh/mattpocock/skills/tdd
3. https://skills.sh/xixu-me/skills/github-actions-docs
4. https://skills.sh/obra/superpowers/verification-before-completion
5. https://skills.sh/addyosmani/web-quality-skills/accessibility
6. https://skills.sh/useai-pro/openclaw-skills-security/skill-vetter
7. https://skills.sh/astrolicious/agent-skills/astro
8. https://skills.sh/mattpocock/skills/grill-me
9. https://skills.sh/mattpocock/skills/improve-codebase-architecture
10. https://skills.sh/mattpocock/skills/diagnose
11. https://skills.sh/obra/superpowers/systematic-debugging
12. https://skills.sh/wshobson/agents/dependency-upgrade
13. https://skills.sh/wshobson/agents/tailwind-design-system
14. https://skills.sh/vercel-labs/agent-skills/web-design-guidelines
15. https://skills.sh/sickn33/antigravity-awesome-skills/docker-expert
16. https://skills.sh/ibelick/ui-skills/fixing-accessibility
17. https://skills.sh/wshobson/agents/github-actions-templates

---

## Verification

After installing, run:
```bash
npx skills check          # verify all skills are up-to-date
cat skills-lock.json      # confirm skills are registered
ls .claude/skills/        # confirm skill files are present
```

## Execution Plan

1. Pick which tiers (1/2/3) you want to install
2. Run the install commands (I'll execute them for you)
3. Verify installation with `npx skills check`
4. Optionally commit the updated `skills-lock.json` and `.claude/skills/` changes
