# Interview Summary

| Field | Value |
|-------|-------|
| Candidate | Candidate |
| Date | 2026-07-06 |
| Tech Stack | Oracle ERP, Java, .NET |
| Difficulty | Medium (Q1–Q3), Low (Q4–Q10) |
| Experience Band | 8–11 years |
| Mode | Scenario-based |
| Questions Answered | 8 of 13 |

## Overall Result

**Classification: Good**
**Average Score: 8.3/10**

## Question-by-Question Breakdown

### Q1: Your organization is running Oracle E-Business Suite and users report that the month-end GL closing process is taking significantly longer than usual — what used to complete in 2 hours now takes over 8 hours. Finance leadership needs the books closed by tomorrow morning. Walk me through how you would investigate and resolve this.
- **Model Answer:** Start by checking the concurrent manager to identify which specific GL closing programs are running long — typically "General Ledger Transfer," "Journal Import," or "GL Posting." Review the concurrent request logs for wait times vs. actual runtime to determine if it's a queuing issue or a processing issue. Check if there's been a spike in transaction volume compared to previous periods. Examine the database side — look at AWR/ASH reports for top SQL statements consuming resources, check for full table scans on `GL_JE_LINES` or `GL_BALANCES`, and verify index health. Investigate whether any recent patches, customizations, or profile option changes were applied. As an immediate fix, consider purging GL interface data that has already been transferred, rebuilding indexes on key GL tables, and temporarily increasing concurrent manager worker processes. For the longer term, implement period-close checklists and schedule resource-intensive jobs in off-peak windows.
- **Key Evaluation Points:**
  - Identifies concurrent manager and specific GL programs as the starting diagnostic point
  - Analyzes database performance (AWR/ASH reports, index health, table statistics)
  - Considers transaction volume growth or data-related root causes
  - Proposes both immediate tactical fixes and longer-term preventive measures
  - Mentions checking for recent patches, customizations, or configuration changes
- **Interviewer Score:** 6/10
- **Interviewer Notes:** None

### Q2: Your company is planning to migrate from Oracle E-Business Suite 12.2 to Oracle Fusion Cloud ERP. The CFO wants to understand the risks and approach. As the Oracle ERP lead, you've been asked to present a high-level migration strategy. How would you structure the migration, and what are the critical considerations?
- **Model Answer:** Begin with a comprehensive assessment phase — inventory all EBS modules in use (GL, AP, AR, FA, CM, etc.), catalog all customizations (reports, forms, interfaces, extensions), and identify integrations with third-party systems. Classify customizations into three buckets: those that can be replaced by standard Fusion Cloud functionality, those that need to be rebuilt as Fusion extensions (using VBCS, BIP, or REST APIs), and those that should be retired. Adopt a phased or modular migration approach rather than a big-bang cutover — consider starting with Finance (GL, AP, AR) as the first wave since it typically has fewer customizations than modules like Manufacturing or SCM. Plan for data migration using tools like Oracle's Data Migration Workbench or FBDI (File-Based Data Import) templates, and decide on historical data strategy — how many years to migrate vs. archive in EBS as read-only. Address change management heavily, as the user experience shift from Forms-based to cloud UI is significant. Plan for a parallel-run period where both systems operate simultaneously to validate data accuracy. Finally, establish a realistic timeline — typically 9–18 months depending on scope — and secure executive sponsorship with a clear governance structure.
- **Key Evaluation Points:**
  - Conducts a thorough assessment of current EBS landscape (modules, customizations, integrations)
  - Classifies customizations with a clear adopt/adapt/retire strategy
  - Recommends phased migration rather than big-bang, with rationale for module sequencing
  - Addresses data migration approach including historical data decisions and FBDI/migration tools
  - Highlights change management, parallel-run validation, and governance as critical success factors
- **Interviewer Score:** 8/10
- **Interviewer Notes:** None

### Q3: *(Tech stack changed to Java before scoring — question skipped)*

### Q4: You're working on a Java web application and a junior developer on your team has committed code that creates database connections inside a servlet's `doGet()` method but never closes them. Users are reporting that the application becomes unresponsive after a few hours of use. Walk me through how you would explain the problem to the junior developer and what fix you would recommend.
- **Model Answer:** Explain that each call to `doGet()` opens a new database connection, and since connections are never closed, they accumulate until the database server hits its maximum connection limit — at that point, new requests block waiting for a connection and the application becomes unresponsive. This is a resource leak. The immediate fix is to use a try-with-resources block (`try (Connection conn = dataSource.getConnection())`) so that the connection is automatically closed when the block exits, even if an exception occurs. The better long-term approach is to use a connection pool (like HikariCP or the application server's built-in JNDI DataSource) instead of creating raw connections — pooling reuses connections efficiently and sets maximum limits. Also explain the importance of closing all JDBC resources — not just `Connection`, but also `Statement` and `ResultSet` — in the proper order. Suggest adding a code review checklist item for resource management to prevent similar issues in future.
- **Key Evaluation Points:**
  - Clearly explains the resource leak — connections are opened but never closed, leading to exhaustion
  - Recommends try-with-resources as the immediate fix for automatic resource cleanup
  - Suggests connection pooling (HikariCP, JNDI DataSource) as the proper long-term solution
  - Mentions closing all JDBC resources (Connection, Statement, ResultSet)
  - Addresses prevention through code review practices or team awareness
- **Interviewer Score:** 9/10
- **Interviewer Notes:** None

### Q5: Your team's Java application uses `HashMap` extensively throughout the codebase. A bug report comes in that a multithreaded batch processing module is producing inconsistent results — sometimes records are missing from the output, and occasionally the process hangs entirely. A colleague suspects it's related to the `HashMap` usage. Walk me through how you would confirm the diagnosis and what you would recommend.
- **Model Answer:** `HashMap` is not thread-safe. When multiple threads read and write to a `HashMap` concurrently without synchronization, it can lead to a corrupted internal state. Specifically, concurrent `put()` operations can cause the internal linked list or tree structure in a bucket to form a cycle, which causes `get()` calls to loop infinitely — explaining the hangs. Missing records happen because concurrent modifications can cause entries to be lost during internal resizing (rehashing). To confirm the diagnosis, review the code to verify that the `HashMap` is indeed shared across threads without synchronization — check if multiple threads reference the same instance. You could also add thread-dump analysis during a hang to see threads stuck in `HashMap.get()` or `HashMap.put()`. For the fix, replace `HashMap` with `ConcurrentHashMap`, which uses fine-grained locking (segment/bucket-level) and is designed for concurrent access. Alternatively, if the access pattern is simple, `Collections.synchronizedMap()` could work but offers worse performance since it locks the entire map on every operation. Advise the team to review other shared mutable data structures in the batch module for similar thread-safety issues.
- **Key Evaluation Points:**
  - Explains that `HashMap` is not thread-safe and concurrent access causes corruption
  - Identifies specific failure modes: infinite loops from bucket cycles (hangs) and lost entries from concurrent resizing (missing records)
  - Recommends `ConcurrentHashMap` as the primary solution with rationale (fine-grained locking)
  - Mentions `Collections.synchronizedMap()` as an alternative with its performance tradeoff
  - Suggests thread-dump analysis as a diagnostic technique to confirm the issue
- **Interviewer Score:** 8/10
- **Interviewer Notes:** None

### Q6: A production Java application that your team maintains is throwing `OutOfMemoryError: Java heap space` intermittently, usually during peak business hours. The application runs on Tomcat and processes CSV file uploads from users. Your manager asks you to investigate and fix it before end of day. How would you approach this?
- **Model Answer:** First, check the current JVM heap settings (`-Xms` and `-Xmx` in Tomcat's `CATALINA_OPTS` or `setenv.sh`) to understand the allocated memory. Enable GC logging (`-verbose:gc` or `-Xlog:gc*` for Java 9+) to observe garbage collection patterns — frequent full GCs with little memory recovery indicate a leak. Capture a heap dump when the error occurs by adding `-XX:+HeapDumpOnOutOfMemoryError -XX:HeapDumpPath=/path/to/dump` to the JVM options. Analyze the heap dump using a tool like Eclipse MAT (Memory Analyzer Tool) to identify which objects are consuming the most memory. The likely culprit with CSV uploads is that the application reads the entire file into memory at once (e.g., loading all rows into an `ArrayList`) rather than processing it line by line using a streaming/buffered approach like `BufferedReader`. The fix is to refactor the CSV processing to use a streaming approach — read and process one line or a small batch at a time instead of loading the entire file. Additionally, consider adding file size validation on upload to reject excessively large files, and set appropriate heap size based on the expected workload.
- **Key Evaluation Points:**
  - Checks current JVM heap settings (`-Xms`, `-Xmx`) as a baseline
  - Enables heap dump capture on OOM and uses a tool like Eclipse MAT for analysis
  - Identifies the likely root cause: entire CSV file loaded into memory instead of streamed
  - Recommends streaming/buffered processing approach as the fix
  - Suggests preventive measures like file size validation and appropriate heap sizing
- **Interviewer Score:** 8/10
- **Interviewer Notes:** None

### Q7: You're reviewing a pull request from a teammate and notice they've written a Java class with several `public` fields, no getters or setters, and all business logic in a single 500-line method called `processEverything()`. The code works and passes all tests. Your teammate asks why it needs changes. How would you explain the issues and what refactoring would you suggest?
- **Model Answer:** Start by acknowledging that while the code works functionally, it has maintainability and design concerns that will cause problems as the codebase grows. Explain encapsulation first — `public` fields expose the internal state of the class, meaning any other class can modify the data directly without validation or control. Using `private` fields with getters and setters allows you to add validation logic, trigger side effects, or change the internal representation later without breaking callers. For the 500-line method, explain the Single Responsibility Principle — a method should do one thing well. A large method is hard to read, hard to test in isolation, and hard to debug because a bug could be anywhere in 500 lines. Suggest breaking it into smaller, well-named private methods where each method handles one logical step of the process (e.g., `validateInput()`, `transformData()`, `persistResults()`). This also enables unit testing of individual steps. Recommend extracting related groups of fields and behavior into separate classes if they represent distinct concepts — this follows good object-oriented design. Frame the feedback positively: the goal is making the code easier for the whole team to maintain, extend, and debug.
- **Key Evaluation Points:**
  - Explains encapsulation — why `public` fields should be `private` with accessors for controlled access
  - Describes the Single Responsibility Principle and why a 500-line method violates it
  - Recommends decomposing the large method into smaller, well-named methods with distinct responsibilities
  - Suggests extracting separate classes if fields/behavior represent distinct concepts
  - Frames the feedback constructively, focusing on team maintainability and testability
- **Interviewer Score:** 9/10
- **Interviewer Notes:** None

### Q8: Your Java application uses a REST API to fetch customer data from an external service. During testing, you notice that when the external service is slow or down, your entire application freezes and stops responding to all users — not just those whose requests depend on the external service. Walk me through what's going wrong and how you would fix it.
- **Model Answer:** The most likely cause is that the HTTP client making calls to the external service has no connection timeout or read timeout configured. When the external service is slow or unresponsive, the threads making those calls block indefinitely waiting for a response. Since the application runs on a servlet container like Tomcat with a fixed thread pool (typically 200 threads by default), all threads eventually get occupied by these blocked requests, leaving no threads available to serve other users — even for pages that don't call the external service. The immediate fix is to set explicit timeouts on the HTTP client — both a connection timeout (e.g., 5 seconds to establish the connection) and a read/socket timeout (e.g., 10 seconds to receive data). In Java, this depends on the client library: for `HttpURLConnection` use `setConnectTimeout()` and `setReadTimeout()`; for Apache HttpClient use `RequestConfig.Builder` with `setConnectTimeout()` and `setSocketTimeout()`; for newer `java.net.HttpClient` use `.connectTimeout()` and the request's `.timeout()`. Beyond timeouts, implement a circuit breaker pattern (using a library like Resilience4j) so that after a threshold of failures, the application stops calling the failing service for a cooldown period and returns a fallback response instead. Also consider making the external service calls asynchronous or moving them to a separate thread pool to isolate them from the main request-handling threads.
- **Key Evaluation Points:**
  - Identifies the root cause: missing HTTP timeouts causing threads to block indefinitely
  - Explains thread pool exhaustion — blocked threads consume all available workers, starving unrelated requests
  - Recommends setting explicit connection and read/socket timeouts with appropriate values
  - Suggests the circuit breaker pattern (e.g., Resilience4j) for resilience against external service failures
  - Proposes isolation strategies such as async calls or a dedicated thread pool for external service requests
- **Interviewer Score:** 10/10
- **Interviewer Notes:** None

### Q9: Your team maintains a .NET 6 Web API application. After a recent deployment, users report that API responses are intermittently slow. You check the logs and see that some requests take over 10 seconds while others complete in milliseconds. The slow requests seem random — no specific endpoint is consistently affected. Walk me through how you would diagnose this.
- **Model Answer:** Start by checking if the issue correlates with garbage collection pauses — in .NET, GC (especially full Gen 2 collections) can cause brief but noticeable freezes across all threads. Review GC logs or use `dotnet-counters` to monitor GC pause times and heap sizes. Check if the application is running in Server GC mode (appropriate for web apps) vs. Workstation GC mode by verifying the `<ServerGarbageCollection>` setting in the `.csproj` or `runtimeconfig.json`. Another common cause is thread pool starvation — if synchronous blocking calls (like `.Result` or `.Wait()` on async methods) are used, they can exhaust the thread pool. Use `dotnet-counters` to monitor the `ThreadPool Queue Length` and `ThreadPool Thread Count`. Also check for async-over-sync anti-patterns using a profiler or code review. Additionally, examine if any middleware in the pipeline is causing delays (logging, authentication, or custom middleware). Use Application Insights or distributed tracing to correlate slow requests with specific dependencies like database calls or external service calls.
- **Key Evaluation Points:**
  - Considers GC pauses (Gen 2 collections) and checks Server vs. Workstation GC mode
  - Identifies thread pool starvation from blocking async calls (`.Result`, `.Wait()`) as a likely cause
  - Uses diagnostic tools like `dotnet-counters` or `dotnet-trace` for runtime analysis
  - Examines the middleware pipeline for potential bottlenecks
  - Suggests distributed tracing or Application Insights to correlate slow requests with dependencies
- **Interviewer Score:** 8/10
- **Interviewer Notes:** None

### Q10: *(Interview ended before scoring)*

## Strengths
- Excellent understanding of resilience patterns (circuit breakers, timeouts, thread isolation) — scored 10/10 on Q8
- Strong grasp of OOP principles and ability to communicate design feedback constructively (Q7: 9/10)
- Solid resource management knowledge in Java (JDBC connections, try-with-resources, connection pooling — Q4: 9/10)
- Good diagnostic methodology across all tech stacks, consistently identifying root causes before proposing fixes
- Comfortable working across multiple technology domains (Oracle ERP, Java, .NET)

## Areas for Improvement
- Oracle ERP depth could be stronger — GL closing performance question scored lowest at 6/10
- Could benefit from deeper operational/DBA-level knowledge for ERP troubleshooting scenarios

## Interviewer Notes
I would like to refer to next round
