import { useCallback, useEffect, useState } from "react";
import ShortenForm from "../components/ShortenForm";
import ShortenedResult from "../components/ShortenedResult";
import UrlList from "../components/UrlList";
import { useShorten } from "../hooks/useShorten";
import { getUrls } from "../api/client";

function HomePage() {
  const { result, isLoading, error, shorten } = useShorten();
  const [urls, setUrls] = useState([]);

  const fetchUrls = useCallback(async () => {
    try {
      const data = await getUrls();
      setUrls(data);
    } catch {
      // silently fail — list is non-critical
    }
  }, []);

  useEffect(() => {
    fetchUrls();
  }, [fetchUrls]);

  useEffect(() => {
    if (result) fetchUrls();
  }, [result, fetchUrls]);

  return (
    <div>
      <ShortenForm onShorten={shorten} isLoading={isLoading} />
      {error && (
        <div style={styles.error}>
          {error}
        </div>
      )}
      <ShortenedResult result={result} />
      <UrlList urls={urls} />
    </div>
  );
}

const styles = {
  error: {
    backgroundColor: "#ffebee",
    color: "#c62828",
    padding: "1rem",
    borderRadius: "8px",
    marginBottom: "1rem",
    border: "1px solid #ef9a9a",
  },
};

export default HomePage;
