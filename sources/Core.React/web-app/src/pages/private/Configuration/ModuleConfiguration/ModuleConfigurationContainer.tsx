import React from 'react';
import ModuleConfigurationForm from './ModuleConfigurationForm';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IModuleComponentInitializationProps } from './models/IModuleComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import { IModule } from '../models/module';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const ModuleConfigurationContainer: React.FC<IProps> = (props: IProps) => {
  const { getResource, setIsLoading } = props;
  const { userRights } = useAccessRights();

  const initializeAsync =
    React.useCallback(async (): Promise<IModuleComponentInitializationProps> => {
      const initializationApi = AppHooks.statelessApi.create<IModule[], void>();

      const saveApi = AppHooks.statelessApi.create<INotificationDataResponse<IModule[]>, IModule>();

      const deleteApi = AppHooks.statelessApi.create<INotificationDataResponse<IModule[]>, void>();
      const [modules] = await Promise.all([
        initializationApi.get('/moduleconfiguration/getmoduleconfigurations'),
      ]);

      return {
        saveApi,
        deleteApi,
        isReadonly: !userRights.moduleAdministrationRight.view,
        modules,
        getResource: getResource,
        setIsLoading: setIsLoading,
      };
    }, [getResource, setIsLoading, userRights]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<IModuleComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }
  return <ModuleConfigurationForm {...initializationProps} getResource={getResource} />;
};

export default ModuleConfigurationContainer;
