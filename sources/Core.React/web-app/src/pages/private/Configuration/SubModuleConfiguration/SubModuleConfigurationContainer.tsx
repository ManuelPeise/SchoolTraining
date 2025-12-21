import React from 'react';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { ISubModuleComoponentInitializationProps } from './models/ISubModuleComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import SubModuleConfigurationForm from './SubModuleConfigurationForm';
import { ISubModuleConfigurationInitializationModel } from './models/ISubModuleConfigurationInitializationModel';
import { ISubModuleBase } from '../models/subModule';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const SubModuleConfigurationContainer: React.FC<IProps> = (props: IProps) => {
  const { getResource, setIsLoading } = props;
  const { accessRights } = useAccessRights();

  const initializeAsync =
    React.useCallback(async (): Promise<ISubModuleComoponentInitializationProps> => {
      const initializationApi = AppHooks.statelessApi.create<
        ISubModuleConfigurationInitializationModel,
        void
      >();

      const saveApi = AppHooks.statelessApi.create<
        INotificationDataResponse<ISubModuleConfigurationInitializationModel>,
        ISubModuleBase
      >();

      const deleteApi = AppHooks.statelessApi.create<
        INotificationDataResponse<ISubModuleConfigurationInitializationModel>,
        void
      >();
      const [model] = await Promise.all([
        initializationApi.get('/moduleconfiguration/getsubmoduleconfigurations'),
      ]);

      return {
        saveApi,
        deleteApi,
        isReadonly: !accessRights.isAdmin && !accessRights.isSystemAdmin,
        model,
        getResource,
        setIsLoading,
      };
    }, [getResource, setIsLoading, accessRights]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<ISubModuleComoponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }

  return <SubModuleConfigurationForm {...initializationProps} />;
};

export default SubModuleConfigurationContainer;
