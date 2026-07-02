import re
import uuid
from datetime import datetime, timezone

from fastapi import APIRouter, HTTPException, Request, status
from fastapi.responses import RedirectResponse

from app.config import settings
from app.temporal.client import get_temporal_client
from app.temporal.dataclasses import GetUrlByCodeInput, RecordClickInput
from app.temporal.workflows import GetUrlByCodeWorkflow, RecordClickWorkflow

router = APIRouter()

SHORT_CODE_PATTERN = re.compile(r"^[0-9a-zA-Z]{6,8}$")


@router.get("/{short_code}")
async def redirect_to_url(short_code: str, request: Request):
    if not SHORT_CODE_PATTERN.match(short_code):
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="URL not found")

    client = await get_temporal_client()
    url = await client.execute_workflow(
        GetUrlByCodeWorkflow.run,
        GetUrlByCodeInput(short_code=short_code),
        id=f"get-url-{short_code}-{uuid.uuid4()}",
        task_queue=settings.TEMPORAL_TASK_QUEUE,
    )
    if url is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="URL not found")

    click_id = str(uuid.uuid4())
    clicked_at = datetime.now(timezone.utc).isoformat()
    await client.start_workflow(
        RecordClickWorkflow.run,
        RecordClickInput(
            short_code=short_code,
            click_id=click_id,
            clicked_at=clicked_at,
            referrer=request.headers.get("referer"),
            user_agent=request.headers.get("user-agent"),
            ip_address=request.client.host if request.client else None,
        ),
        id=f"click-{click_id}",
        task_queue=settings.TEMPORAL_TASK_QUEUE,
    )

    return RedirectResponse(url=url.original_url, status_code=status.HTTP_307_TEMPORARY_REDIRECT)
