import logo from '../../logo.svg';
import '../../css/App.css';

function Basic() {
  return (
    <div className="App">
      <img src={logo} className="App-logo" alt="logo" />
        <p>
          Welcome 'Arun Manglick' to your React Learning starting 05.25.2024 <br></br>
          Edit <code>src/components/Main.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
    </div>
  );
}

export default Basic;