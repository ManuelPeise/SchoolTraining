export interface IModuleAccessRight {
  moduleName: string;
  view: boolean;
  create: boolean;
  edit: boolean;
  delete: boolean;
  canView: boolean;
  canCreate: boolean;
  canEdit: boolean;
  canDelete: boolean;
}
