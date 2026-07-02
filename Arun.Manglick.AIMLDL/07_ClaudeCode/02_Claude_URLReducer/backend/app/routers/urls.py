import uuid

from fastapi import APIRouter, HTTPException, status

from app.config import settings
from app.schemas.click import UrlStatsResponse
from app.schemas.url import UrlCreateRequest, UrlCreateResponse, UrlListItem
from app.temporal.client import get_temporal_client
from app.temporal.dataclasses import (
    CreateShortUrlInput,
    GetUrlWithClicksInput,
    ListUrlsInput,
)
from app.temporal.workflows import (
    CreateShortUrlWorkflow,
    GetUrlWithClicksWorkflow,
    ListUrlsWorkflow,
)

router = APIRouter()


@router.post("/shorten", response_model=UrlCreateResponse, status_code=status.HTTP_201_CREATED)
async def shorten_url(request: UrlCreateRequest):
    client = await get_temporal_client()
    result = await client.execute_workflow(
        CreateShortUrlWorkflow.run,
        CreateShortUrlInput(original_url=str(request.url)),
        id=f"create-url-{uuid.uuid4()}",
        task_queue=settings.TEMPORAL_TASK_QUEUE,
    )
    return UrlCreateResponse(
        short_code=result.short_code,
        short_url=result.short_url,
        original_url=result.original_url,
    )


@router.get("/urls", response_model=list[UrlListItem])
async def get_urls():
    client = await get_temporal_client()
    result = await client.execute_workflow(
        ListUrlsWorkflow.run,
        ListUrlsInput(),
        id=f"list-urls-{uuid.uuid4()}",
        task_queue=settings.TEMPORAL_TASK_QUEUE,
    )
    return result.urls


@router.get("/urls/{short_code}/stats", response_model=UrlStatsResponse)
async def get_url_stats(short_code: str):
    client = await get_temporal_client()
    result = await client.execute_workflow(
        GetUrlWithClicksWorkflow.run,
        GetUrlWithClicksInput(short_code=short_code),
        id=f"stats-{short_code}-{uuid.uuid4()}",
        task_queue=settings.TEMPORAL_TASK_QUEUE,
    )
    if result is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="URL not found")
    return result.url
