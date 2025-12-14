import React from 'react';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IComponentInitializationProps } from './interfaces/IComponentInitializationProps';
import FamilyAdministration from './FamilyAdministration';
import { IFamilyModel } from './interfaces/IFamilyModel';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { INotificationResponse } from 'src/lib/interfaces/INotificationResponse';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const FamilyAdministrationContainer: React.FC<IProps> = (props: IProps) => {
  const { accessRights } = useAccessRights();
  const initializeAsync = React.useCallback(async (): Promise<IComponentInitializationProps> => {
    const familyApi = AppHooks.statelessApi.create<IFamilyModel[], IFamilyModel[]>();

    const fileApi = AppHooks.statelessApi.create<INotificationResponse, any>();
    const [families] = await Promise.all([
      await familyApi.get('/familyadministration/getfamilies'),
    ]);

    return {
      familyApi,
      fileApi,
      isLoading: props.isLoading,
      setIsLoading: props.setIsLoading,
      getResource: props.getResource,
      families,
    };
  }, [props.isLoading, props.setIsLoading, props.getResource]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<IComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }

  return <FamilyAdministration {...initializationProps} isReadonly={accessRights.isAdmin} />;
};

export default FamilyAdministrationContainer;
