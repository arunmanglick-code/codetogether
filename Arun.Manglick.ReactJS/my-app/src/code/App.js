import logo from '../logo.svg';
import '../css/App.css';
import Tab from './components/Tabs';
import Basic from './components/Basic';
import LearnProps from './components/LearnProps';
import Arrange from './components/Arrange';

function App() {
  // return <Tab/>
  return (<main>
      {/* <PropsDynamic firstName='Arun' lastName = 'Manglick'></PropsDynamic>
      <Basic />
      <Tab /> */}
      <Arrange />
    </main>);
}

export default App;
