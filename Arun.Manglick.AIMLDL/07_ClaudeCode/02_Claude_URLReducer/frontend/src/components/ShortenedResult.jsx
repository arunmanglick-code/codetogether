import { useState } from "react";

function ShortenedResult({ result }) {
  const [copied, setCopied] = useState(false);

  if (!result) return null;

  const handleCopy = async () => {
    await navigator.clipboard.writeText(result.short_url);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div style={styles.container}>
      <p style={styles.label}>Shortened URL:</p>
      <div style={styles.resultRow}>
        <a
          href={result.short_url}
          target="_blank"
          rel="noopener noreferrer"
          style={styles.link}
        >
          {result.short_url}
        </a>
        <button onClick={handleCopy} style={styles.copyBtn}>
          {copied ? "Copied!" : "Copy"}
        </button>
      </div>
      <p style={styles.original}>
        Original: {result.original_url}
      </p>
    </div>
  );
}

const styles = {
  container: {
    backgroundColor: "#e8f5e9",
    padding: "1.5rem",
    borderRadius: "8px",
    marginBottom: "1.5rem",
    border: "1px solid #a5d6a7",
  },
  label: {
    fontWeight: "bold",
    marginBottom: "0.5rem",
    color: "#2e7d32",
  },
  resultRow: {
    display: "flex",
    alignItems: "center",
    gap: "0.75rem",
    marginBottom: "0.5rem",
  },
  link: {
    fontSize: "1.125rem",
    fontWeight: "bold",
    color: "#1b5e20",
    wordBreak: "break-all",
  },
  copyBtn: {
    padding: "0.4rem 1rem",
    fontSize: "0.875rem",
    backgroundColor: "#43a047",
    color: "#fff",
    border: "none",
    borderRadius: "4px",
    cursor: "pointer",
    whiteSpace: "nowrap",
  },
  original: {
    fontSize: "0.875rem",
    color: "#555",
    wordBreak: "break-all",
  },
};

export default ShortenedResult;
