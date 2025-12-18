import React from 'react';
import ModuleConfiguration from './ModuleConfiguration';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import {
  IModuleComponentInitializationProps,
  ModuleInitializationModel,
} from './models/IModuleComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import { Module } from './models/Module';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const ModuleConfigurationContainer: React.FC<IProps> = (props: IProps) => {
  const { getResource, setIsLoading } = props;
  const { accessRights } = useAccessRights();

  const initializeAsync =
    React.useCallback(async (): Promise<IModuleComponentInitializationProps> => {
      const initializationApi = AppHooks.statelessApi.create<
        INotificationDataResponse<ModuleInitializationModel>,
        void
      >();

      const api = AppHooks.statelessApi.create<
        INotificationDataResponse<ModuleInitializationModel>,
        Module
      >();
      const [moduleInitializationModel] = await Promise.all([
        initializationApi.get('/moduleconfiguration/getmoduleconfiguration'),
      ]);

      return {
        api,
        isReadonly: !accessRights.accessRights.moduleConfiguration.edit,
        moduleInitializationModel: moduleInitializationModel.data,
        getResource: getResource,
        setIsLoading: setIsLoading,
      };
    }, [getResource, setIsLoading, accessRights]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<IModuleComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }
  return <ModuleConfiguration {...initializationProps} getResource={getResource} />;
};

export default ModuleConfigurationContainer;
