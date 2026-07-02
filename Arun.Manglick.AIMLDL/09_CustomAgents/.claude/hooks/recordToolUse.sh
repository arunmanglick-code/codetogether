#!/usr/bin/env bash
# Records Bash and PowerShell tool calls as JSONL for downstream processing.
# Claude Code equivalent of the Copilot recordToolUse hook.

raw=$(cat)

tool_name=$(printf '%s' "$raw" | grep -oP '"tool_name"\s*:\s*"\K[^"]+')
[ -z "$tool_name" ] && exit 0

if [ "$tool_name" != "Bash" ] && [ "$tool_name" != "PowerShell" ]; then
  exit 0
fi

session_id=$(printf '%s' "$raw" | grep -oP '"session_id"\s*:\s*"\K[^"]+')
[ -z "$session_id" ] && exit 0

hooks_dir=".claude/hooks"
mkdir -p "$hooks_dir"

printf '%s\n' "$(printf '%s' "$raw" | tr '\n' ' ' | tr '\r' ' ')" >> "$hooks_dir/$session_id.json"
