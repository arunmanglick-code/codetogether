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

# How to install ngrok 
 - Ref: https://www.youtube.com/watch?v=aFwrNSfthxU
 - Go to ngrok.com and create account
 - Downlaod ngrok https://dashboard.ngrok.com/get-started/setup/windows 
 - Unzip (preferably in C:\ngrok)
 - Add this path to Env Variable (Path)
 - Get your ngrok token from here - https://dashboard.ngrok.com/get-started/your-authtoken
 - Go to command prompt 
 - Type ngrok config add-authtoken $YOUR_AUTHTOKEN (use the ngrok token here)

 # Your app docker image in docker hdesktop
 ![Alt text](utils/images/dockerdesktop.png)

 # How to run the app
 - cd C:\Arun.Manglick\Arun.Manglick.PRJ\codetogether\Arun.Manglick.AIMLDL\07_ClaudeCode\01_Claude_Portfolio>
 - npm run dev
 ![alt text](utils/images/npmrundev.png)
 Then Browse http://localhost:4321/

 # How to run the in ngrok
 - Command Prompt (Any location and need not neceesary to be prject path)
 - Type ngrok http 4321 (This will give you a public url like  https://obscure-lanky-splashy.ngrok-free.dev)
 ![alt text](utils/images/ngrok%20forwarding.png)


