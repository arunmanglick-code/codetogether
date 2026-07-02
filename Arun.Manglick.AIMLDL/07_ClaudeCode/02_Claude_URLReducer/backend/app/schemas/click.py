from datetime import datetime

from pydantic import BaseModel, ConfigDict


class ClickDetail(BaseModel):
    clicked_at: datetime
    referrer: str | None = None
    user_agent: str | None = None
    ip_address: str | None = None

    model_config = ConfigDict(from_attributes=True)


class UrlStatsResponse(BaseModel):
    short_code: str
    original_url: str
    created_at: datetime
    click_count: int
    clicks: list[ClickDetail] = []

    model_config = ConfigDict(from_attributes=True)
