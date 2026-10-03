import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt


def format_trip_summary_card(travel_request: dict | None, destination_research: dict | None) -> str:
    if not travel_request:
        return "*Trip summary not available — no travel request parsed yet.*"

    req = travel_request
    dest = destination_research or {}

    lines = [
        f"### {dest.get('name', req.get('destination', 'Unknown'))}",
        "",
        f"| Detail | Value |",
        f"|--------|-------|",
        f"| Dates | {req.get('start_date', '?')} to {req.get('end_date', '?')} |",
        f"| Travelers | {req.get('travelers', '?')} |",
        f"| Budget | ${req.get('budget_usd', 0):,.0f} USD |",
        f"| Origin | {req.get('origin', '?')} |",
        f"| Preferences | {', '.join(req.get('preferences', [])) or 'None'} |",
    ]

    if dest:
        lines.append(f"| Best Time to Visit | {dest.get('best_time_to_visit', 'N/A')} |")
        lines.append(f"| Visa | {dest.get('visa_requirements', 'N/A')} |")
        lines.append(f"| Language | {dest.get('language', 'N/A')} |")
        lines.append(f"| Local Currency | {dest.get('currency', 'N/A')} |")
        score = dest.get("suitability_score")
        if score is not None:
            lines.append(f"| Suitability Score | {score}/10 |")

    overview = dest.get("overview")
    if overview:
        lines.extend(["", f"> {overview}"])

    return "\n".join(lines)


def format_flight_table(flight_options: dict | None) -> str:
    if not flight_options or not flight_options.get("options"):
        return "*Flight data not available.*"

    options = flight_options["options"]
    lines = [
        "### Flight Options",
        "",
        "| # | Airline | Route | $/Person | Total | Duration | Stops |",
        "|---|---------|-------|----------|-------|----------|-------|",
    ]
    for i, f in enumerate(options, 1):
        lines.append(
            f"| {i} | {f.get('airline', '?')} | {f.get('route', '?')} "
            f"| ${f.get('price_per_person', 0):,.0f} "
            f"| ${f.get('total_price', 0):,.0f} "
            f"| {f.get('duration', '?')} "
            f"| {f.get('layovers', 0)} |"
        )
    return "\n".join(lines)


def format_hotel_table(hotel_options: dict | None) -> str:
    if not hotel_options or not hotel_options.get("options"):
        return "*Hotel data not available.*"

    options = hotel_options["options"]
    lines = [
        "### Hotel Options",
        "",
        "| # | Hotel | Stars | $/Night | Total | Rating | Location |",
        "|---|-------|-------|---------|-------|--------|----------|",
    ]
    for i, h in enumerate(options, 1):
        amenities = h.get("amenities", [])
        amenity_str = ", ".join(amenities[:3]) if amenities else ""
        lines.append(
            f"| {i} | {h.get('name', '?')} "
            f"| {'*' * h.get('star_rating', 0)} "
            f"| ${h.get('price_per_night', 0):,.0f} "
            f"| ${h.get('total_price', 0):,.0f} "
            f"| {h.get('guest_rating', '?')}/10 "
            f"| {h.get('location', '?')} |"
        )
        if amenity_str:
            lines.append(f"|   |       |       |         |       | *{amenity_str}* |          |")
    return "\n".join(lines)


def format_itinerary(itinerary: dict | None) -> str:
    if not itinerary or not itinerary.get("days"):
        return "*Itinerary not available.*"

    days = itinerary["days"]
    lines = ["### Day-by-Day Itinerary", ""]

    for day in days:
        lines.append(f"**Day {day.get('day', '?')}: {day.get('theme', '')}** — {day.get('weather', '')}")
        m = day.get("morning", {})
        a = day.get("afternoon", {})
        e = day.get("evening", {})
        lines.append(f"| Time | Activity | Cost |")
        lines.append(f"|------|----------|------|")
        lines.append(f"| Morning | {m.get('activity', '?')} | ${m.get('cost', 0)} |")
        lines.append(f"| Afternoon | {a.get('activity', '?')} | ${a.get('cost', 0)} |")
        lines.append(f"| Evening | {e.get('activity', '?')} | ${e.get('cost', 0)} |")
        lines.append("")

    total = itinerary.get("total_activity_cost", 0)
    lines.append(f"**Total Activity Cost**: ${total:,.0f}")
    return "\n".join(lines)


def format_budget_text(budget_summary: dict | None) -> str:
    if not budget_summary:
        return "*Budget data not available.*"

    b = budget_summary
    lines = [
        "### Budget Breakdown",
        "",
        "| Category | Amount |",
        "|----------|--------|",
        f"| Flights | ${b.get('flights', 0):,.0f} |",
        f"| Accommodation | ${b.get('accommodation', 0):,.0f} |",
        f"| Activities | ${b.get('activities', 0):,.0f} |",
        f"| Food & Dining | ${b.get('food_and_dining', 0):,.0f} |",
        f"| Local Transport | ${b.get('local_transport', 0):,.0f} |",
        f"| Miscellaneous | ${b.get('miscellaneous', 0):,.0f} |",
        f"| **Total** | **${b.get('total_usd', 0):,.0f}** |",
        "",
        f"**Budget**: ${b.get('budget_usd', 0):,.0f} | "
        f"**Status**: {b.get('budget_status', 'N/A')} | "
        f"**Per Person**: ${b.get('per_person_cost', 0):,.0f}",
    ]

    conv = b.get("currency_conversion", {})
    if conv:
        lines.append(
            f"\nLocal currency: {conv.get('total_local_currency', 0):,.0f} "
            f"{conv.get('currency_code', '')}"
        )

    tips = b.get("savings_tips", [])
    if tips:
        lines.append("\n**Savings Tips**:")
        for tip in tips:
            lines.append(f"- {tip}")

    return "\n".join(lines)


def create_budget_pie_chart(budget_summary: dict | None):
    if not budget_summary:
        return None

    categories = [
        ("Flights", budget_summary.get("flights", 0)),
        ("Accommodation", budget_summary.get("accommodation", 0)),
        ("Activities", budget_summary.get("activities", 0)),
        ("Food & Dining", budget_summary.get("food_and_dining", 0)),
        ("Transport", budget_summary.get("local_transport", 0)),
        ("Miscellaneous", budget_summary.get("miscellaneous", 0)),
    ]
    categories = [(name, val) for name, val in categories if val > 0]
    if not categories:
        return None

    labels, values = zip(*categories)

    fig, ax = plt.subplots(figsize=(8, 6))
    colors = ["#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7", "#DDA0DD"]
    wedges, texts, autotexts = ax.pie(
        values,
        labels=labels,
        autopct="%1.1f%%",
        colors=colors[: len(values)],
        startangle=90,
    )
    for text in autotexts:
        text.set_fontsize(9)
    ax.set_title(
        f"Budget Breakdown — ${budget_summary.get('total_usd', 0):,.0f} Total",
        fontsize=13,
        fontweight="bold",
    )
    fig.tight_layout()
    return fig


def format_packing_suggestions(itinerary: dict | None, destination_research: dict | None) -> str:
    if not itinerary or not itinerary.get("days"):
        return "*Packing suggestions not available — no itinerary data.*"

    days = itinerary["days"]
    temps = []
    has_rain = False
    has_sun = False

    for day in days:
        weather = day.get("weather", "").lower()
        import re
        temp_match = re.search(r"(\d+)\s*[cC]", weather)
        if temp_match:
            temps.append(int(temp_match.group(1)))
        if any(w in weather for w in ["rain", "shower", "storm", "overcast"]):
            has_rain = True
        if any(w in weather for w in ["sun", "clear", "hot", "warm"]):
            has_sun = True

    avg_temp = sum(temps) / len(temps) if temps else 25

    lines = ["### Packing Suggestions", ""]

    lines.append("**Clothing**:")
    if avg_temp >= 28:
        lines.append("- Light, breathable clothing (cotton/linen)")
        lines.append("- Swimwear")
        lines.append("- Sandals and flip-flops")
    elif avg_temp >= 20:
        lines.append("- Light layers (t-shirts + light jacket)")
        lines.append("- Comfortable walking shoes")
    else:
        lines.append("- Warm layers (sweaters, jacket)")
        lines.append("- Warm socks and closed shoes")

    if has_rain:
        lines.append("")
        lines.append("**Rain Gear**:")
        lines.append("- Compact umbrella")
        lines.append("- Light rain jacket")

    if has_sun:
        lines.append("")
        lines.append("**Sun Protection**:")
        lines.append("- Sunscreen (SPF 30+)")
        lines.append("- Sunglasses")
        lines.append("- Wide-brimmed hat")

    dest = destination_research or {}
    dest_name = dest.get("name", "").lower()
    lines.append("")
    lines.append("**Essentials**:")
    lines.append("- Travel documents (passport, visa, insurance)")
    lines.append("- Phone charger and adapter")
    lines.append("- Reusable water bottle")
    lines.append("- First-aid kit basics")

    if any(w in dest_name for w in ["bali", "maldives", "cancun", "phuket", "hawaii"]):
        lines.append("- Snorkeling gear (or rent locally)")
        lines.append("- Waterproof phone case")
    if any(w in dest_name for w in ["paris", "rome", "tokyo", "barcelona"]):
        lines.append("- Comfortable walking shoes (city exploration)")
        lines.append("- Day pack for museum visits")

    return "\n".join(lines)


def format_full_plan(state: dict) -> dict[str, str]:
    """Format all sections of the travel plan from graph state."""
    return {
        "summary": format_trip_summary_card(
            state.get("travel_request"),
            state.get("destination_research"),
        ),
        "flights": format_flight_table(state.get("flight_options")),
        "hotels": format_hotel_table(state.get("hotel_options")),
        "itinerary": format_itinerary(state.get("itinerary")),
        "budget": format_budget_text(state.get("budget_summary")),
        "packing": format_packing_suggestions(
            state.get("itinerary"),
            state.get("destination_research"),
        ),
    }
