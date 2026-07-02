const API_BASE = import.meta.env.VITE_API_BASE_URL || "";

async function request(path, options = {}) {
  const res = await fetch(`${API_BASE}${path}`, {
    headers: { "Content-Type": "application/json", ...options.headers },
    ...options,
  });
  if (!res.ok) {
    const err = await res.json().catch(() => ({ message: "Request failed" }));
    throw new Error(err.message || err.detail || `HTTP ${res.status}`);
  }
  return res.json();
}

export function shortenUrl(url) {
  return request("/api/shorten", {
    method: "POST",
    body: JSON.stringify({ url }),
  });
}

export function getUrls() {
  return request("/api/urls");
}

export function getUrlStats(shortCode) {
  return request(`/api/urls/${shortCode}/stats`);
}
