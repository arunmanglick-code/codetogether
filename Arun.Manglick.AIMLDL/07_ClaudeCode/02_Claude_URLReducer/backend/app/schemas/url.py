from datetime import datetime

from pydantic import BaseModel, ConfigDict, HttpUrl, field_validator

from app.utils.validators import is_safe_url


class UrlCreateRequest(BaseModel):
    url: HttpUrl

    @field_validator("url")
    @classmethod
    def validate_url_safety(cls, v):
        url_str = str(v)
        safe, reason = is_safe_url(url_str)
        if not safe:
            raise ValueError(reason)
        return v


class UrlCreateResponse(BaseModel):
    short_code: str
    short_url: str
    original_url: str


class UrlListItem(BaseModel):
    short_code: str
    original_url: str
    created_at: datetime
    click_count: int

    model_config = ConfigDict(from_attributes=True)
