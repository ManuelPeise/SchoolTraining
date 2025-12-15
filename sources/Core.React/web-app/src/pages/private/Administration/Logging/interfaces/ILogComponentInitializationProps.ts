import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';

export interface ILogComponentInitializationProps extends ISettingsPageLayoutProps, ILocationProps {
  isReadonly: boolean;
  getResource: (key: string) => string;
}
