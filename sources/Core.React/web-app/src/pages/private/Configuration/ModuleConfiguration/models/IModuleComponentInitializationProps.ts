import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import { IModule } from '../../models/module';

export interface IModuleComponentInitializationProps {
  saveApi: IStatelessApi<INotificationDataResponse<IModule[]>, IModule>;
  deleteApi: IStatelessApi<INotificationDataResponse<IModule[]>, void>;
  isReadonly: boolean;
  modules: IModule[];
  getResource: (key: string) => string;
  setIsLoading: (isLoading: boolean) => void;
}
