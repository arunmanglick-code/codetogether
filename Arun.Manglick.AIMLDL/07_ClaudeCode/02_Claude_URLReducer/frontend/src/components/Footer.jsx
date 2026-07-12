function Footer() {
  return (
    <footer style={styles.footer}>
      <p>LinkForge &mdash; Crafting cleaner links, faster</p>
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
