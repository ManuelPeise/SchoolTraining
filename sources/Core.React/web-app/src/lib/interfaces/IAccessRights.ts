import { IModuleAccessRight } from './IModuleAccessRight';

export interface IAccessRights {
  isAdmin: boolean;
  isSystemAdmin: boolean;
  accessRights: IModuleAccessRights;
}
interface IModuleAccessRights {
  familyAdministration: IModuleAccessRight;
  userAdministration: IModuleAccessRight;
}
