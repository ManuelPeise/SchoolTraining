import { Icon } from '@mui/material';
import React from 'react';

interface ITableStatusIconProps {
  status: 'success' | 'pending' | 'error';
  size?: number;
  toolTipText?: string;
}

const TableStatusIcon: React.FC<ITableStatusIconProps> = (props: ITableStatusIconProps) => {
  const { status, size, toolTipText } = props;

  const getIconClassName = React.useCallback((): string => {
    switch (status) {
      case 'success':
        return 'bi bi-check';
      case 'pending':
        return 'bi bi-arrow-clockwise';
      case 'error':
        return 'bi bi-exclamation';
      default:
        return 'bi bi-question';
    }
  }, [status]);

  const getIconColor = React.useCallback((): string => {
    switch (status) {
      case 'success':
        return '#28a745'; // Green
      case 'pending':
        return '#ffc107'; // Yellow
      case 'error':
        return '#dc3545'; // Red
      default:
        return '#6c757d'; // Gray
    }
  }, [status]);

  return (
    <Icon
      className={getIconClassName()}
      title={toolTipText}
      style={{ padding: '0px', color: getIconColor(), fontSize: size }}
    />
  );
};

export default React.memo(TableStatusIcon);
