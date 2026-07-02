---
name: doc_publisher_skill
description: "Build Confluence page titles for code review publications. Use when the am-doc-publisher agent needs to construct a page title from review context."
user-invocable: false
---

# Build Page Title

## When to Use
- When `am-doc-publisher` is creating a Confluence page for a code review
- Any time a Confluence page title is needed for a review publication

## Title Pattern

The page title **MUST** follow this exact pattern:

```
{fileName}-am-review-{reviewDate}_{reviewTimestamp}
```

### Variables

| Variable | Source | Example |
|----------|--------|---------|
| `fileName` | The name of the reviewed file without extension | `Student` |
| `reviewDate` | The date of the review in `YYYY-MM-DD` format | `2026-04-12` |
| `reviewTimestamp` | The time of the review in `HHmmss` format | `100000` |

### Examples

| File | Date | Timestamp | Title |
|------|------|-----------|-------|
| `Student.java` | 2026-04-12 | 100000 | `Student-am-review-2026-04-12_100000` |
| `StudentDAOImpl.java` | 2026-04-05 | 081506 | `StudentDAOImpl-am-review-2026-04-05_081506` |
| `StudentServiceImpl.java` | 2026-04-06 | 100000 | `StudentServiceImpl-am-review-2026-04-06_100000` |

## Procedure

1. Extract the file name from the review context (`fileName` field), removing the `.java` (or other) extension.
2. Extract the `reviewDate` in `YYYY-MM-DD` format.
3. Extract the `reviewTimestamp` in `HHmmss` format.
4. Concatenate as: `{fileName}-am-review-{reviewDate}_{reviewTimestamp}`
5. Use this as the Confluence page title — do NOT deviate from this pattern.

## Rules
- NEVER hardcode or invent a title format outside this pattern.
- NEVER include spaces in the title.
- ALWAYS strip the file extension from `fileName`.
- If `reviewTimestamp` is not provided, default to `000000`.
