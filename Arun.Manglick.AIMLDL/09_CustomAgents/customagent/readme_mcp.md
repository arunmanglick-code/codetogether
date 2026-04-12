# Atlassian MCP Extension Configuration

Here's where the Atlassian MCP extension is configured:

## MCP Server Configuration

**File:** `mcp.json` (`C:\Users\Arun.Manglick\AppData\Roaming\Code\User`)

```json
{
  "servers": {
    "com.atlassian/atlassian-mcp-server": {
      "type": "http",
      "url": "https://mcp.atlassian.com/v1/mcp",
      "gallery": "https://api.mcp.github.com",
      "version": "1.1.1"
    },
    "github": {
      "type": "http",
      "url": "https://api.githubcopilot.com/mcp/"
    }
  },
  "inputs": []
}
```

### Servers

1. **Atlassian MCP Server** — Connects to Atlassian's hosted MCP server at `https://mcp.atlassian.com/v1/mcp`. The authentication to your specific Confluence and JIRA instance (`vertexinc.atlassian.net`) is handled via **OAuth** through the Atlassian MCP server — it uses the token you authorized when you first set up the extension (not stored locally in config files).

2. **GitHub MCP Server** — Connects to GitHub Copilot's MCP endpoint at `https://api.githubcopilot.com/mcp/`. This enables GitHub tools (repos, issues, PRs, code search, etc.) to be used by agents via the MCP protocol. Authentication is handled through your existing GitHub Copilot session.

## Additional Setting

**File:** `settings.json` (`C:\Users\Arun.Manglick\AppData\Roaming\Code\User`)

```json
"atlascode.jira.enabled": true
```

This enables the Atlassian extension's JIRA integration in VS Code.

## Summary

| Config | Location |
|---|---|
| MCP server config | `%APPDATA%\Code\User\mcp.json` |
| Atlassian MCP cache | `%APPDATA%\Code\User\mcp\com.atlassian.atlassian-mcp-server-1.1.1\` |
| GitHub MCP endpoint | `https://api.githubcopilot.com/mcp/` |
| JIRA enabled flag | `%APPDATA%\Code\User\settings.json` |
| Atlassian OAuth | Managed by Atlassian MCP server (cloud-side, not in local files) |
| GitHub auth | Managed via GitHub Copilot session |