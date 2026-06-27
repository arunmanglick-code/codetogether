# Prompt: PostToolUse Hook Creation

You are tasked to **create a hook for PostToolUse events**.  
Whenever a file is modified, the hook must automatically trigger a **subagent** that reviews the changed code.

## Requirements
- Place the subagent under the **`.claude` folder**.
- Ensure the subagent follows **best coding practices** and **comprehensive code review standards**.
- The review should cover:
  - Code style and formatting
  - Performance considerations
  - Security checks
  - Maintainability and readability
- The subagent must run consistently on every file change event.

## Implementation Notes
- Hook listens for **FileChanged** events in the project.
- On trigger, the subagent is invoked to analyze the diff/changed file.
- Review output should be structured and actionable, highlighting:
  - Issues found
  - Suggested improvements
  - Compliance with coding standards
- Design the hook to be modular and extensible for future workflows.

## Best Practices
- Keep the subagent lightweight and modular.
- Ensure reviews are **non-blocking** but provide clear feedback.
- Maintain logs of reviews for audit and learning purposes.
