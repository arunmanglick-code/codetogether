from collections.abc import AsyncGenerator
from contextlib import asynccontextmanager

from fastapi import FastAPI
from fastapi.exceptions import RequestValidationError
from fastapi.middleware.cors import CORSMiddleware

from app.config import settings
from app.middleware.error_handler import generic_exception_handler, validation_exception_handler
from app.middleware.rate_limiter import RateLimiterMiddleware
from app.routers import redirect, urls
from app.temporal.client import temporal_lifespan


@asynccontextmanager
async def lifespan(app: FastAPI) -> AsyncGenerator[None, None]:
    async with temporal_lifespan():
        yield


app = FastAPI(title="URL Reducer", version="1.0.0", lifespan=lifespan)

app.add_middleware(RateLimiterMiddleware)
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.CORS_ORIGINS,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.add_exception_handler(RequestValidationError, validation_exception_handler)
app.add_exception_handler(Exception, generic_exception_handler)

@app.get("/health")
async def health():
    return {"status": "ok"}


app.include_router(urls.router, prefix="/api", tags=["urls"])
app.include_router(redirect.router, tags=["redirect"])
