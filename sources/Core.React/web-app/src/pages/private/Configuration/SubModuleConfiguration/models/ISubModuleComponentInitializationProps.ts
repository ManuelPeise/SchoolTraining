import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import { ISubModuleConfigurationInitializationModel } from './ISubModuleConfigurationInitializationModel';
import { ISubModuleBase } from '../../models/subModule';

export interface ISubModuleComoponentInitializationProps {
  saveApi: IStatelessApi<
    INotificationDataResponse<ISubModuleConfigurationInitializationModel>,
    ISubModuleBase
  >;
  deleteApi: IStatelessApi<
    INotificationDataResponse<ISubModuleConfigurationInitializationModel>,
    void
  >;
  model: ISubModuleConfigurationInitializationModel;
  getResource: (key: string) => string;
  setIsLoading: (isLoading: boolean) => void;
  isReadonly: boolean;
}
