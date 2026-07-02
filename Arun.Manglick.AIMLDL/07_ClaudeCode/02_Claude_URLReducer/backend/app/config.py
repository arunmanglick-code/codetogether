from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    DATABASE_URL: str = "postgresql+asyncpg://urlreducer:urlreducer@localhost:5432/urlreducer"
    SHORT_CODE_LENGTH: int = 7
    BASE_URL: str = "http://localhost:8000"
    CORS_ORIGINS: list[str] = ["http://localhost:5173"]
    RATE_LIMIT_REQUESTS: int = 60
    RATE_LIMIT_WINDOW_SECONDS: int = 60
    TEMPORAL_SERVER_URL: str = "temporal:7233"
    TEMPORAL_NAMESPACE: str = "default"
    TEMPORAL_TASK_QUEUE: str = "url-reducer"

    model_config = SettingsConfigDict(env_file=".env", extra="ignore")


settings = Settings()
