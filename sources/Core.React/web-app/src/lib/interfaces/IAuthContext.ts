import { LoginRequestModel } from '../types/LoginModel';
import { IAppUser } from './IAppUser';

export interface IAuthContext {
  isAuthenticated: boolean;
  currentUser: IAppUser | null;
  isLoading: boolean;
  login: (model: LoginRequestModel) => Promise<void>;
  logout: () => void;
}
