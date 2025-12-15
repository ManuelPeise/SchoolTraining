import React from 'react';
import Typography from '@mui/material/Typography';

interface IProps {
  text: string;
}

const Title: React.FC<IProps> = ({ text }) => {
  return (
    <Typography variant="h5" component="h2" sx={{ fontWeight: 700, mb: 2 }}>
      {text}
    </Typography>
  );
};

export default Title;
