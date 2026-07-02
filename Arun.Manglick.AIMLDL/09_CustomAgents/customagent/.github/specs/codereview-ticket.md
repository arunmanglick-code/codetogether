name: ticket-feedback
description: "Spec for generating code review feedback as structured ticket items with shared context for multi-agent coordination."
capabilities:
  - generateTicketFeedback
  - saveFeedbackToFile
  - publishToConfluence
  - createJiraTickets
format:
  reviewContext:
    - fileName: Name of the reviewed file
    - filePath: Full path to the reviewed file
    - reviewDate: Date in YYYY-MM-DD format
    - reviewTimestamp: Timestamp in HHmmss format
    - strengths: Array of positive observations
    - tickets: Array of ticket objects
  ticket:
    - id: Ticket ID (e.g., CR-001)
    - category: Security | Correctness | Performance | Maintainability | Architecture
    - severity: Critical | High | Medium | Low
    - title: Short summary of the issue
    - description: Detailed explanation of the problem
    - resolution: Practical steps to fix the issue
    - acceptanceCriteria: Conditions to verify the fix
agents:
  orchestrator: am-code-reviewer
  documentation: am-doc-publisher
  ticketing: am-ticket-creator
confluence:
  cloudId: "2c372697-c4a1-4192-9fa0-5cd146f9d535"
  spaceId: "3493233287"
  pageTitle-pattern: "{fileName}-review-{reviewDate}_{reviewTimestamp}"
  pageLayout:
    - Summary table at top with columns: Ticket ID | Category | Severity | Title | Status
    - Detailed findings below the table
jira:
  cloudId: "2c372697-c4a1-4192-9fa0-5cd146f9d535"
  projectKey: "CCMMER3"
  issueType: "Story"
  summaryPattern: "[Code Review] {fileName} — {ticket.title}"
filename-pattern: "{fileName}-review-{reviewDate}_{reviewTimestamp}.md"
