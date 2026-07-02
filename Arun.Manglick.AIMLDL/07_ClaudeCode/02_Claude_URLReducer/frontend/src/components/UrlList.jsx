import { Link } from "react-router-dom";

function UrlList({ urls }) {
  if (!urls || urls.length === 0) {
    return (
      <div style={styles.empty}>
        <p>No shortened URLs yet. Create your first one above!</p>
      </div>
    );
  }

  return (
    <div style={styles.container}>
      <h3 style={styles.title}>Recent URLs</h3>
      <table style={styles.table}>
        <thead>
          <tr>
            <th style={styles.th}>Short Code</th>
            <th style={styles.th}>Original URL</th>
            <th style={styles.th}>Clicks</th>
            <th style={styles.th}>Analytics</th>
          </tr>
        </thead>
        <tbody>
          {urls.map((url) => (
            <tr key={url.short_code}>
              <td style={styles.td}>
                <code>{url.short_code}</code>
              </td>
              <td style={{ ...styles.td, ...styles.urlCell }}>
                {url.original_url.length > 50
                  ? url.original_url.slice(0, 50) + "..."
                  : url.original_url}
              </td>
              <td style={styles.td}>{url.click_count}</td>
              <td style={styles.td}>
                <Link to={`/analytics/${url.short_code}`}>View</Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const styles = {
  container: {
    backgroundColor: "#fff",
    padding: "1.5rem",
    borderRadius: "8px",
    boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
  },
  title: {
    marginBottom: "1rem",
    fontSize: "1.125rem",
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
    fontSize: "0.875rem",
  },
  urlCell: {
    maxWidth: "300px",
    overflow: "hidden",
    textOverflow: "ellipsis",
    whiteSpace: "nowrap",
  },
  empty: {
    textAlign: "center",
    padding: "2rem",
    color: "#888",
    backgroundColor: "#fff",
    borderRadius: "8px",
    boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
  },
};

export default UrlList;
