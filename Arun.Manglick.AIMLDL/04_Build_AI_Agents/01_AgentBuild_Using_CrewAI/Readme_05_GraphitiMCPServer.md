
# Graphiti MCP Server
  - Graphiti is a framework for building/querying temporally-aware knowledge graphs, specifically tailored for AI agents operating in dynamic environments
  - Here we are using Graphiti by Zep AI as a memory layer for an AI agent

# Graphiti MCP Server Set Up
    Follow these steps to set up the project before running the MCP server.
        Clone GitHub Repository
        git clone https://github.com/getzep/graphiti.git
        cd graphiti/mcp_server
    Install Dependencies
        uv sync

# Prerequisites Before Running MCP Server
Ensure you have Python 3.10 or higher installed.
A running Neo4j database (version 5.26 or later required)
OpenAI API key for LLM operations

# Install Neo4j
    https://neo4j.com/download/
    The simplest way to install Neo4j is via Neo4j Desktop. It provides a user-friendly interface to manage Neo4j instances and databases
    
![alt text](image-2.png)

# Configuration
Before running the MCP server, required step is configure the environment variables. 
    Create .env file in the graphiti/mcp_server directory and add below content

    # Neo4j Database Configuration
    NEO4J_URI=bolt://localhost:7687
    NEO4J_USER=neo4j
    NEO4J_PASSWORD=demodemo

    # OpenAI API Configuration
    OPENAI_API_KEY=<your_openai_api_key>
    MODEL_NAME=gpt-4.1-mini

# Run Graphiti MCP Server
 - Go to Command prompt
 - cd path where mcp server file is available 
   C:\Arun.Manglick\Arun.Manglick.PRJ\codetogether\Arun.Manglick.AIMLDL\05_Graphiti\graphiti\mcp_server\graphiti_mcp_server.py
 - Run Command: uv run graphiti_mcp_server.py --model gpt-4.1-mini --transport sse
 ![alt text](image-3.png)

- Browse http://localhost:8000/sse
![alt text](image-5.png)

# Finally Run Your Crew AI Agent (05_AIAgent2_With Memory.ipynb)
C:\Arun.Manglick\Arun.Manglick.PRJ\codetogether\Arun.Manglick.AIMLDL\04_Build_AI_Agents\01_BasicAgent_CrewAI\05_AIAgent2_With Memory.ipynb



