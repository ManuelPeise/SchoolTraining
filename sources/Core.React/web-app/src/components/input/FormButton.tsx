import React from 'react';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';

interface IProps {
  label: string;
  disabled?: boolean;
  onClick: () => void | Promise<void>;
}

const FormButton: React.FC<IProps> = ({ label, disabled, onClick }) => {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 2 }}>
      <Button variant="contained" color="primary" disabled={disabled} onClick={onClick}>
        {label}
      </Button>
    </Box>
  );
};

export default FormButton;
