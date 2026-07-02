from contextlib import asynccontextmanager

from temporalio.client import Client

from app.config import settings

_client: Client | None = None


async def get_temporal_client() -> Client:
    global _client
    if _client is None:
        _client = await Client.connect(
            settings.TEMPORAL_SERVER_URL,
            namespace=settings.TEMPORAL_NAMESPACE,
        )
    return _client


async def close_temporal_client() -> None:
    global _client
    _client = None


@asynccontextmanager
async def temporal_lifespan():
    await get_temporal_client()
    print("Connected to Temporal")
    try:
        yield
    finally:
        await close_temporal_client()
