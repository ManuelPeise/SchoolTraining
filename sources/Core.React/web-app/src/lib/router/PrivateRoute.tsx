import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from 'src/hooks/useAuth';

const PrivateRoute: React.FC = () => {
  const { isAuthenticated } = useAuth();

  console.log('PrivateRoute - isAuthenticated:', isAuthenticated);
  return isAuthenticated ? <Outlet /> : <Navigate to="/auth" replace />;
};

export default PrivateRoute;
