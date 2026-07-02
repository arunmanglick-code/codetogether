# /plan
@import "./01_planprompt.md"

## Goal
Use `/find-skills` to identify the required skills to build a **URL shortener application**.  
The application must be reliable, with APIs, backend, and frontend all implemented using the right Claude Code skills.  
Database should be **local Postgres** managed via **Docker Compose**.  
Prefer skills that include **guardrails** for safety and reliability.

## Instructions
1. Run `/find-skills` to discover skills for:
   - Backend API development
   - Frontend UI
   - Database integration (Postgres with Docker Compose)
   - Guardrails for input validation and safe execution
   - Deployment and monitoring
2. Use **MCP tools** instead of random code generation — ensure all integrations are skill‑based.
3. Never guess — if a skill is unclear, ask explicit questions.
4. Plan the application architecture with identified skills, ensuring modularity and reliability.

## Specs
- Backend: REST API with guardrails
- Frontend: Simple UI for shortening and resolving URLs
- Database: Local Postgres via Docker Compose
- Reliability: Guardrails for validation, error handling, and monitoring
- Deployment: Dockerized services with MCP integration

## Clarifications
Ask necessary questions before proceeding:
- Should analytics (click counts, timestamps) be included?
- Should shortened URLs be customizable (vanity links)?
- Should authentication/authorization be part of MVP?
