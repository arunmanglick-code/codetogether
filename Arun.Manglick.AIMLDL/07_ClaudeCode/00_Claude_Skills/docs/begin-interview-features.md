# `/begin-interview` — AI-Powered Technical Interview Skill for Claude Code

A Claude Code custom skill that transforms your terminal into a structured, scoring-ready technical interview platform. Generate expert-level questions with instant model answers, score candidates in real-time, switch tech stacks mid-interview, ask your own questions, and produce a professional summary report — all from a single slash command.

---

## Why This Skill?

Running a technical interview is hard. You need questions that match the role, a mental model of what a good answer looks like, a consistent scoring rubric, and a way to capture everything for the debrief. Most interviewers rely on memory, scattered notes, or generic question banks that don't adapt to the candidate in front of them.

`/begin-interview` solves this by giving the interviewer a structured co-pilot:

- **No prep required** — questions are generated on the fly, tailored to the exact tech stack, difficulty, and experience level you specify.
- **Instant model answers** — every question comes with a reference answer and 3-5 key evaluation points, so you know exactly what to listen for.
- **Score as you go** — rate each answer 0-10 while the conversation is fresh. No post-interview recall bias.
- **Flexible mid-interview** — switch tech stacks, inject your own questions, skip, repeat, or end early. The skill adapts.
- **Automated report** — a markdown summary with per-question breakdown, strengths, improvement areas, and a hire/no-hire classification is saved automatically.

---

## Key Features

### 1. Multi-Tech-Stack Coverage

The skill works with **any technology** — not a fixed question bank. Specify one or more technologies and get questions written by an AI that understands the domain deeply.

**Supported stacks include (but are not limited to):**

| Category | Examples |
|----------|----------|
| Languages | Java, Python, C#, Go, TypeScript, Rust, Kotlin, Swift |
| Frontend | React, Angular, Vue, Next.js, Svelte |
| Backend | Spring Boot, .NET, Node.js, Django, FastAPI, Express |
| Cloud & Infra | AWS, Azure, GCP, Kubernetes, Docker, Terraform |
| Data | SQL, PostgreSQL, MongoDB, Redis, Kafka, Spark |
| AI/ML | Agentic AI, LLMs, RAG, LangChain, PyTorch, TensorFlow |
| Enterprise | Oracle ERP, SAP, Salesforce |
| Mobile | React Native, Flutter, iOS (Swift), Android (Kotlin) |

**Multi-stack interviews are supported.** Specify `Java, .NET` or `Oracle ERP, Java, Agentic AI` and the skill generates questions spanning all listed technologies in a single session.

For niche or emerging technologies, the skill accepts the input and generates questions based on its knowledge, with a transparency warning if question quality may vary.

---

### 2. Configurable Difficulty & Experience Levels

Every interview is tuned along two axes — **difficulty** and **experience band** — so questions match the seniority of the role you're hiring for.

**Difficulty levels:**

| Level | Questions | Composition |
|-------|-----------|-------------|
| **Low** | 5 | 60% fundamentals, 30% practical, 10% edge cases |
| **Medium** | 8 | 30% fundamentals, 40% practical/design, 20% edge cases, 10% advanced |
| **High** | 10 | 10% fundamentals, 30% design/architecture, 30% advanced scenarios, 30% tradeoff analysis |

**Experience bands:**

| Band | Focus |
|------|-------|
| **5-8 years** | Solid fundamentals, code-level design patterns, debugging, testing strategies |
| **8-11 years** | System design, performance optimization, CI/CD, architectural tradeoffs |
| **11-15 years** | Architecture at scale, cross-system integration, tech strategy, migration planning |

A *High difficulty, 11-15 years* interview produces questions about large-scale architecture decisions and business-context tradeoffs. A *Low difficulty, 5-8 years* interview focuses on core concepts and practical coding knowledge. The combination shapes every question generated.

---

### 3. Two Question Modes

| Mode | Best For | Style |
|------|----------|-------|
| **Scenario-based** | Evaluating problem-solving, architectural thinking, and communication | Presents a realistic situation requiring multi-step reasoning |
| **One-liner** | Evaluating breadth of knowledge and quick recall | Direct questions expecting concise answers |

**Scenario-based example:**
> *"Your team's Spring Boot microservice is experiencing intermittent 503 errors under load. Walk me through your investigation and resolution approach."*

**One-liner example:**
> *"What is the difference between `@Component` and `@Service` in Spring?"*

---

### 4. Instant Model Answers with Evaluation Points

Every generated question is paired with:

- A **comprehensive model answer** — the reference-quality response the interviewer can compare against in real-time.
- **3-5 key evaluation points** — a checklist of the specific things a strong answer should cover.

This means the interviewer doesn't need to be an expert in every topic. The model answer is visible immediately, so you can listen to the candidate and compare on the spot.

```
Question 3 of 8:
Your Java application uses a REST API to fetch customer data from an
external service. When the external service is slow, your entire
application freezes. Walk me through what's going wrong and how you
would fix it.

Model Answer:
The most likely cause is that the HTTP client has no connection or read
timeout configured. When the external service is unresponsive, threads
block indefinitely. Since Tomcat has a fixed thread pool (~200 threads),
all threads get occupied by blocked requests, starving other users...

Key Evaluation Points:
- [ ] Identifies missing HTTP timeouts as the root cause
- [ ] Explains thread pool exhaustion
- [ ] Recommends explicit connection and read timeouts
- [ ] Suggests circuit breaker pattern (e.g., Resilience4j)
- [ ] Proposes isolation (async calls or dedicated thread pool)

Your score (0-10): ___
Notes (optional): ___
```

---

### 5. Ask Your Own Questions

At any point during the interview, the interviewer can inject their own custom questions. This is useful when:

- You want to **follow up** on something the candidate said
- You have a **team-specific** question that wouldn't appear in a general bank
- You want to probe a **specific area of concern**
- You want to test something **outside the selected tech stack**

**How it works:**

After scoring any question, the skill asks:
> *"Ready for the next question, or would you like to ask your own question?"*

Type `my question` or `custom`, and the skill prompts you to enter your question. You score the candidate's response just like any other question. Custom questions:

- Are tracked **separately** in the report under a "Custom Questions" section
- **Do not consume** a slot from the generated question set
- **Are included** in the overall average score calculation
- Can be asked **multiple times in a row** before returning to generated questions
- Do not require a model answer — you score based on your own expertise

---

### 6. Real-Time Scoring

The interviewer scores each answer **in the moment**, on a 0-10 scale. No need to type or record what the candidate said — just listen, compare against the model answer and key points, and enter a number.

**Scoring rubric (shown before the first question):**

| Score | Label | Criteria |
|-------|-------|----------|
| 9-10 | Excellent | Covers all key points, demonstrates deep understanding |
| 7-8 | Good | Covers most key points (70%+), solid understanding |
| 5-6 | Adequate | Covers some key points (40-70%), basic familiarity |
| 3-4 | Weak | Covers few key points (<40%), significant gaps |
| 0-2 | Poor | Incorrect, irrelevant, or no answer |

At the end, scores are averaged and automatically classified:

| Average | Classification | Recommendation |
|---------|---------------|----------------|
| 7.0+ | **Good** | Recommend for next round |
| 5.0-6.9 | **Average** | Borderline — interviewer discretion |
| <5.0 | **Not Selected** | Does not meet the bar |

The interviewer can **override** the automated classification with a reason (e.g., "Overriding Average to Good — candidate showed exceptional problem-solving despite scoring lower on fundamentals").

---

### 7. Mid-Interview Flexibility

The skill supports several commands that can be used at any point during the interview:

| Command | What It Does |
|---------|-------------|
| `change tech stack to [X]` | Keeps all completed answers. Regenerates remaining questions for the new tech stack. |
| `skip` | Records a 0 for the current question and moves on |
| `repeat` | Shows the current question again |
| `end interview` / `stop` | Ends early and proceeds to evaluation with all answers so far |
| `my question` / `custom` | Lets the interviewer ask their own question |
| `cancel` | Offers to save a partial report or discard entirely |

**Tech stack switching** is particularly powerful for multi-domain roles. Start with Oracle ERP questions, switch to Java after question 3, then switch to .NET for the final questions — all in a single interview session with a unified report.

---

### 8. Automated Report Generation

When the interview ends (or is stopped early), the skill generates a structured markdown report and saves it to the `interviews/` directory.

**Filename format:** `interviews/YYYY-MM-DD_candidate-name_tech-stack.md`

**The report includes:**

| Section | Contents |
|---------|----------|
| **Interview Metadata** | Candidate name, date, tech stack, difficulty, experience band, mode, questions answered |
| **Overall Result** | Classification (Good / Average / Not Selected), average score, override details if applicable |
| **Question-by-Question Breakdown** | Each question with its model answer, key evaluation points, interviewer score, and notes |
| **Custom Questions Section** | All interviewer-provided questions with scores and notes |
| **Strengths** | Areas where the candidate performed well |
| **Areas for Improvement** | Gaps and weaknesses identified |
| **Interviewer Notes** | Free-form notes added by the interviewer before the report is saved |

Reports are self-contained markdown files that can be shared with hiring committees, attached to ATS records, or archived for compliance.

---

## Getting Started

### Prerequisites

- [Claude Code](https://docs.anthropic.com/en/docs/claude-code) installed and configured
- Clone or copy this project so the `.claude/skills/begin-interview/` directory is present

### Usage

**Start with a tech stack pre-filled:**
```
/begin-interview Java
```

**Start with interactive setup (all parameters prompted):**
```
/begin-interview
```

The skill walks you through confirming or modifying all parameters before generating questions. Once confirmed, the interview begins — one question at a time, each with its model answer displayed, ready for you to score.

---

## Example Interview Scenarios

### Scenario A: Senior Java Developer (High Difficulty)

```
/begin-interview Java
  Tech Stack:        Java
  Difficulty:        High
  Experience Level:  11-15 years
  Mode:              Scenario-based
  Candidate Name:    Jane Smith
```

Produces **10 scenario-based questions** covering architecture at scale, performance optimization, concurrency, resilience patterns, and cross-system design tradeoffs — each with a detailed model answer referencing production-grade patterns.

### Scenario B: Mid-Level Python Developer (Medium Difficulty)

```
/begin-interview Python
  Tech Stack:        Python
  Difficulty:        Medium
  Experience Level:  5-8 years
  Mode:              One-liner
  Candidate Name:    Alex Johnson
```

Produces **8 one-liner questions** spanning fundamentals, practical usage, edge cases, and a few advanced topics — calibrated for someone with solid but not senior-level experience.

### Scenario C: Multi-Stack Enterprise Role

```
/begin-interview Oracle ERP, Java, .NET
  Tech Stack:        Oracle ERP, Java, .NET
  Difficulty:        Medium
  Experience Level:  8-11 years
  Mode:              Scenario-based
  Candidate Name:    Pat Williams
```

Produces **8 scenario-based questions** distributed across all three technologies. The interviewer can also switch tech stack mid-interview if they want to shift focus.

### Scenario D: Emerging Tech — Agentic AI

```
/begin-interview Agentic AI
  Tech Stack:        Agentic AI
  Difficulty:        Low
  Experience Level:  5-8 years
  Mode:              One-liner
  Candidate Name:    George
```

Produces **5 one-liner questions** covering AI agent fundamentals, multi-agent architectures, tool use, safety, and evaluation — demonstrating the skill's ability to generate questions for cutting-edge domains.

---

## Feature Summary

| Feature | Description |
|---------|-------------|
| Any tech stack | Works with any technology — mainstream or niche |
| Multi-stack interviews | Combine multiple technologies in a single session |
| 3 difficulty levels | Low (5 Qs), Medium (8 Qs), High (10 Qs) with scaled question composition |
| 3 experience bands | 5-8, 8-11, 11-15 years — questions adapt to expected seniority |
| 2 question modes | Scenario-based or one-liner |
| Instant model answers | Reference answer + 3-5 key evaluation points for every question |
| Real-time scoring | Score 0-10 per question with optional notes |
| Custom questions | Inject your own questions at any point — scored and included in the report |
| Mid-interview commands | Skip, repeat, end early, switch tech stack, cancel with partial save |
| Automated classification | Good / Average / Not Selected based on average score |
| Manual override | Override the automated classification with a reason |
| Markdown report | Full interview summary saved to `interviews/` with all scores, answers, and analysis |
| Strengths & gaps analysis | Automatically identifies candidate strengths and areas for improvement |
| No prep needed | Just type the command and start — questions generated in seconds |

---

## License

This skill is part of the Claude Code custom skills collection. See the repository root for license details.
