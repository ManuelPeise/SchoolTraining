import { LogLevelEnum } from 'src/lib/enums/LogLevelEnum';

export interface ILogMessage {
  id: number;
  message: string;
  exeptionMessage: string;
  module?: string;
  stackTrace?: string;
  timeStamp: string;
  logLevel: LogLevelEnum;
}
