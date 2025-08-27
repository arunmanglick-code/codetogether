# What is LinkupSearchTool (Way to integrate Linkup’s information retrieval capabilities into CrewAI agents. )
https://docs.crewai.com/en/tools/search-research/linkupsearchtool

1) The LinkupSearchTool provides a seamless way to integrate Linkup’s contextual information retrieval capabilities into your CrewAI agents. 
2) By leveraging this tool, agents can access relevant and up-to-date information to enhance their decision-making and task execution.
3) The LinkupSearchTool provides the ability to query the Linkup API for contextual information and retrieve structured results. This tool is ideal for enriching workflows with up-to-date and reliable information from Linkup, allowing agents to access relevant data during their tasks.

# What is Linkup (AI search engine)
https://www.linkup.so/
1) Linkup is an AI search engine optimized for LLMs and agents, offering seamless internet access and fast, accurate results through an API.


# Installation
To use this tool, you need to install the Linkup SDK: 
1) Create a file in your project directory - pyproject.toml
2) Add default content  
    [project]
    name = "my-agent-project"
    version = "0.1.0"
    dependencies = []
3) Go to command prompt and cd to your project directory and type: uv add linkup-sdk


# Steps to Get Started
To effectively use the LinkupSearchTool, follow these steps:
1) API Key: Obtain a Linkup API key - Done
2) Environment Setup: Set up your environment with the API key - Done
3) Install SDK: Install the Linkup SDK using the command: uv add linkup-sdk

# 1 How to Obtain a Linkup API key.
 1) Create a Linkup Account - https://docs.linkup.so/pages/documentation/get-started/quickstart
 2) Go to the page and sign up for a free account. (Used gmail: arunmanglickawsnew)
 2) Access Your API Key Once logged in, you'll find your API key in your dashboard or developer settings.
 3) Use It in Your Code You can pass the key directly or set it as an environment variable:

