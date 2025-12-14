import { Snackbar, Alert } from '@mui/material';
import React from 'react';

interface INotificationBadgeProps {
  message: string;
  color: 'success' | 'error' | 'info' | 'warning';
  show: boolean;
  duration?: number;
  handleResetNotification: () => void;
}

const Notification: React.FC<INotificationBadgeProps> = (props: INotificationBadgeProps) => {
  const { message, color, show, duration = 3000, handleResetNotification } = props;
  const [open, setOpen] = React.useState(false);

  React.useEffect(() => {
    setOpen(show);
  }, [show]);

  const handleClose = (_event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') return;
    setOpen(false);
    handleResetNotification();
  };

  return (
    <Snackbar
      open={open}
      autoHideDuration={duration}
      onClose={handleClose}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
    >
      <Alert onClose={handleClose} severity={color} sx={{ width: '100%' }}>
        {message}
      </Alert>
    </Snackbar>
  );
};

export default React.memo(Notification);
