import { Box, InputLabel } from '@mui/material';
import TextField from '@mui/material/TextField';

interface IProps<TModel> {
  propertyKey: keyof TModel;
  label?: string;
  type?: 'text' | 'password';
  placeholder?: string;
  disabled?: boolean;
  value: string;
  isRequired?: boolean;
  onChange: (key: keyof TModel, value: string) => void;
}

function FormTextInput<TModel>(props: IProps<TModel>) {
  const { label, placeholder, disabled, type, value, isRequired, onChange } = props;
  return (
    <Box sx={{ my: 1 }}>
      <InputLabel shrink>{label}</InputLabel>
      <TextField
        required={isRequired}
        fullWidth
        placeholder={placeholder}
        disabled={disabled}
        type={type ?? 'text'}
        value={value}
        onChange={(e) => onChange(props.propertyKey, e.target.value)}
        variant="standard"
        size="small"
      />
    </Box>
  );
}

export default FormTextInput;
