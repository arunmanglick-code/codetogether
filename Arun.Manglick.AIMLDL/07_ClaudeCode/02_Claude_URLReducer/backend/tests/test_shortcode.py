import string

from app.services.shortcode import BASE62, generate_short_code


def test_generate_short_code_default_length():
    code = generate_short_code()
    assert len(code) == 7


def test_generate_short_code_custom_length():
    code = generate_short_code(length=8)
    assert len(code) == 8


def test_generate_short_code_characters_are_base62():
    code = generate_short_code()
    allowed = set(string.digits + string.ascii_letters)
    assert all(c in allowed for c in code)


def test_generate_short_code_uniqueness():
    codes = {generate_short_code() for _ in range(1000)}
    assert len(codes) == 1000
