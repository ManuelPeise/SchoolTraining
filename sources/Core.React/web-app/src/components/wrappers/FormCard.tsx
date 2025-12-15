import React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';

interface IProps extends React.PropsWithChildren {
  minwidth?: string;
  padding?: string;
}

const FormCard: React.FC<IProps> = (props: IProps) => {
  return (
    <Card sx={{ minWidth: props.minwidth, p: 0, boxShadow: 3 }}>
      <CardContent sx={{ p: props.padding ? undefined : 2, padding: props.padding }}>
        {props.children}
      </CardContent>
    </Card>
  );
};

export default FormCard;
