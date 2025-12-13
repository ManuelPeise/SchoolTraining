import React from 'react';
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import Box from '@mui/material/Box';

interface IProps {
  label: string;
  checked: boolean;
  disabled?: boolean;
  onChange: (checked: boolean) => void;
}

const FormCheckbox: React.FC<IProps> = ({ label, checked, disabled, onChange }) => {
  return (
    <Box sx={{ display: 'flex', alignItems: 'center', my: 1 }}>
      <FormControlLabel
        control={
          <Checkbox
            checked={checked}
            disabled={disabled}
            onChange={(e) => onChange(e.target.checked)}
            color="primary"
          />
        }
        label={label}
      />
    </Box>
  );
};

export default FormCheckbox;
