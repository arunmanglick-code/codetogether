# Phase 1 Decisions – Static Portfolio Setup

This file records all major decisions taken during Phase 1 of the portfolio website project.

---

## Decision 1: Frontend Framework
- **Choice**: [e.g., React / Vue / Plain HTML+CSS]
- **Reasoning**: Selected for simplicity, community support, and ease of integration with GitHub project data.
- **Timestamp**: YYYY-MM-DD HH:MM
- **Verification**: Initial layout renders correctly in local environment.

---

## Decision 2: Project Metadata Extraction
- **Choice**: Use GitHub API vs manual JSON file.
- **Reasoning**: GitHub API ensures live updates of project list.
- **Timestamp**: YYYY-MM-DD HH:MM
- **Verification**: Portfolio displays project titles and links from repo.

---

## Decision 3: Nginx Configuration
- **Choice**: Serve static files from `/usr/share/nginx/html`.
- **Reasoning**: Standard Nginx pattern, minimal configuration overhead.
- **Timestamp**: YYYY-MM-DD HH:MM
- **Verification**: Site accessible locally via `http://localhost`.

---

## Decision 4: Docker Setup
- **Choice**: Base image `nginx:alpine`.
- **Reasoning**: Lightweight, widely used for static site hosting.
- **Timestamp**: YYYY-MM-DD HH:MM
- **Verification**: Container builds and runs successfully.

---

## Decision 5: Ngrok Exposure
- **Choice**: Use free Ngrok tunnel for public access.
- **Reasoning**: Quick external access without backend hosting.
- **Timestamp**: YYYY-MM-DD HH:MM
- **Verification**: Site accessible via Ngrok URL.

---

## Notes
- Each decision includes **Choice, Reasoning, Timestamp, Verification**.  
- This file ensures traceability for Phase 1.  
- Future phases will have their own decision logs under `decisions/phase2.md`, `decisions/phase3.md`, etc.
