import { Link } from "react-router-dom";

function Header() {
  return (
    <header style={styles.header}>
      <div style={styles.container}>
        <Link to="/" style={styles.logo}>
          URL Reducer
        </Link>
        <nav>
          <Link to="/" style={styles.navLink}>
            Home
          </Link>
        </nav>
      </div>
    </header>
  );
}

const styles = {
  header: {
    backgroundColor: "#1a1a2e",
    color: "#fff",
    padding: "1rem 0",
  },
  container: {
    maxWidth: "800px",
    margin: "0 auto",
    padding: "0 1rem",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
  },
  logo: {
    color: "#fff",
    fontSize: "1.5rem",
    fontWeight: "bold",
    textDecoration: "none",
  },
  navLink: {
    color: "#e0e0e0",
    textDecoration: "none",
  },
};

export default Header;
