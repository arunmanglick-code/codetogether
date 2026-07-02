import time
from collections import defaultdict

from starlette.middleware.base import BaseHTTPMiddleware
from starlette.requests import Request
from starlette.responses import JSONResponse

from app.config import settings


class RateLimiterMiddleware(BaseHTTPMiddleware):
    def __init__(self, app):
        super().__init__(app)
        self.requests: dict[str, list[float]] = defaultdict(list)
        self.max_requests = settings.RATE_LIMIT_REQUESTS
        self.window_seconds = settings.RATE_LIMIT_WINDOW_SECONDS

    async def dispatch(self, request: Request, call_next):
        client_ip = request.client.host if request.client else "unknown"
        now = time.time()
        window_start = now - self.window_seconds

        timestamps = self.requests[client_ip]
        self.requests[client_ip] = [t for t in timestamps if t > window_start]

        if len(self.requests[client_ip]) >= self.max_requests:
            oldest = min(self.requests[client_ip])
            retry_after = int(oldest + self.window_seconds - now) + 1
            return JSONResponse(
                status_code=429,
                content={
                    "error": True,
                    "status_code": 429,
                    "message": "Too many requests",
                    "details": [],
                },
                headers={"Retry-After": str(retry_after)},
            )

        self.requests[client_ip].append(now)
        return await call_next(request)
