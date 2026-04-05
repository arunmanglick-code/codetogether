name: ticket-feedback
description: 'Spec for generating code review feedback as structured ticket items.'
capabilities:
  - generateTicketFeedback
  - saveFeedbackToFile
format:
  ticket:
    - Title: Short summary of the issue
    - Issue Description: Detailed explanation of the problem
    - Issue Severity: Severity of the issue (Critical, High, Medium, Low)
    - Resolution Steps: Practical steps to fix the issue
    - Acceptance Criteria: Conditions to verify the fix
workflow:
  - Analyze code issues
  - Create ticket items for each issue
  - Prompt user to save tickets into a feedback file
  - Ensure the feedback file name includes the current date and timestamp
  - Prompt user to push review to Confluence
  - Create Confluence page using Atlassian MCP with title matching feedback file name
confluence:
  cloudId: "2c372697-c4a1-4192-9fa0-5cd146f9d535"
  spaceId: "3493233287"
  pageTitle-pattern: "{FileName}-review-{YYYYMMDD}_{HHmmss}"
  pageLayout:
    - Summary table at top with columns: Ticket ID | Category | Severity | Title | Status
    - Detailed findings below the table
jira:
  cloudId: "2c372697-c4a1-4192-9fa0-5cd146f9d535"
  projectKey: "CCMMER3"
  issueType: "Story"
  summaryPattern: "[Code Review] {FileName} — {Ticket Title}"
filename-pattern: "am-code-review-feedback-YYYYMMDD-HHMM.md"
