import hashlib
import json
from langchain_core.tools import tool

HOTEL_CHAINS = {
    "luxury": [
        ("Four Seasons", 5, 9.3, ["Spa", "Pool", "Fine dining", "Concierge", "Gym", "Free WiFi"]),
        ("Ritz-Carlton", 5, 9.1, ["Spa", "Pool", "Restaurant", "Bar", "Gym", "Free WiFi", "Club lounge"]),
        ("St. Regis", 5, 9.2, ["Butler service", "Spa", "Pool", "Fine dining", "Free WiFi"]),
        ("Mandarin Oriental", 5, 9.0, ["Spa", "Pool", "Michelin restaurant", "Bar", "Gym"]),
    ],
    "upscale": [
        ("Hyatt Regency", 4, 8.6, ["Pool", "Restaurant", "Gym", "Free WiFi", "Business center"]),
        ("Marriott", 4, 8.4, ["Pool", "Restaurant", "Gym", "Free WiFi", "Lounge"]),
        ("Hilton", 4, 8.3, ["Pool", "Restaurant", "Gym", "Free WiFi", "Bar"]),
        ("Sheraton", 4, 8.2, ["Pool", "Restaurant", "Gym", "Free WiFi", "Concierge"]),
    ],
    "midrange": [
        ("Courtyard by Marriott", 3, 8.0, ["Free WiFi", "Restaurant", "Gym", "Business center"]),
        ("Holiday Inn", 3, 7.8, ["Free WiFi", "Restaurant", "Pool", "Parking"]),
        ("Novotel", 3, 7.9, ["Free WiFi", "Restaurant", "Pool", "Kids area"]),
        ("Best Western", 3, 7.6, ["Free WiFi", "Breakfast", "Parking", "Fitness room"]),
    ],
    "boutique": [
        ("Local Boutique Hotel", 4, 8.7, ["Free WiFi", "Unique decor", "Rooftop bar", "Breakfast"]),
        ("Heritage Inn", 3, 8.5, ["Free WiFi", "Garden", "Local cuisine", "Cultural tours"]),
    ],
}

REGION_PRICE_MULTIPLIER = {
    "southeast_asia": 0.55,
    "east_asia": 1.10,
    "indian_ocean": 1.30,
    "south_asia": 0.50,
    "europe": 1.00,
    "middle_east": 0.90,
    "oceania": 1.05,
    "pacific": 0.85,
    "central_america": 0.65,
    "south_america": 0.60,
    "africa": 0.70,
    "north_america": 1.00,
}

BASE_NIGHTLY_PRICES = {
    5: 350,
    4: 180,
    3: 95,
}

DEST_LOCATION_MAP = {
    "bali": ["Seminyak Beach", "Ubud Center", "Nusa Dua", "Canggu"],
    "maldives": ["North Male Atoll", "South Ari Atoll", "Baa Atoll", "Maafushi Island"],
    "paris": ["Le Marais", "Saint-Germain-des-Pres", "Champs-Elysees", "Montmartre"],
    "tokyo": ["Shinjuku", "Shibuya", "Ginza", "Asakusa"],
    "london": ["Westminster", "Kensington", "Covent Garden", "Shoreditch"],
    "singapore": ["Marina Bay", "Orchard Road", "Chinatown", "Sentosa"],
    "bangkok": ["Sukhumvit", "Silom", "Old City", "Riverside"],
    "dubai": ["Downtown Dubai", "Palm Jumeirah", "Dubai Marina", "Deira"],
    "rome": ["Centro Storico", "Trastevere", "Monti", "Vatican Area"],
    "barcelona": ["Gothic Quarter", "Eixample", "Barceloneta", "Gracia"],
    "sydney": ["Circular Quay", "Darling Harbour", "Bondi Beach", "Surry Hills"],
    "cancun": ["Hotel Zone", "Downtown Cancun", "Playa Mujeres", "Puerto Morelos"],
    "honolulu": ["Waikiki", "Ala Moana", "North Shore", "Diamond Head"],
    "santorini": ["Oia", "Fira", "Imerovigli", "Kamari"],
    "amsterdam": ["Canal Ring", "Jordaan", "De Pijp", "Museum Quarter"],
    "cape town": ["V&A Waterfront", "Camps Bay", "City Bowl", "Sea Point"],
}

DEST_ATTRACTION_MAP = {
    "bali": "Uluwatu Temple, Sacred Monkey Forest, Tegallalang Rice Terraces",
    "maldives": "snorkeling reefs, Male Fish Market, bioluminescent beaches",
    "paris": "Eiffel Tower, Louvre Museum, Notre-Dame",
    "tokyo": "Shibuya Crossing, Meiji Shrine, Senso-ji Temple",
    "london": "Big Ben, Tower of London, British Museum",
    "singapore": "Gardens by the Bay, Marina Bay Sands, Chinatown",
    "bangkok": "Grand Palace, Wat Arun, Chatuchak Market",
    "dubai": "Burj Khalifa, Dubai Mall, Palm Jumeirah",
    "rome": "Colosseum, Vatican, Trevi Fountain",
    "barcelona": "Sagrada Familia, Park Guell, La Rambla",
    "sydney": "Opera House, Harbour Bridge, Bondi Beach",
    "cancun": "Chichen Itza, Isla Mujeres, Xcaret Park",
}

REVIEW_TEMPLATES = {
    5: [
        "Exceptional luxury experience. Staff went above and beyond. The {feature} was world-class.",
        "Stunning property with impeccable service. The {feature} alone is worth the stay.",
        "Absolutely breathtaking. From the {feature} to the dining, everything was perfection.",
    ],
    4: [
        "Very comfortable stay with excellent {feature}. Great value for the quality.",
        "Modern and well-maintained. The {feature} was a highlight. Would recommend.",
        "Solid upscale experience. {feature} impressed us. Location was convenient.",
    ],
    3: [
        "Good value for money. {feature} was decent. Clean and comfortable rooms.",
        "Nice budget-friendly option. {feature} was a plus. Friendly staff.",
        "Perfectly adequate stay. {feature} worked well. Good location for the price.",
    ],
}


def _seed_int(seed_str: str, max_val: int) -> int:
    h = int(hashlib.md5(seed_str.encode()).hexdigest()[:8], 16)
    return h % max_val


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


def _generate_hotels(destination: str, nights: int, budget_per_night: float | None = None) -> list[dict]:
    dest_key = destination.lower().strip()
    region = _get_dest_region(destination)
    price_mult = REGION_PRICE_MULTIPLIER.get(region, 1.0)

    locations = DEST_LOCATION_MAP.get(dest_key, [f"{destination} Center", f"{destination} Beach", f"Old {destination}", f"Downtown {destination}"])
    attractions = DEST_ATTRACTION_MAP.get(dest_key, f"local attractions in {destination}")

    tiers = ["luxury", "upscale", "midrange"]
    if budget_per_night and budget_per_night < 120:
        tiers = ["midrange", "boutique", "upscale"]
    elif budget_per_night and budget_per_night > 300:
        tiers = ["luxury", "upscale", "boutique"]

    hotels = []
    for tier_idx, tier in enumerate(tiers):
        chain_options = HOTEL_CHAINS.get(tier, HOTEL_CHAINS["midrange"])
        seed = f"{dest_key}-{tier}-{tier_idx}"
        chain_idx = _seed_int(seed, len(chain_options))
        chain_name, stars, base_rating, amenities = chain_options[chain_idx]

        if chain_name in ("Local Boutique Hotel", "Heritage Inn"):
            hotel_name = f"{chain_name} {destination.title()}"
        else:
            hotel_name = f"{chain_name} {destination.title()}"

        base_price = BASE_NIGHTLY_PRICES.get(stars, 150)
        price_per_night = int(base_price * price_mult * (1 + (_seed_int(seed + "pv", 20) - 10) / 100.0))

        loc_idx = _seed_int(seed + "loc", len(locations))
        location = locations[loc_idx]

        rating_var = (_seed_int(seed + "rat", 6) - 3) / 10.0
        guest_rating = round(min(10.0, max(6.0, base_rating + rating_var)), 1)

        proximity_min = 5 + _seed_int(seed + "prox", 25)

        hotels.append({
            "name": hotel_name,
            "star_rating": stars,
            "location": location,
            "price_per_night": price_per_night,
            "total_price": price_per_night * nights,
            "nights": nights,
            "amenities": amenities,
            "guest_rating": guest_rating,
            "proximity_to_attractions": f"{proximity_min} min to {attractions.split(',')[0].strip()}",
        })

    hotels.sort(key=lambda h: (-h["guest_rating"], h["price_per_night"]))
    return hotels


@tool
def search_hotels(destination: str, nights: int = 5, budget_per_night: float = 0) -> str:
    """Search for hotel options in a destination. Returns top 3 hotels across different price tiers.
    Provide the destination city name, number of nights, and optional budget per night in USD."""
    budget = budget_per_night if budget_per_night > 0 else None
    hotels = _generate_hotels(destination, nights, budget)

    result = {
        "destination": destination,
        "nights": nights,
        "budget_filter": f"${budget_per_night}/night" if budget else "No budget filter",
        "hotels": hotels,
    }
    return json.dumps(result, indent=2)


@tool
def get_hotel_reviews(hotel_name: str, destination: str) -> str:
    """Get guest reviews for a specific hotel. Provide the hotel name and destination city."""
    seed = f"{hotel_name}-{destination}".lower()

    star_guess = 4
    name_lower = hotel_name.lower()
    if any(lux in name_lower for lux in ["four seasons", "ritz", "st. regis", "mandarin"]):
        star_guess = 5
    elif any(mid in name_lower for mid in ["courtyard", "holiday inn", "novotel", "best western", "heritage"]):
        star_guess = 3

    templates = REVIEW_TEMPLATES.get(star_guess, REVIEW_TEMPLATES[4])
    features = ["pool", "spa", "breakfast", "location", "service", "rooms", "view", "restaurant"]

    reviews = []
    for i in range(3):
        tmpl_idx = _seed_int(seed + f"tmpl{i}", len(templates))
        feat_idx = _seed_int(seed + f"feat{i}", len(features))
        text = templates[tmpl_idx].format(feature=features[feat_idx])
        rating = round(7.0 + _seed_int(seed + f"rev{i}", 30) / 10.0, 1)
        reviews.append({
            "reviewer": f"Traveler_{_seed_int(seed + f'name{i}', 9000) + 1000}",
            "rating": min(10.0, rating),
            "text": text,
        })

    overall = round(sum(r["rating"] for r in reviews) / len(reviews), 1)

    result = {
        "hotel": hotel_name,
        "destination": destination,
        "overall_rating": overall,
        "review_count": 50 + _seed_int(seed + "count", 450),
        "sample_reviews": reviews,
    }
    return json.dumps(result, indent=2)
