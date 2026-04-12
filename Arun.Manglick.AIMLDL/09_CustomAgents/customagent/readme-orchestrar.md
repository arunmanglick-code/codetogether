# Custom Code Review Agent — Multi-Agent Architecture

## Overview

This project uses a multi-agent architecture for Java Spring Boot code reviews with Atlassian (Confluence + JIRA) integration via MCP.

---

## Agent Files

| File | Role | User-invocable |
|------|------|----------------|
| `am-code-reviewer.agent.md` | Standalone — reviews, publishes, tickets, fixes all in one | Yes |
| `am-code-review-orchestrator.agent.md` | Orchestrator — reviews and delegates to subagents | Yes |
| `am-doc-publisher.agent.md` | Subagent — creates Confluence pages | No (subagent only) |
| `am-ticket-creator.agent.md` | Subagent — creates JIRA tickets in CCMMER3 | No (subagent only) |
| `am-raise-pr.agent.md` | Subagent — creates GitHub pull requests via MCP | No (subagent only) |

---

## Key Design Decisions

- **Minimal tools per agent** — subagents only have the MCP tools they need (`com_atlassian/*` for Confluence/JIRA, `github/*` for PRs), no unnecessary file editing or terminal access
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
│  am-code-review-orchestrator (Orchestrator)      │
│  Tools: read, edit, search, execute, agent, todo │
│  Does: review + file save + apply fixes          │
│  Delegates: Confluence → am-doc-publisher        │
│             JIRA → am-ticket-creator             │
│             PRs  → am-raise.pr                   │
└──────────┬─────────────┬─────────────┬───────────┘
           │             │             │
   ┌───────▼────────┐ ┌──▼───────────┐ ┌▼─────────────┐
   │am-doc-publisher │ │am-ticket-    │ │am-raise.pr   │
   │Tools: Atlassian │ │creator       │ │Tools: GitHub  │
   │MCP only         │ │Tools:Atlassi-│ │MCP + read,    │
   │Subagent only    │ │an MCP only   │ │edit, search   │
   │→ Confluence     │ │Subagent only │ │Subagent only  │
   └─────────────────┘ │→ JIRA tickets│ │→ Pull Requests│
                       └──────────────┘ └──────────────┘
```
## Orchestration - Behind the Scene

How am-code-review-orchestrator Orchestrates Three Subagents
The connection works through three mechanisms in the YAML frontmatter and the markdown body:

1. Frontmatter agents Declaration (The Wiring)
In am-code-review-orchestrator.agent.md:

This tells VS Code Copilot that this agent is allowed to invoke the three named agents as subagents. It also requires the agent tool in the tools list, which is present:

The agent tool is what gives the orchestrator the ability to call runSubagent to delegate work.

2. Shared Context Object (The Data Contract)
Lines 36-51 of the orchestrator define a review context object — a structured data payload containing fileName, tickets[], reviewDate, etc. Both subagents are designed to consume this same context:

am-doc-publisher.agent.md says: "You receive a review context object from the orchestrator (am-code-review-orchestrator)" and uses it to build a Confluence page.
am-ticket-creator.agent.md says: "You receive a review context object from the orchestrator (am-code-review-orchestrator)" and uses it to create JIRA Stories.
am-raise.pr.agent.md receives the review context (file path, findings, proposed changes) and uses it to create a GitHub pull request.
This shared schema is the implicit contract between the orchestrator and its subagents.

3. Sequential Workflow with User Gating (The Control Flow)
The orchestrator's Workflow section (steps 5-7) defines when delegation happens:

Step	Prompt to User	If Yes → Delegate To
5	"Publish to Confluence?"	am-doc-publisher
6	"Create JIRA tickets?"	am-ticket-creator
7	"Raise a PR?"	am-raise.pr
Each delegation is gated by user approval — the orchestrator asks permission, waits for confirmation, and only then calls the subagent with the review context.

4. Separation of Concerns (The Boundaries)
The Constraints section enforces strict boundaries:

Orchestrator: Does code review, saves files, applies fixes. Explicitly told "DO NOT create Confluence pages directly", "DO NOT create JIRA tickets directly", and "DO NOT create Pull Requests directly".
am-doc-publisher: Only publishes to Confluence. Has tools: ["com.atlassian/atlassian-mcp-server/*"] — restricted to Atlassian MCP tools only.
am-ticket-creator: Only creates JIRA tickets. Same Atlassian-only tool restriction. Also has user-invocable: false, meaning it can only be called by the orchestrator, not directly by the user.
am-raise.pr: Only creates GitHub pull requests. Has tools: [github/*, read, edit, search] — uses GitHub MCP tools for branch creation, file pushing, and PR creation. Also has user-invocable: false, meaning it can only be called by the orchestrator.
Visual Flow
The key insight: the agents: frontmatter property is the declaration, the agent tool is the capability, and the shared review context object is the protocol that ties them together.
