import os

from dotenv import load_dotenv

load_dotenv()

ANTHROPIC_MODEL = os.getenv("ANTHROPIC_MODEL", "claude-sonnet-4-20250514")
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", "llama3.1")
LLM_PROVIDER = os.getenv("LLM_PROVIDER", "anthropic")


def get_llm(temperature: float = 0.0):
    if LLM_PROVIDER == "anthropic":
        from langchain_anthropic import ChatAnthropic

        return ChatAnthropic(model=ANTHROPIC_MODEL, temperature=temperature)

    from langchain_ollama import ChatOllama

    return ChatOllama(model=OLLAMA_MODEL, temperature=temperature)
