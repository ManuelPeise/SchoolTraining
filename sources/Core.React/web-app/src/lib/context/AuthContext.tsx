import React, { createContext, useCallback, ReactNode } from 'react';
import { IAuthContext } from 'src/lib/interfaces/IAuthContext';
import { ITokenStore } from '../interfaces/ITokenStore';
import { AppHooks } from 'src/hooks/AppHooks';
import { LoginRequestModel } from '../types/LoginModel';
import moment from 'moment';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { LocalStorageKeyEnum } from '../enums/LocalStorageKeyEnum';
import { IUserRight } from '../interfaces/IUserRight';

const AuthContext = createContext<IAuthContext | null>(null);

type JwtTokenResponse = {
  userId: number;
  jwt: string;
  refreshToken: string;
  expiresAt: string;
};

type AuthProviderProps = {
  children: ReactNode;
};

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const { appUser, initialize } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState<boolean>(true);

  const tokenStore = AppHooks.useLocalStorage<ITokenStore>(LocalStorageKeyEnum.Jwt);
  const userRightStore = AppHooks.useLocalStorage<IUserRight[]>(LocalStorageKeyEnum.UserRights);

  const authenticationApi = AppHooks.statelessApi.create<JwtTokenResponse, LoginRequestModel>();
  const accessRightsApi = AppHooks.statelessApi.create<IUserRight[], void>();

  const login = useCallback(
    async (model: LoginRequestModel) => {
      const response = await authenticationApi.post('/login/authenticate', model);

      if (response) {
        tokenStore.setItem({
          userId: response.userId,
          jwt: response.jwt,
          refreshToken: response.refreshToken,
          expiresAt: moment(response.expiresAt).local(),
        });

        const userRightsResponse = await accessRightsApi.get(
          `/userrights/getuserrights?userId=${response.userId}`
        );

        if (userRightsResponse) {
          userRightStore.setItem(userRightsResponse);
        }

        initialize();
      }
    },
    [authenticationApi, accessRightsApi, tokenStore, userRightStore, initialize]
  );

  const logout = useCallback(() => {
    tokenStore.removeItem();
    userRightStore.removeItem();

    initialize();
  }, [initialize, tokenStore, userRightStore]);

  React.useEffect(() => {
    setIsLoading(true);
    const initializeSync = () => {
      initialize();
      setIsLoading(false);
    };
    initializeSync();
  }, [initialize]);

  const value: IAuthContext = {
    isAuthenticated: appUser != null,
    currentUser: appUser,
    isLoading,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthContext;
