import React, { PropsWithChildren } from 'react';
import { useAccessRights } from 'src/hooks/useAccessRights';
import NavBar from './navigation/NavBar';

interface IProps extends PropsWithChildren {}

const PageLayout: React.FC<IProps> = (props: IProps) => {
  const { appUser } = useAccessRights();

  return (
    <div>
      <NavBar user={appUser} />
      <div>{props.children}</div>
    </div>
  );
};

export default PageLayout;
