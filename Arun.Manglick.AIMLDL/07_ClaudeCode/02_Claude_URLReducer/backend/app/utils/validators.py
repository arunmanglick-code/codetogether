import ipaddress
from urllib.parse import urlparse

BLOCKED_SCHEMES = {"javascript", "data", "file", "ftp", "vbscript"}

PRIVATE_NETWORKS = [
    ipaddress.ip_network("127.0.0.0/8"),
    ipaddress.ip_network("10.0.0.0/8"),
    ipaddress.ip_network("172.16.0.0/12"),
    ipaddress.ip_network("192.168.0.0/16"),
    ipaddress.ip_network("169.254.0.0/16"),
    ipaddress.ip_network("::1/128"),
    ipaddress.ip_network("fc00::/7"),
    ipaddress.ip_network("fe80::/10"),
]


def is_safe_url(url: str) -> tuple[bool, str]:
    parsed = urlparse(url)

    if parsed.scheme.lower() not in ("http", "https"):
        return False, f"Scheme '{parsed.scheme}' is not allowed. Only http and https are permitted."

    if len(url) > 2048:
        return False, "URL must not exceed 2048 characters."

    hostname = parsed.hostname
    if not hostname:
        return False, "URL must have a valid hostname."

    try:
        addr = ipaddress.ip_address(hostname)
        for network in PRIVATE_NETWORKS:
            if addr in network:
                return False, "URLs pointing to private or reserved IP addresses are not allowed."
    except ValueError:
        pass

    return True, ""
