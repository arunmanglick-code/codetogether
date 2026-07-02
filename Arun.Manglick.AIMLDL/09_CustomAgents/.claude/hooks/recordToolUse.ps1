# Records Bash and PowerShell tool calls as JSONL for downstream processing.
# Claude Code equivalent of the Copilot recordToolUse hook.

$raw = [Console]::In.ReadToEnd()

if ($raw -notmatch '"tool_name"\s*:\s*"([^"]+)"') { exit 0 }
$toolName = $Matches[1]

if ($toolName -ne 'Bash' -and $toolName -ne 'PowerShell') { exit 0 }

if ($raw -notmatch '"session_id"\s*:\s*"([^"]+)"') { exit 0 }
$sessionId = $Matches[1]

$hooksDir = '.claude\hooks'
if (-not (Test-Path $hooksDir)) { New-Item -ItemType Directory -Path $hooksDir -Force | Out-Null }

$line = ($raw -replace '[\r\n]+', ' ').Trim() + "`n"
[System.IO.File]::AppendAllText("$hooksDir\$sessionId.json", $line, [System.Text.UTF8Encoding]::new($false))
