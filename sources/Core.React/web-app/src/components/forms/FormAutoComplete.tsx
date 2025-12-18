import React from 'react';
import { Autocomplete, Box, InputLabel, TextField } from '@mui/material';
import { DropdownItem } from '../input/Dropdown';

interface IProps<TModel> {
  propertyKey: keyof TModel;
  isRequired?: boolean;
  options: DropdownItem[];
  value: string;
  placeholder: string;
  isReadOnly?: boolean;
  onChange: (key: keyof TModel, value: string) => void;
  onSelectionChange?: (key: keyof TModel, id: number | null) => void;
}

function FormAutoComplete<TModel>(props: IProps<TModel>) {
  const {
    propertyKey,
    options,
    value,
    placeholder,
    isReadOnly,
    isRequired,
    onChange,
    onSelectionChange,
  } = props;

  const handleSelect = React.useCallback(
    (_: React.SyntheticEvent<Element, Event>, value: DropdownItem) => {
      if (value != null && typeof value === 'object') {
        const item = value as DropdownItem;
        onSelectionChange && onSelectionChange(propertyKey, item.id);
      } else {
        onSelectionChange && onSelectionChange(propertyKey, null);
      }
    },
    [onSelectionChange, propertyKey]
  );

  const handleChange = React.useCallback(
    (_: React.SyntheticEvent<Element, Event>, value: string) => {
      onChange(props.propertyKey, value);
    },
    [onChange, props.propertyKey]
  );
  return (
    <Box sx={{ my: 1 }}>
      <InputLabel shrink>{placeholder}</InputLabel>
      <Autocomplete
        options={options}
        freeSolo
        disabled={isReadOnly}
        getOptionLabel={(option) => (typeof option === 'string' ? option : option.label)}
        inputValue={value}
        onChange={(_, value) => handleSelect(_, value as DropdownItem)}
        onInputChange={handleChange}
        size="small"
        renderInput={(params) => (
          <TextField
            {...params}
            required={isRequired}
            variant="standard"
            disabled={isReadOnly}
            size="small"
          />
        )}
      />
    </Box>
  );
}

export default FormAutoComplete;
