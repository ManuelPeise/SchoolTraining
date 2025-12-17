import { MenuItem, Select } from '@mui/material';
import React from 'react';

export type DropdownItem = {
  id: number;
  label: string;
};

interface IProps {
  selectedItemId: number | null;
  isReadOnly: boolean;
  items: DropdownItem[];
  placeholder: string;
}

const Dropdown: React.FC<IProps> = (props: IProps) => {
  const { selectedItemId, isReadOnly, items, placeholder } = props;

  return (
    <Select
      value={selectedItemId ?? ''}
      displayEmpty
      disabled={isReadOnly || items.length === 0}
      onChange={(e) => {
        console.log(e.target.value);
      }}
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

export default Dropdown;
