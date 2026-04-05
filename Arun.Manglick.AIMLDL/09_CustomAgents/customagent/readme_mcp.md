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
    }
  }
}
```

This connects to Atlassian's hosted MCP server at `https://mcp.atlassian.com/v1/mcp`. The authentication to your specific Confluence and JIRA instance (`vertexinc.atlassian.net`) is handled via **OAuth** through the Atlassian MCP server — it uses the token you authorized when you first set up the extension (not stored locally in config files).

## Additional Setting

**File:** `settings.json` (`C:\Users\Arun.Manglick\AppData\Roaming\Code\User`)

```json
"atlascode.jira.enabled": true
```

This enables the Atlassian extension's JIRA integration in VS Code.

## Summary

| Config | Location |
|---|---|
| MCP server endpoint | `%APPDATA%\Code\User\mcp.json` |
| MCP server cache | `%APPDATA%\Code\User\mcp\com.atlassian.atlassian-mcp-server-1.1.1\` |
| JIRA enabled flag | `%APPDATA%\Code\User\settings.json` |
| OAuth credentials | Managed by Atlassian MCP server (cloud-side, not in local files) |