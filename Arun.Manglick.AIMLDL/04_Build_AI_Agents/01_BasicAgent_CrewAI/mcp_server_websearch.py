from dotenv import load_dotenv
from linkup import LinkupClient
from mcp.server.fastmcp import FastMCP
import asyncio
import nest_asyncio

# The nest_asyncio patch is still a good safety measure,
# as some underlying libraries might still cause conflicts.
nest_asyncio.apply()

load_dotenv()

# Change the transport to "websocket" to use a different
# communication protocol that is often more compatible
# with nested environments.
mcp = FastMCP('linkup-server', port=8070)
client = LinkupClient()

@mcp.tool()
def web_search(query: str = "") -> str:
    """Search the web for the given query."""
    search_response = client.search(
        query=query,
        depth="standard",  # "standard" or "deep"
        output_type="sourcedAnswer",  # "searchResults" or "sourcedAnswer" or "structured"
        structured_output_schema=None,  # must be filled if output_type is "structured"
    )
    return str(search_response)

if __name__ == "__main__":
    mcp.run(transport="sse")

# This code is an MCP (Multi-Computer Project) server application that exposes a single tool,
# web_search, which uses a LinkupClient to perform web queries.

# Here's a detailed breakdown of each part:

# Imports and Setup
    # from dotenv import load_dotenv: This imports a function to load environment variables from a .env file, which is a common practice for securely storing API keys and other secrets.
    # from linkup import LinkupClient: Imports the LinkupClient class, which is used to interact with the Linkup API for searching the web.
    # from mcp.server.fastmcp import FastMCP: Imports the core FastMCP class, which handles the server logic for exposing tools.
    # import asyncio and import nest_asyncio: These libraries are for managing asynchronous operations. 
    # nest_asyncio is used to patch asyncio to allow it to run within an already-running event loop, which can prevent threading conflicts.

# nest_asyncio.apply(): This line applies the patch to the asyncio library
# load_dotenv(): This function call loads your environment variables from the .env file into the application's environment.

# mcp = FastMCP('linkup-server', port=8080): This initializes a new FastMCP server. It's given a unique name ('linkup-server') and is set to run on port 8080.
# client = LinkupClient(): This creates an instance of the LinkupClient that the server will use to perform searches.

# What is FastMCP
# FastMCP is a Python framework that simplifies the creation of MCP (Model Context Protocol) servers.
# FastMCP framework handles all the complex networking and protocol details behind the scenes, allowing you to focus on simply defining what your tool does.

# The web_search Tool
# @mcp.tool(): This is a decorator. It tells the FastMCP server that the function immediately following it (web_search) should be exposed as an available tool.

# def web_search(query: str = "") -> str:: This defines the tool itself. It's a Python function that takes a query string and is expected to return a string.
# search_response = client.search(...): This is the core logic of the tool. It calls the search method on the LinkupClient instance, passing in the user's query along with other search parameters like depth and output_type.
# return str(search_response): The function returns the search result as a string.

# Running the Server
# if __name__ == "__main__":: This is a standard Python block that ensures the code inside it only runs when the script is executed directly (not when it's imported as a module).
# mcp.run(transport="sse"): This command starts the server. 
# It tells the FastMCP instance to begin listening for incoming connections and to use the Server-Sent Events (SSE) protocol for communication. 