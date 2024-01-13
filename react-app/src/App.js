import logo from './logo.svg';
import './App.css';
import { store } from "./actions/store";
import { Provider } from "react-redux";
import Projects from './components/Projects';

function App() {
  return (
    <Provider store={store}>
      <Projects/>
    </Provider>
  );
}

export default App;
