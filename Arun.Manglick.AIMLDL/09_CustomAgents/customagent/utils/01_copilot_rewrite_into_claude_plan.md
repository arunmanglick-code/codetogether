# Plan: Convert Copilot Custom Agents to Claude Code Equivalents

## Context

This repository has 5 GitHub Copilot custom agents (`.github/agents/`), a spec (`.github/specs/`), a skill (`customagent/.github/skills/`), MCP server configs, and hook scripts — all wired for an orchestrated code review pipeline on a Spring Boot Student CRUD app. The goal is to create Claude Code equivalents **without touching any existing Copilot files**. All new files go under `.claude/`.

## Files to Create

| # | Path | Purpose |
|---|---|---|
| 1 | `.claude/settings.json` | MCP server (Atlassian SSE with placeholder auth) + hooks config |
| 2 | `.claude/agents/claude-code-review-orchestrator.md` | Orchestrator — reviews code, delegates to subagents |
| 3 | `.claude/agents/claude-code-reviewer.md` | Standalone all-in-one reviewer |
| 4 | `.claude/agents/claude-doc-publisher.md` | Confluence publisher subagent |
| 5 | `.claude/agents/claude-ticket-creator.md` | JIRA ticket creator subagent |
| 6 | `.claude/agents/claude-raise-pr.md` | PR creator subagent (uses `gh` CLI) |
| 7 | `.claude/specs/codereview-ticket.md` | Spec data contract (copied from Copilot spec) |
| 8 | `.claude/hooks/recordToolUse.ps1` | Tool-use recording hook (PowerShell) |
| 9 | `.claude/hooks/recordToolUse.sh` | Tool-use recording hook (Bash) |

**Files NOT modified**: All existing Copilot agents, specs, skills, and hook scripts remain untouched.

---

## 1. `.claude/settings.json` — MCP + Hooks Config

Configure the Atlassian MCP server as SSE with a placeholder Authorization header. No GitHub MCP needed — the PR agent uses `gh` CLI instead.

```json
{
  "mcpServers": {
    "atlassian": {
      "type": "sse",
      "url": "https://mcp.atlassian.com/v1/mcp",
      "headers": {
        "Authorization": "Bearer <YOUR_ATLASSIAN_TOKEN>"
      }
    }
  },
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Bash|PowerShell",
        "command": "powershell.exe -ExecutionPolicy Bypass -File .claude/hooks/recordToolUse.ps1"
      }
    ]
  }
}
```

**Note**: The user must replace `<YOUR_ATLASSIAN_TOKEN>` with a real Atlassian API token or move the token to `.claude/settings.local.json` (gitignored) to avoid committing secrets.

## 2. Agent Conversions — Key Mapping Rules

### Tool Mapping (Copilot → Claude Code)

| Copilot | Claude Code | Notes |
|---|---|---|
| `read` | `Read` | 1:1 |
| `edit` | `Edit` | 1:1 |
| `search` | `Grep`, `Glob` | Content search + file search |
| `execute` | `Bash` | Terminal execution |
| `agent` | `Agent` | Subagent spawning — no frontmatter declaration needed |
| `todo` | _(omit)_ | Not a tool in Claude Code agents |
| `com.atlassian/*` | _(implicit via MCP)_ | Available automatically from settings.json config |
| `github/*` | `Bash` with `gh` CLI | Native CLI replaces GitHub MCP |

### Structural Differences

- **No `argument-hint`** field in Claude Code agents
- **No `agents:` frontmatter** — subagent names are referenced in body instructions and invoked via the `Agent` tool at runtime
- **No `skills:` system** — skill logic (title pattern) is inlined into agent body
- **No `user-invocable: false`** — all Claude Code agents are invocable; behavioral constraints go in instructions
- **No `specs` auto-loading** — agents are told to read `.claude/specs/codereview-ticket.md` when needed
- **MCP tools are implicit** — not listed in `tools:` frontmatter; available once the MCP server is configured

## 3. Agent: `claude-code-review-orchestrator.md`

**Source**: `.github/agents/am-code-review-orchestrator.agent.md`

**Frontmatter**: `tools: [Read, Edit, Write, Bash, Grep, Glob, Agent]`

**Body structure** (translated from Copilot original):
- Instructions — same review criteria (correctness → security → performance → maintainability)
- Communication & Analysis Style — same
- Shared Context Object — inline the full schema from the spec (fileName, filePath, reviewDate, reviewTimestamp, strengths, tickets array)
- **Workflow** — same 5-prompt sequential flow:
  1. Review code
  2. Prompt: save feedback → use `Write` tool to `customagent/review-feedback/`
  3. Prompt: publish to Confluence → spawn `claude-doc-publisher` via `Agent` tool, passing serialized review context
  4. Prompt: create JIRA tickets → spawn `claude-ticket-creator` via `Agent` tool
  5. Prompt: raise PR → spawn `claude-raise-pr` via `Agent` tool
  6. Prompt: apply fixes → use `Edit` tool directly
- Constraints — same delegation boundaries (do NOT use MCP tools directly)
- Reference to `.claude/specs/codereview-ticket.md` for config values

## 4. Agent: `claude-code-reviewer.md`

**Source**: `.github/agents/am-code-reviewer.agent.md`

**Frontmatter**: `tools: [Read, Edit, Write, Bash, Grep, Glob]`

**Body structure**:
- Same review instructions and analysis style
- **Workflow** — same 4-prompt flow but handles everything directly:
  1. Review code
  2. Prompt: save feedback → `Write` tool
  3. Prompt: push to Confluence → use Atlassian MCP tool directly (reference by name: `mcp__atlassian__createConfluencePage` or discover at runtime)
  4. Prompt: create JIRA tickets → use Atlassian MCP tool directly
  5. Prompt: apply fixes → `Edit` tool
- Inline the Confluence config (cloud ID, space ID) and JIRA config (cloud ID, project key)
- Inline the title pattern from doc_publisher_skill: `{fileName}-am-review-{reviewDate}_{reviewTimestamp}`
- Inline ADF formatting rules for Confluence pages

## 5. Agent: `claude-doc-publisher.md`

**Source**: `.github/agents/am-doc-publisher.agent.md` + `customagent/.github/skills/doc_publisher_skill/SKILL.md`

**Frontmatter**: `tools: [Read]`

**Body structure**:
- Instructions — receive review context, create Confluence page
- Confluence config inlined (cloud ID: `2c372697-c4a1-4192-9fa0-5cd146f9d535`, space ID: `3493233287`)
- **Title pattern inlined** from skill: `{fileName}-am-review-{reviewDate}_{reviewTimestamp}` with rules (strip extension, default timestamp `000000`, no spaces)
- ADF formatting rules (table for summary, heading/bulletList/codeBlock/rule for structure)
- Constraints — same (no reviews, no JIRA, no source edits)
- Reference Atlassian MCP tools for page creation

## 6. Agent: `claude-ticket-creator.md`

**Source**: `.github/agents/am-ticket-creator.agent.md`

**Frontmatter**: `tools: [Read]`

**Body structure**:
- Instructions — receive review context, create JIRA Stories
- JIRA config inlined (cloud ID, project key `CCMMER3`, issue type `Story`)
- Summary pattern: `[Code Review] {fileName} — {ticket.title}`
- Constraints — same (no reviews, no Confluence, no source edits)
- Output format — summary table with JIRA keys and links

## 7. Agent: `claude-raise-pr.md`

**Source**: `.github/agents/am-raise-pr.agent.md`

**Frontmatter**: `tools: [Read, Edit, Write, Bash, Grep, Glob]`

**Critical change**: Replaces all GitHub MCP tools with `gh` CLI commands:

| Copilot GitHub MCP | Claude Code (`gh` CLI) |
|---|---|
| `github-get_me` | `gh api user` |
| `github-list_branches` | `git branch -r` |
| `github-create_branch` | `git checkout -b fix/{name}` |
| `github-push_files` | `git add` + `git commit` + `git push -u origin` |
| `github-create_pull_request` | `gh pr create --title "..." --body "..."` |

**Body structure**:
- Instructions — analyze issue, determine if PR needed
- Approach rewritten for `gh` CLI workflow (check user, create branch, apply edits, commit, push, create PR)
- Constraints — same (no PRs for comment-only changes, no duplicates)
- **Prerequisite note**: `gh` CLI must be installed and authenticated

## 8. Spec: `.claude/specs/codereview-ticket.md`

Direct copy of `customagent/.github/specs/codereview-ticket.md` — the spec data contract (review context schema, ticket structure, Confluence/JIRA config, naming patterns). Agents reference this file path in their instructions.

## 9. Hooks: `.claude/hooks/recordToolUse.ps1` and `.sh`

Adapted from `customagent/.github/java-upgrade/hooks/scripts/recordToolUse.ps1` and `.sh`.

**Changes from Copilot originals**:
- Filter for Claude Code tool names (`Bash`, `PowerShell`) instead of Copilot names (`run_in_terminal`, `appmod-*`)
- Read JSON from stdin (same as Copilot — Claude Code hooks also pipe tool data on stdin)
- Extract `tool_name` and `session_id` from the JSON
- Append to `.claude/hooks/{sessionId}.json` as JSONL

## Verification

After implementation, verify by:
1. Run `claude` in the repo root — confirm the 5 new agents appear when you type `@` or `/agents`
2. Invoke `@claude-code-reviewer` on a Java file (e.g., `StudentDAOImpl.java`) — confirm it produces a structured review with the correct ticket format
3. Invoke `@claude-code-review-orchestrator` — confirm it asks prompts sequentially and correctly spawns subagents when delegating
4. Check that `.claude/settings.json` is loaded (Atlassian MCP placeholder visible, hooks registered)
5. Test the hook by running a Bash command and checking for JSONL output in `.claude/hooks/`
