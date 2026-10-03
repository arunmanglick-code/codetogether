import hashlib
import json
from langchain_core.tools import tool

AIRPORT_DB = {
    "new york": {"code": "JFK", "name": "John F. Kennedy International Airport", "city": "New York"},
    "nyc": {"code": "JFK", "name": "John F. Kennedy International Airport", "city": "New York"},
    "los angeles": {"code": "LAX", "name": "Los Angeles International Airport", "city": "Los Angeles"},
    "la": {"code": "LAX", "name": "Los Angeles International Airport", "city": "Los Angeles"},
    "chicago": {"code": "ORD", "name": "O'Hare International Airport", "city": "Chicago"},
    "san francisco": {"code": "SFO", "name": "San Francisco International Airport", "city": "San Francisco"},
    "miami": {"code": "MIA", "name": "Miami International Airport", "city": "Miami"},
    "seattle": {"code": "SEA", "name": "Seattle-Tacoma International Airport", "city": "Seattle"},
    "boston": {"code": "BOS", "name": "Boston Logan International Airport", "city": "Boston"},
    "dallas": {"code": "DFW", "name": "Dallas/Fort Worth International Airport", "city": "Dallas"},
    "atlanta": {"code": "ATL", "name": "Hartsfield-Jackson Atlanta International Airport", "city": "Atlanta"},
    "denver": {"code": "DEN", "name": "Denver International Airport", "city": "Denver"},
    "london": {"code": "LHR", "name": "London Heathrow Airport", "city": "London"},
    "paris": {"code": "CDG", "name": "Charles de Gaulle Airport", "city": "Paris"},
    "tokyo": {"code": "NRT", "name": "Narita International Airport", "city": "Tokyo"},
    "dubai": {"code": "DXB", "name": "Dubai International Airport", "city": "Dubai"},
    "singapore": {"code": "SIN", "name": "Singapore Changi Airport", "city": "Singapore"},
    "hong kong": {"code": "HKG", "name": "Hong Kong International Airport", "city": "Hong Kong"},
    "bangkok": {"code": "BKK", "name": "Suvarnabhumi Airport", "city": "Bangkok"},
    "bali": {"code": "DPS", "name": "Ngurah Rai International Airport", "city": "Bali"},
    "denpasar": {"code": "DPS", "name": "Ngurah Rai International Airport", "city": "Bali"},
    "maldives": {"code": "MLE", "name": "Velana International Airport", "city": "Male"},
    "male": {"code": "MLE", "name": "Velana International Airport", "city": "Male"},
    "sydney": {"code": "SYD", "name": "Sydney Kingsford Smith Airport", "city": "Sydney"},
    "rome": {"code": "FCO", "name": "Leonardo da Vinci-Fiumicino Airport", "city": "Rome"},
    "barcelona": {"code": "BCN", "name": "Barcelona-El Prat Airport", "city": "Barcelona"},
    "istanbul": {"code": "IST", "name": "Istanbul Airport", "city": "Istanbul"},
    "cairo": {"code": "CAI", "name": "Cairo International Airport", "city": "Cairo"},
    "mumbai": {"code": "BOM", "name": "Chhatrapati Shivaji Maharaj International Airport", "city": "Mumbai"},
    "delhi": {"code": "DEL", "name": "Indira Gandhi International Airport", "city": "Delhi"},
    "new delhi": {"code": "DEL", "name": "Indira Gandhi International Airport", "city": "Delhi"},
    "seoul": {"code": "ICN", "name": "Incheon International Airport", "city": "Seoul"},
    "kuala lumpur": {"code": "KUL", "name": "Kuala Lumpur International Airport", "city": "Kuala Lumpur"},
    "luang prabang": {"code": "LPQ", "name": "Luang Prabang International Airport", "city": "Luang Prabang"},
    "cancun": {"code": "CUN", "name": "Cancun International Airport", "city": "Cancun"},
    "rio de janeiro": {"code": "GIG", "name": "Rio de Janeiro-Galeao International Airport", "city": "Rio de Janeiro"},
    "cape town": {"code": "CPT", "name": "Cape Town International Airport", "city": "Cape Town"},
    "nairobi": {"code": "NBO", "name": "Jomo Kenyatta International Airport", "city": "Nairobi"},
    "amsterdam": {"code": "AMS", "name": "Amsterdam Airport Schiphol", "city": "Amsterdam"},
    "zurich": {"code": "ZRH", "name": "Zurich Airport", "city": "Zurich"},
    "athens": {"code": "ATH", "name": "Athens International Airport", "city": "Athens"},
    "lisbon": {"code": "LIS", "name": "Lisbon Portela Airport", "city": "Lisbon"},
    "doha": {"code": "DOH", "name": "Hamad International Airport", "city": "Doha"},
    "johannesburg": {"code": "JNB", "name": "O.R. Tambo International Airport", "city": "Johannesburg"},
    "hanoi": {"code": "HAN", "name": "Noi Bai International Airport", "city": "Hanoi"},
    "ho chi minh city": {"code": "SGN", "name": "Tan Son Nhat International Airport", "city": "Ho Chi Minh City"},
    "phuket": {"code": "HKT", "name": "Phuket International Airport", "city": "Phuket"},
    "colombo": {"code": "CMB", "name": "Bandaranaike International Airport", "city": "Colombo"},
    "fiji": {"code": "NAN", "name": "Nadi International Airport", "city": "Nadi"},
    "honolulu": {"code": "HNL", "name": "Daniel K. Inouye International Airport", "city": "Honolulu"},
    "hawaii": {"code": "HNL", "name": "Daniel K. Inouye International Airport", "city": "Honolulu"},
    "santorini": {"code": "JTR", "name": "Santorini Airport", "city": "Santorini"},
    "reykjavik": {"code": "KEF", "name": "Keflavik International Airport", "city": "Reykjavik"},
    "marrakech": {"code": "RAK", "name": "Marrakech Menara Airport", "city": "Marrakech"},
}

REGION_MAP = {
    "DPS": "southeast_asia", "SIN": "southeast_asia", "BKK": "southeast_asia",
    "HKG": "east_asia", "NRT": "east_asia", "ICN": "east_asia",
    "KUL": "southeast_asia", "LPQ": "southeast_asia", "HKT": "southeast_asia",
    "HAN": "southeast_asia", "SGN": "southeast_asia",
    "MLE": "indian_ocean", "CMB": "indian_ocean", "NAN": "pacific",
    "BOM": "south_asia", "DEL": "south_asia",
    "CDG": "europe", "LHR": "europe", "FCO": "europe", "BCN": "europe",
    "AMS": "europe", "ZRH": "europe", "ATH": "europe", "LIS": "europe",
    "IST": "europe", "JTR": "europe", "KEF": "europe",
    "DXB": "middle_east", "DOH": "middle_east", "CAI": "middle_east",
    "SYD": "oceania", "HNL": "pacific",
    "CUN": "central_america", "GIG": "south_america",
    "CPT": "africa", "NBO": "africa", "JNB": "africa", "RAK": "africa",
    "JFK": "north_america", "LAX": "north_america", "ORD": "north_america",
    "SFO": "north_america", "MIA": "north_america", "SEA": "north_america",
    "BOS": "north_america", "DFW": "north_america", "ATL": "north_america",
    "DEN": "north_america",
}

AIRLINES_BY_REGION = {
    "southeast_asia": [
        ("Singapore Airlines", "SQ", 4.7, 1.15),
        ("Thai Airways", "TG", 4.3, 0.95),
        ("Cathay Pacific", "CX", 4.5, 1.05),
        ("ANA", "NH", 4.6, 1.10),
    ],
    "east_asia": [
        ("ANA", "NH", 4.6, 1.10),
        ("Japan Airlines", "JL", 4.5, 1.08),
        ("Cathay Pacific", "CX", 4.5, 1.05),
        ("Korean Air", "KE", 4.4, 1.00),
    ],
    "indian_ocean": [
        ("Emirates", "EK", 4.8, 1.20),
        ("Singapore Airlines", "SQ", 4.7, 1.15),
        ("Qatar Airways", "QR", 4.7, 1.18),
        ("Sri Lankan Airlines", "UL", 4.0, 0.80),
    ],
    "south_asia": [
        ("Emirates", "EK", 4.8, 1.20),
        ("Qatar Airways", "QR", 4.7, 1.18),
        ("Air India", "AI", 3.8, 0.75),
        ("Etihad Airways", "EY", 4.5, 1.10),
    ],
    "europe": [
        ("Delta Air Lines", "DL", 4.2, 1.00),
        ("Air France", "AF", 4.4, 1.05),
        ("British Airways", "BA", 4.3, 1.08),
        ("United Airlines", "UA", 4.0, 0.90),
    ],
    "middle_east": [
        ("Emirates", "EK", 4.8, 1.20),
        ("Qatar Airways", "QR", 4.7, 1.18),
        ("Turkish Airlines", "TK", 4.3, 0.85),
        ("Etihad Airways", "EY", 4.5, 1.10),
    ],
    "oceania": [
        ("Qantas", "QF", 4.4, 1.12),
        ("United Airlines", "UA", 4.0, 0.95),
        ("Delta Air Lines", "DL", 4.2, 1.00),
        ("Air New Zealand", "NZ", 4.5, 1.08),
    ],
    "pacific": [
        ("Hawaiian Airlines", "HA", 4.2, 0.95),
        ("Fiji Airways", "FJ", 4.1, 0.90),
        ("United Airlines", "UA", 4.0, 0.95),
        ("Qantas", "QF", 4.4, 1.12),
    ],
    "central_america": [
        ("American Airlines", "AA", 4.0, 0.90),
        ("United Airlines", "UA", 4.0, 0.92),
        ("Delta Air Lines", "DL", 4.2, 0.95),
        ("JetBlue", "B6", 4.1, 0.85),
    ],
    "south_america": [
        ("LATAM Airlines", "LA", 4.1, 0.90),
        ("American Airlines", "AA", 4.0, 0.95),
        ("United Airlines", "UA", 4.0, 0.95),
        ("Delta Air Lines", "DL", 4.2, 1.00),
    ],
    "africa": [
        ("Ethiopian Airlines", "ET", 4.0, 0.85),
        ("Emirates", "EK", 4.8, 1.20),
        ("Turkish Airlines", "TK", 4.3, 0.90),
        ("South African Airways", "SA", 3.9, 0.88),
    ],
    "north_america": [
        ("Delta Air Lines", "DL", 4.2, 0.90),
        ("United Airlines", "UA", 4.0, 0.88),
        ("American Airlines", "AA", 4.0, 0.85),
        ("JetBlue", "B6", 4.1, 0.80),
    ],
}

BASE_PRICES = {
    ("north_america", "europe"): 650,
    ("north_america", "southeast_asia"): 950,
    ("north_america", "east_asia"): 900,
    ("north_america", "indian_ocean"): 1100,
    ("north_america", "south_asia"): 850,
    ("north_america", "middle_east"): 800,
    ("north_america", "oceania"): 1000,
    ("north_america", "pacific"): 600,
    ("north_america", "central_america"): 350,
    ("north_america", "south_america"): 700,
    ("north_america", "africa"): 950,
    ("north_america", "north_america"): 250,
    ("europe", "southeast_asia"): 700,
    ("europe", "east_asia"): 650,
    ("europe", "indian_ocean"): 800,
    ("europe", "south_asia"): 600,
    ("europe", "middle_east"): 450,
    ("europe", "oceania"): 1100,
    ("europe", "africa"): 500,
    ("europe", "europe"): 200,
}

FLIGHT_DURATIONS = {
    ("north_america", "europe"): (7, 9),
    ("north_america", "southeast_asia"): (18, 24),
    ("north_america", "east_asia"): (13, 17),
    ("north_america", "indian_ocean"): (19, 25),
    ("north_america", "south_asia"): (16, 22),
    ("north_america", "middle_east"): (12, 16),
    ("north_america", "oceania"): (17, 22),
    ("north_america", "pacific"): (8, 12),
    ("north_america", "central_america"): (3, 6),
    ("north_america", "south_america"): (9, 14),
    ("north_america", "africa"): (14, 20),
    ("north_america", "north_america"): (2, 6),
    ("europe", "southeast_asia"): (11, 15),
    ("europe", "east_asia"): (10, 14),
    ("europe", "indian_ocean"): (10, 14),
    ("europe", "south_asia"): (8, 12),
    ("europe", "middle_east"): (5, 8),
    ("europe", "oceania"): (20, 26),
    ("europe", "africa"): (6, 10),
    ("europe", "europe"): (1, 4),
}

HUB_AIRPORTS = {
    "southeast_asia": ["SIN", "BKK", "HKG"],
    "east_asia": ["NRT", "ICN", "HKG"],
    "indian_ocean": ["DXB", "SIN", "DOH"],
    "south_asia": ["DXB", "DOH", "SIN"],
    "europe": [],
    "middle_east": ["IST", "DXB"],
    "oceania": ["LAX", "SYD", "AKL"],
    "pacific": ["LAX", "HNL"],
    "central_america": ["MIA", "DFW"],
    "south_america": ["MIA", "GIG"],
    "africa": ["DXB", "IST", "ADD"],
    "north_america": [],
}


def _seed_int(seed_str: str, max_val: int) -> int:
    h = int(hashlib.md5(seed_str.encode()).hexdigest()[:8], 16)
    return h % max_val


def _get_region(airport_code: str) -> str:
    return REGION_MAP.get(airport_code, "europe")


def _generate_flights(origin_code: str, dest_code: str, travelers: int) -> list[dict]:
    origin_region = _get_region(origin_code)
    dest_region = _get_region(dest_code)

    route_key = (origin_region, dest_region)
    reverse_key = (dest_region, origin_region)
    base_price = BASE_PRICES.get(route_key, BASE_PRICES.get(reverse_key, 750))
    duration_range = FLIGHT_DURATIONS.get(route_key, FLIGHT_DURATIONS.get(reverse_key, (8, 16)))

    airlines = AIRLINES_BY_REGION.get(dest_region, AIRLINES_BY_REGION["europe"])

    seed_base = f"{origin_code}-{dest_code}"
    flights = []

    for i, (airline_name, airline_code, rating, price_mult) in enumerate(airlines[:4]):
        seed = f"{seed_base}-{i}"
        price_var = (_seed_int(seed + "price", 30) - 15) / 100.0
        price_pp = int(base_price * price_mult * (1 + price_var))

        min_dur, max_dur = duration_range
        dur_range = max_dur - min_dur
        duration_h = min_dur + (_seed_int(seed + "dur", max(dur_range * 4, 1))) / 4.0
        duration_h = round(duration_h, 1)
        hours = int(duration_h)
        minutes = int((duration_h - hours) * 60)

        is_direct = origin_region == dest_region or (duration_range[1] - duration_range[0] <= 3)
        if is_direct and _seed_int(seed + "direct", 3) == 0:
            layovers = 0
            hub = None
        else:
            hubs = HUB_AIRPORTS.get(dest_region, [])
            valid_hubs = [h for h in hubs if h != origin_code and h != dest_code]
            if valid_hubs:
                hub = valid_hubs[_seed_int(seed + "hub", len(valid_hubs))]
                layovers = 1
            elif origin_region == dest_region:
                layovers = 0
                hub = None
            else:
                layovers = 1
                hub = "DXB" if dest_region in ("indian_ocean", "south_asia", "africa") else "SIN"
                if hub == origin_code or hub == dest_code:
                    hub = "DOH"

        flt_num = 100 + _seed_int(seed + "flt", 899)
        dep_hour = 6 + _seed_int(seed + "dep", 16)
        dep_min = _seed_int(seed + "min", 4) * 15

        if layovers == 0:
            route_str = f"{origin_code} -> {dest_code}"
            flt_str = f"{airline_code} {flt_num}"
        else:
            flt_num2 = 100 + _seed_int(seed + "flt2", 899)
            route_str = f"{origin_code} -> {hub} -> {dest_code}"
            flt_str = f"{airline_code} {flt_num} / {airline_code} {flt_num2}"

        arr_hours = dep_hour + hours
        days_added = arr_hours // 24
        arr_hour = arr_hours % 24
        arr_min = (dep_min + minutes) % 60
        day_suffix = f" +{days_added}" if days_added > 0 else ""

        flights.append({
            "airline": airline_name,
            "flight_number": flt_str,
            "route": route_str,
            "departure": f"{dep_hour:02d}:{dep_min:02d}",
            "arrival": f"{arr_hour:02d}:{arr_min:02d}{day_suffix}",
            "duration": f"{hours}h {minutes:02d}m",
            "layovers": layovers,
            "price_per_person": price_pp,
            "total_price": price_pp * travelers,
            "class": "Economy",
            "rating": rating,
        })

    flights.sort(key=lambda f: (-f["rating"], f["price_per_person"]))
    return flights[:3]


@tool
def search_flights(origin: str, destination: str, travelers: int = 1) -> str:
    """Search for flight options between two cities. Returns top 3 flights ranked by value.
    Provide city names (e.g. 'New York', 'Bali') — airport codes are resolved automatically."""
    origin_entry = AIRPORT_DB.get(origin.lower().strip())
    dest_entry = AIRPORT_DB.get(destination.lower().strip())

    origin_code = origin_entry["code"] if origin_entry else origin[:3].upper()
    dest_code = dest_entry["code"] if dest_entry else destination[:3].upper()

    flights = _generate_flights(origin_code, dest_code, travelers)

    result = {
        "origin": {"city": origin, "airport_code": origin_code},
        "destination": {"city": destination, "airport_code": dest_code},
        "travelers": travelers,
        "flights": flights,
    }
    return json.dumps(result, indent=2)


@tool
def get_airport_code(city: str) -> str:
    """Look up the IATA airport code for a city. Returns the code, airport name, and city."""
    key = city.lower().strip()
    entry = AIRPORT_DB.get(key)
    if entry:
        return json.dumps(entry)
    for db_key, info in AIRPORT_DB.items():
        if key in db_key or db_key in key:
            return json.dumps(info)
    return json.dumps({
        "code": city[:3].upper(),
        "name": f"{city} Airport (estimated)",
        "city": city,
        "note": "Airport not in database; code is an estimate. Results may vary.",
    })
