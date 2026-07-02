from datetime import timedelta

from temporalio import workflow
from temporalio.common import RetryPolicy

with workflow.unsafe.imports_passed_through():
    from app.temporal.activities import (
        create_short_url_activity,
        get_url_by_code_activity,
        get_url_with_clicks_activity,
        list_urls_activity,
        record_click_activity,
    )
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

WRITE_RETRY_POLICY = RetryPolicy(
    initial_interval=timedelta(seconds=1),
    maximum_interval=timedelta(seconds=30),
    backoff_coefficient=2.0,
    maximum_attempts=5,
)

READ_RETRY_POLICY = RetryPolicy(
    initial_interval=timedelta(milliseconds=500),
    maximum_interval=timedelta(seconds=10),
    backoff_coefficient=2.0,
    maximum_attempts=3,
)

ACTIVITY_TIMEOUT = timedelta(seconds=10)


@workflow.defn
class CreateShortUrlWorkflow:
    @workflow.run
    async def run(self, input: CreateShortUrlInput) -> CreateShortUrlOutput:
        return await workflow.execute_activity(
            create_short_url_activity,
            input,
            start_to_close_timeout=ACTIVITY_TIMEOUT,
            retry_policy=WRITE_RETRY_POLICY,
        )


@workflow.defn
class GetUrlByCodeWorkflow:
    @workflow.run
    async def run(self, input: GetUrlByCodeInput) -> GetUrlByCodeOutput | None:
        return await workflow.execute_activity(
            get_url_by_code_activity,
            input,
            start_to_close_timeout=ACTIVITY_TIMEOUT,
            retry_policy=READ_RETRY_POLICY,
        )


@workflow.defn
class ListUrlsWorkflow:
    @workflow.run
    async def run(self, input: ListUrlsInput) -> ListUrlsOutput:
        return await workflow.execute_activity(
            list_urls_activity,
            input,
            start_to_close_timeout=ACTIVITY_TIMEOUT,
            retry_policy=READ_RETRY_POLICY,
        )


@workflow.defn
class RecordClickWorkflow:
    @workflow.run
    async def run(self, input: RecordClickInput) -> RecordClickOutput:
        return await workflow.execute_activity(
            record_click_activity,
            input,
            start_to_close_timeout=ACTIVITY_TIMEOUT,
            retry_policy=WRITE_RETRY_POLICY,
        )


@workflow.defn
class GetUrlWithClicksWorkflow:
    @workflow.run
    async def run(self, input: GetUrlWithClicksInput) -> GetUrlWithClicksOutput | None:
        return await workflow.execute_activity(
            get_url_with_clicks_activity,
            input,
            start_to_close_timeout=ACTIVITY_TIMEOUT,
            retry_policy=READ_RETRY_POLICY,
        )
