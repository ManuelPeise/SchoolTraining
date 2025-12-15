import { IconButton, InputAdornment, Select, MenuItem, Input } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import React from 'react';

export type FilterDropdownItem = {
  id: number;
  label: string;
};

export interface IFilterDropdownProps {
  items: FilterDropdownItem[];
  selectedItemId: number | null;
  placeholder: string;
  isReadonly: boolean;
  onSelectItem: (itemId: number | null) => void;
  onClearSelection: () => void;
}

const FilterDropdown: React.FC<IFilterDropdownProps> = (props: IFilterDropdownProps) => {
  const { items, selectedItemId, placeholder, isReadonly, onSelectItem, onClearSelection } = props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<{ value: unknown }>) => {
      const value = e.target.value === '' ? null : Number(e.target.value);
      onSelectItem(value);
    },
    [onSelectItem]
  );

  return (
    <Select
      value={selectedItemId ?? ''}
      displayEmpty
      disabled={isReadonly || items.length === 0}
      renderValue={(selected) => {
        if (selectedItemId === null) {
          return <span style={{ color: '#aaa' }}>{placeholder}</span>;
        }
        const found = items.find((item) => item.id === selected);
        return found ? found.label : '';
      }}
      input={
        <Input
          onChange={handleChange}
          endAdornment={
            selectedItemId !== null ? (
              <InputAdornment position="end">
                <IconButton
                  aria-label="clear filter"
                  onClick={onClearSelection}
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
            )
          }
        />
      }
    >
      <MenuItem value="">
        <em>{placeholder}</em>
      </MenuItem>
      {items.map((item) => (
        <MenuItem key={item.id} value={item.id}>
          {item.label}
        </MenuItem>
      ))}
    </Select>
  );
};

export default React.memo(FilterDropdown);
