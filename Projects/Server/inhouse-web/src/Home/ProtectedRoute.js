import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { useGlobalStore } from '../store/store';

export default function ProtectedRoute({ children }) {
  const { validateToken } = useGlobalStore();
  const [loading, setLoading] = useState(true);
  const [valid, setValid] = useState(false);

  useEffect(() => {
    validateToken((res) => {
      setValid(res.valid);
      setLoading(false);
    });
  }, [validateToken]);

  if (loading) return <div> 로딩 중... </div>;
  if (!valid) return <Navigate to="/" replace />;

  return children;
}
