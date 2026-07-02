import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

function ClicksChart({ clicks }) {
  const grouped = {};
  clicks.forEach((click) => {
    const date = new Date(click.clicked_at).toLocaleDateString();
    grouped[date] = (grouped[date] || 0) + 1;
  });

  const data = Object.entries(grouped)
    .map(([date, count]) => ({ date, clicks: count }))
    .sort((a, b) => new Date(a.date) - new Date(b.date));

  if (data.length === 0) return null;

  return (
    <div style={styles.container}>
      <h3 style={styles.title}>Clicks Over Time</h3>
      <ResponsiveContainer width="100%" height={250}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="date" fontSize={12} />
          <YAxis allowDecimals={false} fontSize={12} />
          <Tooltip />
          <Bar dataKey="clicks" fill="#4361ee" radius={[4, 4, 0, 0]} />
        </BarChart>
      </ResponsiveContainer>
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
    fontSize: "1rem",
    marginBottom: "1rem",
  },
};

export default ClicksChart;
