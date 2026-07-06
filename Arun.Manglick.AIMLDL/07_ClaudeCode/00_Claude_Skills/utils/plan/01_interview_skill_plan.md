# Plan: Create `begin-interview` Skill

## Context

The `00_Claude_Skills` project is a new, empty directory for housing Claude Code custom skills. The first skill to build is `begin-interview` — a conversational skill that conducts structured technical interviews with configurable parameters, records candidate answers, scores them against model answers, and produces a classification report.

The design follows the existing skill pattern from `01_Claude_Portfolio/.claude/skills/github-issues/SKILL.md` (YAML frontmatter + markdown body). Unlike that skill, this one uses no MCP tools — it's purely conversational with a file-write step at the end for the report.

**Design decisions** (user was asked but didn't respond — using sensible defaults):
- Evaluation: Automated scoring with manual override option
- Export: Save interview summary as a markdown file in `interviews/`
- Rounds: Single technical round (extensible later)

---

## Files to Create/Modify

### 1. `.claude/skills/begin-interview/SKILL.md` (NEW — primary deliverable)

The core skill file with YAML frontmatter and 7 sections:

**Frontmatter**: `name: begin-interview`, `version: 1.0.0`, `argument-hint: [tech-stack]`

**Section 1 — Role**: Senior technical interviewer persona. Professional, objective, one question at a time, no hints during interview.

**Section 2 — Setup Phase**: Collect 4 parameters interactively:
| Parameter | Values | Default |
|-----------|--------|---------|
| Tech Stack | Any technology | Required |
| Difficulty | high / medium / low | medium |
| Experience | 5-8 / 8-11 / 11-15 years | 8-11 |
| Mode | scenario-based / one-liner | scenario-based |

Also ask for candidate name (optional, defaults to "Candidate").

**Section 3 — Question Generation**:
- Count: low=5, medium=8, high=10
- Composition scales with difficulty (fundamentals → architecture/tradeoffs)
- Experience level adjusts question depth (code-level → system design → tech strategy)
- Mode determines format (multi-step scenarios vs. concise Q&A)
- Each question has a model answer with 3-5 key evaluation points (checklist)
- All questions generated internally at once, presented one at a time

**Section 4 — Interview Execution**:
- Present "Question N of M", wait for response, store it, move on
- No feedback or hints between questions
- Mid-interview commands: `change tech stack to [X]`, `skip`, `end interview`, `repeat`
- Tech stack change: retain answered questions, regenerate remaining ones for new stack

**Section 5 — Scoring Rubric**:
- Per-question: 0-10 scale based on key evaluation points covered
- Classification thresholds: ≥7.0 = Good, 5.0-6.9 = Average, <5.0 = Not Selected
- Manual override offered after automated classification

**Section 6 — Summary Report**:
- Saved to `interviews/YYYY-MM-DD_candidate-name_tech-stack.md`
- Contains: metadata table, overall result, per-question breakdown (candidate answer, model answer, key points, score), strengths, areas for improvement, interviewer notes

**Section 7 — Error Handling**:
- Unrecognized tech stack: accept with warning
- No arguments: interactive setup
- Cancel mid-interview: offer partial report or discard
- Missing `interviews/` directory: create it

### 2. `.claude/settings.json` (NEW)

Minimal permissions for directory operations the skill needs:
```json
{
  "permissions": {
    "allow": [
      "Bash(mkdir -p interviews)",
      "Bash(ls interviews/)"
    ]
  }
}
```

### 3. `docs/interview-skill.md` (NEW)

User documentation covering: overview, quick start, parameters reference, question modes, scoring system, mid-interview commands, output format, manual override, usage examples, troubleshooting.

### 4. `interviews/.gitkeep` (NEW)

Empty file to ensure the output directory exists in version control.

### 5. `CLAUDE.md` (UPDATE)

Replace placeholder content with project-specific documentation: what the project is, the skill architecture, how to invoke it, and key files.

---

## Implementation Order

1. Create directory structure (`.claude/skills/begin-interview/`, `interviews/`, `docs/`)
2. Write `SKILL.md` — the core skill definition (~250-300 lines)
3. Write `.claude/settings.json`
4. Write `docs/interview-skill.md`
5. Create `interviews/.gitkeep`
6. Update `CLAUDE.md`

---

## Verification

1. Open Claude Code in `00_Claude_Skills/` and confirm `/begin-interview` appears in skill autocomplete
2. Run `/begin-interview Java` — verify tech stack pre-fills and remaining params are requested
3. Complete a full interview (Java, medium, 8-11, scenario-based) — verify 8 questions generated, one at a time
4. Test mid-interview commands: `skip` a question, `change tech stack to Python`
5. Complete the interview and verify report saved to `interviews/` with correct format
6. Test manual override of classification
7. Run a low-difficulty one-liner interview — verify only 5 short-answer questions
