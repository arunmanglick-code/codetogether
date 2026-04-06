# Code Review — Top 5 Issues

**Project**: customagent (Java Spring Boot)
**Date**: 2026-04-06
**Reviewer**: am-code-reviewer agent

---

## Summary

| Ticket | Category | Severity | Title | Status |
|--------|----------|----------|-------|--------|
| CR-001 | Security | CRITICAL | Hardcoded DB credentials in `application.properties` | Open |
| CR-002 | Security | HIGH | No input validation on `addStudent()` endpoint | Open |
| CR-003 | Architecture | MEDIUM | Redundant DAO layer — `StudentRepository` unused | Open |
| CR-004 | Maintainability | MEDIUM | `@Transactional` on DAO instead of service layer | Open |
| CR-005 | Correctness | MEDIUM | Inconsistent DI — mix of field and constructor injection | Open |

---

## CR-001 | Security | Severity: CRITICAL
### Hardcoded Database Credentials in `application.properties`

- **File**: `src/main/resources/application.properties` (lines 5–6)
- **Issue**: Database username (`root`) and password (`admin`) are hardcoded in plain text and will be committed to version control.
- **Resolution**:
  - Use environment variables or Spring profiles: `spring.datasource.password=${DB_PASSWORD}`
  - Or use Spring Cloud Config / Vault for secrets management
  - Add `application.properties` to `.gitignore` if it contains secrets, or use `application-local.properties`
- **Acceptance Criteria**: No plaintext credentials in source-controlled files.

---

## CR-002 | Security | Severity: HIGH
### No Input Validation on `addStudent()` Endpoint

- **File**: `src/main/java/com/spring/customagent/controller/StudentController.java` (lines 33–36)
- **Issue**: The `@RequestBody Student student` is accepted without any validation. Malicious or malformed input (null names, negative age, invalid email) flows directly to the database.
- **Current code**:
  ```java
  @PostMapping("/student/add")
  public Student addStudent(@RequestBody Student student) {
      return studentService.addStudent(student);
  }
  ```
- **Resolution**:
  - Add Jakarta Bean Validation annotations to `Student` entity (`@NotBlank`, `@Email`, `@Min`, `@Max`)
  - Add `@Valid` to the controller parameter: `@Valid @RequestBody Student student`
  - Return proper error responses via `@ExceptionHandler` or `@ControllerAdvice`
- **Acceptance Criteria**: Invalid input returns `400 Bad Request` with field-level error messages.

---

## CR-003 | Architecture | Severity: MEDIUM
### Redundant DAO Layer — `StudentRepository` is Unused

- **Files**: `src/main/java/com/spring/customagent/repository/StudentRepository.java`, `src/main/java/com/spring/customagent/dao/StudentDAOImpl.java`
- **Issue**: A `StudentRepository` (Spring Data JPA) exists but is never used. Instead, a manual `StudentDAOImpl` duplicates CRUD logic with raw `EntityManager` calls. This defeats the purpose of Spring Data JPA and creates unnecessary boilerplate.
- **Resolution**:
  - **Option A** (recommended): Remove `StudentDAO`/`StudentDAOImpl` and inject `StudentRepository` directly into the service layer.
  - **Option B**: Remove `StudentRepository` if you intentionally want manual EntityManager control.
- **Acceptance Criteria**: Only one data access pattern is used; no dead code.

---

## CR-004 | Maintainability | Severity: MEDIUM
### `@Transactional` Placed on DAO Instead of Service Layer

- **File**: `src/main/java/com/spring/customagent/dao/StudentDAOImpl.java` (lines 38–53)
- **Issue**: `@Transactional` is on individual DAO methods (`addStudent`, `updateStudent`, `deleteStudent`). In Spring Boot best practice, transactions should be managed at the service layer to allow multiple DAO operations within a single transaction boundary.
- **Current code**:
  ```java
  @Override
  @Transactional
  public Student addStudent(Student student) { ... }
  ```
- **Resolution**:
  - Move `@Transactional` to `StudentServiceImpl` methods
  - Remove `@Transactional` from `StudentDAOImpl`
- **Acceptance Criteria**: All `@Transactional` annotations are on service-layer methods only.

---

## CR-005 | Correctness | Severity: MEDIUM
### Inconsistent DI Pattern — Mix of `@Autowired` Field Injection and Constructor Injection

- **Files**: `src/main/java/com/spring/customagent/service/StudentServiceImpl.java` (lines 13–14), `src/main/java/com/spring/customagent/dao/StudentDAOImpl.java` (lines 17–20)
- **Issue**: `StudentServiceImpl` uses field injection (`@Autowired private StudentDAO`), while `StudentDAOImpl` uses constructor injection. Field injection is harder to test, hides dependencies, and is discouraged by the Spring team.
- **Current code** (`StudentServiceImpl`):
  ```java
  @Autowired
  private StudentDAO studentDAO;
  ```
- **Resolution**:
  ```java
  private final StudentDAO studentDAO;

  public StudentServiceImpl(StudentDAO studentDAO) {
      this.studentDAO = studentDAO;
  }
  ```
- **Acceptance Criteria**: All classes use constructor injection consistently.
