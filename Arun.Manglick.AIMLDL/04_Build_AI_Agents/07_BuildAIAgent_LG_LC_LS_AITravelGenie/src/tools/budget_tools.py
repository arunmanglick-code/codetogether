import json
from langchain_core.tools import tool

CURRENCY_DB = {
    "bali": {"code": "IDR", "name": "Indonesian Rupiah", "rate": 15800, "symbol": "Rp"},
    "indonesia": {"code": "IDR", "name": "Indonesian Rupiah", "rate": 15800, "symbol": "Rp"},
    "maldives": {"code": "MVR", "name": "Maldivian Rufiyaa", "rate": 15.42, "symbol": "Rf"},
    "paris": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "france": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "london": {"code": "GBP", "name": "British Pound", "rate": 0.79, "symbol": "GBP"},
    "uk": {"code": "GBP", "name": "British Pound", "rate": 0.79, "symbol": "GBP"},
    "tokyo": {"code": "JPY", "name": "Japanese Yen", "rate": 149.0, "symbol": "JPY"},
    "japan": {"code": "JPY", "name": "Japanese Yen", "rate": 149.0, "symbol": "JPY"},
    "dubai": {"code": "AED", "name": "UAE Dirham", "rate": 3.67, "symbol": "AED"},
    "singapore": {"code": "SGD", "name": "Singapore Dollar", "rate": 1.35, "symbol": "S$"},
    "bangkok": {"code": "THB", "name": "Thai Baht", "rate": 35.5, "symbol": "THB"},
    "thailand": {"code": "THB", "name": "Thai Baht", "rate": 35.5, "symbol": "THB"},
    "hong kong": {"code": "HKD", "name": "Hong Kong Dollar", "rate": 7.82, "symbol": "HK$"},
    "sydney": {"code": "AUD", "name": "Australian Dollar", "rate": 1.54, "symbol": "A$"},
    "australia": {"code": "AUD", "name": "Australian Dollar", "rate": 1.54, "symbol": "A$"},
    "rome": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "italy": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "barcelona": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "spain": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "amsterdam": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "santorini": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "greece": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "athens": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "lisbon": {"code": "EUR", "name": "Euro", "rate": 0.92, "symbol": "E"},
    "zurich": {"code": "CHF", "name": "Swiss Franc", "rate": 0.88, "symbol": "CHF"},
    "istanbul": {"code": "TRY", "name": "Turkish Lira", "rate": 32.5, "symbol": "TRY"},
    "cairo": {"code": "EGP", "name": "Egyptian Pound", "rate": 30.9, "symbol": "EGP"},
    "mumbai": {"code": "INR", "name": "Indian Rupee", "rate": 83.2, "symbol": "INR"},
    "delhi": {"code": "INR", "name": "Indian Rupee", "rate": 83.2, "symbol": "INR"},
    "india": {"code": "INR", "name": "Indian Rupee", "rate": 83.2, "symbol": "INR"},
    "seoul": {"code": "KRW", "name": "South Korean Won", "rate": 1330.0, "symbol": "KRW"},
    "kuala lumpur": {"code": "MYR", "name": "Malaysian Ringgit", "rate": 4.72, "symbol": "RM"},
    "cancun": {"code": "MXN", "name": "Mexican Peso", "rate": 17.2, "symbol": "MXN"},
    "mexico": {"code": "MXN", "name": "Mexican Peso", "rate": 17.2, "symbol": "MXN"},
    "rio de janeiro": {"code": "BRL", "name": "Brazilian Real", "rate": 4.97, "symbol": "R$"},
    "brazil": {"code": "BRL", "name": "Brazilian Real", "rate": 4.97, "symbol": "R$"},
    "cape town": {"code": "ZAR", "name": "South African Rand", "rate": 18.5, "symbol": "R"},
    "nairobi": {"code": "KES", "name": "Kenyan Shilling", "rate": 153.0, "symbol": "KES"},
    "marrakech": {"code": "MAD", "name": "Moroccan Dirham", "rate": 10.1, "symbol": "MAD"},
    "morocco": {"code": "MAD", "name": "Moroccan Dirham", "rate": 10.1, "symbol": "MAD"},
    "honolulu": {"code": "USD", "name": "US Dollar", "rate": 1.0, "symbol": "$"},
    "hawaii": {"code": "USD", "name": "US Dollar", "rate": 1.0, "symbol": "$"},
    "fiji": {"code": "FJD", "name": "Fijian Dollar", "rate": 2.25, "symbol": "FJ$"},
    "reykjavik": {"code": "ISK", "name": "Icelandic Krona", "rate": 138.0, "symbol": "ISK"},
    "iceland": {"code": "ISK", "name": "Icelandic Krona", "rate": 138.0, "symbol": "ISK"},
    "colombo": {"code": "LKR", "name": "Sri Lankan Rupee", "rate": 325.0, "symbol": "LKR"},
    "luang prabang": {"code": "LAK", "name": "Lao Kip", "rate": 20800.0, "symbol": "LAK"},
    "laos": {"code": "LAK", "name": "Lao Kip", "rate": 20800.0, "symbol": "LAK"},
    "hanoi": {"code": "VND", "name": "Vietnamese Dong", "rate": 24500.0, "symbol": "VND"},
    "vietnam": {"code": "VND", "name": "Vietnamese Dong", "rate": 24500.0, "symbol": "VND"},
    "ho chi minh city": {"code": "VND", "name": "Vietnamese Dong", "rate": 24500.0, "symbol": "VND"},
    "phuket": {"code": "THB", "name": "Thai Baht", "rate": 35.5, "symbol": "THB"},
    "johannesburg": {"code": "ZAR", "name": "South African Rand", "rate": 18.5, "symbol": "R"},
}

DEFAULT_CURRENCY = {"code": "USD", "name": "US Dollar (destination currency unknown)", "rate": 1.0, "symbol": "$"}

DAILY_FOOD_ESTIMATES = {
    "southeast_asia": 30, "east_asia": 50, "south_asia": 25,
    "indian_ocean": 70, "europe": 60, "middle_east": 50,
    "oceania": 55, "pacific": 45, "central_america": 35,
    "south_america": 35, "africa": 35, "north_america": 55,
}

DAILY_TRANSPORT_ESTIMATES = {
    "southeast_asia": 10, "east_asia": 15, "south_asia": 8,
    "indian_ocean": 20, "europe": 15, "middle_east": 15,
    "oceania": 18, "pacific": 12, "central_america": 10,
    "south_america": 12, "africa": 15, "north_america": 20,
}


def _get_dest_region(destination: str) -> str:
    from src.tools.flight_tools import AIRPORT_DB, REGION_MAP
    key = destination.lower().strip()
    entry = AIRPORT_DB.get(key)
    if entry:
        return REGION_MAP.get(entry["code"], "europe")
    for db_key, info in AIRPORT_DB.items():
        if key in db_key or db_key in key:
            return REGION_MAP.get(info["code"], "europe")
    return "europe"


@tool
def convert_currency(amount_usd: float, destination: str) -> str:
    """Convert an amount from USD to the local currency of a destination.
    Provide the amount in USD and the destination city or country name."""
    key = destination.lower().strip()
    currency = CURRENCY_DB.get(key)
    if not currency:
        for db_key, info in CURRENCY_DB.items():
            if key in db_key or db_key in key:
                currency = info
                break
    if not currency:
        currency = DEFAULT_CURRENCY

    local_amount = round(amount_usd * currency["rate"], 2)

    result = {
        "amount_usd": amount_usd,
        "destination": destination,
        "local_currency": currency["name"],
        "currency_code": currency["code"],
        "exchange_rate": currency["rate"],
        "local_amount": local_amount,
        "formatted": f"{currency['symbol']} {local_amount:,.2f}",
    }
    return json.dumps(result, indent=2)


@tool
def calculate_budget(
    budget_usd: float,
    travelers: int,
    num_days: int,
    flight_cost: float,
    hotel_cost: float,
    activity_cost: float,
    destination: str,
) -> str:
    """Calculate a comprehensive travel budget breakdown. Provide the total budget in USD,
    number of travelers, trip days, and known costs for flights, hotels, and activities.
    Estimates food, transport, and miscellaneous costs based on the destination region."""
    region = _get_dest_region(destination)

    daily_food = DAILY_FOOD_ESTIMATES.get(region, 50) * travelers
    daily_transport = DAILY_TRANSPORT_ESTIMATES.get(region, 15) * travelers
    food_cost = daily_food * num_days
    transport_cost = daily_transport * num_days
    misc_cost = round(0.05 * budget_usd)

    total = flight_cost + hotel_cost + activity_cost + food_cost + transport_cost + misc_cost

    if total <= budget_usd * 0.95:
        status = "WITHIN BUDGET"
    elif total <= budget_usd * 1.05:
        status = "ON TARGET"
    else:
        status = "OVER BUDGET"

    remaining = budget_usd - total

    key = destination.lower().strip()
    currency = CURRENCY_DB.get(key)
    if not currency:
        for db_key, info in CURRENCY_DB.items():
            if key in db_key or db_key in key:
                currency = info
                break
    if not currency:
        currency = DEFAULT_CURRENCY

    savings_tips = []
    if status == "OVER BUDGET":
        if flight_cost > budget_usd * 0.4:
            savings_tips.append("Flights consume >40% of budget. Consider cheaper airlines or flexible dates.")
        if hotel_cost > budget_usd * 0.35:
            savings_tips.append("Accommodation is expensive. Consider lower-star hotels or hostels.")
        if activity_cost > budget_usd * 0.2:
            savings_tips.append("Reduce paid activities. Many destinations have free walking tours and attractions.")
        savings_tips.append("Eat at local restaurants instead of tourist spots to save 30-50% on food.")
        if not savings_tips[:-1]:
            savings_tips.insert(0, "Look for combo deals or travel during off-peak season for better rates.")

    result = {
        "budget_usd": budget_usd,
        "travelers": travelers,
        "num_days": num_days,
        "breakdown": {
            "flights": flight_cost,
            "accommodation": hotel_cost,
            "activities": activity_cost,
            "food_and_dining": food_cost,
            "local_transport": transport_cost,
            "miscellaneous": misc_cost,
        },
        "total_usd": total,
        "budget_status": status,
        "remaining_or_over": remaining,
        "per_person_cost": round(total / max(travelers, 1), 2),
        "daily_spending": round((total - flight_cost) / max(num_days, 1), 2),
        "currency_conversion": {
            "local_currency": currency["name"],
            "currency_code": currency["code"],
            "exchange_rate": currency["rate"],
            "total_local_currency": round(total * currency["rate"], 2),
        },
        "savings_tips": savings_tips,
    }
    return json.dumps(result, indent=2)
