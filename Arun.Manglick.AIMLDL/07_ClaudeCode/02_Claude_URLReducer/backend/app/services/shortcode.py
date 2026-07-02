import secrets
import string

BASE62 = string.digits + string.ascii_letters


def generate_short_code(length: int = 7) -> str:
    return "".join(secrets.choice(BASE62) for _ in range(length))
