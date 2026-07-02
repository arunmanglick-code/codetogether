import { useParams, Link } from "react-router-dom";
import { useAnalytics } from "../hooks/useAnalytics";
import AnalyticsDashboard from "../components/AnalyticsDashboard";

function AnalyticsPage() {
  const { shortCode } = useParams();
  const { stats, isLoading, error } = useAnalytics(shortCode);

  if (isLoading) {
    return <p style={{ textAlign: "center", padding: "2rem" }}>Loading analytics...</p>;
  }

  if (error) {
    return (
      <div style={styles.error}>
        <p>{error}</p>
        <Link to="/">Back to Home</Link>
      </div>
    );
  }

  if (!stats) {
    return (
      <div style={styles.error}>
        <p>URL not found.</p>
        <Link to="/">Back to Home</Link>
      </div>
    );
  }

  return (
    <div>
      <Link to="/" style={styles.backLink}>
        &larr; Back to Home
      </Link>
      <AnalyticsDashboard stats={stats} />
    </div>
  );
}

const styles = {
  error: {
    textAlign: "center",
    padding: "2rem",
    backgroundColor: "#fff",
    borderRadius: "8px",
    boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
  },
  backLink: {
    display: "inline-block",
    marginBottom: "1rem",
    color: "#4361ee",
  },
};

export default AnalyticsPage;
