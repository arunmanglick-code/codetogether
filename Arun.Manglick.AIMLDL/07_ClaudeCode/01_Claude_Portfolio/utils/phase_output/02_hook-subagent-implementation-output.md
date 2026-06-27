# PostToolUse Hook — Implementation Output

## Status: COMPLETE

The PostToolUse hook and code review subagent have been fully implemented. Every file modification (Edit, Write, NotebookEdit) now automatically triggers an AI-powered code review.

---

## What Was Built

### 1. Hook Configuration — `.claude/settings.json`

Created a project-level settings file with PostToolUse hook configuration containing two chained hooks:

| Hook | Type | Purpose |
|------|------|---------|
| Code Review Agent | `agent` | AI-powered review using claude-haiku-4-5 (60s timeout) |
| Audit Logger | `command` | Appends timestamp + file path to `.claude/review-log.txt` |

**Matcher:** `Edit|Write|NotebookEdit` — only file-modifying tools trigger the hook. Read, Grep, Bash, and other non-modifying tools are excluded.

**Hook flow:**
```
File modified (Edit/Write/NotebookEdit)
  → PostToolUse fires
    → Agent hook: Haiku 4.5 reviews the changed file
      → Returns feedback as additionalContext (non-blocking)
    → Command hook: log-review.sh appends audit entry
```

### 2. Code Review Subagent — `.claude/agents/code-review-agent.md`

Subagent definition covering 5 review dimensions:

| Dimension | What It Checks |
|-----------|---------------|
| Code Style & Formatting | Naming conventions (camelCase/PascalCase), 2-space indentation, import ordering |
| Performance | Duplicate data fetching, N+1 patterns, unnecessary runtime work vs build-time |
| Security | Hardcoded secrets, XSS vectors, missing `rel="noopener noreferrer"`, injection risks, OWASP Top 10 |
| Maintainability | Single responsibility, nesting depth, duplication, naming clarity |
| Astro/Tailwind Specific | `glob()` loader usage, `entry.id` vs `entry.data.slug`, `render()` API, mobile-first responsive, dark mode `class` strategy |

**Output format:** Each issue tagged with severity level:
- `[CRITICAL]` — Security vulnerabilities, data loss, broken functionality
- `[WARNING]` — Performance issues, anti-patterns, potential bugs
- `[INFO]` — Style improvements, minor suggestions

### 3. Audit Logger — `.claude/hooks/log-review.sh`

Shell script that:
- Reads the PostToolUse event JSON from stdin
- Extracts tool name and file path
- Appends a timestamped log entry to `.claude/review-log.txt`

**Log format:**
```
[2025-06-25 16:30:45] Edit — src/components/home/Hero.astro
[2025-06-25 16:31:12] Write — src/content/projects/new-project.md
```

---

## Files Created

```
.claude/
├── settings.json                  # Hook configuration (PostToolUse → agent + command)
├── settings.local.json            # Pre-existing permissions (unchanged)
├── agents/
│   └── code-review-agent.md       # Subagent definition with review criteria
└── hooks/
    └── log-review.sh              # Audit logging script (executable)
```

| File | Size | Action |
|------|------|--------|
| `.claude/settings.json` | New | PostToolUse hook config with agent + command hooks |
| `.claude/agents/code-review-agent.md` | New | Review criteria and output format definition |
| `.claude/hooks/log-review.sh` | New | Bash script for audit logging (chmod +x applied) |
| `.claude/settings.local.json` | Unchanged | Existing permissions preserved |

---

## Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Hook type | `agent` (not `command`) | AI-powered review provides contextual feedback, not just lint rules |
| Model | `claude-haiku-4-5` | Fast and lightweight — avoids slowing down the main workflow |
| Config location | `.claude/settings.json` (project-level) | Shared with team; personal permissions stay in `settings.local.json` |
| Matcher scope | `Edit\|Write\|NotebookEdit` | Only file-modifying tools; excludes Read/Grep/Bash noise |
| Timeout | 60 seconds | Generous for complex files but bounded to prevent hangs |
| Logging | Separate `command` hook | Decoupled from agent review; simple append-only audit trail |

---

## How It Works

1. **Trigger:** Any Edit, Write, or NotebookEdit tool call completes
2. **Agent review:** Claude Haiku 4.5 receives the tool event (file path, content, output) and runs a focused code review
3. **Feedback:** Review results appear as `additionalContext` in the main conversation — non-blocking, the main workflow continues
4. **Audit log:** The command hook appends the event timestamp and file path to `.claude/review-log.txt`

---

## Verification Checklist

- [x] `.claude/settings.json` created with valid JSON hook configuration
- [x] `.claude/agents/code-review-agent.md` created with comprehensive review criteria
- [x] `.claude/hooks/log-review.sh` created and made executable
- [x] Existing `.claude/settings.local.json` preserved (no modifications)
- [x] Hook matcher targets only file-modifying tools
- [x] Agent uses lightweight model (Haiku 4.5) for fast reviews
- [x] Audit logging decoupled as a separate command hook
