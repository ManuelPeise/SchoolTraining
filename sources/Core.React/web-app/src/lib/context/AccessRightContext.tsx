import React from 'react';
import { IAccessRightsContext } from '../interfaces/IAccessRightsContext';
import { IAccessRights } from '../interfaces/IAccessRights';
import { UserRoleEnum } from '../enums/UserRoleEnum';
import { IAppUser } from '../interfaces/IAppUser';
import { parseJwtToken } from '../utils/jwtHelper';

const defaultAccessRights: IAccessRights = {
  isAdmin: false,
  isSystemAdmin: false,
  accessRights: [],
};

export const AccessRightsContext = React.createContext<IAccessRightsContext | null>(null);

interface IProps extends React.PropsWithChildren {}

const AccessRightContextProvider: React.FC<IProps> = (props: IProps) => {
  const [user, setUser] = React.useState<IAppUser | null>(null);

  const [accessRights, setAccessRights] = React.useState<IAccessRights>({
    isAdmin: false,
    isSystemAdmin: false,
    accessRights: [],
  });

  const tryInitializeAccessRights = React.useCallback(() => {
    const jwt = localStorage.getItem('jwt');

    const userFromToken = parseJwtToken(jwt);

    if (userFromToken == null) {
      setAccessRights(defaultAccessRights);
      return;
    }

    const userRole = userFromToken.userRole;

    setAccessRights({
      isAdmin: userRole === UserRoleEnum.Admin || userRole === UserRoleEnum.SystemAdmin,
      isSystemAdmin: userRole === UserRoleEnum.SystemAdmin,
      accessRights: [],
    });

    setUser({
      id: userFromToken.id,
      familyId: userFromToken.familyId,
      userName: userFromToken.name,
      userRole: userRole,
    });
  }, []);

  const initialize = React.useCallback((jwt: string) => {
    const user = parseJwtToken(jwt);
    if (user == null) {
      setAccessRights(defaultAccessRights);
      return;
    }
    setUser({
      id: user.id,
      familyId: user.familyId,
      userName: user.name,
      userRole: user.userRole,
    });

    const userRole = user.userRole;

    setAccessRights({
      isAdmin: userRole === UserRoleEnum.Admin || userRole === UserRoleEnum.SystemAdmin,
      isSystemAdmin: userRole === UserRoleEnum.SystemAdmin,
      accessRights: [],
    });
  }, []);

  React.useEffect(() => {
    console.log('AccessRightContextProvider useEffect - tryInitializeAccessRights');
    tryInitializeAccessRights();
  }, [tryInitializeAccessRights]);

  return (
    <AccessRightsContext.Provider value={{ accessRights, appUser: user, initialize }}>
      {props.children}
    </AccessRightsContext.Provider>
  );
};

export default AccessRightContextProvider;
