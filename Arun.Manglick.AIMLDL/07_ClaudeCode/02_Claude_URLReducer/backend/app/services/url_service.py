from sqlalchemy import select, update
from sqlalchemy.ext.asyncio import AsyncSession

from app.models.click import Click
from app.models.url import Url
from app.services.shortcode import generate_short_code

MAX_COLLISION_RETRIES = 5


async def create_short_url(db: AsyncSession, original_url: str, code_length: int = 7) -> Url:
    for _ in range(MAX_COLLISION_RETRIES):
        short_code = generate_short_code(code_length)
        existing = await db.execute(select(Url).where(Url.short_code == short_code))
        if existing.scalar_one_or_none() is None:
            url = Url(short_code=short_code, original_url=original_url)
            db.add(url)
            await db.commit()
            await db.refresh(url)
            return url
    raise RuntimeError("Failed to generate a unique short code after maximum retries")


async def get_url_by_code(db: AsyncSession, short_code: str) -> Url | None:
    result = await db.execute(select(Url).where(Url.short_code == short_code, Url.is_active == True))
    return result.scalar_one_or_none()


async def list_urls(db: AsyncSession, limit: int = 50) -> list[Url]:
    result = await db.execute(select(Url).where(Url.is_active == True).order_by(Url.created_at.desc()).limit(limit))
    return list(result.scalars().all())


async def record_click(
    db: AsyncSession,
    url_id: int,
    referrer: str | None = None,
    user_agent: str | None = None,
    ip_address: str | None = None,
) -> None:
    click = Click(url_id=url_id, referrer=referrer, user_agent=user_agent, ip_address=ip_address)
    db.add(click)
    await db.execute(update(Url).where(Url.id == url_id).values(click_count=Url.click_count + 1))
    await db.commit()


async def get_url_with_clicks(db: AsyncSession, short_code: str) -> Url | None:
    result = await db.execute(select(Url).where(Url.short_code == short_code, Url.is_active == True))
    return result.scalar_one_or_none()
