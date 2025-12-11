import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from 'src/lib/context/AuthContext';
import PrivateRoute from 'src/lib/router/PrivateRoute';
import LoginPage from 'src/pages/public/LoginPage';
import HomePage from 'src/pages/private/HomePage';
import NotFoundPage from 'src/pages/NotFoundPage';
import AccessRightContextProvider from '../context/AccessRightContext';

const AppRouter: React.FC = () => {
  return (
    <AccessRightContextProvider>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Navigate to="/home" replace />} />
            <Route path="/auth" element={<LoginPage />} />
            <Route
              path="/home"
              element={
                <PrivateRoute>
                  <HomePage />
                </PrivateRoute>
              }
            />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </AccessRightContextProvider>
  );
};

export default AppRouter;
