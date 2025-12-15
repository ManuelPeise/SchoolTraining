import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { ILogMessage } from './ILogMessage';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';

export interface ILogComponentInitializationProps extends ISettingsPageLayoutProps, ILocationProps {
  messageLogDeleteApi: IStatelessApi<INotificationDataResponse<ILogMessage[]>, void>;
  logMessages: ILogMessage[];
  isReadonly: boolean;
  getResource: (key: string) => string;
}
