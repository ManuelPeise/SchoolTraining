import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from 'src/hooks/useAuth';

interface PrivateRouteProps {
  children: React.ReactElement;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
  const { isAuthenticated } = useAuth();

  console.log('PrivateRoute render', { isAuthenticated });
  return isAuthenticated ? children : <Navigate to="/auth" replace />;
};

export default PrivateRoute;
