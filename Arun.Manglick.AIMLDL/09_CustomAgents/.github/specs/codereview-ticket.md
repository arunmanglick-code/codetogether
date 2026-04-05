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
filename-pattern: "am-code-review-feedback-YYYYMMDD-HHMM.md"
