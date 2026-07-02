function Footer() {
  return (
    <footer style={styles.footer}>
      <p>URL Reducer &mdash; Built with FastAPI + React</p>
    </footer>
  );
}

const styles = {
  footer: {
    textAlign: "center",
    padding: "1rem",
    color: "#666",
    fontSize: "0.875rem",
    borderTop: "1px solid #ddd",
  },
};

export default Footer;
