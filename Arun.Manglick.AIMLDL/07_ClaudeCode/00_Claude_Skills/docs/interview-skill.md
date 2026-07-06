# Interview Skill Documentation

## Overview

The `begin-interview` skill conducts structured technical interviews inside Claude Code. It generates questions tailored to a specific tech stack, difficulty level, and experience band, and displays the **model answer immediately** with each question. The interviewer listens to the candidate's verbal response and **scores it in real-time** (0–10) — no need to type or log the candidate's answer. At the end, a markdown summary report is produced with a classification of **Good**, **Average**, or **Not Selected**.

## Quick Start

```
/begin-interview
```

Or pre-fill the tech stack:

```
/begin-interview Java
```

Follow the setup prompts, then conduct the interview one question at a time. Each question is shown with its model answer — listen to the candidate and score their response directly. At the end, a scored report is saved to `interviews/`.

## Parameters

| Parameter | Valid Values | Default |
|-----------|-------------|---------|
| Tech Stack | Any technology (Java, Python, React, Angular, .NET, Spring Boot, AWS, Node.js, Go, etc.) | Required |
| Difficulty | high, medium, low | medium |
| Experience Level | 5–8 years, 8–11 years, 11–15 years | 8–11 years |
| Mode | scenario-based, one-liner | scenario-based |
| Candidate Name | Any name | Candidate |

## Question Modes

**Scenario-based** — Presents realistic situations requiring multi-step reasoning. Best for evaluating problem-solving and architectural thinking.

> Example: "Your team's Spring Boot microservice is experiencing intermittent 503 errors under load. Walk me through your investigation and resolution approach."

**One-liner** — Direct questions expecting concise answers. Best for evaluating breadth of knowledge.

> Example: "What is the difference between `@Component` and `@Service` in Spring?"

## Question Count

| Difficulty | Questions |
|-----------|-----------|
| Low | 5 |
| Medium | 8 |
| High | 10 |

## Interview Flow

For each question, the skill presents:

1. The question text
2. The **model answer** (shown immediately)
3. **Key evaluation points** (3–5 checklist items)
4. A prompt for the interviewer to enter a **score (0–10)** and optional notes

The interviewer listens to the candidate's verbal response, compares it against the model answer and key points, and enters a score directly. No need to type or record what the candidate said.

## Scoring System

The interviewer scores each answer 0–10 in real-time based on the key evaluation points:

| Score | Label |
|-------|-------|
| 9–10 | Excellent |
| 7–8 | Good |
| 5–6 | Adequate |
| 3–4 | Weak |
| 0–2 | Poor |

### Classification Thresholds

| Average Score | Classification |
|--------------|----------------|
| ≥ 7.0 | **Good** — Recommend for next round |
| 5.0–6.9 | **Average** — Borderline |
| < 5.0 | **Not Selected** |

After automated scoring, you can manually override the classification with a reason.

## Mid-Interview Commands

| Command | Effect |
|---------|--------|
| `change tech stack to [X]` | Regenerate remaining questions for the new stack |
| `skip` | Skip current question (scored 0) |
| `end interview` | Stop early and proceed to evaluation |
| `repeat` | Show the current question again |
| `cancel` | Offer to save partial report or discard |

## Output

Reports are saved as markdown files in the `interviews/` directory.

**Filename format:** `interviews/YYYY-MM-DD_candidate-name_tech-stack.md`

Each report contains:
- Interview metadata (tech stack, difficulty, experience, mode)
- Overall classification and average score
- Per-question breakdown with model answer, key evaluation points, interviewer score, and optional notes
- Strengths and areas for improvement
- Interviewer notes

## Examples

### High-difficulty Java scenario interview

```
/begin-interview Java
> Tech Stack: Java (confirmed)
> Difficulty: high
> Experience Level: 11–15 years
> Mode: scenario-based
> Candidate Name: John Smith

Question 1 of 10:
Your organization is migrating from a monolithic Java application to microservices...

Model Answer:
[Detailed model answer shown here]

Key Evaluation Points:
- [ ] Identifies bounded contexts for service decomposition
- [ ] Discusses data migration strategy
- [ ] Addresses inter-service communication patterns
- [ ] Considers observability and monitoring

Your score (0–10): ___
Notes (optional): ___
```

### Low-difficulty Python one-liner interview

```
/begin-interview Python
> Tech Stack: Python (confirmed)
> Difficulty: low
> Experience Level: 5–8 years
> Mode: one-liner
> Candidate Name: Jane Doe

Question 1 of 5:
What is the difference between a list and a tuple in Python?

Model Answer:
Lists are mutable, tuples are immutable. Lists use [], tuples use ().
Tuples are hashable and can be used as dict keys. Tuples are slightly
faster due to immutability optimizations.

Key Evaluation Points:
- [ ] Identifies mutability as the core difference
- [ ] Mentions syntax difference ([] vs ())
- [ ] Notes hashability / use as dict keys

Your score (0–10): ___
Notes (optional): ___
```

## Troubleshooting

**Skill not found in autocomplete:** Ensure you are running Claude Code from the `00_Claude_Skills` project directory. The `.claude/skills/` directory must be present.

**Report not saved:** The skill creates `interviews/` if it doesn't exist. If write permissions are blocked, approve the `mkdir -p interviews` permission when prompted.

**Niche tech stack warning:** The skill accepts any tech stack but warns for less mainstream technologies where question quality may vary.
