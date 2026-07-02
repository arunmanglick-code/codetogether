# /plan
@import "./01_planprompt.md"

## Goal
Extend the existing **URL shortener application** by integrating **Temporal** to make the system failure‑proof.  
Use `temporal-skills` to enforce best practices for durable workflows, retries, compensation logic, and scheduling.  
Ensure the application continues to use MCP tools and guardrails — no random code generation or guesswork.

## Instructions
1. Identify the required `temporal-skills` for:
   - Reliable workflow orchestration
   - Automatic retries for failed DB/API operations
   - Compensation logic for partial failures
   - Scheduling for background tasks (e.g., analytics logging, cleanup)
2. Integrate Temporal into the current architecture:
   - Backend API → wrapped in Temporal workflows
   - Database (Postgres via Docker Compose) → durable transactions with retries
   - Frontend → resilient calls to backend through Temporal
3. Ensure guardrails are applied to all inputs and outputs.
4. Use MCP tools for all integrations — avoid ad‑hoc code generation.
5. Ask clarifying questions if requirements are ambiguous.

## Specs
- Backend: REST API wrapped in Temporal workflows
- Frontend: UI resilient to backend failures via Temporal orchestration
- Database: Local Postgres with Docker Compose, durable transactions
- Reliability: Guardrails + Temporal retries, compensation, scheduling
- Deployment: Dockerized services with MCP integration

## Clarifications
Before proceeding, confirm:
- Should Temporal workflows cover only DB operations, or also backend/frontend retries?
- Should analytics (click counts, timestamps) be scheduled via Temporal?
- Should cleanup tasks (expired URLs) be managed by Temporal workflows?
