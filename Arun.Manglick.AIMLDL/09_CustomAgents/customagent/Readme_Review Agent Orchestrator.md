# Agentic AI Code Review with GitHub Copilot Custom Agents

---

## 🚀 Automated Code Reviews with Agentic AI — Powered by GitHub Copilot Custom Agents, Specs & MCP Servers

I recently built an **Agentic AI solution** that fully automates Java Spring Boot code reviews — from analysis to documentation to ticketing to pull requests — all orchestrated inside VS Code using **GitHub Copilot's Custom Agents**.

### 💡 The Problem

Code reviews are essential but time-consuming. The feedback loop — review → document → create tickets → raise PRs — involves multiple tools and manual handoffs.

### 🧠 The Solution: A Multi-Agent Orchestration Architecture

I designed an **Orchestrator Agent** (`am-code-review-orchestrator`) that is itself the **code review agent** and coordinates **three specialized sub-agents**, each with a single responsibility:

```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│       🧠 ORCHESTRATOR — am-code-review-orchestrator         │
│       (Also the Code Review Agent)                          │
│                                                             │
│   ✅ Performs layered code review                           │
│      (Correctness → Security → Performance →                │
│       Maintainability)                                      │
│   ✅ Saves structured review feedback locally               │
│   ✅ Applies code fixes directly                            │
│   ✅ Builds shared "review context" object                  │
│   ✅ Delegates downstream work to sub-agents                │
│                                                             │
└──────────┬──────────────────┬──────────────────┬────────────┘
           │                  │                  │
           │ Delegate         │ Delegate         │ Delegate
           ▼                  ▼                  ▼
┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐
│                  │ │                  │ │                  │
│ 📄 Sub-Agent 1   │ │ 🎫 Sub-Agent 2   │ │ 🔀 Sub-Agent 3   │
│ am-doc-publisher │ │ am-ticket-creator│ │ am-raise.pr      │
│                  │ │                  │ │                  │
│ Publishes review │ │ Creates JIRA     │ │ Creates branch,  │
│ to Confluence    │ │ Story tickets    │ │ pushes fixes &   │
│ (ADF format)     │ │ per finding      │ │ raises a PR on   │
│                  │ │ in CCMMER3       │ │ GitHub           │
│                  │ │                  │ │                  │
│ 🔌 Atlassian MCP │ │ 🔌 Atlassian MCP │ │ 🔌 GitHub MCP     │
└──────────────────┘ └──────────────────┘ └──────────────────┘
```

---

### 🔄 End-to-End Orchestration Flow

```
Developer invokes @am-code-review-orchestrator
            │
            ▼
   ┌─────────────────────┐
   │ 1. Load Java Spring │
   │    Boot source file  │
   └─────────┬───────────┘
             ▼
   ┌─────────────────────┐
   │ 2. Perform layered  │
   │    code review       │
   │  (Correctness →      │
   │   Security →         │
   │   Performance →      │
   │   Maintainability)   │
   └─────────┬───────────┘
             ▼
   ┌─────────────────────┐
   │ 3. Build shared     │
   │    Review Context    │
   │  {fileName, tickets, │
   │   strengths, date}   │
   └─────────┬───────────┘
             ▼
   ┌─────────────────────┐
   │ 4. Prompt: "Save    │───Yes──▶ Save to review-feedback/
   │    feedback to file?"│         {File}-review-{date}_{time}.md
   └─────────┬───────────┘
             ▼
   ┌─────────────────────┐         ┌─────────────────────────┐
   │ 5. Prompt: "Publish │───Yes──▶│ DELEGATE to             │
   │    to Confluence?"   │         │ 📄 am-doc-publisher      │
   └─────────┬───────────┘         │ (via Atlassian MCP)     │
             │                     │ → Creates Confluence page│
             ▼                     └─────────────────────────┘
   ┌─────────────────────┐         ┌─────────────────────────┐
   │ 6. Prompt: "Create  │───Yes──▶│ DELEGATE to             │
   │    JIRA tickets?"    │         │ 🎫 am-ticket-creator     │
   └─────────┬───────────┘         │ (via Atlassian MCP)     │
             │                     │ → Creates Story per      │
             ▼                     │   finding in CCMMER3    │
   ┌─────────────────────┐        └─────────────────────────┘
   │ 7. Prompt: "Raise   │         ┌─────────────────────────┐
   │    a PR?"            │───Yes──▶│ DELEGATE to             │
   └─────────┬───────────┘         │ 🔀 am-raise.pr           │
             │                     │ (via GitHub MCP)        │
             ▼                     │ → Creates branch, pushes│
   ┌─────────────────────┐        │   code, opens PR        │
   │ 8. Prompt: "Apply   │        └─────────────────────────┘
   │    fixes to code?"   │
   └─────────┬───────────┘
             │ Yes
             ▼
   ┌─────────────────────┐
   │ 9. Apply fixes      │
   │    directly to source│
   └─────────────────────┘
```

Each step is **gated by developer approval** — the orchestrator asks one prompt at a time and only proceeds on confirmation.

---

### 🔌 Two MCP Servers Power the Integrations

| MCP Server | Endpoint | Used By | Purpose |
|---|---|---|---|
| **Atlassian MCP** | `https://mcp.atlassian.com/v1/mcp` | am-doc-publisher, am-ticket-creator | Confluence pages + JIRA tickets (OAuth-based) |
| **GitHub MCP** | `https://api.githubcopilot.com/mcp/` | am-raise.pr | Branch creation, file push, PR creation |

MCP (Model Context Protocol) servers act as the **bridge between AI agents and external platforms** — giving each sub-agent scoped access to only the tools it needs.

---

### 🏗️ The Building Blocks — GitHub Copilot's Extensibility Model

This solution is built entirely using **GitHub Copilot's native extensibility**:

| Concept | What It Does | How I Used It |
|---|---|---|
| **Agents** (`.agent.md`) | Define reusable AI personas with specific tools, behaviors, and workflows | 5 agents — 1 orchestrator + 1 standalone + 3 sub-agents |
| **Specs** (`.github/specs/`) | Define data contracts and output schemas | `codereview-ticket.md` — shared format for review findings |
| **MCP Servers** (`mcp.json`) | Connect agents to external platforms (Confluence, JIRA, GitHub) | Atlassian MCP + GitHub MCP |
| **Skills** (`SKILL.md`) | Package domain knowledge into reusable instruction sets | Review patterns, Spring Boot best practices |
| **Handoffs** (`agents:` frontmatter) | Orchestrate transitions between specialized agents | Orchestrator delegates to 3 sub-agents |

**GitHub Copilot's agents, skills, specs, and MCP servers give you a way to define reusable personas, workflows, and integrations directly inside your development environment. It's very developer-centric: lightweight, declarative, and tightly bound to VS Code/GitHub repos.**

Everything lives in `.github/agents/` and `.github/specs/` — version-controlled, shareable, and portable across teams.

---

### 🔑 Key Design Principles

- **Separation of Concerns** — Each sub-agent has exactly one job and only the tools it needs
- **Shared Context Protocol** — A structured review context object flows from orchestrator to all sub-agents
- **User-Gated Workflow** — Every action (publish, ticket, PR, fix) requires explicit developer approval
- **Minimal Tool Surface** — Sub-agents are `user-invocable: false` and restricted to their MCP tools only

---

### 📊 Real Output

The system reviews Java Spring Boot code (controllers, DAOs, services, repositories) and generates structured findings like:

| ID | Category | Severity | Finding |
|---|---|---|---|
| CR-001 | Security | High | `System.err.println` leaks student PII to logs |
| CR-002 | Architecture | High | `@Transactional` belongs on Service layer, not DAO |
| CR-003 | Correctness | Medium | `deleteStudent` silently ignores non-existent IDs |

Each finding becomes a Confluence page section, a JIRA Story ticket, and a potential PR fix — all automated.

---

### 🎯 Bottom Line

With GitHub Copilot Custom Agents, you're not just getting AI code completion — you're building **autonomous, multi-agent workflows** that integrate with your entire DevOps ecosystem.

No external frameworks. No LangChain. No custom infrastructure. Just `.md` files, YAML frontmatter, and MCP servers — all inside VS Code. 🔥

---

#GitHubCopilot #AgenticAI #CustomAgents #MCPServers #JavaSpringBoot #CodeReview #Confluence #JIRA #GitHub #VSCode #AIAgents #DevOps #Automation #MultiAgentArchitecture #DeveloperProductivity #AI #SoftwareEngineering
