import React from 'react';
import LinearProgress from '@mui/material/LinearProgress';
import Box from '@mui/material/Box';

interface IProps {
  isLoading: boolean;
}

const LoadingIndicator: React.FC<IProps> = ({ isLoading }) => {
  if (!isLoading) return null;
  return (
    <Box sx={{ width: '100%', position: 'fixed', top: 0, left: 0, zIndex: 2000 }}>
      <LinearProgress color="primary" />
    </Box>
  );
};

export default LoadingIndicator;
