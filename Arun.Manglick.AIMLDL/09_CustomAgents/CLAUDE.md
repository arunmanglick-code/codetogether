# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This repository serves two purposes:
1. A **Spring Boot 4.0 REST API** (`customagent/`) — a Student CRUD service backed by MySQL and JPA
2. A showcase of **VS Code Copilot Custom Agents** (`.github/agents/`) — an orchestrated code review pipeline that delegates to specialized subagents for Confluence publishing, JIRA ticketing, and PR creation

## Build & Run Commands

All commands run from the `customagent/` directory. Uses Maven Wrapper (no global Maven install required).

```bash
# Build (skip tests)
./mvnw clean package -DskipTests

# Build with tests (requires MySQL running)
./mvnw clean package

# Run the application
./mvnw spring-boot:run

# Run a single test class
./mvnw test -Dtest=CustomagentApplicationTests

# Run a single test method
./mvnw test -Dtest=CustomagentApplicationTests#contextLoads
```

On Windows, use `mvnw.cmd` instead of `./mvnw`.

## Prerequisites

- **Java 17**
- **MySQL** running on `localhost:3306` with a database named `student_tracker` (user: `root`, password: `admin`)
- Hibernate auto-creates/updates the `student` table via `ddl-auto=update`

## Architecture

### Spring Boot Application (`customagent/`)

Standard layered architecture with a **manual DAO pattern** (not using Spring Data JPA's repository methods for the active code path):

```
Controller → Service (interface) → DAO (interface) → EntityManager (JPA)
```

- **Controller layer**: `StudentController` handles CRUD at `/student/**`. Uses `StudentRequest` DTO with Bean Validation for create/update input.
- **Service layer**: `StudentServiceImpl` delegates directly to the DAO with no additional business logic.
- **DAO layer**: `StudentDAOImpl` uses JPA `EntityManager` directly for queries, persist, merge, and remove. Pagination is manual (`setFirstResult`/`setMaxResults`).
- **Entity**: `Student` maps to the `student` table with fields: id, first_name, last_name, email, age, status.
- **`StudentRepository`** (Spring Data JPA) exists but is **not wired into the active service/DAO chain** — the app uses the manual DAO pattern instead.

### REST Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/student/list?page=0&size=20` | Paginated list |
| GET | `/student/{id}` | Get by ID |
| POST | `/student/add` | Create (validated DTO body) |
| PUT | `/student/{id}` | Update (validated DTO body) |
| DELETE | `/student/{id}` | Delete |

### Copilot Custom Agents (`.github/agents/`)

An orchestrator-delegate pattern for automated code reviews:

- **am-code-review-orchestrator** — lead agent: performs review, then sequentially offers to save feedback, publish to Confluence, create JIRA tickets, raise a PR, or apply fixes. Delegates external actions to subagents.
- **am-code-reviewer** — standalone variant: does everything in one agent (no delegation).
- **am-doc-publisher** — publishes review results to Confluence as ADF-formatted pages. Uses `doc_publisher_skill` for title generation.
- **am-ticket-creator** — creates JIRA Stories in project `CCMMER3` from review findings.
- **am-raise-pr** — creates GitHub PRs for code fixes via GitHub MCP tools.

Review feedback files are saved to `customagent/review-feedback/` with the naming pattern `{FileName}-review-{YYYY-MM-DD}_{HHmmss}.md`.

### Claude Code Agents (`.claude/agents/`)

Claude Code equivalents of the Copilot agents above, using native Claude Code tools and conventions:

- **claude-code-review-orchestrator** — orchestrator that delegates via the `Agent` tool to subagents.
- **claude-code-reviewer** — standalone all-in-one reviewer (no delegation).
- **claude-doc-publisher** — Confluence publisher subagent. Title pattern and skill logic inlined.
- **claude-ticket-creator** — JIRA ticket creator subagent for project `CCMMER3`.
- **claude-raise-pr** — PR creator subagent using `gh` CLI (replaces GitHub MCP).

**Key differences from Copilot agents**:
- MCP servers configured in `.claude/settings.json` (Atlassian SSE — requires token setup).
- GitHub operations use `gh` CLI via `Bash` tool instead of GitHub MCP.
- Skill/spec logic is inlined into agent instructions; spec data contract at `.claude/specs/codereview-ticket.md`.
- Tool-use recording hooks at `.claude/hooks/` (adapted from Copilot `recordToolUse` scripts).

## Conventions

- **File headers**: Every Java source file should begin with a comment block containing Author Name, Created Date, and Updated Date (per `copilot-instructions.md`).
- **`@Transactional`** is applied at the DAO layer (on individual write methods), not at the service layer.
