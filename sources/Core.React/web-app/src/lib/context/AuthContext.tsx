import React, { createContext, useState, useCallback, ReactNode } from 'react';
import { IAuthContext } from 'src/lib/interfaces/IAuthContext';
import { ITokenStore } from '../interfaces/ITokenStore';
import { AppHooks } from 'src/hooks/AppHooks';
import { LoginRequestModel } from '../types/LoginModel';
import moment from 'moment';
import { useAccessRights } from 'src/hooks/useAccessRights';

const AuthContext = createContext<IAuthContext | null>(null);

type JwtTokenResponse = {
  jwt: string;
  refreshToken: string;
  expireSeconds: number;
};

type AuthProviderProps = {
  children: ReactNode;
};

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const { appUser, initialize } = useAccessRights();

  const [tokenStore, setTokenStore] = useState<ITokenStore | null>(null);

  const authenticationApi = AppHooks.StatelessApi.create<JwtTokenResponse, LoginRequestModel>();

  const login = useCallback(
    async (model: LoginRequestModel) => {
      const response = await authenticationApi.post('/login/authenticate', model);

      if (response) {
        setTokenStore({
          jwt: response.jwt,
          refreshToken: response.refreshToken,
          expiresAt: moment().add(response.expireSeconds, 'seconds'),
        });
        initialize(response.jwt);
        localStorage.setItem('jwt', response.jwt);
      }
    },
    [authenticationApi, initialize]
  );

  const logout = useCallback(() => {
    localStorage.removeItem('jwt');
    setTokenStore(null);
  }, []);

  const value: IAuthContext = {
    isAuthenticated: appUser != null,
    tokenStore,
    currentUser: appUser,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthContext;
