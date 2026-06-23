# This project is a combinaton of tech stack: Astro 5 + nginx + ngrok

Astro 5  - 
    A modern static site generator (SSG) and server‑side rendering (SSR) framework. 
    You build your site with Astro, and the output is either static files (/dist) or a Node.js server entry point.

Nginx  
    Acts as a reverse proxy or static file server.
    For static builds → Nginx serves files from /usr/share/nginx/html.
    For SSR builds → Nginx proxies requests to the Node server running Astro (e.g., port 4321).
    Benefits: caching, SSL termination, load balancing, and production‑grade performance.

ngrok  
    Creates a secure tunnel from your local machine to a public URL.
    Useful for demos, testing webhooks, or sharing your Astro site without deploying.
    Works with Nginx by forwarding traffic to your local port (e.g., ngrok http 8080).
    Requires setting the --host-header flag correctly so Nginx routes requests to the right virtual host.

