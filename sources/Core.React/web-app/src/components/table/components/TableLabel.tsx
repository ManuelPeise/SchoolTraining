import React from 'react';
import Typography from '@mui/material/Typography';

interface IProps {
  value: string;
}

const TableLabel: React.FC<IProps> = (props: IProps) => {
  const { value } = props;

  return <Typography variant="body2">{value}</Typography>;
};

export default React.memo(TableLabel);
