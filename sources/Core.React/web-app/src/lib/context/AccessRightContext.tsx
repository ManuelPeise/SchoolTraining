import React from 'react';
import { IAccessRightsContext, UserRights } from '../interfaces/IAccessRightsContext';
import { UserRoleEnum } from '../enums/UserRoleEnum';
import { IAppUser } from '../interfaces/IAppUser';
import { parseJwtToken } from '../utils/jwtHelper';
import { useLocalStorage } from 'src/hooks/AppHooks';
import { IUserRight } from '../interfaces/IUserRight';
import { LocalStorageKeyEnum } from '../enums/LocalStorageKeyEnum';
import { ITokenStore } from '../interfaces/ITokenStore';

export const AccessRightsContext = React.createContext<IAccessRightsContext | null>(null);

const FamilyAdministration = 'FamilyAdministration';
const ModuleAdministration = 'ModuleAdministration';
const SubModuleAdministration = 'SubModuleAdministration';

const defaultAccessRights: UserRights = {
  isLocalAdmin: false,
  isSystemAdmin: false,
  familyAdministrationRight: {
    isActive: false,
    deny: false,
    delete: false,
    edit: false,
    view: false,
    create: false,
    rightGuid: '',
    name: '',
    nameResourceKey: '',
    descriptionResourceKey: '',
  },
  moduleAdministrationRight: {
    isActive: false,
    deny: false,
    delete: false,
    edit: false,
    view: false,
    create: false,
    rightGuid: '',
    name: '',
    nameResourceKey: '',
    descriptionResourceKey: '',
  },
  subModuleAdministrationRight: {
    isActive: false,
    deny: false,
    delete: false,
    edit: false,
    view: false,
    create: false,
    rightGuid: '',
    name: '',
    nameResourceKey: '',
    descriptionResourceKey: '',
  },
};

interface IProps extends React.PropsWithChildren {}

const AccessRightContextProvider: React.FC<IProps> = (props: IProps) => {
  const [user, setUser] = React.useState<IAppUser | null>(null);
  const tokenStore = useLocalStorage<ITokenStore>(LocalStorageKeyEnum.Jwt);
  const accessRightsStore = useLocalStorage<IUserRight[]>(LocalStorageKeyEnum.UserRights);

  const [userRights, setUserRights] = React.useState<UserRights>({} as UserRights);

  const initializeUserRights = React.useCallback(
    (rights?: IUserRight[], userRole?: UserRoleEnum): UserRights => {
      if (!rights || !userRole) {
        return defaultAccessRights;
      }
      return {
        isLocalAdmin: userRole === UserRoleEnum.LocalAdmin,
        isSystemAdmin: userRole === UserRoleEnum.SystemAdmin,
        familyAdministrationRight:
          rights.find((r) => r.name === FamilyAdministration) ||
          defaultAccessRights.familyAdministrationRight,
        moduleAdministrationRight:
          rights.find((r) => r.name === ModuleAdministration) ||
          defaultAccessRights.moduleAdministrationRight,
        subModuleAdministrationRight:
          rights.find((r) => r.name === SubModuleAdministration) ||
          defaultAccessRights.subModuleAdministrationRight,
      };
    },
    []
  );

  const tryInitializeUserRights = React.useCallback(() => {
    const userFromToken = parseJwtToken(tokenStore.model?.jwt || '');

    if (userFromToken == null) {
      setUserRights(initializeUserRights());
      return;
    }

    const userRole =
      typeof userFromToken.userRole === 'string'
        ? UserRoleEnum[userFromToken.userRole as keyof typeof UserRoleEnum]
        : userFromToken.userRole;

    setUser({
      id: userFromToken.id,
      familyId: userFromToken.familyId,
      userName: userFromToken.name,
      userRole: userRole,
    });

    setUserRights(initializeUserRights(accessRightsStore.model || [], userRole));
  }, [accessRightsStore.model, initializeUserRights, tokenStore.model]);

  const initialize = React.useCallback(() => {
    const user = parseJwtToken(tokenStore.model?.jwt || '');

    if (user == null) {
      setUserRights(initializeUserRights());
      return;
    }

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

    setUserRights(initializeUserRights(accessRightsStore.model || [], userRole));
  }, [accessRightsStore.model, initializeUserRights, tokenStore.model]);

  React.useEffect(() => {
    tryInitializeUserRights();
  }, [tryInitializeUserRights]);

  return (
    <AccessRightsContext.Provider
      value={{
        userRights,
        appUser: user,
        initialize,
      }}
    >
      {props.children}
    </AccessRightsContext.Provider>
  );
};

export default AccessRightContextProvider;
