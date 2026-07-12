## Goal
Use `/find-skills` to evaluate whether any additional skills are required to make this project more **secure, reliable, and robust**.  
If such skills exist, identify them and suggest installation links or setup details.

## Instructions
1. Run `/find-skills` to discover skills that enhance:
   - Security (e.g., secret scanning, vulnerability detection, guardrails).
   - Reliability (e.g., temporal workflows, retries, monitoring).
   - Robustness (e.g., error handling, compliance logging, resilience).
2. If multiple skills are available, evaluate them and select the most appropriate ones.
3. Provide me with:
   - The best skill(s) to add.
   - Links to install those skills or installation details.
4. Integrate the chosen skills into the project so they can be invoked by existing agents (e.g., code-reviewer, fixer, pr-generator).
5. Do not guess — if any detail is unclear, ask explicit questions.

## Artifacts
- `skills/security-enhancer.skill.md` → Security skill (e.g., secret scanning, Snyk integration).
- `skills/reliability-temporal.skill.md` → Reliability skill (Temporal workflows).
- `skills/robustness-guardrails.skill.md` → Guardrails skill for safe execution.
- `docs/skills-extension.md` → Documentation of new skills and installation links.

## Hooks
- **PrePush**: Trigger security skill to scan for secrets/vulnerabilities.
- **WorkflowFailed**: Trigger reliability skill (Temporal) to retry or compensate.
- **ErrorDetected**: Trigger guardrails skill to enforce safe handling.

## Specs
- Must support integration with existing agents.
- Must provide clear installation links or setup instructions.
- Must improve project’s overall security, reliability, and robustness.

## Clarifications
Before proceeding, confirm:
- Should security skills integrate with existing tools like Snyk or SonarQube?
- Should reliability skills cover only backend workflows, or also frontend retries?
- Should robustness include compliance logging for audit trails?
