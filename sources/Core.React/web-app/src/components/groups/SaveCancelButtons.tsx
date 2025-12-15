import React from 'react';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import { Grid } from '@mui/material';

export interface ISaveCancelButtonsProps {
  labelSave: string;
  saveDisabled: boolean;
  labelCancel?: string;
  saveAction: () => void;
  cancelAction?: () => void;
}

const SaveCancelButtons: React.FC<ISaveCancelButtonsProps> = (props: ISaveCancelButtonsProps) => {
  const { labelSave, saveDisabled, labelCancel, saveAction, cancelAction } = props;

  if (saveDisabled) return null;
  return (
    <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 2 }}>
      <Grid container aria-label="save cancel button group" sx={{ display: 'flex', gap: 2 }}>
        {labelCancel && cancelAction && (
          <Button
            sx={{ borderRadius: '999px' }}
            variant="outlined"
            color="inherit"
            onClick={cancelAction}
          >
            {labelCancel}
          </Button>
        )}
        <Button
          sx={{ borderRadius: '999px' }}
          variant="outlined"
          color="success"
          onClick={saveAction}
          disabled={saveDisabled}
        >
          {labelSave}
        </Button>
      </Grid>
    </Box>
  );
};

export default React.memo(SaveCancelButtons);
