# Custom Agents in VS Code

Custom Agents in VS Code are simply a set of instructions that are defined by you, informing the AI Agent exactly what you want it to do and perform tailored development tasks. They let you define instructions, tools, and workflows so Copilot behaves like a dedicated reviewer, planner, or architect rather than a general-purpose assistant.

You can also use **handoffs** to create guided workflows between agents. Transition seamlessly from one specialized agent to another with a single select. For example, move from a planning agent directly into an implementation agent, or hand off to a code reviewer with the relevant context.

---

## How They Work in VS Code

### Agent Definition

- Custom agents are defined in a `.agent.md` Markdown file, inside `.github/agents/`.
- Includes metadata (name, description, argument-hint, agents, tools) and detailed instructions.

### Specs Folder

- `.github/specs/` holds spec definitions (e.g., data format and content structure for review tickets).
- Agents reference these specs to know the output format and configuration values.

### Selection in Copilot Chat

- Agents appear in the Copilot Chat dropdown.
- Developers can switch between agents depending on the task (e.g., planning → implementation → review).

---

## Example Use Cases

| Agent Type | Purpose |
|---|---|
| **Code Reviewer** | Enforces coding standards, flags bugs, suggests improvements. |
| **Security Agent** | Checks for vulnerabilities, unsafe dependencies, compliance issues. |
| **Planner Agent** | Breaks down requirements into tasks, designs architecture. |
| **Implementation Agent** | Generates boilerplate code, scaffolds frameworks. |

## Relevance of having Workflow in Custom Agent and Specs

They serve **different purposes** and operate at **different levels**:

### Agent Workflow ([am-code-reviewer.agent.md](.github/agents/am-code-reviewer.agent.md))
- Defines the **end-to-end interaction flow** between the agent and the developer
- Controls **when to prompt**, **what actions to take**, and **in what order** (review → save file → push to Confluence → apply fixes)
- Governs **agent behavior** — tools to use, how to communicate, when to wait for input
- Think of it as the **orchestration layer**

### Spec Workflow ([codereview-ticket.md](.github/specs/codereview-ticket.md))
- Defines the **data format and content structure** for review tickets
- Specifies **what a ticket looks like** (Title, Description, Severity, Resolution Steps, Acceptance Criteria)
- Controls **output shape** — naming patterns, Confluence page layout, table columns
- Think of it as the **schema/template layer**

### Why both?

| Concern | Agent | Spec |
|---|---|---|
| **What** to do step-by-step | ✅ | |
| **How** to format output | | ✅ |
| **When** to prompt the user | ✅ | |
| **What** fields each ticket has | | ✅ |
| **Where** to save/publish | Both reference it | Holds the config values |

The agent says *"after the review, ask to push to Confluence"*. The spec says *"the Confluence page must have a summary table with these columns and this title pattern"*. The agent references the spec (`Use the codereview-ticket spec to structure all feedback`) to know the output format.

**In short:** The agent owns the *workflow*. The spec owns the *data contract*. The overlap in the spec's `workflow` section is somewhat redundant — it could be trimmed to just the format/config concerns, since the agent file is the authoritative source for interaction flow.
