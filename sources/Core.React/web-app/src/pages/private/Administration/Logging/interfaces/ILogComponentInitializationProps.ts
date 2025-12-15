import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { ILogMessage } from './ILogMessage';

export interface ILogComponentInitializationProps extends ISettingsPageLayoutProps, ILocationProps {
  logMessages: ILogMessage[];
  isReadonly: boolean;
  getResource: (key: string) => string;
}
