from datetime import datetime

from sqlalchemy import select, update
from sqlalchemy.exc import IntegrityError
from temporalio import activity

from app.config import settings
from app.database import async_session
from app.models.click import Click
from app.models.url import Url
from app.services.shortcode import generate_short_code
from app.temporal.dataclasses import (
    CreateShortUrlInput,
    CreateShortUrlOutput,
    GetUrlByCodeInput,
    GetUrlByCodeOutput,
    GetUrlWithClicksInput,
    GetUrlWithClicksOutput,
    ListUrlsInput,
    ListUrlsOutput,
    RecordClickInput,
    RecordClickOutput,
)

MAX_COLLISION_RETRIES = 5


@activity.defn
async def create_short_url_activity(input: CreateShortUrlInput) -> CreateShortUrlOutput:
    async with async_session() as db:
        for _ in range(MAX_COLLISION_RETRIES):
            short_code = generate_short_code(settings.SHORT_CODE_LENGTH)
            existing = await db.execute(select(Url).where(Url.short_code == short_code))
            if existing.scalar_one_or_none() is None:
                url = Url(short_code=short_code, original_url=input.original_url)
                db.add(url)
                await db.commit()
                await db.refresh(url)
                return CreateShortUrlOutput(
                    short_code=url.short_code,
                    original_url=url.original_url,
                    short_url=f"{settings.BASE_URL}/{url.short_code}",
                    created_at=url.created_at.isoformat(),
                )
        raise RuntimeError("Failed to generate a unique short code after maximum retries")


@activity.defn
async def get_url_by_code_activity(input: GetUrlByCodeInput) -> GetUrlByCodeOutput | None:
    async with async_session() as db:
        result = await db.execute(
            select(Url).where(Url.short_code == input.short_code, Url.is_active == True)
        )
        url = result.scalar_one_or_none()
        if url is None:
            return None
        return GetUrlByCodeOutput(
            id=url.id,
            short_code=url.short_code,
            original_url=url.original_url,
            is_active=url.is_active,
        )


@activity.defn
async def list_urls_activity(input: ListUrlsInput) -> ListUrlsOutput:
    async with async_session() as db:
        result = await db.execute(
            select(Url)
            .where(Url.is_active == True)
            .order_by(Url.created_at.desc())
            .offset(input.skip)
            .limit(input.limit)
        )
        urls = result.scalars().all()
        return ListUrlsOutput(
            urls=[
                {
                    "short_code": u.short_code,
                    "original_url": u.original_url,
                    "short_url": f"{settings.BASE_URL}/{u.short_code}",
                    "click_count": u.click_count,
                    "created_at": u.created_at.isoformat(),
                }
                for u in urls
            ]
        )


@activity.defn
async def record_click_activity(input: RecordClickInput) -> RecordClickOutput:
    async with async_session() as db:
        url_result = await db.execute(
            select(Url).where(Url.short_code == input.short_code, Url.is_active == True)
        )
        url = url_result.scalar_one_or_none()
        if url is None:
            return RecordClickOutput(recorded=False)

        clicked_at = datetime.fromisoformat(input.clicked_at)
        click = Click(
            url_id=url.id,
            clicked_at=clicked_at,
            referrer=input.referrer,
            user_agent=input.user_agent,
            ip_address=input.ip_address,
            click_id=input.click_id,
        )
        db.add(click)
        await db.execute(update(Url).where(Url.id == url.id).values(click_count=Url.click_count + 1))
        try:
            await db.commit()
        except IntegrityError:
            await db.rollback()
            return RecordClickOutput(recorded=False)
        return RecordClickOutput(recorded=True)


@activity.defn
async def get_url_with_clicks_activity(input: GetUrlWithClicksInput) -> GetUrlWithClicksOutput | None:
    async with async_session() as db:
        result = await db.execute(
            select(Url).where(Url.short_code == input.short_code, Url.is_active == True)
        )
        url = result.scalar_one_or_none()
        if url is None:
            return None
        return GetUrlWithClicksOutput(
            url={
                "short_code": url.short_code,
                "original_url": url.original_url,
                "short_url": f"{settings.BASE_URL}/{url.short_code}",
                "click_count": url.click_count,
                "created_at": url.created_at.isoformat(),
                "clicks": [
                    {
                        "clicked_at": c.clicked_at.isoformat(),
                        "referrer": c.referrer,
                        "user_agent": c.user_agent,
                        "ip_address": c.ip_address,
                    }
                    for c in url.clicks
                ],
            }
        )
