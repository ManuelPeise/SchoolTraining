import React from 'react';
import './App.css';
import AppRouter from 'src/lib/router/AppRouter';
import { ThemeProvider, CssBaseline } from '@mui/material';
import darkTheme from './theme';

const App: React.FC = () => {
  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <AppRouter />
    </ThemeProvider>
  );
};

export default App;
