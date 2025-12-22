import { IAppUser } from './IAppUser';
import { IUserRight } from './IUserRight';

export type UserRights = {
  isLocalAdmin: boolean;
  isSystemAdmin: boolean;
  familyAdministrationRight: IUserRight;
  moduleAdministrationRight: IUserRight;
  subModuleAdministrationRight: IUserRight;
};

export interface IAccessRightsContext {
  userRights: UserRights;
  appUser: IAppUser | null;
  initialize: () => void;
}
