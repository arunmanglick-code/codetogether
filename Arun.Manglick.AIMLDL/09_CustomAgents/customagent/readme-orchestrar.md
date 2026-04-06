# Custom Code Review Agent — Multi-Agent Architecture

## Overview

This project uses a multi-agent architecture for Java Spring Boot code reviews with Atlassian (Confluence + JIRA) integration via MCP.

---

## Agent Files

| File | Role | User-invocable |
|------|------|----------------|
| `am-code-reviewer.agent.md` | Standalone — reviews, publishes, tickets, fixes all in one | Yes |
| `am-code-only-reviewer.agent.md` | Orchestrator — reviews and delegates to subagents | Yes |
| `am-doc-publisher.agent.md` | Subagent — creates Confluence pages | No (subagent only) |
| `am-ticket-creator.agent.md` | Subagent — creates JIRA tickets in CCMMER3 | No (subagent only) |

---

## Key Design Decisions

- **Minimal tools per agent** — subagents only have `com_atlassian/*`, no file editing or terminal access
- **`user-invocable: false` on subagents** — they only appear as subagents, not in the agent picker
- **Shared context object** — the orchestrator builds a structured review context (`fileName`, `tickets` array, etc.) and passes it to subagents, maintaining traceability
- **Sequential prompting preserved** — the orchestrator still asks one prompt at a time before delegating

---

## Architecture Diagram

### Standalone Path

```
┌──────────────────────────────────────────────────┐
│  am-code-reviewer (Standalone — does everything) │
│  Tools: read, edit, search, execute, MCP         │
│  Does: review + file save + Confluence + JIRA +  │
│        apply fixes — all by itself               │
└──────────────────────────────────────────────────┘
```

### Orchestrated Path

```
┌──────────────────────────────────────────────────┐
│  am-code-only-reviewer (Orchestrator)            │
│  Tools: read, edit, search, execute, agent, todo │
│  Does: review + file save + apply fixes          │
│  Delegates: Confluence → am-doc-publisher        │
│             JIRA → am-ticket-creator             │
└──────────┬─────────────────────┬─────────────────┘
           │                     │
   ┌───────▼────────┐   ┌───────▼──────────┐
   │am-doc-publisher │   │am-ticket-creator │
   │Tools: MCP only  │   │Tools: MCP only   │
   │Subagent only    │   │Subagent only     │
   │→ Confluence     │   │→ JIRA tickets    │
   └─────────────────┘   └──────────────────┘
```


