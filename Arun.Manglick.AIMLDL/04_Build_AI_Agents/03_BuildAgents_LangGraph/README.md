# LangGraph Agent Learning Project

This folder is a progressive, notebook-based introduction to building stateful AI agents with [LangGraph](https://www.langchain.com/langgraph). The examples start with deterministic graph workflows and gradually add conditional routing, an Ollama-backed chat model, tool calling, short-term memory, and LangSmith tracing.

The notebooks are educational examples rather than a packaged application. Each notebook builds and invokes its own graph in a Jupyter session.

## Learning progression

| Notebook | Main topic | What it demonstrates |
| --- | --- | --- |
| [`01_Fundamental_langGraphAgent.ipynb`](01_Fundamental_langGraphAgent.ipynb) | Basic graph | A typed state, two sequential nodes, and `START`/`END` edges. It calculates a USD return after a 4% fee and converts it to INR. |
| [`02_Conditional_langGraphAgent.ipynb`](02_Conditional_langGraphAgent.ipynb) | Conditional routing | A router chooses INR or EUR conversion using `add_conditional_edges`. |
| [`03_Chat_langGraphAgentWithOllama.ipynb`](03_Chat_langGraphAgentWithOllama.ipynb) | LLM chat | A `ChatOllama` model responds to accumulated chat messages through a simple graph and an `ipywidgets` interface. |
| [`04_Chat+Tool_langGraphAgentWithOllama.ipynb`](04_Chat%2BTool_langGraphAgentWithOllama.ipynb) | Tool calling | The model can call a stock-price tool. `ToolNode` executes the call and routes the result back to the model. |
| [`05_Chat+Tool+Memory_langGraphAgentWithOllama copy.ipynb`](05_Chat%2BTool%2BMemory_langGraphAgentWithOllama%20copy.ipynb) | Checkpointed memory | `MemorySaver` stores graph state and `thread_id` separates conversations. |
| [`06_LangSmith_Tracing_langGraphAgentWithOllama.ipynb`](06_LangSmith_Tracing_langGraphAgentWithOllama.ipynb) | Observability | Adds LangSmith's `@traceable` wrapper to capture graph execution, model calls, tool calls, and timing information. |

## Architecture

The project uses a consistent state-graph pattern:

```mermaid
flowchart TD
	START --> Chatbot[chatbotMessage node]
	Chatbot --> Decision{tools_condition}
	Decision -->|tool call| Tools[ToolNode]
	Tools --> Chatbot
	Decision -->|no tool call| END
	Chatbot -.-> State[ChatState\nchatMessages + add_messages]
	Memory[MemorySaver\nthread_id checkpoints] -.-> State
	Trace[LangSmith\n@traceable] -.-> Chatbot
```

The first two notebooks use deterministic calculation graphs:

```text
START -> calcUSDTotal_Node -> convertINRTotal_Node -> END
						   \-> convertEURTotal_Node -> END
```

The chat notebooks define `ChatState` with a `chatMessages` list annotated with LangGraph's `add_messages` reducer. The reducer appends new messages while preserving the conversation history. In the tool-enabled versions, the LLM is bound to `latest_stock_price`, `tools_condition` checks whether the latest response contains a tool call, and the graph loops through `ToolNode` until the model can answer.

## Tools and technologies

- **Python 3.14**: selected in [`.python-version`](.python-version).
- **LangGraph**: `StateGraph`, typed state, nodes, edges, conditional edges, message reducers, `ToolNode`, `tools_condition`, and checkpointing.
- **LangChain**: tool decoration and message/model integration.
- **Ollama**: accessed through `langchain_ollama.ChatOllama` with the `gpt-oss:120b-cloud` model configured in the notebooks.
- **LangSmith**: optional tracing and observability in notebook 6.
- **Jupyter Notebook**: interactive execution and persisted notebook outputs.
- **ipywidgets**: text input, send/quit controls, and chat output displays.
- **python-dotenv**: loads environment configuration from `.env`.
- **IPython display**: renders graph diagrams using `draw_mermaid_png()`.
- **uv**: dependency and lock-file workflow represented by [`pyproject.toml`](pyproject.toml) and [`uv.lock`](uv.lock).

## Tools implemented

Notebook 4 and later define one model-callable tool:

| Tool | Purpose | Data source |
| --- | --- | --- |
| `latest_stock_price(symbol: str) -> float` | Returns a price for stock calculations | Hard-coded demonstration data: `MSFT` 250.40, `AAPL` 270.40, `AMZN` 350.00, and `RIL` 99.60 |

This is mock data for demonstrating tool calling. It is not a market-data integration and must not be used for financial decisions.

## Project structure

```text
03_BuildAgents_LangGraph/
├── 01_Fundamental_langGraphAgent.ipynb
├── 02_Conditional_langGraphAgent.ipynb
├── 03_Chat_langGraphAgentWithOllama.ipynb
├── 04_Chat+Tool_langGraphAgentWithOllama.ipynb
├── 05_Chat+Tool+Memory_langGraphAgentWithOllama copy.ipynb
├── 06_LangSmith_Tracing_langGraphAgentWithOllama.ipynb
├── main.py                 # IDE-generated placeholder; not used by the notebooks
├── pyproject.toml          # Project metadata and base dependencies
├── uv.lock                 # Locked dependency resolution
├── .python-version        # Python version selection
├── .env                   # Local provider and tracing configuration
└── .gitignore
```

## Setup

From this directory, create or synchronize the environment with `uv`:

```powershell
uv sync
```

The base project metadata declares LangGraph, Jupyter Notebook, and `python-dotenv`. The notebooks also import packages that should be installed in the environment:

```powershell
uv add langchain langchain-ollama ipywidgets langsmith
```

Install and run Ollama separately, then pull a model that is available in your Ollama environment. The notebooks currently instantiate:

```python
ChatOllama(model="gpt-oss:120b-cloud")
```

Start Jupyter and open the notebooks in numeric order:

```powershell
uv run jupyter notebook
```

## Start the application

This project runs as interactive Jupyter notebooks; it does not expose a web server or command-line application. Start the required components as follows:

1. Open PowerShell in this directory.
2. Synchronize the Python environment:

	```powershell
	uv sync
	```

3. Verify the notebook dependencies can be imported:

	```powershell
	uv run python -c "import langgraph, langchain_ollama, ipywidgets; print('Dependencies are available')"
	```

4. Start Ollama in a separate terminal and make sure the model configured in the notebooks is available:

	```powershell
	ollama serve
	ollama list
	```

	The chat notebooks use `gpt-oss:120b-cloud`. Pull or otherwise make that model available according to your Ollama setup before running notebooks 3-6.

5. Start Jupyter from the project directory:

	```powershell
	uv run jupyter notebook
	```

6. Run the notebooks in order, from top to bottom. Restart the notebook kernel before rerunning a full notebook so that graph state and widget callbacks do not carry over from an earlier run.

Notebooks 1 and 2 are deterministic and do not require Ollama. Notebooks 3-5 require Ollama. Notebook 6 additionally requires valid LangSmith configuration if tracing is enabled.

## Test the application

There is no standalone `pytest` suite or HTTP endpoint in this folder. Testing is performed with notebook smoke checks that invoke the compiled graphs and inspect their returned state.

### Environment smoke test

Run this from PowerShell after installing dependencies:

```powershell
uv run python -c "from langgraph.graph import StateGraph; from langchain_ollama import ChatOllama; print('LangGraph and Ollama integrations are importable')"
```

### Deterministic graph checks

Run all cells in notebooks 1 and 2 and verify:

- Notebook 1 accepts `{"usd_Amount": 1000}` and produces `usd_ReturnTotal` of `1040` and `inr_Total` of `92560`.
- Notebook 2 routes `targetCurrency="INR"` to the INR node and `targetCurrency="EUR"` to the EUR node.
- The graph visualization renders successfully through `draw_mermaid_png()`.

### Chat and tool checks

In notebooks 3 and 4, invoke the graph with a message such as:

```python
message = {"role": "user", "content": "What is the price of AAPL?"}
result = graph.invoke({"chatMessages": [message]})
print(result["chatMessages"][-1].content)
```

Verify that notebook 3 returns an assistant response and notebook 4 calls `latest_stock_price` before returning the mock AAPL price of `$270.40`. Also test a symbol such as `MSFT`, `AMZN`, or `RIL` to verify tool routing.

### Memory check

In notebook 5, use the same thread for two calls and a different thread for an independent conversation:

```python
thread_one = {"configurable": {"thread_id": "smoke-test-1"}}
graph.invoke({"chatMessages": [{"role": "user", "content": "What is the price of AAPL?"}]}, config=thread_one)
follow_up = graph.invoke({"chatMessages": [{"role": "user", "content": "Add 10 AMZN shares to the previous context."}]}, config=thread_one)
assert follow_up["chatMessages"]
```

Confirm that the second call can use the first conversation's context, while a new `thread_id` does not see it. Because `MemorySaver` is in-memory, this check must be repeated after a kernel restart.

### LangSmith tracing check

In notebook 6, enable the LangSmith variables described below and run the traced `run_chatgraph(...)` example. Confirm that a run appears in the configured LangSmith project and includes the graph invocation, model call, and tool call when a stock-price question is used. Disable tracing or remove the API key when testing with sensitive prompts.

## Configuration

Notebook 3 and later call `load_dotenv()`. Depending on which notebook is being run, configure the following variables in a local `.env` file:

```text
OPENAI_API_KEY=<optional, only for experiments that use OpenAI>
GOOGLE_API_KEY=<optional, only for experiments that use Google>
LANGSMITH_TRACING=true
LANGSMITH_ENDPOINT=https://api.smith.langchain.com
LANGSMITH_API_KEY=<your LangSmith key>
LANGSMITH_PROJECT=<your LangSmith project name>
```

The current notebooks use Ollama for the chat model; the OpenAI and Google variables are not required by the demonstrated Ollama flows. Never commit real API keys. Keep secrets in a local ignored file, rotate any credentials that may have been exposed, and use placeholder values in shared examples.

## Memory and tracing behavior

Notebook 5 compiles the graph with `MemorySaver`:

```python
graph = builder.compile(checkpointer=memory)
config = {"configurable": {"thread_id": "1"}}
result = graph.invoke(input_state, config=config)
```

The same `thread_id` continues a conversation; a different `thread_id` starts an independent in-memory conversation. `MemorySaver` is process-local and temporary, so it is not a durable production database.

Notebook 6 wraps graph invocation with `@traceable` and sends execution data to LangSmith. Tracing requires a LangSmith account and valid environment variables. Avoid tracing sensitive prompts or credentials.

## Example state shapes

Deterministic conversion:

```python
{"usd_Amount": 1000, "targetCurrency": "INR"}
```

Chat input:

```python
{"chatMessages": [{"role": "user", "content": "What is the price of AAPL?"}]}
```

The graph returns the updated state, including assistant messages and, when applicable, tool-call and tool-result messages.

## Notes and limitations

- The stock tool uses fixed values and does not call a live data provider.
- `MemorySaver` provides short-term in-memory state only; restarting the kernel loses checkpoints.
- Notebook outputs contain model responses and execution metadata from previous runs. Re-run cells when changing models, credentials, or dependency versions.
- `main.py` is an untouched IDE sample script and is not the application entry point.
- The notebooks are intentionally repetitive so each LangGraph concept can be studied in isolation.
