# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Claude Code custom skills for technical interview workflows. The primary skill (`begin-interview`) conducts structured technical interviews with configurable tech stack, difficulty, experience level, and question mode. It generates questions with **instant model answers** so the interviewer can compare in real-time, lets the interviewer **score each response directly** (no typed candidate answers required), and saves a classification report.

## Architecture

This is a skills-only project — no build step, no runtime dependencies. Skills are plain markdown files with YAML frontmatter, loaded by Claude Code at session start.

- `.claude/skills/begin-interview/SKILL.md` — Skill definition (invoked via `/begin-interview`)
- `interviews/` — Generated interview summary reports (markdown, gitignored output)
- `docs/` — User-facing documentation
- `utils/prompt/` — Original prompt specifications and design artifacts

The skill follows a 5-phase conversational workflow: Setup → Question Generation → Interview Execution (question + model answer shown together, interviewer scores in real-time) → Evaluation → Summary Report.

## Development Commands

```bash
# No build or test commands — this is a pure skills project.

# Verify the skill is registered (open Claude Code in this directory):
# Type /begin-interview in the prompt

# List generated interview reports:
ls interviews/
```

## Key Files

- `.claude/skills/begin-interview/SKILL.md` — Core skill logic and interview workflow
- `docs/interview-skill.md` — Usage documentation and parameter reference
- `utils/prompt/01_interview_skill_prompt.md` — Original requirements specification
