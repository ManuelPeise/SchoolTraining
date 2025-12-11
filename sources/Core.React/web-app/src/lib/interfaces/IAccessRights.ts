import { IModuleAccessRight } from './IModuleAccessRight';

export interface IAccessRights {
  isAdmin: boolean;
  isSystemAdmin: boolean;
  accessRights: IModuleAccessRight[];
}
