## Goal
Add a new skill named **`github-issues`** to this project.  
This skill should assist with creating, updating, and managing GitHub issues directly from Claude Code workflows.

## Instructions
1. Use `/find-skills` to discover the best available skill for GitHub issue management.  
2. If multiple options exist, evaluate them and select the most reliable one.  
3. Let me know if you need me to provide:
   - The link to the skill, or
   - Installation details for setup.
4. Integrate the chosen skill into the project so it can:
   - Create new GitHub issues.
   - Update existing issues.
   - Manage issue lifecycle (assign, label, close).
5. Ensure the skill is modular and can be invoked from other agents (e.g., code-reviewer, fixer, pr-generator).
6. Do not guess — if any detail is unclear, ask explicit questions.

## Artifacts
- `skills/github-issues.skill.md` → Defines the GitHub issues skill.
- `docs/github-issues.md` → Documentation of usage, parameters, and integration.

## Hooks
- **IssueCreated**: Trigger `github-issues` skill to create a new issue.
- **IssueUpdated**: Trigger `github-issues` skill to update existing issue details.
- **IssueClosed**: Trigger `github-issues` skill to close or resolve issues.

## Specs
- Must support issue creation, updates, and closure.
- Must allow integration with other agents for automated workflows.
- Must be flexible enough to handle different repositories.
- Must provide clear feedback when actions succeed or fail.

## Clarifications
Before proceeding, confirm:
- Should the skill support multiple repositories or just one default repo?
- Should issue templates (e.g., bug, feature request) be included?
- Should labels and assignees be configurable at runtime?
