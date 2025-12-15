import React from 'react';
import IconButton from '@mui/material/IconButton';
import Icon from '@mui/material/Icon';

interface IProps {
  iconClassName: string;
  disabled?: boolean;
  size?: number;
  onClick?: () => void | Promise<void>;
}

const TableIconButton: React.FC<IProps> = (props: IProps) => {
  const { iconClassName, disabled, size, onClick } = props;

  return (
    <IconButton disabled={disabled} onClick={onClick} size={size ? 'large' : 'medium'}>
      <Icon className={iconClassName} style={size ? { fontSize: size } : {}} />
    </IconButton>
  );
};

export default React.memo(TableIconButton);
