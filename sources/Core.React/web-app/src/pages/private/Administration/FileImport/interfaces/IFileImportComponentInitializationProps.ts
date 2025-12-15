import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { IFileImportModel } from './IFileImportModel';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';

export interface IFileImportComponentInitializationProps {
  fileImportApi: IStatelessApi<IFileImportModel[], void>;
  executeFileImportApi: IStatelessApi<INotificationDataResponse<IFileImportModel[]>, void>;
  fileModels: IFileImportModel[];
  isReadonly: boolean;
  getResource: (key: string) => string;
}
