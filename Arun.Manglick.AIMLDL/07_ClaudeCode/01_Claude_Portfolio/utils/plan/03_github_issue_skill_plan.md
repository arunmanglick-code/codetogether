# Plan: Add `github-issues` Skill

## Context

No quality pre-built GitHub Issues skill exists in the ecosystem (checked Anthropic's official skills repo, ComposioHQ, alirezarezvani/claude-skills, and the official plugins marketplace). The ComposioHQ one requires the Composio SDK — an unnecessary extra dependency.

You already have the GitHub MCP server fully configured (`.mcp.json` with `@modelcontextprotocol/server-github`), giving you access to all issue management MCP tools. The best approach is a **custom skill** that wraps these existing tools with structured workflows, templates, and conventions.

## What the Skill Will Do

| Action | Trigger phrases |
|--------|----------------|
| **Create issues** (bug, feature, task) with markdown templates | "create issue", "new bug", "new feature", "file issue" |
| **List/search issues** with filters (state, labels, assignees) | "list issues", "show issues", "search issues" |
| **Update issues** (close, reopen, relabel, reassign) | "update issue", "close issue", "reopen issue" |
| **Comment on issues** with structured markdown | "comment on issue", "add comment" |
| **Triage issues** (review, suggest labels/priority) | "triage issues", "review open issues" |
| **Bulk operations** (close stale, batch label) with confirmation | "close stale issues", "bulk close" |

Key behaviors:
- Auto-detects `owner`/`repo` from git remote (no repeated prompts)
- Uses MCP tools exclusively (not `gh` CLI) since those are already configured
- Includes issue body templates (bug report, feature request, task)
- Requires explicit user confirmation before any bulk/destructive operation

## Implementation Steps (in order)

### Step 1: Create `.claude/skills/github-issues/SKILL.md`

Create the directory and skill file with the following complete content:

````markdown
---
name: github-issues
description: "Create, list, update, triage, comment on, and bulk-manage GitHub issues."
version: 1.0.0
argument-hint: <action> [options]
---

# GitHub Issues Manager

You are a GitHub issue management assistant. You use the MCP GitHub tools (never the `gh` CLI) to manage issues.

## Auto-detect owner and repo

ALWAYS extract `owner` and `repo` from the git remote before any operation. Run:

```bash
git config --get remote.origin.url
```

Parse the result:
- `https://github.com/OWNER/REPO.git` -> owner=OWNER, repo=REPO
- `git@github.com:OWNER/REPO.git` -> owner=OWNER, repo=REPO

If the remote URL does not match a GitHub pattern (e.g., GitLab, Gitea, self-hosted), ask the user to provide `owner` and `repo` explicitly.

Use these values for every MCP tool call. NEVER ask the user for owner/repo when a valid GitHub remote exists.

## Actions

### 1. CREATE ISSUE

When the user says: "create issue", "new bug", "new feature", "new task", "file issue", "open issue"

Parse the user's request to determine the issue type. Apply the matching template below.

**Required fields** (must always be populated from the user's request): Description and Title.
**Optional fields** (include only when the user provides them, otherwise omit the section entirely): Steps to Reproduce, Environment, Alternatives Considered, Implementation Notes.

**Bug Report template:**

Title: concise bug summary (under 80 chars)

Body:
```
## Bug Description
{description from user's request}

## Steps to Reproduce
1. {step -- omit this section if user didn't describe steps}

## Expected Behavior
{what should happen}

## Actual Behavior
{what happens instead}

## Environment
{omit this section if user didn't mention environment details}

## Additional Context
{omit this section if no extra context provided}
```
Labels: `["bug"]`

**Feature Request template:**

Title: concise feature summary (under 80 chars)

Body:
```
## Feature Description
{description from user's request}

## Motivation
{why this feature is needed}

## Proposed Solution
{how it could work -- omit if user didn't suggest one}

## Alternatives Considered
{omit this section if user didn't mention alternatives}
```
Labels: `["enhancement"]`

**Task template:**

Title: concise task summary (under 80 chars)

Body:
```
## Task Description
{description from user's request}

## Acceptance Criteria
- [ ] {criterion 1}
- [ ] {criterion 2}

## Implementation Notes
{omit this section if no technical notes provided}
```
Labels: `["task"]`

After filling in the template, call `mcp__github__create_issue` with the owner, repo, title, body, and labels.

Report back the issue number and URL: `https://github.com/{owner}/{repo}/issues/{number}`

### 2. LIST / SEARCH ISSUES

When the user says: "list issues", "show issues", "show open issues", "search issues", "find issues"

**List with filters:** Call `mcp__github__list_issues` with owner, repo, and optional filters.
Defaults: state="open", sort="created", direction="desc", per_page=20

**Search across repos:** Call `mcp__github__search_issues` with a query string.
Build the query string `q` from the user's intent:
- Scope to repo: `repo:OWNER/REPO`
- Filter by state: `is:open` or `is:closed`
- Filter by label: `label:bug`
- Filter by assignee: `assignee:username`
- Text search: include the user's keywords

If the user's search terms contain GitHub query operators (e.g., `repo:`, `label:`, `is:`), use them as-is rather than wrapping them.

Present results as a formatted table:

| # | Title | State | Labels | Assignee | Created |
|---|-------|-------|--------|----------|---------|
| 42 | Fix broken nav link | open | bug | arun | 2026-06-20 |

If results exceed one page (per_page limit), mention the total count and offer to fetch the next page.

### 3. UPDATE ISSUE

When the user says: "update issue", "close issue", "reopen issue", "change labels", "reassign", "rename issue"

First fetch the current issue with `mcp__github__get_issue` to show the user what exists.

Then apply the requested changes with `mcp__github__update_issue`:
- **Close**: state="closed"
- **Reopen**: state="open"
- **Rename**: title="new title"
- **Relabel**: labels=["label1", "label2"] (replaces all labels)
- **Reassign**: assignees=["username"]
- **Edit body**: body="new body"

Confirm the change: "Issue #N updated: {summary of changes}"

### 4. COMMENT ON ISSUE

When the user says: "comment on issue", "add comment", "reply to issue"

Call `mcp__github__add_issue_comment` with structured markdown body.

For status updates, structure as:
```
## Status Update
{user's comment}

**Next steps:**
- {action items if mentioned}
```

### 5. TRIAGE ISSUES

When the user says: "triage issues", "review open issues", "prioritize issues"

1. Fetch open issues with `mcp__github__list_issues` (state="open", per_page=100). If the repo has 100+ open issues, paginate and inform the user of total count before proceeding.
2. Analyze each issue's title and body. Suggest priority and type labels.
3. Present a triage report table showing issues needing attention vs well-labeled ones.
4. Ask the user which suggestions to apply, then batch-update with `mcp__github__update_issue`.

### 6. BULK OPERATIONS

When the user says: "close stale issues", "bulk close", "bulk label"

1. Fetch issues matching the criteria
2. Show the full list of affected issues
3. **ASK FOR EXPLICIT CONFIRMATION** before proceeding
4. For stale closures: add a comment explaining the closure, then close with "stale" label
5. Report summary: "Closed N issues: #1, #2, ..."

IMPORTANT: For any bulk destructive operation (closing, relabeling), ALWAYS show the full list and get explicit user confirmation before proceeding.

## Formatting Rules

- Always use markdown in issue bodies and comments
- Include checklists for actionable items
- Use headers to structure long issue bodies
- Keep titles concise (under 80 characters) and descriptive
- Reference source files by their path when relevant (e.g., `src/components/header.ts`)

## Error Handling

- If an MCP tool call fails, report the error clearly and suggest a fix
- If an issue number does not exist, say so and offer to search
- If the git remote cannot be parsed or is not GitHub, ask the user for owner and repo
- If the user's request is ambiguous, ask a clarifying question before acting
````

### Step 2: Update `.claude/settings.local.json`

Add 6 MCP tool permissions to the `permissions.allow` array (after the existing `"mcp__github__list_pull_requests"` entry) to suppress confirmation prompts:

```json
"mcp__github__create_issue",
"mcp__github__get_issue",
"mcp__github__list_issues",
"mcp__github__update_issue",
"mcp__github__add_issue_comment",
"mcp__github__search_issues"
```

No other files need changes. `.mcp.json` and `.claude/settings.json` remain as-is.

## Verification (after Steps 1 and 2 are complete)

1. Restart Claude Code session, type `/` and confirm `github-issues` appears in autocomplete
2. Run `/github-issues list open issues` — should show a formatted table without asking for owner/repo
3. Run `/github-issues create bug: test issue from skill` — should create issue with bug template and `bug` label
4. Run `/github-issues close issue #N` (the test issue) — should close it
5. Clean up test issues
