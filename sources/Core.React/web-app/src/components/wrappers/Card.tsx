import React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';

interface IProps extends React.PropsWithChildren {}

const CardWrapper: React.FC<IProps> = (props: IProps) => {
  return (
    <Card sx={{ boxShadow: 3 }}>
      <CardContent>{props.children}</CardContent>
    </Card>
  );
};

export default CardWrapper;
