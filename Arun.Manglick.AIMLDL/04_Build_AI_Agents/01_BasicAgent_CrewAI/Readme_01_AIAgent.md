# About: 

This contains a python script that uses crewai to set up an AI agent powered by a local Ollama instance running the llama3.2 model. This script defines a simple agent and a task for it to perform, then executes the crew to get a result.

This script provides a complete example of how to configure a crewai agent to use a local LLM. It shows how to initialize the LLM, define the agent and its task, and then run the crew.

# Required Insatallation: Ollama
You will need to have Ollama installed and have the llama3.2 model pulled and running (ollama run llama3.2) for this code to work.

Step1: For Windows: Download and install from Ollama's official website - https://ollama.com/download
Step2: Pull the required model: ollama pull llama3.2

# Required Insatallation: CrewAI
Open your terminal or command prompt and run: pip install crewai


# What is Ollama
Ollama is a powerful platform that lets you run LLMs locally on your machine—no cloud dependency required. It’s designed for developers who want fast, private, and customizable access to models like LLaMA, Mistral, Gemma, Phi-4, and more.

# What is opik
https://github.com/comet-ml/opik
Open-source LLM evaluation platform
Opik helps you evaluate and optimize LLM systems that run better, faster, and cheaper. 
Opik provides comprehensive tracing, evaluations, dashboards, and powerful features like Opik Agent Optimizer and Opik Guardrails to improve and secure your LLM powered applications in production.

# opik integration with crew ai
Opik has integrations with numerous frameworks - https://www.comet.com/docs/opik/tracing/integrations/overview?from=llm
Here framework used is crewAI - https://www.comet.com/docs/opik/tracing/integrations/crewai'
Opik integrates with CrewAI to log traces for all CrewAI activity.

# File Strucutre
01_AIAgent.ipynb - 
    - This script contains code to Creating Agents, Defining Tasks, Orchestrating a Crew.
    - This agent script is using Local LLM Setup

02_AIAgent_Opik.ipynb - 
    - This script has 100% copy of #1
    - Addition is inclusion of 'opik' 
    - Opik allows you to monitor the agent's thought process, tool usage, and task execution in real-time
