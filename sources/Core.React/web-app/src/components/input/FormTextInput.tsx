import React from 'react';
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';

interface IProps {
  label?: string;
  placeholder?: string;
  disabled?: boolean;
  type: string;
  value: string;
  onChange: (value: string) => void;
}

const FormTextInput: React.FC<IProps> = ({
  label,
  placeholder,
  disabled,
  type,
  value,
  onChange,
}) => {
  return (
    <Box sx={{ my: 1 }}>
      <TextField
        fullWidth
        label={label}
        placeholder={placeholder}
        disabled={disabled}
        type={type}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        variant="outlined"
        size="medium"
      />
    </Box>
  );
};

export default FormTextInput;
