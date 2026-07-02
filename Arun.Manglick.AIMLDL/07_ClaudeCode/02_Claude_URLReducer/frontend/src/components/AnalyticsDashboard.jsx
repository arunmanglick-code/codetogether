import ClicksChart from "./ClicksChart";

function AnalyticsDashboard({ stats }) {
  return (
    <div>
      <div style={styles.summary}>
        <h2 style={styles.title}>Analytics: {stats.short_code}</h2>
        <p style={styles.original}>
          <strong>Original URL:</strong>{" "}
          <a href={stats.original_url} target="_blank" rel="noopener noreferrer">
            {stats.original_url}
          </a>
        </p>
        <div style={styles.statsRow}>
          <div style={styles.statBox}>
            <span style={styles.statValue}>{stats.click_count}</span>
            <span style={styles.statLabel}>Total Clicks</span>
          </div>
          <div style={styles.statBox}>
            <span style={styles.statValue}>
              {new Date(stats.created_at).toLocaleDateString()}
            </span>
            <span style={styles.statLabel}>Created</span>
          </div>
        </div>
      </div>

      {stats.clicks && stats.clicks.length > 0 && (
        <>
          <ClicksChart clicks={stats.clicks} />
          <div style={styles.tableContainer}>
            <h3 style={styles.sectionTitle}>Click Log</h3>
            <table style={styles.table}>
              <thead>
                <tr>
                  <th style={styles.th}>Time</th>
                  <th style={styles.th}>Referrer</th>
                  <th style={styles.th}>User Agent</th>
                </tr>
              </thead>
              <tbody>
                {stats.clicks.map((click, i) => (
                  <tr key={i}>
                    <td style={styles.td}>
                      {new Date(click.clicked_at).toLocaleString()}
                    </td>
                    <td style={styles.td}>{click.referrer || "Direct"}</td>
                    <td style={{ ...styles.td, ...styles.uaCell }}>
                      {click.user_agent
                        ? click.user_agent.slice(0, 60) + (click.user_agent.length > 60 ? "..." : "")
                        : "Unknown"}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
    </div>
  );
}

const styles = {
  summary: {
    backgroundColor: "#fff",
    padding: "2rem",
    borderRadius: "8px",
    boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
    marginBottom: "1.5rem",
  },
  title: {
    fontSize: "1.25rem",
    marginBottom: "0.75rem",
  },
  original: {
    fontSize: "0.875rem",
    color: "#555",
    marginBottom: "1rem",
    wordBreak: "break-all",
  },
  statsRow: {
    display: "flex",
    gap: "1.5rem",
  },
  statBox: {
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    padding: "1rem 2rem",
    backgroundColor: "#f5f5f5",
    borderRadius: "8px",
  },
  statValue: {
    fontSize: "1.5rem",
    fontWeight: "bold",
    color: "#1a1a2e",
  },
  statLabel: {
    fontSize: "0.75rem",
    color: "#888",
    textTransform: "uppercase",
    marginTop: "0.25rem",
  },
  tableContainer: {
    backgroundColor: "#fff",
    padding: "1.5rem",
    borderRadius: "8px",
    boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
    marginTop: "1.5rem",
  },
  sectionTitle: {
    fontSize: "1rem",
    marginBottom: "1rem",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
  },
  th: {
    textAlign: "left",
    padding: "0.75rem",
    borderBottom: "2px solid #eee",
    fontSize: "0.875rem",
    color: "#666",
  },
  td: {
    padding: "0.75rem",
    borderBottom: "1px solid #eee",
    fontSize: "0.8rem",
  },
  uaCell: {
    maxWidth: "250px",
    overflow: "hidden",
    textOverflow: "ellipsis",
    whiteSpace: "nowrap",
  },
};

export default AnalyticsDashboard;
