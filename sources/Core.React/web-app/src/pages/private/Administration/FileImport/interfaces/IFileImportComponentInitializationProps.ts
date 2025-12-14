import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { IFileImportModel } from './IFileImportModel';

export interface IFileImportComponentInitializationProps {
  fileImportApi: IStatelessApi<IFileImportModel[], void>;
  fileModels: IFileImportModel[];
  isReadonly: boolean;
  getResource: (key: string) => string;
}
