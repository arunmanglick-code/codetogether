# Interview Summary

| Field | Value |
|-------|-------|
| Candidate | Candidate |
| Date | 2026-07-06 |
| Tech Stack | Java |
| Difficulty | Medium |
| Experience Band | 8–11 years |
| Mode | Scenario-based |
| Questions Answered | 3 of 8 (+ 1 custom) |

## Overall Result

**Classification: Average**
**Average Score: 6.0/10**

## Question-by-Question Breakdown

### Q1: Exception Handling Strategy
Your team maintains a Java 17 REST service that processes incoming order payloads. A junior developer has written a method that catches `Exception` at the top level and returns a generic 500 response for every failure — whether it's a validation error, a database timeout, or a null pointer. Users are complaining they never get useful error messages. How would you redesign the exception handling strategy for this service?

- **Model Answer:** Introduce a layered exception handling approach. Define a hierarchy of custom exceptions (e.g., `ValidationException`, `ResourceNotFoundException`, `ServiceUnavailableException`) each mapping to an appropriate HTTP status code (400, 404, 503). Use a global exception handler (`@ControllerAdvice` with `@ExceptionHandler` methods in Spring) to centralize the mapping. Let exceptions propagate naturally rather than catching and swallowing them. Use a consistent error response schema. Ensure unexpected 500s are logged but do not leak internal details to the client.
- **Key Evaluation Points:**
  - [ ] Proposes custom exception hierarchy mapped to specific HTTP status codes
  - [ ] Mentions centralized exception handling (`@ControllerAdvice` or equivalent global handler)
  - [ ] Emphasizes not catching generic `Exception` — let specific exceptions propagate
  - [ ] Describes a consistent, structured error response format for API consumers
  - [ ] Addresses security concern of not leaking internal details in responses
- **Interviewer Score:** 9/10
- **Interviewer Notes:** None

### Q2: HashMap equals/hashCode Contract
Your Java application uses a `HashMap<Employee, List<String>>` to cache employee permissions. After a recent release, the team notices that duplicate entries are appearing — the same employee shows up multiple times with different permission lists, and lookups by employee object are returning `null` even though the data was inserted moments earlier. The `Employee` class was recently refactored to add new fields. What is the likely root cause, and how would you fix it?

- **Model Answer:** The root cause is a broken `equals()` and `hashCode()` contract on the `Employee` class. When the class was refactored, either `hashCode()` was not updated to include the new fields, or `equals()` was updated but `hashCode()` was not — violating the contract that equal objects must return the same hash code. The fix is to ensure both methods use the same set of fields defining logical equality, and that those fields are immutable while the object is a map key. Using `Objects.hash()`, Java records, or Lombok's `@EqualsAndHashCode` can prevent this.
- **Key Evaluation Points:**
  - [ ] Identifies the broken `equals()`/`hashCode()` contract as the root cause
  - [ ] Explains that `HashMap` relies on `hashCode()` for bucket placement and `equals()` for key matching
  - [ ] States the rule: if two objects are equal via `equals()`, they must have the same `hashCode()`
  - [ ] Warns against using mutable fields in `hashCode()` when the object is used as a map key
  - [ ] Suggests practical safeguards (Java records, `Objects.hash()`, Lombok, or unit tests for the contract)
- **Interviewer Score:** 8/10
- **Interviewer Notes:** None

### Q3: (Skipped)
- **Interviewer Score:** 0/10
- **Interviewer Notes:** Skipped

## Custom Questions (Interviewer's Own)

### CQ1: Can you explain the difference between HashMap and ConcurrentHashMap in Java?
- **Interviewer Score:** 7/10
- **Interviewer Notes:** None

## Strengths
- Strong understanding of exception handling best practices and API design (scored 9/10)
- Solid grasp of Java fundamentals including the equals/hashCode contract (scored 8/10)
- Adequate knowledge of Java concurrency constructs (ConcurrentHashMap)

## Areas for Improvement
- Interview ended early — only 3 of 8 generated questions were attempted
- One question was skipped, which significantly impacted the average score
- Depth in advanced topics (system design, performance optimization) was not assessed due to early termination

## Interviewer Notes
None.
