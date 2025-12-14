import React from 'react';
import Checkbox from '@mui/material/Checkbox';

interface IProps {
  propertyName: string;
  rowIndex: number;
  checked: boolean;
  disabled?: boolean;
  model: any;
  onChange?: (index: number, newItem: any) => void;
}

const TableCheckbox: React.FC<IProps> = (props: IProps) => {
  const { checked, propertyName, disabled, onChange } = props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      if (onChange) {
        onChange(props.rowIndex, { ...props.model, [propertyName]: e.target.checked });
      }
    },
    [onChange, props.rowIndex, props.model, propertyName]
  );

  return <Checkbox checked={checked} disabled={disabled} onChange={handleChange} />;
};

export default React.memo(TableCheckbox);
