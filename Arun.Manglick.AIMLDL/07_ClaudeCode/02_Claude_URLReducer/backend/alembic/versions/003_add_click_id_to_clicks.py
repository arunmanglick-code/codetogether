"""add click_id to clicks

Revision ID: 003
Revises: 002
Create Date: 2026-07-02

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

revision: str = "003"
down_revision: Union[str, None] = "002"
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    op.add_column("clicks", sa.Column("click_id", sa.String(length=36), nullable=True))
    op.create_index("ix_clicks_click_id", "clicks", ["click_id"], unique=True)


def downgrade() -> None:
    op.drop_index("ix_clicks_click_id", table_name="clicks")
    op.drop_column("clicks", "click_id")
