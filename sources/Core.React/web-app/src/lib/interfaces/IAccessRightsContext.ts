import { IAccessRights } from './IAccessRights';
import { IAppUser } from './IAppUser';

export interface IAccessRightsContext {
  accessRights: IAccessRights;
  appUser: IAppUser | null;
  initialize: (jwt: string) => void;
}
