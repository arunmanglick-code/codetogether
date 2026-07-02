import asyncio

from temporalio.client import Client
from temporalio.worker import Worker

from app.config import settings
from app.temporal.activities import (
    create_short_url_activity,
    get_url_by_code_activity,
    get_url_with_clicks_activity,
    list_urls_activity,
    record_click_activity,
)
from app.temporal.workflows import (
    CreateShortUrlWorkflow,
    GetUrlByCodeWorkflow,
    GetUrlWithClicksWorkflow,
    ListUrlsWorkflow,
    RecordClickWorkflow,
)


async def main():
    client = await Client.connect(
        settings.TEMPORAL_SERVER_URL,
        namespace=settings.TEMPORAL_NAMESPACE,
    )

    worker = Worker(
        client,
        task_queue=settings.TEMPORAL_TASK_QUEUE,
        workflows=[
            CreateShortUrlWorkflow,
            GetUrlByCodeWorkflow,
            ListUrlsWorkflow,
            RecordClickWorkflow,
            GetUrlWithClicksWorkflow,
        ],
        activities=[
            create_short_url_activity,
            get_url_by_code_activity,
            list_urls_activity,
            record_click_activity,
            get_url_with_clicks_activity,
        ],
    )

    print(f"Listening on queue: {settings.TEMPORAL_TASK_QUEUE}")
    await worker.run()


if __name__ == "__main__":
    asyncio.run(main())
