from sqlalchemy import BigInteger, DateTime, ForeignKey, String, func
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.database import Base


class Click(Base):
    __tablename__ = "clicks"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True, autoincrement=True)
    url_id: Mapped[int] = mapped_column(BigInteger, ForeignKey("urls.id"), nullable=False, index=True)
    clicked_at: Mapped[str] = mapped_column(DateTime(timezone=True), server_default=func.now(), nullable=False, index=True)
    referrer: Mapped[str | None] = mapped_column(String(2048), nullable=True)
    user_agent: Mapped[str | None] = mapped_column(String(1024), nullable=True)
    ip_address: Mapped[str | None] = mapped_column(String(45), nullable=True)
    click_id: Mapped[str | None] = mapped_column(String(36), unique=True, nullable=True)

    url = relationship("Url", back_populates="clicks")
