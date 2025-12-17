import React from 'react';
import { IAccessRightsContext } from '../interfaces/IAccessRightsContext';
import { IAccessRights } from '../interfaces/IAccessRights';
import { UserRoleEnum } from '../enums/UserRoleEnum';
import { IAppUser } from '../interfaces/IAppUser';
import { parseJwtToken } from '../utils/jwtHelper';

const defaultAccessRights: IAccessRights = {
  isAdmin: false,
  isSystemAdmin: false,
  accessRights: {
    familyAdministration: {
      view: false,
      edit: false,
      create: false,
      delete: false,
    },
    userAdministration: {
      view: false,
      edit: false,
      create: false,
      delete: false,
    },
    moduleConfiguration: {
      view: false,
      edit: false,
      create: false,
      delete: false,
    },
  },
};

export const AccessRightsContext = React.createContext<IAccessRightsContext | null>(null);

interface IProps extends React.PropsWithChildren {}

const AccessRightContextProvider: React.FC<IProps> = (props: IProps) => {
  const [user, setUser] = React.useState<IAppUser | null>(null);

  const [accessRights, setAccessRights] = React.useState<IAccessRights>({
    isAdmin: false,
    isSystemAdmin: false,
    accessRights: defaultAccessRights.accessRights,
  });

  const tryInitializeAccessRights = React.useCallback(() => {
    const jwt = localStorage.getItem('jwt');

    const userFromToken = parseJwtToken(jwt);

    if (userFromToken == null) {
      setAccessRights(defaultAccessRights);
      return;
    }

    const userRole =
      typeof userFromToken.userRole === 'string'
        ? UserRoleEnum[userFromToken.userRole as keyof typeof UserRoleEnum]
        : userFromToken.userRole;

    setAccessRights({
      isAdmin: userRole === UserRoleEnum.Admin,
      isSystemAdmin: userRole === UserRoleEnum.SystemAdmin,
      accessRights: defaultAccessRights.accessRights,
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

    // Convert userRole from string to number if needed
    const userRole =
      typeof user.userRole === 'string'
        ? UserRoleEnum[user.userRole as keyof typeof UserRoleEnum]
        : user.userRole;

    setUser({
      id: user.id,
      familyId: user.familyId,
      userName: user.name,
      userRole: userRole,
    });

    setAccessRights({
      isAdmin: userRole === UserRoleEnum.Admin,
      isSystemAdmin: userRole === UserRoleEnum.SystemAdmin,
      accessRights: {
        familyAdministration: {
          view: userRole === UserRoleEnum.SystemAdmin,
          edit: userRole === UserRoleEnum.SystemAdmin,
          create: false,
          delete: false,
        },
        userAdministration: {
          view: userRole === UserRoleEnum.Admin || userRole === UserRoleEnum.SystemAdmin,
          edit: userRole === UserRoleEnum.Admin,
          create: userRole === UserRoleEnum.Admin,
          delete: userRole === UserRoleEnum.Admin,
        },
        moduleConfiguration: {
          view: userRole === UserRoleEnum.Admin || userRole === UserRoleEnum.SystemAdmin,
          edit: userRole === UserRoleEnum.Admin,
          create: userRole === UserRoleEnum.Admin,
          delete: userRole === UserRoleEnum.SystemAdmin,
        },
      },
    });
  }, []);

  React.useEffect(() => {
    tryInitializeAccessRights();
  }, [tryInitializeAccessRights]);

  return (
    <AccessRightsContext.Provider value={{ accessRights, appUser: user, initialize }}>
      {props.children}
    </AccessRightsContext.Provider>
  );
};

export default AccessRightContextProvider;
