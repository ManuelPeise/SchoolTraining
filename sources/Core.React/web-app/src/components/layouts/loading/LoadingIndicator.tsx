import React from 'react';
import styles from './loadingIndicator.module.css';

interface IProps {
  isLoading: boolean;
}

const LoadingIndicator: React.FC<IProps> = ({ isLoading }) => {
  if (!isLoading) {
    return null;
  }

  return (
    <div className={styles.loadingContainer}>
      <div className={styles.loadingBar}></div>
    </div>
  );
};

export default LoadingIndicator;
