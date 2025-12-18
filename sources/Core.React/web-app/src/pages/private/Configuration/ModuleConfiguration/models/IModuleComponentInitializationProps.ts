import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import { Module } from './Module';

export interface IModuleComponentInitializationProps {
  api: IStatelessApi<INotificationDataResponse<ModuleInitializationModel>, Module>;
  isReadonly: boolean;
  moduleInitializationModel: ModuleInitializationModel;
  getResource: (key: string) => string;
  setIsLoading: (isLoading: boolean) => void;
}

export type ModuleInitializationModel = {
  modules: Module[];
  subModules: Module[];
};
