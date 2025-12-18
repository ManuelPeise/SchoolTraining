import { Box, Button } from '@mui/material';
import React, { PropsWithChildren } from 'react';

export type DeleteButtonProps = {
  isModified: boolean;
  isDisabled?: boolean;
  deleteLabel: string;
  onDelete: () => void;
};
export type SaveCancelButtonProps = {
  isModified: boolean;
  saveLabel: string;
  isSaveDisabled?: boolean;
  onSave: () => void;
  cancelLabel?: string;
  isCancelDisabled?: boolean;
  onCancel?: () => void;
};

interface IProps extends PropsWithChildren {
  saveCancelButtonProps: SaveCancelButtonProps;
  deleteButtonProps?: DeleteButtonProps;
}

const FormContainer: React.FC<IProps> = (props: IProps) => {
  const { children, saveCancelButtonProps, deleteButtonProps } = props;

  return (
    <Box width="100%" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
      <Box width="100%" sx={{ flexGrow: 1, padding: 2 }}>
        {children}
      </Box>
      <Box width="100%" display="flex" alignContent="center" minHeight="50px" maxHeight="50px">
        {deleteButtonProps && (
          <Box
            width="100%"
            display="flex"
            flexDirection="row"
            alignItems="center"
            justifyContent="flex-start"
            sx={{ flexGrow: 1 }}
          >
            <Button
              variant="contained"
              sx={{
                ml: 2,
                borderRadius: '16px',
                backgroundColor: '#262626',
                boxShadow: 'none',
                color: '#000000',
                minWidth: '100px',
                '&:hover': { backgroundColor: '#bfbfbf' },
              }}
              onClick={deleteButtonProps.onDelete}
              disabled={deleteButtonProps.isDisabled}
            >
              {deleteButtonProps.deleteLabel}
            </Button>
          </Box>
        )}
        {saveCancelButtonProps.isModified && (
          <Box
            width="100%"
            display="flex"
            flexDirection="row"
            alignItems="center"
            justifyContent="flex-end"
            sx={{ flexGrow: 1 }}
          >
            {saveCancelButtonProps?.cancelLabel && saveCancelButtonProps.onCancel && (
              <Button
                variant="contained"
                sx={{
                  mr: 2,
                  borderRadius: '16px',
                  backgroundColor: '#d9d9d9',
                  color: '#000000',
                  minWidth: '100px',
                  '&:hover': { backgroundColor: '#bfbfbf' },
                }}
                onClick={saveCancelButtonProps.onCancel}
                disabled={saveCancelButtonProps.isCancelDisabled}
              >
                {saveCancelButtonProps?.cancelLabel || 'Cancel'}
              </Button>
            )}
            <Button
              variant="contained"
              sx={{
                mr: 2,
                borderRadius: '16px',
                backgroundColor: '#ff9933',
                color: '#000000',
                minWidth: '100px',
                '&:hover': { backgroundColor: '#e68a00' },
              }}
              onClick={saveCancelButtonProps.onSave}
              disabled={saveCancelButtonProps?.isSaveDisabled}
            >
              {saveCancelButtonProps?.saveLabel || 'Save'}
            </Button>
          </Box>
        )}
      </Box>
    </Box>
  );
};

export default React.memo(FormContainer);
