import os
import sys
import uuid

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from dotenv import load_dotenv
load_dotenv(os.path.join(os.path.dirname(__file__), "..", ".env"))

import gradio as gr
from langchain_core.messages import HumanMessage
from langgraph.checkpoint.memory import MemorySaver
from langgraph.types import Command

from src.graph_builder import build_graph
from src.utils.formatters import create_budget_pie_chart, format_full_plan

NODE_LABELS = {
    "intent_parser": "Parsing your request...",
    "destination_research": "Researching destination...",
    "flight_search": "Searching flights...",
    "hotel_search": "Searching hotels...",
    "activity_itinerary": "Planning activities & itinerary...",
    "budget_currency": "Analyzing budget & currency...",
    "aggregator": "Assembling your travel plan...",
    "human_review": "Ready for your review!",
}

memory = MemorySaver()
graph = build_graph(checkpointer=memory, include_human_review=True)


# ---------------------------------------------------------------------------
# Shared helpers
# ---------------------------------------------------------------------------

def _stream_graph(input_val, config):
    """Stream graph execution, yielding (status_lines, is_error) tuples."""
    status_lines = []
    try:
        for chunk in graph.stream(input_val, config, stream_mode="updates"):
            node_name = list(chunk.keys())[0]
            label = NODE_LABELS.get(node_name, node_name)
            status_lines.append(f">> {label}")
            yield status_lines, False
    except Exception as e:
        status_lines.append(f">> ERROR: {e}")
        yield status_lines, True


def _get_results(config):
    """Get formatted plan and chart from current graph state."""
    state = graph.get_state(config)
    plan = format_full_plan(state.values)
    chart = create_budget_pie_chart(state.values.get("budget_summary"))
    plan_status = state.values.get("plan_status", "")
    return plan, chart, plan_status


def _empty_results():
    return "", "", "", "", "", None, ""


def _generate_edit_prompts(travel_request):
    """Build contextual edit-plan prompts from the user's actual trip details."""
    if not travel_request:
        return []

    dest = travel_request.get("destination", "")
    budget = int(travel_request.get("budget_usd", 5000))
    travelers = int(travel_request.get("travelers", 2))
    prefs = travel_request.get("preferences", [])

    ALT_DEST = {
        "maldives": "Bali", "bali": "Maldives", "paris": "Rome",
        "tokyo": "Seoul", "rome": "Barcelona", "cancun": "Phuket",
        "santorini": "Amalfi Coast", "dubai": "Abu Dhabi",
        "new york": "San Francisco", "london": "Edinburgh",
    }
    alt = ALT_DEST.get(dest.lower(), "Bali" if dest.lower() != "bali" else "Maldives")

    prompts = [
        f"Looks great but can we find a slightly cheaper hotel?",
        f"Can we move the trip to April 10-17 instead?",
        f"Make it a 5-day trip instead.",
        f"We have one more traveler joining — now {travelers + 1} people.",
        f"Actually, change the destination to {alt}.",
        f"Make the trip more adventurous and add scuba diving.",
        f"Can you find a nonstop flight, even if it costs a little more?",
        f"Increase the budget to ${budget + 1000:,} and upgrade to a luxury resort.",
        f"One of us is vegetarian. Please update the meal recommendations.",
    ]
    return prompts


# ---------------------------------------------------------------------------
# Chat mode — natural language prompts with multi-turn conversation
# ---------------------------------------------------------------------------

def chat_plan(user_message, chat_history, thread_id):
    """Handle a chat message: first message starts a trip, subsequent messages
    are treated as feedback (approve / modify) via Command(resume=...).

    Outputs: chatbot, 7 result tabs, chat_input (cleared), thread_state.
    """
    if not user_message or not user_message.strip():
        yield chat_history, *_empty_results(), "", thread_id, gr.update()
        return

    is_new_session = not thread_id
    if is_new_session:
        thread_id = str(uuid.uuid4())

    config = {"configurable": {"thread_id": thread_id}}

    chat_history = chat_history or []
    chat_history.append({"role": "user", "content": user_message})

    if is_new_session:
        input_val = {"messages": [HumanMessage(content=user_message)]}
    else:
        input_val = Command(resume=user_message)

    agent_status = []
    for status_lines, _ in _stream_graph(input_val, config):
        agent_status = status_lines
        progress = "\n".join(f"⏳ {s[3:]}" for s in agent_status)
        streaming_history = chat_history + [{"role": "assistant", "content": progress}]
        yield streaming_history, *_empty_results(), "", thread_id, gr.update()

    try:
        plan, chart, plan_status = _get_results(config)
    except Exception as e:
        chat_history.append({"role": "assistant", "content": f"Error: {e}"})
        yield chat_history, *_empty_results(), "", thread_id, gr.update()
        return

    travel_req = graph.get_state(config).values.get("travel_request")
    edit_prompts = _generate_edit_prompts(travel_req)

    if plan_status == "complete":
        chat_history.append({"role": "assistant", "content": "✅ **Plan approved!** Have a wonderful trip!"})
        yield (
            chat_history,
            plan["summary"], plan["flights"], plan["hotels"],
            plan["itinerary"], plan["budget"], chart, plan["packing"],
            "", thread_id,
            gr.update(choices=[], value=None),
        )
        return

    agent_summary = "\n".join(f"✅ {s[3:]}" for s in agent_status)
    reply = (
        f"{agent_summary}\n\n"
        "---\n"
        "Your travel plan is ready! Check the **Results** tabs above.\n\n"
        "Type **approve** to finalize, or tell me what to change "
        "(see **Edit Plan Prompts** below for suggestions)."
    )
    chat_history.append({"role": "assistant", "content": reply})

    yield (
        chat_history,
        plan["summary"], plan["flights"], plan["hotels"],
        plan["itinerary"], plan["budget"], chart, plan["packing"],
        "", thread_id,
        gr.update(choices=edit_prompts, value=None),
    )


def chat_new_session():
    """Reset chat for a new trip."""
    # chatbot, 7 result tabs, chat_input, thread_state, edit_prompt_dropdown
    return [], "", "", "", "", "", None, "", "", "", gr.update(choices=[], value=None)


# ---------------------------------------------------------------------------
# Form mode — structured fields
# ---------------------------------------------------------------------------

def form_plan(destination, origin, start_date, end_date, budget, travelers, preferences, special_req):
    thread_id = str(uuid.uuid4())

    pref_text = preferences if preferences else "general sightseeing"
    prompt = (
        f"Plan a trip to {destination} for {int(travelers)} people, "
        f"budget ${int(budget)}, departing from {origin}. "
        f"Dates: {start_date} to {end_date}. "
        f"Preferences: {pref_text}."
    )
    if special_req and special_req.strip():
        prompt += f" Special requirements: {special_req}."

    config = {"configurable": {"thread_id": thread_id}}
    status_lines = []

    for lines, _ in _stream_graph({"messages": [HumanMessage(content=prompt)]}, config):
        status_lines = lines
        yield (
            "\n".join(status_lines),
            *_empty_results(),
            gr.update(visible=False),
            thread_id,
        )

    try:
        plan, chart, _ = _get_results(config)
    except Exception as e:
        status_lines.append(f">> Error: {e}")
        yield "\n".join(status_lines), *_empty_results(), gr.update(visible=False), thread_id
        return

    yield (
        "\n".join(status_lines),
        plan["summary"], plan["flights"], plan["hotels"],
        plan["itinerary"], plan["budget"], chart, plan["packing"],
        gr.update(visible=True),
        thread_id,
    )


def form_feedback(feedback_text, thread_id):
    if not thread_id:
        yield "Error: No active session.", *_empty_results(), gr.update(visible=False)
        return

    config = {"configurable": {"thread_id": thread_id}}
    status_lines = [f">> Submitting: {feedback_text[:80]}"]

    for lines, _ in _stream_graph(Command(resume=feedback_text), config):
        status_lines = lines
        yield "\n".join(status_lines), *_empty_results(), gr.update(visible=True)

    try:
        plan, chart, plan_status = _get_results(config)
    except Exception:
        yield "\n".join(status_lines), *_empty_results(), gr.update(visible=False)
        return

    if plan_status == "complete":
        status_lines.append(">> Plan approved! Have a wonderful trip!")
        yield "\n".join(status_lines), *_empty_results(), gr.update(visible=False)
        return

    yield (
        "\n".join(status_lines),
        plan["summary"], plan["flights"], plan["hotels"],
        plan["itinerary"], plan["budget"], chart, plan["packing"],
        gr.update(visible=True),
    )


# ---------------------------------------------------------------------------
# Gradio UI
# ---------------------------------------------------------------------------

EXAMPLES = [
    "Plan a 7-day romantic getaway to the Maldives for 2 people. Budget is $5000. We love snorkeling and spa treatments. Flying from Chicago in March 2025.",
    "Plan a 5-day beach vacation in Bali for 2 people, budget $3000, departing from New York. Dates: 2025-03-15 to 2025-03-20. Preferences: beach, culture.",
    "Plan a 10-day food adventure in Tokyo for 3 people, budget $6000, departing from San Francisco. Dates: 2025-04-01 to 2025-04-11. Preferences: food, adventure.",
    "Plan a 5-day cultural trip to Paris for 1 person, budget $4000, from Chicago. Dates: 2025-09-10 to 2025-09-15. Preferences: museums, food, history.",
]

TRAVEL_CSS = """
/* ═══ Travel-themed Conversation Panel ═══ */
#chat_column {
    background:
        /* Soft cloud shapes */
        radial-gradient(ellipse 130px 42px at 18% 5%, rgba(255,255,255,0.55), transparent),
        radial-gradient(ellipse 90px 30px at 68% 3%, rgba(255,255,255,0.4), transparent),
        radial-gradient(ellipse 70px 24px at 48% 9%, rgba(255,255,255,0.3), transparent),
        /* Sky-to-ocean gradient */
        linear-gradient(175deg,
            #daeef9 0%, #c4e3f5 18%, #addaf1 36%,
            #95cfed 54%, #7ec4e9 72%, #66b8e4 90%);
    border-radius: 20px;
    padding: 20px !important;
    border: 1px solid rgba(59,130,246,0.25);
    box-shadow: 0 8px 32px rgba(59,130,246,0.12),
                inset 0 2px 0 rgba(255,255,255,0.5);
    position: relative;
    overflow: hidden;
}
#chat_column > * { position: relative; z-index: 1; }

/* ── Airplane flying left-to-right across the sky + trailing bird ── */
#chat_column::before {
    content: '';
    position: absolute; inset: 0;
    pointer-events: none; z-index: 0;
    background-image:
        /* Airplane */
        url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='70' height='35'%3E%3Cpath d='M60 17L38 8V2.5a3 3 0 00-6 0V8L8 17v4l24-4v12l-5 3v3l8.5-2.5L39 35v-3l-5-3V17l24 4v-4z' fill='rgba(30,80,160,0.15)'/%3E%3C/svg%3E"),
        /* Small bird */
        url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='20' height='12'%3E%3Cpath d='M2 8q4-7 8 0M10 8q4-7 8 0' fill='none' stroke='rgba(30,80,160,0.12)' stroke-width='1.5' stroke-linecap='round'/%3E%3C/svg%3E");
    background-repeat: no-repeat;
    background-size: 70px, 20px;
    animation: fly-plane 14s linear infinite, plane-bob 3s ease-in-out infinite;
}

/* ── Sailboat + surfer cruising right-to-left on the water + scrolling waves ── */
#chat_column::after {
    content: '';
    position: absolute; inset: 0;
    pointer-events: none; z-index: 0;
    background-image:
        /* Sailboat */
        url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='65' height='55'%3E%3Cline x1='30' y1='5' x2='30' y2='38' stroke='rgba(100,60,20,0.20)' stroke-width='2'/%3E%3Cpath d='M30 5L14 35h16z' fill='rgba(255,255,255,0.35)' stroke='rgba(30,80,160,0.18)' stroke-width='1'/%3E%3Cpath d='M12 38h40l-5 8H17z' fill='rgba(30,80,160,0.15)'/%3E%3Cpath d='M2 48q10-4 20 0t20 0t20 0' fill='none' stroke='rgba(30,80,160,0.12)' stroke-width='1.5'/%3E%3C/svg%3E"),
        /* Surfer on board */
        url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='50' height='42'%3E%3Ccircle cx='25' cy='8' r='4' fill='rgba(30,80,160,0.16)'/%3E%3Cpath d='M25 12v10M20 17l5-3 5 3' stroke='rgba(30,80,160,0.16)' stroke-width='1.5' fill='none' stroke-linecap='round'/%3E%3Cpath d='M22 22l-3 5M28 22l3 5' stroke='rgba(30,80,160,0.16)' stroke-width='1.5' fill='none' stroke-linecap='round'/%3E%3Cellipse cx='25' cy='30' rx='15' ry='3' fill='rgba(30,80,160,0.11)'/%3E%3Cpath d='M5 35q10-4 20 0t20 0' fill='none' stroke='rgba(30,80,160,0.09)' stroke-width='1.5'/%3E%3C/svg%3E"),
        /* Wave band */
        url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 600 35' preserveAspectRatio='none'%3E%3Cpath d='M0 20q25-20 50 0t50 0 50 0 50 0 50 0 50 0 50 0 50 0 50 0 50 0 50 0 50 0v15H0z' fill='rgba(30,80,160,0.06)'/%3E%3C/svg%3E");
    background-repeat: no-repeat, no-repeat, repeat-x;
    background-size: 65px, 50px, 600px 30px;
    animation: sail-scene 22s linear infinite, sea-bob 4s ease-in-out infinite;
}

/* Airplane + bird fly left → right */
@keyframes fly-plane {
    0%   { background-position: -70px 7%, -20px 13%; }
    100% { background-position: calc(100% + 70px) 4%, calc(100% + 20px) 10%; }
}
@keyframes plane-bob {
    0%, 100% { transform: translateY(0); }
    50%      { transform: translateY(-5px); }
}

/* Ship + surfer sail right → left, waves scroll */
@keyframes sail-scene {
    0%   { background-position: calc(100% + 65px) 76%, calc(100% + 130px) 68%, 0px 94%; }
    100% { background-position: -65px 76%, -50px 68%, -600px 94%; }
}
@keyframes sea-bob {
    0%, 100% { transform: translateY(0); }
    50%      { transform: translateY(-3px); }
}

/* Header glow for readability over the gradient */
#chat_column h3 {
    text-shadow: 0 1px 3px rgba(255,255,255,0.7);
}
"""


with gr.Blocks(title="AI Travel Genie") as demo:
    gr.Markdown(
        "# AI Travel Genie\n"
        "*Multi-agent vacation planner powered by LangGraph*\n\n"
        "Use the **Travel Copilot** to describe your trip in natural language, "
        "or the **Quick Form** to fill in structured fields."
    )

    thread_state = gr.State("")

    # ---- Results tabs (shared by both modes) ----
    with gr.Tabs() as result_tabs:
        with gr.Tab("Summary"):
            tab_summary = gr.Markdown()
        with gr.Tab("Flights"):
            tab_flights = gr.Markdown()
        with gr.Tab("Hotels"):
            tab_hotels = gr.Markdown()
        with gr.Tab("Itinerary"):
            tab_itinerary = gr.Markdown()
        with gr.Tab("Budget"):
            tab_budget = gr.Markdown()
        with gr.Tab("Budget Chart"):
            tab_chart = gr.Plot()
        with gr.Tab("Packing"):
            tab_packing = gr.Markdown()

    result_outputs = [
        tab_summary, tab_flights, tab_hotels, tab_itinerary,
        tab_budget, tab_chart, tab_packing,
    ]

    # ---- Input modes: side by side ----
    with gr.Row(equal_height=False):

        # ============== LEFT: Travel Copilot ==============
        with gr.Column(scale=1, elem_id="chat_column"):
            gr.Markdown("### Travel Copilot")
            gr.Markdown(
                "Describe your trip in plain English. After the plan is ready, "
                "type **approve** or describe changes."
            )
            chatbot = gr.Chatbot(
                label="Conversation",
                height=380,
            )
            with gr.Row():
                chat_input = gr.Textbox(
                    label="Your message",
                    placeholder='e.g., "Plan a 7-day trip to Maldives for 2, budget $5000, from Chicago"',
                    scale=5,
                    lines=2,
                )
                chat_send = gr.Button("Plan My Trip", variant="primary", scale=1)

            with gr.Accordion("Example prompts", open=False):
                gr.Examples(
                    examples=[[e] for e in EXAMPLES],
                    inputs=[chat_input],
                    label="Click an example to fill in the prompt",
                )

            with gr.Accordion("Edit Plan Prompts (available after first plan)", open=False):
                edit_prompt_dropdown = gr.Dropdown(
                    choices=[],
                    label="Pick a suggestion to modify your plan",
                    interactive=True,
                    value=None,
                )

            chat_new = gr.Button("New Trip", variant="secondary", size="sm")

            chat_outputs = [chatbot, *result_outputs, chat_input, thread_state, edit_prompt_dropdown]

            chat_send.click(
                fn=chat_plan,
                inputs=[chat_input, chatbot, thread_state],
                outputs=chat_outputs,
            )
            chat_input.submit(
                fn=chat_plan,
                inputs=[chat_input, chatbot, thread_state],
                outputs=chat_outputs,
            )
            chat_new.click(
                fn=chat_new_session,
                outputs=chat_outputs,
            )
            edit_prompt_dropdown.change(
                fn=lambda choice: choice or "",
                inputs=[edit_prompt_dropdown],
                outputs=[chat_input],
            )

        # ============== RIGHT: Form ==============
        with gr.Column(scale=1):
            gr.Markdown("### Quick Form")
            with gr.Row():
                destination = gr.Textbox(label="Destination", placeholder="e.g., Bali, Paris, Tokyo")
                origin = gr.Textbox(label="Departing From", placeholder="e.g., New York")
            with gr.Row():
                start_date = gr.Textbox(label="Start Date", placeholder="YYYY-MM-DD", value="2025-06-10")
                end_date = gr.Textbox(label="End Date", placeholder="YYYY-MM-DD", value="2025-06-17")
            with gr.Row():
                budget_input = gr.Number(label="Budget (USD)", value=5000)
                travelers_input = gr.Number(label="Travelers", value=2, precision=0)
            with gr.Row():
                preferences = gr.Textbox(label="Preferences", placeholder="beach, culture, adventure")
                special_req = gr.Textbox(label="Special Requirements", placeholder="vegetarian, wheelchair accessible")

            form_plan_btn = gr.Button("Plan My Trip", variant="primary", size="lg")

            form_status = gr.Textbox(label="Agent Progress", lines=6, interactive=False)

            with gr.Group(visible=False) as form_review_section:
                gr.Markdown(
                    "**Review Your Plan** — Type **approve** to finalize, or describe changes."
                )
                with gr.Row():
                    form_feedback_box = gr.Textbox(label="Your Feedback", scale=4, placeholder="approve")
                    form_feedback_btn = gr.Button("Submit Feedback", scale=1, variant="secondary")

            form_plan_btn.click(
                fn=form_plan,
                inputs=[destination, origin, start_date, end_date, budget_input, travelers_input, preferences, special_req],
                outputs=[form_status, *result_outputs, form_review_section, thread_state],
            )

            form_feedback_btn.click(
                fn=form_feedback,
                inputs=[form_feedback_box, thread_state],
                outputs=[form_status, *result_outputs, form_review_section],
            )


if __name__ == "__main__":
    demo.launch(share=False, server_name="0.0.0.0", server_port=7860, theme=gr.themes.Soft(), css=TRAVEL_CSS)
