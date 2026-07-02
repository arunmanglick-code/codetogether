# GitHub Issues Skill — Implementation Output

## Status: COMPLETE

The `github-issues` custom skill has been fully implemented. GitHub issues can now be created, listed, updated, triaged, commented on, and bulk-managed directly from Claude Code using `/github-issues`.

---

## What Was Built

### 1. Custom Skill — `.claude/skills/github-issues/SKILL.md`

A Claude Code skill that wraps the existing GitHub MCP server tools into structured workflows with templates and conventions.

**6 supported actions:**

| Action | Trigger Phrases | MCP Tools Used |
|--------|----------------|----------------|
| Create Issue | "create issue", "new bug", "new feature", "file issue" | `mcp__github__create_issue` |
| List/Search Issues | "list issues", "show issues", "search issues" | `mcp__github__list_issues`, `mcp__github__search_issues` |
| Update Issue | "update issue", "close issue", "reopen issue", "reassign" | `mcp__github__get_issue`, `mcp__github__update_issue` |
| Comment on Issue | "comment on issue", "add comment" | `mcp__github__add_issue_comment` |
| Triage Issues | "triage issues", "review open issues" | `mcp__github__list_issues`, `mcp__github__update_issue` |
| Bulk Operations | "close stale issues", "bulk close", "bulk label" | `mcp__github__list_issues`, `mcp__github__update_issue`, `mcp__github__add_issue_comment` |

**Issue templates included:**

| Type | Label | Required Sections | Optional Sections |
|------|-------|-------------------|-------------------|
| Bug Report | `bug` | Bug Description, Expected/Actual Behavior | Steps to Reproduce, Environment, Additional Context |
| Feature Request | `enhancement` | Feature Description, Motivation | Proposed Solution, Alternatives Considered |
| Task | `task` | Task Description, Acceptance Criteria | Implementation Notes |

**Key design decisions:**
- **MCP-only** — uses `mcp__github__*` tools exclusively (not `gh` CLI), leveraging the already-configured GitHub MCP server in `.mcp.json`
- **Auto-detection** — extracts `owner`/`repo` from `git remote` at runtime, making the skill portable across repositories
- **Confirmation gates** — bulk/destructive operations (close stale, batch relabel) always show a preview and require explicit user approval
- **Pagination awareness** — list/triage operations note when results exceed one page and offer to fetch more

### 2. MCP Permissions — `.claude/settings.local.json`

Added 6 MCP tool permissions to the `permissions.allow` array to suppress confirmation prompts during issue operations:

```
mcp__github__create_issue
mcp__github__get_issue
mcp__github__list_issues
mcp__github__update_issue
mcp__github__add_issue_comment
mcp__github__search_issues
```

These were added after the existing `mcp__github__list_pull_requests` entry. The GitHub MCP server itself (`enableAllProjectMcpServers: true`, `enabledMcpjsonServers: ["github"]`) was already configured and required no changes.

---

## Files Changed

| File | Action | Purpose |
|------|--------|---------|
| `.claude/skills/github-issues/SKILL.md` | Created | Skill definition with 6 actions, 3 templates, auto-detection, and error handling |
| `.claude/settings.local.json` | Updated | Added 6 MCP tool permissions for issue operations |

## Files Unchanged (already configured)

| File | Why |
|------|-----|
| `.mcp.json` | GitHub MCP server already configured with `@modelcontextprotocol/server-github` |
| `.claude/settings.json` | PostToolUse hooks unaffected by skill addition |

---

## Research Summary

Before building the custom skill, the following ecosystem sources were checked for existing GitHub issues skills:

| Source | Result |
|--------|--------|
| [anthropics/skills](https://github.com/anthropics/skills) | No GitHub issues skill — only document skills (docx, pdf, pptx, xlsx) and creative/dev examples |
| [ComposioHQ/awesome-claude-skills](https://github.com/ComposioHQ/awesome-claude-skills) | Has "GitHub Automation" skill but requires Composio SDK — unnecessary dependency |
| [alirezarezvani/claude-skills](https://github.com/alirezarezvani/claude-skills) | No GitHub issues skill — has PR review and CI/CD but not issue management |
| claude-plugins-official marketplace | Has `code-review`, `commit-commands`, `pr-review-toolkit` — no issue management |
| [mcpmarket.com](https://mcpmarket.com/tools/skills/github-issues) | Third-party listing — could not verify quality (429 rate limit) |

**Conclusion:** A custom skill leveraging the existing MCP tools was the best approach — no extra dependencies, no additional API keys, fully tailored to the project's setup.

---

## Usage Examples

```
/github-issues create bug: the nav link to /projects returns 404 on mobile
/github-issues new feature: add dark mode toggle to the header
/github-issues list open issues labeled bug
/github-issues search issues about docker deployment
/github-issues close issue #5
/github-issues comment on #3: fixed in latest commit
/github-issues triage
/github-issues close stale issues older than 60 days
```

## Verification Steps

1. Restart Claude Code session and confirm `/github-issues` appears in skill autocomplete
2. Run `/github-issues list open issues` — verify formatted table output without owner/repo prompt
3. Run `/github-issues create bug: test issue` — verify issue created on GitHub with bug template
4. Run `/github-issues close issue #N` — verify issue closed
5. Clean up test issues


## Usage Steps
1. Prompt: List Open or Closed Issues
2. Claude finds the skill and execute (Example Below)
 
    List Open or Closed Issues
     Thought for 5s

    ● Skill(github-issues)
    Successfully loaded skill

    Thought for 5s, ran 1 shell command

