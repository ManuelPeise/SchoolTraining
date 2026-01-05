import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { IFamilyModel } from './IFamilyModel';
import { INotificationResponse } from 'src/lib/interfaces/INotificationResponse';

export interface IFamilyAdministrationComponentInitializationProps
  extends ISettingsPageLayoutProps, ILocationProps {
  getResource: (key: string) => string;
  isReadonly: boolean;
  familyApi: IStatelessApi<any, any>;
  fileApi: IStatelessApi<INotificationResponse, any>;
  families: IFamilyModel[];
}
