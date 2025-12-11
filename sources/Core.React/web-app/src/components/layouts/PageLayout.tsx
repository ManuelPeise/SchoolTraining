import React, { PropsWithChildren } from 'react';
import { useAccessRights } from 'src/hooks/useAccessRights';
import NavBar from './navigation/NavBar';
import styles from './layout.module.css';

interface IProps extends PropsWithChildren {
  isLoading: boolean;
}

const PageLayout: React.FC<IProps> = (props: IProps) => {
  const { isLoading } = props;
  const { appUser } = useAccessRights();

  return (
    <div className={styles.pageLayout}>
      {/* Header */}
      <div className={styles.header}>
        <NavBar user={appUser} isLoading={isLoading} />
      </div>
      {/* Main content */}
      <div className={styles.mainContent}>{props.children}</div>
    </div>
  );
};

export default PageLayout;
