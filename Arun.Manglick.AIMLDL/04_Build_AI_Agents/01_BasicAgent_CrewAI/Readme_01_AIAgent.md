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
