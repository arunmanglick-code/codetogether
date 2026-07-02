# Output: Copilot Custom Agents Converted to Claude Code Equivalents

## Summary

Converted 5 GitHub Copilot custom agents, 1 spec, 1 skill, 2 MCP server configs, and hook scripts into Claude Code equivalents. All new files are under `.claude/` — no existing Copilot files were modified.

## Files Created

| # | File | Copilot Equivalent | Description |
|---|---|---|---|
| 1 | `.claude/settings.json` | VS Code `mcp.json` | Atlassian MCP (SSE + placeholder token), PostToolUse hooks |
| 2 | `.claude/specs/codereview-ticket.md` | `customagent/.github/specs/codereview-ticket.md` | Copied data contract with agent names updated |
| 3 | `.claude/agents/claude-code-review-orchestrator.md` | `am-code-review-orchestrator.agent.md` | Orchestrator — delegates via `Agent` tool |
| 4 | `.claude/agents/claude-code-reviewer.md` | `am-code-reviewer.agent.md` | Standalone — MCP tools implicit from settings |
| 5 | `.claude/agents/claude-doc-publisher.md` | `am-doc-publisher.agent.md` + `doc_publisher_skill` | Confluence publisher — skill logic inlined |
| 6 | `.claude/agents/claude-ticket-creator.md` | `am-ticket-creator.agent.md` | JIRA ticket creator for CCMMER3 |
| 7 | `.claude/agents/claude-raise-pr.md` | `am-raise-pr.agent.md` | PR creator — `gh` CLI replaces GitHub MCP |
| 8 | `.claude/hooks/recordToolUse.ps1` | `recordToolUse.ps1` | Filters `Bash`/`PowerShell` instead of `run_in_terminal` |
| 9 | `.claude/hooks/recordToolUse.sh` | `recordToolUse.sh` | Bash equivalent of the hook |

## Final Directory Structure

```
09_CustomAgents/
├── .claude/
│   ├── settings.json                              # MCP + hooks config
│   ├── agents/
│   │   ├── claude-code-review-orchestrator.md      # Orchestrator agent
│   │   ├── claude-code-reviewer.md                 # Standalone reviewer
│   │   ├── claude-doc-publisher.md                 # Confluence publisher
│   │   ├── claude-ticket-creator.md                # JIRA ticket creator
│   │   └── claude-raise-pr.md                      # PR creator (gh CLI)
│   ├── hooks/
│   │   ├── recordToolUse.ps1                       # Tool-use recording (PowerShell)
│   │   └── recordToolUse.sh                        # Tool-use recording (Bash)
│   └── specs/
│       └── codereview-ticket.md                    # Review data contract
├── .github/                                        # UNTOUCHED — existing Copilot agents
│   └── agents/
│       ├── am-code-review-orchestrator.agent.md
│       ├── am-code-reviewer.agent.md
│       ├── am-doc-publisher.agent.md
│       ├── am-raise-pr.agent.md
│       └── am-ticket-creator.agent.md
├── CLAUDE.md                                       # Updated with Claude Code agents section
└── customagent/                                    # Source code (untouched)
```

## Key Conversion Decisions

### 1. Tool Mapping

| Copilot Tool | Claude Code Equivalent | Notes |
|---|---|---|
| `read` | `Read` | Direct 1:1 |
| `edit` | `Edit` | Direct 1:1 |
| `search` | `Grep`, `Glob` | Content search + file name search |
| `execute` | `Bash` | Terminal execution |
| `agent` (subagent spawn) | `Agent` | No frontmatter declaration needed |
| `todo` | _(omitted)_ | Not a tool in Claude Code agent frontmatter |
| `com.atlassian/atlassian-mcp-server/*` | _(implicit via MCP)_ | Configured in `.claude/settings.json`, tools available automatically |
| `github/*` (GitHub MCP) | `Bash` with `gh` CLI | Native CLI replaces the MCP server entirely |

### 2. Structural Differences Handled

| Copilot Feature | Claude Code Approach |
|---|---|
| `argument-hint` frontmatter | Not supported — omitted |
| `agents:` frontmatter (declare subagents) | Subagent names referenced in body text, invoked via `Agent` tool at runtime |
| `skills:` frontmatter | Skill logic (title pattern) inlined into agent body |
| `user-invocable: false` | Not supported — behavioral constraints enforced in instructions |
| Spec auto-loading | Agents instructed to read `.claude/specs/codereview-ticket.md` |
| MCP tools in `tools:` list | MCP tools implicit from `.claude/settings.json` — not listed in frontmatter |

### 3. MCP Server Configuration

**Atlassian MCP** — Configured as SSE type at `https://mcp.atlassian.com/v1/mcp` with a placeholder `Authorization: Bearer <YOUR_ATLASSIAN_TOKEN>` header. The Copilot version uses OAuth managed by the VS Code extension; Claude Code requires manual token setup.

**GitHub MCP** — Not configured. The `claude-raise-pr` agent uses `gh` CLI commands (`gh api user`, `gh pr create`, etc.) via the `Bash` tool, which is more natural for Claude Code and already authenticated via the user's `gh auth` session.

### 4. Hook Scripts Adaptation

The Copilot hooks filter for `run_in_terminal` and `appmod-*` tool names. The Claude Code equivalents filter for `Bash` and `PowerShell` tool names. Both read JSON from stdin, extract `tool_name` and `session_id`, and append matching records to JSONL files.

| Aspect | Copilot | Claude Code |
|---|---|---|
| Tool filter | `run_in_terminal`, `appmod-*` | `Bash`, `PowerShell` |
| Output directory | `.github/java-upgrade/hooks/` | `.claude/hooks/` |
| Hook trigger | VS Code extension event | `PostToolUse` event in `.claude/settings.json` |

### 5. Skill Inlining

The `doc_publisher_skill` (Confluence page title builder) was inlined directly into `claude-doc-publisher.md` since Claude Code has no skill system. The title pattern `{fileName}-am-review-{reviewDate}_{reviewTimestamp}` and all rules (strip extension, no spaces, default timestamp) are included in the agent body.

## Action Required Before Use

1. **Atlassian MCP token**: Replace `<YOUR_ATLASSIAN_TOKEN>` in `.claude/settings.json` with a real Atlassian API token. Consider moving to `.claude/settings.local.json` (gitignored) for security.
2. **`gh` CLI**: Ensure `gh` is installed and authenticated (`gh auth login`) for the PR creation agent.

## CLAUDE.md Update

Added a new "Claude Code Agents" subsection under Architecture documenting the 5 new agents, key differences from Copilot agents, and the MCP/hooks setup.
