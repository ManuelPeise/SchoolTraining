import { Box, InputLabel, MenuItem, Select } from '@mui/material';
import { DropdownItem } from '../input/Dropdown';

interface IProps<TModel> {
  propertyKey?: keyof TModel;
  selectedItemId: number | null;
  isReadOnly: boolean;
  items: DropdownItem[];
  label: string;
  placeholder?: string;
  isRequired?: boolean;
  onChange: (key: keyof TModel, itemId: number) => void;
}

function FormDropdown<TModel>(props: IProps<TModel>) {
  const {
    propertyKey,
    selectedItemId,
    isReadOnly,
    items,
    isRequired,
    placeholder,
    label,
    onChange,
  } = props;

  return (
    <Box width="100%" sx={{ my: 1 }}>
      <InputLabel shrink>{label}</InputLabel>
      <Select
        required={isRequired}
        fullWidth
        displayEmpty={true}
        value={selectedItemId ?? ''}
        disabled={isReadOnly || items.length === 0}
        onChange={(e) => onChange(propertyKey!, e.target.value as number)}
        variant="standard"
      >
        {placeholder && (
          <MenuItem value="">
            <em>{placeholder}</em>
          </MenuItem>
        )}
        {items.map((item) => (
          <MenuItem key={item.id} value={item.id}>
            {item.label}
          </MenuItem>
        ))}
      </Select>
    </Box>
  );
}

export default FormDropdown;
