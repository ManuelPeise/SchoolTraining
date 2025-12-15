import { Grid, Icon, IconButton } from '@mui/material';
import React from 'react';

export interface ITableIconProps {
  id: number;
  size: number;
  iconClassName: string;
  disabled: boolean;
  tooltip?: string;
  color?: string;
  onClick: (id: number) => void | Promise<void>;
}

interface IProps {
  icons: ITableIconProps[];
  spacing?: number;
}
const TableIconGroup: React.FC<IProps> = (props: IProps) => {
  const { icons, spacing } = props;

  return (
    <Grid container display="flex" justifyContent="center" spacing={spacing}>
      {icons.map((iconProps, index) => (
        <IconButton
          key={index}
          title={iconProps.tooltip}
          onClick={iconProps.onClick.bind(null, iconProps.id)}
          disabled={iconProps.disabled}
        >
          <Icon
            className={iconProps.iconClassName}
            style={{ fontSize: iconProps.size, color: iconProps.color }}
          />
        </IconButton>
      ))}
    </Grid>
  );
};

export default React.memo(TableIconGroup);
