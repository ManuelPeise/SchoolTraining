import React from 'react';
import FormChip, { IFormChipProps } from './FormChip';
import { Box, FormLabel } from '@mui/material';

interface IFormChipContainerProps {
  label: string;
  models: IFormChipProps[];
}

const FormChipContainer: React.FC<IFormChipContainerProps> = (props: IFormChipContainerProps) => {
  const { models } = props;

  return (
    <Box width="100%" sx={{ display: 'flex', flexDirection: 'column', my: 2 }}>
      <Box>
        <FormLabel sx={{ fontSize: '12px' }}>{props.label}</FormLabel>
      </Box>
      <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', my: 2 }}>
        {models.map((model, index) => (
          <FormChip key={index} {...model} />
        ))}
      </Box>
    </Box>
  );
};

export default FormChipContainer;
