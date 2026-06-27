# PostToolUse Hook — Automated Code Review Subagent

## Context

The portfolio project needs a PostToolUse hook that automatically triggers a code review subagent whenever a file is modified (via Edit, Write, or NotebookEdit tools). This provides real-time, AI-powered code review feedback during development — covering code style, performance, security, and maintainability.

**Current state:**
- `.claude/settings.local.json` exists with permissions but no hooks
- No `.claude/settings.json` (project-level shared config) exists
- No subagent definitions exist under `.claude/`

**Key constraint:** Claude Code `type: "agent"` hooks spawn a lightweight subagent that can use Read/Grep/Glob/Bash to analyze code and return feedback as `additionalContext` to the main conversation. This is the right mechanism — it's non-blocking (PostToolUse fires after the tool completes) and provides structured feedback inline.

---

## Implementation Plan

### Step 1 — Create the code review subagent definition

**File:** `.claude/agents/code-review-agent.md`

A markdown file defining the subagent's persona, review criteria, and output format. While the `type: "agent"` hook uses an inline `prompt`, this file serves as the canonical reference for the review standards and can also be invoked manually via the Agent tool.

**Review criteria to cover:**
- **Code style & formatting** — naming conventions, indentation, consistency with project patterns
- **Performance** — unnecessary re-renders, N+1 queries, unoptimized loops, missing caching
- **Security** — XSS vectors, injection risks, exposed secrets, OWASP top 10
- **Maintainability** — readability, complexity, duplication, proper abstractions
- **Astro/Tailwind specific** — correct use of content collections, Tailwind utility patterns, component structure

**Output format:** Structured markdown with sections for Issues Found, Suggested Improvements, and Standards Compliance. Each issue tagged with severity (critical/warning/info).

### Step 2 — Create the hook configuration

**File:** `.claude/settings.json` (new file — project-level, committable)

```json
{
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Edit|Write|NotebookEdit",
        "hooks": [
          {
            "type": "agent",
            "prompt": "You are a code review agent. Review the file that was just modified. Focus on: (1) Code style — naming, formatting, consistency with surrounding code, (2) Performance — inefficient patterns, unnecessary work, (3) Security — XSS, injection, exposed secrets, (4) Maintainability — readability, complexity, duplication. For Astro files check content collection usage and Tailwind patterns. Report only genuine issues. Format: list each issue with severity [CRITICAL/WARNING/INFO], file location, description, and suggested fix. If no issues found, say 'No issues detected.' Be concise — no preamble.",
            "model": "claude-haiku-4-5",
            "timeout": 60
          }
        ]
      }
    ]
  }
}
```

**Design choices:**
- `matcher: "Edit|Write|NotebookEdit"` — only file-modifying tools, not Read/Grep/Bash
- `model: "claude-haiku-4-5"` — fast and cheap for quick reviews; avoids slowing down the main workflow
- `timeout: 60` — generous but bounded
- Project-level `.claude/settings.json` — shared with team, separate from personal permissions in `settings.local.json`

### Step 3 — Create a review log script (optional enhancement)

**File:** `.claude/hooks/log-review.sh`

A lightweight shell script that can be chained as a second `command` hook to append review timestamps and file paths to a log file at `.claude/review-log.txt`. This satisfies the "maintain logs for audit" requirement.

---

## Files Created

| File | Purpose |
|------|---------|
| `.claude/settings.json` | Hook configuration (PostToolUse → agent + command) |
| `.claude/agents/code-review-agent.md` | Subagent definition with review criteria |
| `.claude/hooks/log-review.sh` | Review audit logging script |

---

## Verification

1. Modify any source file using Edit tool → hook should fire and provide review feedback
2. Create a new file using Write tool → hook should fire
3. Verify feedback appears as additionalContext in the conversation
4. Check that the review is non-blocking (main workflow continues)
5. Verify `.claude/review-log.txt` gets entries after reviews
