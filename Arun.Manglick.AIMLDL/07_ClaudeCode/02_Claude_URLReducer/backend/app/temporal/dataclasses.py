from dataclasses import dataclass


@dataclass
class CreateShortUrlInput:
    original_url: str


@dataclass
class CreateShortUrlOutput:
    short_code: str
    original_url: str
    short_url: str
    created_at: str


@dataclass
class GetUrlByCodeInput:
    short_code: str


@dataclass
class GetUrlByCodeOutput:
    id: int
    short_code: str
    original_url: str
    is_active: bool


@dataclass
class ListUrlsInput:
    skip: int = 0
    limit: int = 20


@dataclass
class ListUrlsOutput:
    urls: list[dict]


@dataclass
class RecordClickInput:
    short_code: str
    click_id: str
    clicked_at: str
    referrer: str | None = None
    user_agent: str | None = None
    ip_address: str | None = None


@dataclass
class RecordClickOutput:
    recorded: bool


@dataclass
class GetUrlWithClicksInput:
    short_code: str


@dataclass
class GetUrlWithClicksOutput:
    url: dict
