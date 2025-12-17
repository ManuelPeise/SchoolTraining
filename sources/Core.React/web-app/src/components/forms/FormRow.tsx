import { Box } from '@mui/material';
import React, { PropsWithChildren } from 'react';

interface IProps extends PropsWithChildren {
  numberOfColumns?: number;
  gapSize?: number;
}

const FormRow: React.FC<IProps> = (props: IProps) => {
  const { gapSize = 2, children } = props;

  return (
    <Box
      width="100%"
      display="flex"
      flexDirection={{ sm: 'column', lg: 'row', md: 'row', xl: 'row' }}
      gap={gapSize}
    >
      {children}
    </Box>
  );
};

export default FormRow;
