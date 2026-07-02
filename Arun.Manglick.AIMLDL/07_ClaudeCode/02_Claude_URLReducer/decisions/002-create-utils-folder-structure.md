# Decision 002: Create Utils Folder Structure

**Date:** 2026-07-02  
**Status:** Accepted

## Context
Need a structured workspace to organize project artifacts — plans, outputs, tasks, prompts, images, and templates — separately from source code.

## Decision
Create a `utils/` folder with six subfolders, each serving a specific purpose:

| Subfolder | Purpose |
|---|---|
| `images/` | Store project-related images and diagrams |
| `phase_output/` | Store output artifacts from plan phase implementation |
| `plan/` | Store all plans created during the project |
| `prompt/` | Store prompts used during the project |
| `tasks/` | Store tasks identified during plan creation |
| `templates/` | Store reusable templates |

## Consequences
- Plans go in `utils/plan/`, implementation outputs go in `utils/phase_output/`, and identified tasks go in `utils/tasks/`.
- `.gitkeep` files ensure empty directories are tracked by git.
