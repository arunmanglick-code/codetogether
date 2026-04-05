# Custom Agents in VS Code

Custom Agents in VS Code are simply a set of instructions that are defined by you, informing the AI Agent exactly what you want it to do and perform tailored development tasks. They let you define instructions, tools, and workflows so Copilot behaves like a dedicated reviewer, planner, or architect rather than a general-purpose assistant.

You can also use **handoffs** to create guided workflows between agents. Transition seamlessly from one specialized agent to another with a single select. For example, move from a planning agent directly into an implementation agent, or hand off to a code reviewer with the relevant context.

---

## How They Work in VS Code

### Agent Definition

- Custom agents are defined in a `.agent.md` Markdown file, inside `.github/copilot/agents/`.
- Includes metadata (name, description, argument-hint, agent) and detailed instructions.

### Specs Folder

- `.github/copilot/specs/` holds MCP (Model Context Protocol) definitions.
- Agents reference these specs to know which external servers/tools they can call.

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
