import ReactDOM from 'react-dom/client';
import { GlobalProvider } from './store/store';
import App from './App/App';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <GlobalProvider>
    <App />
  </GlobalProvider>
);