from langchain_core.tools import tool


@tool
def web_search(query: str) -> str:
    """Search the web for travel destination information. Use this to find current info about
    attractions, safety advisories, visa requirements, weather, and travel tips for any destination."""
    try:
        from langchain_community.tools import DuckDuckGoSearchResults

        search = DuckDuckGoSearchResults(max_results=5)
        results = search.invoke(query)
        return results
    except Exception as e:
        return f"Search unavailable: {e}. Please use your training knowledge instead."


@tool
def get_destination_info(destination: str) -> str:
    """Get structured travel information about a destination including visa requirements,
    currency, language, safety rating, and best time to visit."""
    info_db = {
        "bali": {
            "full_name": "Bali, Indonesia",
            "visa": "Visa-free for US citizens up to 30 days",
            "currency": "Indonesian Rupiah (IDR), ~15,800 per USD",
            "language": "Bahasa Indonesia; English widely spoken in tourist areas",
            "safety": "Generally safe for tourists; petty theft in crowded areas",
            "best_season": "April-October (dry season)",
            "avg_temp": "27-30°C (80-86°F) year-round",
            "time_zone": "WITA (UTC+8)",
            "electricity": "230V, Type C/F plugs",
            "emergency": "112 (general), 118 (ambulance)",
        },
        "maldives": {
            "full_name": "Maldives",
            "visa": "Visa on arrival for US citizens, free for 30 days",
            "currency": "Maldivian Rufiyaa (MVR), ~15.42 per USD; USD widely accepted",
            "language": "Dhivehi; English spoken in all resorts",
            "safety": "Very safe for tourists; alcohol only in resorts",
            "best_season": "November-April (dry northeast monsoon)",
            "avg_temp": "28-31°C (82-88°F) year-round",
            "time_zone": "MVT (UTC+5)",
            "electricity": "230V, Type G plugs (UK style)",
            "emergency": "119 (police), 102 (ambulance)",
        },
        "paris": {
            "full_name": "Paris, France",
            "visa": "No visa for US citizens up to 90 days (Schengen area)",
            "currency": "Euro (EUR), ~0.92 per USD",
            "language": "French; English common in tourist areas",
            "safety": "Generally safe; watch for pickpockets at tourist spots and metro",
            "best_season": "April-June, September-October",
            "avg_temp": "3-25°C (37-77°F) depending on season",
            "time_zone": "CET (UTC+1)",
            "electricity": "230V, Type C/E plugs",
            "emergency": "112 (EU), 15 (medical), 17 (police)",
        },
        "tokyo": {
            "full_name": "Tokyo, Japan",
            "visa": "Visa-free for US citizens up to 90 days",
            "currency": "Japanese Yen (JPY), ~149 per USD",
            "language": "Japanese; English signage in tourist areas, limited spoken English",
            "safety": "Extremely safe; one of the safest major cities",
            "best_season": "March-May (cherry blossom), October-November (autumn foliage)",
            "avg_temp": "5-30°C (41-86°F) depending on season",
            "time_zone": "JST (UTC+9)",
            "electricity": "100V, Type A/B plugs (US compatible)",
            "emergency": "110 (police), 119 (fire/ambulance)",
        },
    }

    key = destination.lower().strip()
    for db_key, info in info_db.items():
        if db_key in key or key in info.get("full_name", "").lower():
            lines = [f"Destination Info: {info['full_name']}"]
            for k, v in info.items():
                if k != "full_name":
                    lines.append(f"  {k.replace('_', ' ').title()}: {v}")
            return "\n".join(lines)

    return (
        f"No pre-loaded info for '{destination}'. "
        "Use web_search to find visa, currency, safety, and travel details."
    )
