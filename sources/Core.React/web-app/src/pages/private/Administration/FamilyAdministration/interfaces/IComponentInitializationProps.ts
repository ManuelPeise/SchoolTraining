import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { IFamilyModel } from './IFamilyModel';

export interface IComponentInitializationProps extends ISettingsPageLayoutProps, ILocationProps {
  getResource: (key: string) => string;
  api: IStatelessApi<any, any>;
  families: IFamilyModel[];
}
