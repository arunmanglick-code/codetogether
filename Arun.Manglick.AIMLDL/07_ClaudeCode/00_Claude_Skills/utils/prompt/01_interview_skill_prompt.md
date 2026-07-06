## Goal
Create a new skill using `/skill-creator` (available at https://www.skills.sh/anthropics/skills/skill-creator) that can conduct **candidate interviews**.  
This skill should handle interview setup, question generation, answer recording, and final evaluation.

## Instructions
1. Build a skill named `interview-skill` with the following capabilities:
   - When invoked with `/begin-interview`, the skill should ask for:
     - Tech stack (e.g., Java, Python, React)
     - Difficulty level (high, medium, low)
     - Experience level (5–8, 8–11, 11–15 years)
     - Mode (scenario‑based or one‑liner)
   - Generate interview questions based on the chosen parameters.
   - Display **model answers instantly** alongside each question so the interviewer can compare in real-time.
   - The interviewer listens to the candidate’s verbal response and **scores directly** (0–10) — no need to type or log candidate answers.
   - At the end, summarize the candidate’s performance based on the interviewer’s scores.
   - Output a result classification: **Good**, **Average**, or **Not Selected**.
   - Allow the tech stack to be changed at any time during the interview.

2. Ensure the skill:
   - Uses structured prompts for consistency.
   - Stores interviewer scores temporarily for session evaluation.
   - Provides clear, professional summaries at the end.

## Artifacts
- `skills/interview-skill.skill.md` → Defines the interview skill.
- `docs/interview-skill.md` → Documentation of usage, parameters, and evaluation logic.

## Hooks
- **InterviewStarted**: Trigger `interview-skill` to ask setup questions.
- **ScoreRecorded**: Log interviewer score for each question.
- **InterviewCompleted**: Summarize results and output classification.

## Specs
- Must support multiple tech stacks dynamically.
- Must generate questions aligned with difficulty and experience level.
- Must provide both questions and answers.
- Must allow the interviewer to score candidate responses in real-time without logging them.
- Must classify results into Good, Average, Not Selected.

## Clarifications
Before proceeding, confirm:
- Should the evaluation be purely automated, or allow manual override?
- Should results be exported (e.g., to Confluence or Jira)?
- Should interviews support multiple rounds (technical + behavioral)?
