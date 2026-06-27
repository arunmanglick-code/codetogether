#!/usr/bin/env bash
# PostToolUse hook: logs file modification events for audit purposes.
# Receives tool event JSON on stdin.

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
LOG_FILE="$SCRIPT_DIR/../review-log.txt"

INPUT=$(cat)

if command -v jq &>/dev/null; then
  TOOL_NAME=$(echo "$INPUT" | jq -r '.tool_name // "unknown"')
  FILE_PATHS=$(echo "$INPUT" | jq -r '.tool_input.file_path // .file_paths[0] // "unknown"')
else
  TOOL_NAME=$(echo "$INPUT" | grep -o '"tool_name":"[^"]*"' | head -1 | cut -d'"' -f4)
  FILE_PATHS=$(echo "$INPUT" | grep -o '"file_path":"[^"]*"' | head -1 | cut -d'"' -f4)
fi

TIMESTAMP=$(date '+%Y-%m-%d %H:%M:%S')

echo "[$TIMESTAMP] $TOOL_NAME — $FILE_PATHS" >> "$LOG_FILE"
