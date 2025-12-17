import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from 'src/lib/context/AuthContext';
import PrivateRoute from 'src/lib/router/PrivateRoute';
import LoginPage from 'src/pages/public/LoginPage';
import HomePage from 'src/pages/private/HomePage';
import NotFoundPage from 'src/pages/NotFoundPage';
import AccessRightContextProvider from '../context/AccessRightContext';
import AdministrationPageContainer from 'src/pages/private/Administration/AdministrationPageContainer';
import ConfigurationContainer from 'src/pages/private/Configuration/ConfigurationContainer';

const AppRouter: React.FC = () => {
  return (
    <AccessRightContextProvider>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Navigate to="/home" replace />} />
            <Route path="/auth" element={<LoginPage />} />
            <Route element={<PrivateRoute />}>
              <Route path="/home" element={<HomePage />} />
              <Route path="/administration/*" Component={AdministrationPageContainer} />
              <Route path="/configuration/*" Component={ConfigurationContainer} />
            </Route>
            <Route path="*" Component={NotFoundPage} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </AccessRightContextProvider>
  );
};

export default AppRouter;
