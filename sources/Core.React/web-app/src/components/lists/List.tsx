import React from 'react';
import MuiList from '@mui/material/List';

interface IListProps extends React.PropsWithChildren {
  minHeight?: number;
  maxHeight?: number;
}

const List: React.FC<IListProps> = (props: IListProps) => {
  return (
    <MuiList sx={{ minHeight: props.minHeight, maxHeight: props.maxHeight, width: '100%' }}>
      {props.children}
    </MuiList>
  );
};

export default React.memo(List);
