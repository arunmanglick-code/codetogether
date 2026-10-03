import hashlib
import json
from langchain_core.tools import tool

ACTIVITY_DB = {
    "bali": {
        "cultural": [
            {"name": "Uluwatu Temple sunset visit", "cost": 5, "duration": "2-3h", "type": "cultural"},
            {"name": "Tegallalang Rice Terraces guided walk", "cost": 15, "duration": "2h", "type": "cultural"},
            {"name": "Sacred Monkey Forest Sanctuary", "cost": 5, "duration": "1.5h", "type": "cultural"},
            {"name": "Tirta Empul holy water temple purification", "cost": 8, "duration": "2h", "type": "cultural"},
            {"name": "Traditional Kecak fire dance performance", "cost": 15, "duration": "1.5h", "type": "cultural"},
            {"name": "Batik-making workshop in Ubud", "cost": 25, "duration": "3h", "type": "cultural"},
        ],
        "adventure": [
            {"name": "Mount Batur sunrise trek", "cost": 60, "duration": "6h", "type": "adventure"},
            {"name": "White water rafting on Ayung River", "cost": 45, "duration": "3h", "type": "adventure"},
            {"name": "ATV ride through rice paddies", "cost": 35, "duration": "2h", "type": "adventure"},
            {"name": "Snorkeling at Blue Lagoon Beach", "cost": 35, "duration": "3h", "type": "adventure"},
            {"name": "Scuba diving at USS Liberty wreck", "cost": 80, "duration": "4h", "type": "adventure"},
        ],
        "relaxation": [
            {"name": "Balinese spa and massage", "cost": 40, "duration": "2h", "type": "relaxation"},
            {"name": "Beach club day at Potato Head", "cost": 30, "duration": "4h", "type": "relaxation"},
            {"name": "Yoga class overlooking rice terraces", "cost": 15, "duration": "1.5h", "type": "relaxation"},
        ],
        "food": [
            {"name": "Ubud food tour with local guide", "cost": 45, "duration": "3h", "type": "food"},
            {"name": "Cooking class with market visit", "cost": 35, "duration": "4h", "type": "food"},
            {"name": "Gianyar night market street food", "cost": 15, "duration": "2h", "type": "food"},
        ],
    },
    "maldives": {
        "cultural": [
            {"name": "Male city walking tour", "cost": 30, "duration": "3h", "type": "cultural"},
            {"name": "Local island visit and fishing village", "cost": 45, "duration": "4h", "type": "cultural"},
        ],
        "adventure": [
            {"name": "Guided reef snorkeling tour", "cost": 65, "duration": "3h", "type": "adventure"},
            {"name": "Scuba diving at Banana Reef", "cost": 95, "duration": "4h", "type": "adventure"},
            {"name": "Night snorkeling with bioluminescent plankton", "cost": 55, "duration": "2h", "type": "adventure"},
            {"name": "Kayaking and paddleboarding", "cost": 40, "duration": "2h", "type": "adventure"},
            {"name": "Jet ski tour around the atoll", "cost": 80, "duration": "1h", "type": "adventure"},
        ],
        "relaxation": [
            {"name": "Overwater spa treatment", "cost": 150, "duration": "2h", "type": "relaxation"},
            {"name": "Sunset dolphin watching cruise", "cost": 85, "duration": "2h", "type": "relaxation"},
            {"name": "Sandbank picnic and swimming", "cost": 45, "duration": "3h", "type": "relaxation"},
            {"name": "Sunrise yoga on the beach", "cost": 15, "duration": "1h", "type": "relaxation"},
        ],
        "food": [
            {"name": "Romantic beach dinner under the stars", "cost": 90, "duration": "2h", "type": "food"},
            {"name": "Wine pairing dinner at overwater restaurant", "cost": 110, "duration": "2.5h", "type": "food"},
            {"name": "Fishing trip and cook-your-catch", "cost": 75, "duration": "4h", "type": "food"},
        ],
    },
    "paris": {
        "cultural": [
            {"name": "Louvre Museum guided tour", "cost": 25, "duration": "3h", "type": "cultural"},
            {"name": "Musee d'Orsay impressionist collection", "cost": 16, "duration": "2h", "type": "cultural"},
            {"name": "Notre-Dame area and Sainte-Chapelle", "cost": 12, "duration": "2h", "type": "cultural"},
            {"name": "Versailles Palace day trip", "cost": 50, "duration": "6h", "type": "cultural"},
            {"name": "Montmartre art walk and Sacre-Coeur", "cost": 0, "duration": "2h", "type": "cultural"},
        ],
        "adventure": [
            {"name": "Eiffel Tower summit with skip-the-line", "cost": 35, "duration": "2h", "type": "adventure"},
            {"name": "Seine River cruise", "cost": 20, "duration": "1.5h", "type": "adventure"},
            {"name": "Catacombs underground tour", "cost": 30, "duration": "1.5h", "type": "adventure"},
            {"name": "Bike tour through Paris landmarks", "cost": 40, "duration": "3h", "type": "adventure"},
        ],
        "relaxation": [
            {"name": "Luxembourg Gardens picnic", "cost": 15, "duration": "2h", "type": "relaxation"},
            {"name": "Paris hammam spa experience", "cost": 60, "duration": "2h", "type": "relaxation"},
        ],
        "food": [
            {"name": "Le Marais food and wine walking tour", "cost": 65, "duration": "3h", "type": "food"},
            {"name": "French cooking class", "cost": 80, "duration": "3h", "type": "food"},
            {"name": "Cheese and wine tasting in Saint-Germain", "cost": 45, "duration": "2h", "type": "food"},
        ],
    },
    "tokyo": {
        "cultural": [
            {"name": "Meiji Shrine and Harajuku walk", "cost": 0, "duration": "2h", "type": "cultural"},
            {"name": "Senso-ji Temple and Asakusa district", "cost": 0, "duration": "2h", "type": "cultural"},
            {"name": "Imperial Palace East Gardens tour", "cost": 0, "duration": "1.5h", "type": "cultural"},
            {"name": "Sumo wrestling morning practice viewing", "cost": 0, "duration": "2h", "type": "cultural"},
            {"name": "Tea ceremony experience", "cost": 35, "duration": "1.5h", "type": "cultural"},
        ],
        "adventure": [
            {"name": "Shibuya Crossing and Shinjuku night walk", "cost": 0, "duration": "2h", "type": "adventure"},
            {"name": "TeamLab Borderless digital art museum", "cost": 30, "duration": "2h", "type": "adventure"},
            {"name": "Go-kart tour through Tokyo streets", "cost": 80, "duration": "2h", "type": "adventure"},
            {"name": "Day trip to Mount Fuji (5th station)", "cost": 90, "duration": "8h", "type": "adventure"},
        ],
        "relaxation": [
            {"name": "Traditional onsen (hot spring) experience", "cost": 25, "duration": "2h", "type": "relaxation"},
            {"name": "Shinjuku Gyoen National Garden stroll", "cost": 5, "duration": "1.5h", "type": "relaxation"},
        ],
        "food": [
            {"name": "Tsukiji Outer Market food tour", "cost": 50, "duration": "3h", "type": "food"},
            {"name": "Ramen-tasting tour in Shinjuku", "cost": 40, "duration": "2h", "type": "food"},
            {"name": "Sushi-making class", "cost": 70, "duration": "2.5h", "type": "food"},
            {"name": "Izakaya (Japanese pub) hop in Yurakucho", "cost": 35, "duration": "2h", "type": "food"},
        ],
    },
}

GENERIC_ACTIVITIES = {
    "cultural": [
        {"name": "City walking tour with local guide", "cost": 30, "duration": "3h", "type": "cultural"},
        {"name": "Visit main historical museum", "cost": 15, "duration": "2h", "type": "cultural"},
        {"name": "Old town exploration and architecture walk", "cost": 0, "duration": "2h", "type": "cultural"},
        {"name": "Local artisan workshop visit", "cost": 25, "duration": "2h", "type": "cultural"},
    ],
    "adventure": [
        {"name": "Scenic viewpoint hike", "cost": 10, "duration": "3h", "type": "adventure"},
        {"name": "Water sports experience", "cost": 50, "duration": "2h", "type": "adventure"},
        {"name": "Guided nature excursion", "cost": 40, "duration": "4h", "type": "adventure"},
    ],
    "relaxation": [
        {"name": "Local spa and wellness treatment", "cost": 50, "duration": "2h", "type": "relaxation"},
        {"name": "Park or garden relaxation", "cost": 0, "duration": "2h", "type": "relaxation"},
        {"name": "Beach or waterfront leisure", "cost": 0, "duration": "3h", "type": "relaxation"},
    ],
    "food": [
        {"name": "Local food market tour", "cost": 35, "duration": "2h", "type": "food"},
        {"name": "Traditional cooking class", "cost": 55, "duration": "3h", "type": "food"},
        {"name": "Street food evening walk", "cost": 20, "duration": "2h", "type": "food"},
    ],
}

WEATHER_PROFILES = {
    "tropical": {
        "dry": [
            {"desc": "Sunny", "temp_c": 30, "rain_pct": 10},
            {"desc": "Sunny", "temp_c": 31, "rain_pct": 5},
            {"desc": "Partly cloudy", "temp_c": 29, "rain_pct": 20},
            {"desc": "Clear", "temp_c": 30, "rain_pct": 5},
            {"desc": "Sunny", "temp_c": 31, "rain_pct": 10},
            {"desc": "Sunny with light breeze", "temp_c": 29, "rain_pct": 15},
            {"desc": "Clear", "temp_c": 30, "rain_pct": 5},
        ],
        "wet": [
            {"desc": "Partly cloudy, afternoon showers", "temp_c": 28, "rain_pct": 60},
            {"desc": "Overcast with rain", "temp_c": 27, "rain_pct": 70},
            {"desc": "Morning sun, evening rain", "temp_c": 29, "rain_pct": 50},
            {"desc": "Tropical showers", "temp_c": 28, "rain_pct": 65},
            {"desc": "Partly cloudy", "temp_c": 29, "rain_pct": 40},
            {"desc": "Humid with showers", "temp_c": 28, "rain_pct": 55},
            {"desc": "Cloudy with breaks", "temp_c": 27, "rain_pct": 45},
        ],
    },
    "mediterranean": {
        "dry": [
            {"desc": "Sunny and warm", "temp_c": 28, "rain_pct": 5},
            {"desc": "Clear skies", "temp_c": 29, "rain_pct": 5},
            {"desc": "Sunny", "temp_c": 27, "rain_pct": 10},
            {"desc": "Hot and sunny", "temp_c": 30, "rain_pct": 5},
            {"desc": "Warm with light breeze", "temp_c": 28, "rain_pct": 10},
            {"desc": "Clear", "temp_c": 29, "rain_pct": 5},
            {"desc": "Sunny", "temp_c": 28, "rain_pct": 5},
        ],
        "wet": [
            {"desc": "Cool and overcast", "temp_c": 14, "rain_pct": 50},
            {"desc": "Light rain", "temp_c": 12, "rain_pct": 60},
            {"desc": "Partly cloudy", "temp_c": 15, "rain_pct": 35},
            {"desc": "Mild with showers", "temp_c": 13, "rain_pct": 55},
            {"desc": "Overcast", "temp_c": 14, "rain_pct": 45},
            {"desc": "Cool and cloudy", "temp_c": 12, "rain_pct": 40},
            {"desc": "Partly cloudy", "temp_c": 15, "rain_pct": 30},
        ],
    },
    "temperate": {
        "dry": [
            {"desc": "Pleasant and sunny", "temp_c": 22, "rain_pct": 15},
            {"desc": "Warm", "temp_c": 24, "rain_pct": 10},
            {"desc": "Partly cloudy", "temp_c": 21, "rain_pct": 25},
            {"desc": "Clear", "temp_c": 23, "rain_pct": 10},
            {"desc": "Sunny", "temp_c": 25, "rain_pct": 10},
            {"desc": "Mild and pleasant", "temp_c": 22, "rain_pct": 20},
            {"desc": "Warm with clouds", "temp_c": 23, "rain_pct": 15},
        ],
        "wet": [
            {"desc": "Cool and rainy", "temp_c": 10, "rain_pct": 65},
            {"desc": "Overcast", "temp_c": 8, "rain_pct": 55},
            {"desc": "Cold with rain", "temp_c": 6, "rain_pct": 70},
            {"desc": "Drizzle", "temp_c": 9, "rain_pct": 60},
            {"desc": "Cloudy", "temp_c": 11, "rain_pct": 50},
            {"desc": "Cool", "temp_c": 8, "rain_pct": 45},
            {"desc": "Overcast with breaks", "temp_c": 10, "rain_pct": 40},
        ],
    },
}

DEST_CLIMATE = {
    "bali": "tropical", "maldives": "tropical", "phuket": "tropical",
    "cancun": "tropical", "hawaii": "tropical", "honolulu": "tropical",
    "fiji": "tropical", "bangkok": "tropical", "colombo": "tropical",
    "paris": "temperate", "london": "temperate", "amsterdam": "temperate",
    "tokyo": "temperate", "seoul": "temperate", "zurich": "temperate",
    "reykjavik": "temperate",
    "santorini": "mediterranean", "rome": "mediterranean", "barcelona": "mediterranean",
    "athens": "mediterranean", "lisbon": "mediterranean", "marrakech": "mediterranean",
    "dubai": "mediterranean", "cape town": "mediterranean",
    "singapore": "tropical", "kuala lumpur": "tropical", "hanoi": "tropical",
    "ho chi minh city": "tropical", "luang prabang": "tropical",
    "sydney": "temperate", "nairobi": "temperate",
    "mumbai": "tropical", "delhi": "tropical",
    "istanbul": "mediterranean", "cairo": "mediterranean",
    "rio de janeiro": "tropical", "johannesburg": "temperate",
}

DRY_MONTHS = {
    "tropical": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
    "mediterranean": [4, 5, 6, 7, 8, 9, 10],
    "temperate": [4, 5, 6, 7, 8, 9],
}


def _seed_int(seed_str: str, max_val: int) -> int:
    h = int(hashlib.md5(seed_str.encode()).hexdigest()[:8], 16)
    return h % max_val


def _get_climate(destination: str) -> str:
    return DEST_CLIMATE.get(destination.lower().strip(), "temperate")


def _get_season(climate: str, month: int) -> str:
    return "dry" if month in DRY_MONTHS.get(climate, [5, 6, 7, 8]) else "wet"


@tool
def search_activities(destination: str, preferences: str = "", num_days: int = 5) -> str:
    """Search for activities and attractions at a destination. Returns activities organized
    by type (cultural, adventure, relaxation, food). Provide the destination, traveler
    preferences (comma-separated), and number of days."""
    dest_key = destination.lower().strip()
    pref_list = [p.strip().lower() for p in preferences.split(",") if p.strip()] if preferences else []

    activities = ACTIVITY_DB.get(dest_key)
    if not activities:
        activities = {}
        for cat, items in GENERIC_ACTIVITIES.items():
            customized = []
            for item in items:
                entry = dict(item)
                entry["name"] = entry["name"].replace("Local", destination.title())
                customized.append(entry)
            activities[cat] = customized

    if pref_list:
        pref_map = {
            "beach": ["adventure", "relaxation"],
            "culture": ["cultural"],
            "adventure": ["adventure"],
            "food": ["food"],
            "romantic": ["relaxation", "food"],
            "nature": ["adventure", "relaxation"],
            "spa": ["relaxation"],
            "snorkeling": ["adventure"],
            "diving": ["adventure"],
            "temples": ["cultural"],
            "history": ["cultural"],
            "nightlife": ["food", "adventure"],
            "shopping": ["cultural"],
        }
        boosted_cats = set()
        for p in pref_list:
            boosted_cats.update(pref_map.get(p, []))
        if not boosted_cats:
            boosted_cats = {"cultural", "adventure", "relaxation", "food"}
    else:
        boosted_cats = {"cultural", "adventure", "relaxation", "food"}

    result = {
        "destination": destination,
        "preferences": pref_list,
        "num_days": num_days,
        "activities_by_type": {},
        "total_unique_activities": 0,
    }

    for cat in ["cultural", "adventure", "relaxation", "food"]:
        items = activities.get(cat, [])
        if cat in boosted_cats:
            result["activities_by_type"][cat] = items
        else:
            result["activities_by_type"][cat] = items[:2]
        result["total_unique_activities"] += len(result["activities_by_type"][cat])

    return json.dumps(result, indent=2)


@tool
def get_weather_forecast(destination: str, start_date: str = "", num_days: int = 5) -> str:
    """Get a weather forecast for a destination. Returns daily weather conditions including
    temperature, description, and rain probability. Provide the destination city,
    start date (YYYY-MM-DD), and number of days."""
    dest_key = destination.lower().strip()
    climate = _get_climate(dest_key)

    month = 6
    if start_date:
        try:
            month = int(start_date.split("-")[1])
        except (IndexError, ValueError):
            pass

    season = _get_season(climate, month)
    weather_list = WEATHER_PROFILES.get(climate, WEATHER_PROFILES["temperate"]).get(season, [])

    seed_base = f"{dest_key}-{start_date}"
    forecast = []
    for day in range(num_days):
        idx = day % len(weather_list)
        w = weather_list[idx]
        temp_var = _seed_int(f"{seed_base}-temp-{day}", 5) - 2
        forecast.append({
            "day": day + 1,
            "description": w["desc"],
            "temperature_c": w["temp_c"] + temp_var,
            "temperature_f": round((w["temp_c"] + temp_var) * 9 / 5 + 32),
            "rain_probability_pct": w["rain_pct"],
            "outdoor_friendly": w["rain_pct"] < 40,
        })

    result = {
        "destination": destination,
        "climate": climate,
        "season": season,
        "start_date": start_date,
        "forecast": forecast,
    }
    return json.dumps(result, indent=2)
