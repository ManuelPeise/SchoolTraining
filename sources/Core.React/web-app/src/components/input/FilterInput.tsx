import { TextField, InputAdornment, IconButton } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import React from 'react';

interface IFilterTextInputProps {
  filterText: string;
  isReadonly: boolean;
  placeholder: string;
  onFilterTextChange: (newValue: string) => void;
  onClearFilter: () => void;
}

const FilterInput: React.FC<IFilterTextInputProps> = (props: IFilterTextInputProps) => {
  const { filterText, isReadonly, placeholder, onFilterTextChange, onClearFilter } = props;
  return (
    <TextField
      variant="standard"
      disabled={isReadonly}
      type="text"
      value={filterText}
      onChange={(e) => onFilterTextChange(e.target.value)}
      placeholder={placeholder}
      slotProps={{
        input: {
          endAdornment: filterText ? (
            <InputAdornment position="end">
              <IconButton
                aria-label="clear filter"
                onClick={onClearFilter}
                edge="end"
                size="small"
                disabled={isReadonly}
              >
                <ClearIcon />
              </IconButton>
            </InputAdornment>
          ) : (
            <InputAdornment position="end">
              <IconButton disabled aria-label="clear filter" edge="end" size="small">
                <ClearIcon sx={{ color: 'transparent' }} />
              </IconButton>
            </InputAdornment>
          ),
        },
      }}
    />
  );
};

export default React.memo(FilterInput);
