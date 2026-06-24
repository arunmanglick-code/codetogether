# Portfolio Website Tasks

This file tracks tasks for each phase of the portfolio website project.  
Tasks should be updated as decisions are made and clarifications are provided.

---

## Phase 1: Static Portfolio Setup
- [ ] Define frontend framework or static site generator (e.g., HTML/CSS, React, Vue).
- [ ] Clone GitHub repo: https://github.com/arunmanglick-code/codetogether
- [ ] Extract project metadata (titles, descriptions, links).
- [ ] Create portfolio layout to showcase projects.
- [ ] Configure Nginx to serve static files.
- [ ] Build Dockerfile for containerized deployment.
- [ ] Run site locally inside Docker.
- [ ] Expose site via Ngrok tunnel.
- [ ] Verify site accessibility and responsiveness.

---

## Phase 2: Backend Introduction
- [ ] Decide backend framework (Spring Boot, Node.js, Flask).
- [ ] Define API endpoints for project data.
- [ ] Containerize backend service.
- [ ] Integrate backend with frontend portfolio.
- [ ] Update Nginx reverse proxy configuration.
- [ ] Test API + frontend integration.

---

## Phase 3: Database Integration
- [ ] Choose database (PostgreSQL, MySQL, MongoDB).
- [ ] Define schema for project metadata.
- [ ] Connect backend to database.
- [ ] Implement CRUD operations for projects.
- [ ] Update portfolio to fetch data dynamically.
- [ ] Validate persistence and data integrity.

---

## Phase 4: Enhancements
- [ ] Add CI/CD pipeline (GitHub Actions, VIPER workflows).
- [ ] Add authentication/authorization if required.
- [ ] Add monitoring/logging (DataDog, Prometheus).
- [ ] Optimize Docker images and deployment.
- [ ] Document decisions in `decisions/` folder.

---

## Notes
- Each task should be marked complete (`[x]`) once implemented.
- Dependencies and clarifications should be logged in `decisions/`.
- This file evolves as Claude asks clarifying questions and refines the plan.
