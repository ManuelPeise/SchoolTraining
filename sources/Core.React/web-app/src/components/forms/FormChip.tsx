import { Chip } from '@mui/material';
import React from 'react';

export interface IFormChipProps {
  label: string;
  isReadOnly: boolean;
  onDelete?: () => void;
}

const FormChip: React.FC<IFormChipProps> = (props: IFormChipProps) => {
  const { label, isReadOnly, onDelete } = props;

  return (
    <Chip
      label={label}
      variant="outlined"
      onDelete={isReadOnly ? undefined : onDelete}
      color={isReadOnly ? 'default' : 'primary'}
    />
  );
};

export default FormChip;
