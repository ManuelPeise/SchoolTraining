import React, { PropsWithChildren } from 'react';
import { useAccessRights } from 'src/hooks/useAccessRights';
import NavBar from './navigation/NavBar';
import styles from './layout.module.css';

interface IProps extends PropsWithChildren {}

const PageLayout: React.FC<IProps> = (props: IProps) => {
  const { appUser } = useAccessRights();

  return (
    <div className={styles.pageLayout}>
      {/* Header */}
      <div className={styles.header}>
        <NavBar user={appUser} isLoading={true} />
      </div>
      {/* Main content */}
      <div className={styles.mainContent}>{props.children}</div>
    </div>
  );
};

export default PageLayout;
