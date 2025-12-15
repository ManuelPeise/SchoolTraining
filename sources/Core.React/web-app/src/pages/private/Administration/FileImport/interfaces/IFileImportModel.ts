import { FileImportStatusEnum } from 'src/lib/enums/FileImportStatusEnum';
import { FileImportTypeEnum } from 'src/lib/enums/FileImportTypeEnum';

export interface IFileImportModel {
  fileId: number;
  fileName: string;
  fileDate: string;
  fileType: FileImportTypeEnum;
  status: FileImportStatusEnum;
  lastUpdate: string;
  lastUpdateBy: string;
}
