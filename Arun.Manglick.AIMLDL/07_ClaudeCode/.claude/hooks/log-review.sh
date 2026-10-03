#!/usr/bin/env bash
# PostToolUse hook: logs file modification events for audit purposes.
# Receives tool event JSON on stdin.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
LOG_FILE="${SCRIPT_DIR}/../review-log.txt"

INPUT="$(cat)"

if ! command -v jq &>/dev/null; then
  echo "jq is required for log-review hook" >&2
  exit 0
fi

TOOL_NAME="$(jq -r '.tool_name // "unknown"' <<<"${INPUT}")"
FILE_PATHS="$(jq -r '.tool_input.file_path // .file_paths[0] // "unknown"' <<<"${INPUT}")"
TIMESTAMP="$(date '+%Y-%m-%d %H:%M:%S')"

(
  flock -n 200 || exit 0
  echo "[${TIMESTAMP}] ${TOOL_NAME} — ${FILE_PATHS}" >>"${LOG_FILE}"
) 200>"${LOG_FILE}.lock"
