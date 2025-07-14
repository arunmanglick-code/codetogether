import { useState } from "react";
import "../../css/styles.css";

const content = [
  [
    "React is extremely popular",
    "It makes building complex, interactive UIs a breeze",
    "It's powerful & flexible",
    "It has a very active and versatile ecosystem",
    "Components, JSX & Props",
    "State",
    "Hooks (e.g., useEffect())",
    "Dynamic rendering"
  ],
  [
    "Course Git Repo: https://github.com/academind/react-complete-guide-course-resources/tree/main",
    "To run downloaded code, use command 'npm run dev' as the code is created using 'vite",
    
    "To hands-on online, just type in browser 'react.new' - It'll open codesandbox.io with basic react project",
    "To hands-in locally, use the command to create basic react project",
    "npx create-react-app my-app",
    "cd my-app",
    "npm start",
    "ORRRRRRRRRRR...." ,
    "npm create vite",
    "Choose Project Name: vite-project",
    "Select Framework: React",
    "Select Variant: JavaScript",
    "cd vite-project",
    "npm install",
    "nmp run dev",
    "Then browse: http://127.0.0.1:5173/"
  ],
  [   
    "Official web page (react.dev)",
    "Next.js (Fullstack framework)",
    "React Native (build native mobile apps with React)"
  ],
  [
    "Vanilla JavaScript requires imperative programming",
    "Imperative Programming: You define all the steps needed to achieve a result",
    "React on the other hand embraces declarative programming",
    "With React, you define the goal and React figures out how to get there"
  ],
  [
    "useState is React Hook that allows you to add state to a functional component.",
    " It returns an array with two values: the current state and a function to update it.",
    "The Hook takes an initial state value as an argument and returns an updated state value whenever the setter function is called."
  ]
];

function Tab() {
  const [activeContentIndex, setActiveContentIndex] = useState(0);

  return (
    <div>
      <header>
        <div>
          <h1>React.js</h1>
          <p>i.e., using the React library for rendering the UI</p>
        </div>
      </header>

      <div id="tabs">
        <menu>
          <button
            className={activeContentIndex === 0 ? "active" : ""}
            onClick={() => setActiveContentIndex(0)}
          >
            Why React?
          </button>
          <button
            className={activeContentIndex === 1 ? "active" : ""}
            onClick={() => setActiveContentIndex(1)}
          >
           Code Source
          </button>
          <button
            className={activeContentIndex === 2 ? "active" : ""}
            onClick={() => setActiveContentIndex(2)}
          >
            Related Resources
          </button>
          <button
            className={activeContentIndex === 3 ? "active" : ""}
            onClick={() => setActiveContentIndex(3)}
          >
            React vs JS
          </button>
          <button
            className={activeContentIndex === 4 ? "active" : ""}
            onClick={() => setActiveContentIndex(4)}
          >
            What is 'useState'
          </button>
        </menu>
        <div id="tab-content">
          <ul>
            {content[activeContentIndex].map((item) => (
              <li key={item}>{item}</li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default Tab;
