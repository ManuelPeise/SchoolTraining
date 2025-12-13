import React from 'react';
import Button from '@mui/material/Button';
import ButtonGroup from '@mui/material/ButtonGroup';
import Box from '@mui/material/Box';

export interface ISaveCancelButtonsProps {
  labelSave: string;
  saveDisabled: boolean;
  labelCancel?: string;
  saveAction: () => void;
  cancelAction?: () => void;
}

const SaveCancelButtons: React.FC<ISaveCancelButtonsProps> = (props: ISaveCancelButtonsProps) => {
  const { labelSave, saveDisabled, labelCancel, saveAction, cancelAction } = props;

  console.log('SaveCancelButtons render', saveDisabled);
  if (saveDisabled) return null;
  return (
    <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 2 }}>
      <ButtonGroup variant="contained">
        {labelCancel && cancelAction && (
          <Button color="error" onClick={cancelAction}>
            {labelCancel}
          </Button>
        )}
        <Button color="success" onClick={saveAction} disabled={saveDisabled}>
          {labelSave}
        </Button>
      </ButtonGroup>
    </Box>
  );
};

export default React.memo(SaveCancelButtons);
