import { useState } from "react";
import { shortenUrl } from "../api/client";

export function useShorten() {
  const [result, setResult] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const shorten = async (url) => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await shortenUrl(url);
      setResult(data);
    } catch (err) {
      setError(err.message);
      setResult(null);
    } finally {
      setIsLoading(false);
    }
  };

  return { result, isLoading, error, shorten };
}
