# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Portfolio site built with **Astro 5** (static site generator / SSR framework), served via **Nginx** (reverse proxy / static file server), and exposed locally via **ngrok** (secure tunnel for demos and webhook testing).

## Architecture

- **Astro 5** produces either static files in `/dist` (SSG mode) or a Node.js server entry point (SSR mode).
- **Nginx** serves static builds from `/usr/share/nginx/html`, or proxies to the Astro Node server (port 4321) for SSR builds. Handles caching, SSL termination, and load balancing.
- **ngrok** tunnels local Nginx (typically port 8080) to a public URL. Use `--host-header` flag so Nginx routes to the correct virtual host.

## Development Commands

```bash
# Install dependencies
npm install

# Dev server with hot reload
npm run dev          # starts on http://localhost:4321

# Production build
npm run build        # outputs to ./dist

# Preview production build locally
npm run preview

# ngrok tunnel (after starting Nginx on port 8080)
ngrok http 8080 --host-header=localhost:8080
```

## Project Status

This project is in early setup — no source code has been scaffolded yet. When initializing, use `npm create astro@latest` and configure for the stack described above.
