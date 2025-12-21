import {
  Autocomplete,
  Box,
  Checkbox,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import {
  FormNumberFieldProps,
  FormAutoCompleteProps,
  FormTextFieldProps,
  TextFieldProps,
  FormAutocompleteFieldProps,
  FormDropdownFieldProps,
  DropdownFieldProps,
  FormCheckboxFieldProps,
  BooleanFieldProps,
} from './FormTypes';
import React from 'react';
import { DropdownItem } from 'src/components/input/Dropdown';

function FormTextField<TModel>({ fieldKey, fieldPropsCallback }: TextFieldProps<TModel>) {
  const { key, value, label, onChange, isPassword, isReadonly, isRequired } = fieldPropsCallback(
    fieldKey
  ) as FormTextFieldProps<TModel>;

  return (
    <Box width="100%" sx={{ my: 1 }}>
      <InputLabel shrink>{label}</InputLabel>
      <TextField
        key={String(key)}
        type={isPassword ? 'password' : 'text'}
        disabled={isReadonly}
        fullWidth
        required={isRequired}
        value={value}
        onChange={(e) => onChange(key, e.currentTarget.value)}
        variant="standard"
        size="small"
      />
    </Box>
  );
}

function FormNumberField<TModel>({ fieldKey, fieldPropsCallback }: TextFieldProps<TModel>) {
  const {
    key,
    value,
    isReadonly,
    isRequired,
    label,
    onChange,
    min = 0,
    max,
    step = 1,
  } = fieldPropsCallback(fieldKey) as FormNumberFieldProps<TModel>;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const inputValue = e.currentTarget.value.replace(',', '.');
      // Only allow valid numbers (including decimals)
      if (/^-?\d*(\.\d*)?$/.test(inputValue)) {
        const newValue = parseFloat(inputValue);
        if (!isNaN(newValue)) {
          onChange(key, newValue);
        }
      }
    },
    [key, onChange]
  );

  return (
    <Box width="100%" sx={{ my: 1 }}>
      <InputLabel shrink>{label}</InputLabel>
      <TextField
        key={String(key)}
        type="number"
        disabled={isReadonly}
        fullWidth
        required={isRequired}
        label={label}
        value={value}
        slotProps={{
          htmlInput: {
            min: min,
            max: max,
            step: step,
          },
        }}
        onChange={handleChange}
        variant="standard"
        size="small"
      />
    </Box>
  );
}

function FormDropdownField<TModel>({ fieldKey, fieldPropsCallback }: DropdownFieldProps<TModel>) {
  const { key, value, isReadonly, isRequired, options, label, onChange } = fieldPropsCallback(
    fieldKey
  ) as FormDropdownFieldProps<TModel>;
  return (
    <Box width="100%" sx={{ my: 1 }}>
      <InputLabel shrink>{label}</InputLabel>
      <Select
        required={isRequired}
        fullWidth
        displayEmpty={true}
        value={value}
        disabled={isReadonly || options.length === 0}
        onChange={(e) => onChange(key!, e.target.value as number)}
        variant="standard"
      >
        {label && (
          <MenuItem value="">
            <em>{label}</em>
          </MenuItem>
        )}
        {options.map((item) => (
          <MenuItem key={item.id} value={item.id}>
            {item.label}
          </MenuItem>
        ))}
      </Select>
    </Box>
  );
}

function FormAutoCompleteField<TModel>({
  fieldKey,
  fieldPropsCallback,
}: FormAutocompleteFieldProps<TModel>) {
  const { key, value, isReadonly, isRequired, label, options, onChange, onSelectionChange } =
    fieldPropsCallback(fieldKey) as FormAutoCompleteProps<TModel>;

  const handleSelect = React.useCallback(
    (_: React.SyntheticEvent<Element, Event>, value: DropdownItem) => {
      if (value != null && typeof value === 'object') {
        const item = value as DropdownItem;
        onSelectionChange && onSelectionChange(key, item.id);
      } else {
        onSelectionChange && onSelectionChange(key, 0);
      }
    },
    [onSelectionChange, key]
  );

  const handleChange = React.useCallback(
    (_: React.SyntheticEvent<Element, Event>, value: string) => {
      onChange && onChange(key, value);
    },
    [onChange, key]
  );

  return (
    <Box width="100%" key={key as string} sx={{ my: 1 }}>
      <InputLabel shrink>{label}</InputLabel>
      <Autocomplete
        options={options?.length > 0 ? options : []}
        freeSolo
        disabled={isReadonly}
        getOptionLabel={(option) => (typeof option === 'string' ? option : (option?.label ?? ''))}
        inputValue={value}
        onChange={(_, value) => handleSelect(_, value as DropdownItem)}
        onInputChange={handleChange}
        size="small"
        renderInput={(params) => (
          <TextField
            {...params}
            required={isRequired}
            variant="standard"
            disabled={isReadonly}
            size="small"
          />
        )}
      />
    </Box>
  );
}

function FormCheckbox<TModel>({ fieldKey, fieldPropsCallback }: BooleanFieldProps<TModel>) {
  const { key, value, isReadonly, isRequired, label, onChange } = fieldPropsCallback(
    fieldKey
  ) as FormCheckboxFieldProps<TModel>;

  return (
    <Box width="100%" sx={{ my: 0.5 }}>
      <Box display="flex" alignItems="baseline">
        <Checkbox
          required={isRequired}
          checked={value || false}
          disabled={isReadonly}
          onChange={(e) => onChange(key, e.currentTarget.checked)}
        />
        <InputLabel shrink>{label}</InputLabel>
      </Box>
    </Box>
  );
}

export const FormComponents = {
  FormTextField,
  FormNumberField,
  FormAutoCompleteField,
  FormDropdownField,
  FormCheckbox,
};
