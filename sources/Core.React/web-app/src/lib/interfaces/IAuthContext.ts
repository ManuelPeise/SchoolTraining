import { LoginRequestModel } from '../types/LoginModel';
import { IAppUser } from './IAppUser';
import { ITokenStore } from './ITokenStore';

export interface IAuthContext {
  isAuthenticated: boolean;
  tokenStore: ITokenStore | null;
  currentUser: IAppUser | null;
  login: (model: LoginRequestModel) => Promise<void>;
  logout: () => void;
}
