# Code Review — `TestInstructionsController.java`

**File:** `src/main/java/com/spring/customagent/controller/TestInstructionsController.java`
**Review Date:** 2026-06-29
**Reviewed By:** claude-code-review-orchestrator

---

## Strengths
- File header present with Author/Created/Updated, complying with project convention
- Minimal and focused — returns a simple health-check string
- Uses `@RestController` correctly

---

## Summary Table

| ID | Category | Severity | Title |
|----|----------|----------|-------|
| CR-001 | Architecture | **Medium** | Endpoint path violates project routing convention (`/student/**`) |
| CR-002 | Maintainability | **Medium** | Method name `CheckInstructions()` violates Java naming conventions |
| CR-003 | Maintainability | **Low** | Inconsistent indentation (one space vs four spaces) |
| CR-004 | Architecture | **Medium** | Controller lacks `@RequestMapping` base path |
| CR-005 | Maintainability | **Low** | Controller serves no business purpose — appears to be a test/debug artifact |
| CR-006 | Security | **Low** | Debug/test endpoint exposed with no access restriction |

---

## Detailed Findings

### CR-001 — Endpoint path violates project routing convention (`/student/**`)
- **Category:** Architecture
- **Severity:** Medium
- **Description:** The project's CLAUDE.md documents that all REST endpoints live under `/student/**`. This controller maps to `/checkinstructions`, which is outside that namespace. This breaks the uniform API prefix and could bypass any future path-based security or API-gateway rules applied to `/student/**`.
- **Resolution:** Move the endpoint under `/student/checkinstructions` by adding `@RequestMapping("/student")` at the class level, or move it to a dedicated `/admin/**` namespace.
- **Acceptance Criteria:**
  - The endpoint path starts with `/student/` or a clearly separated admin/debug prefix
  - Existing integration tests (if any) are updated to use the new path

### CR-002 — Method name `CheckInstructions()` violates Java naming conventions
- **Category:** Maintainability
- **Severity:** Medium
- **Description:** Java method names should follow camelCase, starting with a lowercase letter. `CheckInstructions()` starts with an uppercase letter, which makes it look like a constructor or class name.
- **Resolution:** Rename the method to `checkInstructions()`.
- **Acceptance Criteria:**
  - The method name begins with a lowercase letter
  - The application compiles and the endpoint still returns the expected response

### CR-003 — Inconsistent indentation
- **Category:** Maintainability
- **Severity:** Low
- **Description:** The `@GetMapping` annotation is indented with one space, while the method body uses four spaces. The rest of the project uses consistent four-space indentation.
- **Resolution:** Re-indent to use consistent four-space indentation.
- **Acceptance Criteria:**
  - All lines within the class body use four-space indentation consistently

### CR-004 — Controller lacks `@RequestMapping` base path
- **Category:** Architecture
- **Severity:** Medium
- **Description:** Unlike `StudentController`, which uses `@RequestMapping("/student")` at the class level, this controller has no class-level mapping. This is inconsistent with the project pattern.
- **Resolution:** Add `@RequestMapping("/student")` at the class level and adjust the `@GetMapping` value accordingly.
- **Acceptance Criteria:**
  - The controller has a class-level `@RequestMapping` annotation
  - The full endpoint path is preserved or intentionally updated

### CR-005 — Controller serves no business purpose
- **Category:** Maintainability
- **Severity:** Low
- **Description:** This controller returns a hardcoded string confirming that GitHub Copilot instructions are loaded. It does not participate in the Student CRUD workflow and appears to be a debugging artifact.
- **Resolution:** Gate it behind `@Profile("dev")` so it is only active during development.
- **Acceptance Criteria:**
  - The controller is gated behind a profile or documented with a clear rationale

### CR-006 — Debug/test endpoint exposed with no access restriction
- **Category:** Security
- **Severity:** Low
- **Description:** The `/checkinstructions` endpoint is publicly accessible. While it only returns a static string, it reveals internal tooling details (the existence of `copilot-instructions.md`).
- **Resolution:** Gate the endpoint behind `@Profile("dev")` so it is not reachable in production.
- **Acceptance Criteria:**
  - The endpoint is not accessible when the application runs with the default or production profile
