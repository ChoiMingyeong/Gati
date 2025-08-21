import { BrowserRouter, Routes, Route, Navigate  } from 'react-router-dom';
import Login from '../Home/Login';
import Home from '../Home/Home';

export default function App() {
   return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/home/*" element={<Home />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}