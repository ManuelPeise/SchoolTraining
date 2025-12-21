import { ISubModuleBase } from './subModule';

export interface IModule {
  moduleId: number;
  idExternal: string;
  title: string;
  description: string;
  lastUpdateBy: string | null;
  lastUpdateAt: string | null;
  subModules: ISubModuleBase[];
}
