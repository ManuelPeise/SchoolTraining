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
}

function FormAutoComplete<TModel>(props: IProps<TModel>) {
  const { options, value, placeholder, isReadOnly, isRequired, onChange } = props;

  const handleSelect = React.useCallback(
    (_: React.SyntheticEvent<Element, Event>, value: string | DropdownItem | null) => {
      if (value == null) {
        onChange(props.propertyKey, '');

        return;
      }

      if (typeof value === 'object') {
        onChange(props.propertyKey, value.label);
        return;
      }

      onChange(props.propertyKey, value);
    },
    [onChange, props.propertyKey]
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
        onChange={(_, value) => handleSelect(_, value)}
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
