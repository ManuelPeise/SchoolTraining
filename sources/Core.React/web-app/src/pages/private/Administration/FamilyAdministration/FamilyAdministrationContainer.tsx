import React from 'react';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IComponentInitializationProps } from './interfaces/IComponentInitializationProps';
import FamilyAdministration from './FamilyAdministration';
import { IFamilyModel } from './interfaces/IFamilyModel';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const FamilyAdministrationContainer: React.FC<IProps> = (props: IProps) => {
  const initializeAsync = React.useCallback(async (): Promise<IComponentInitializationProps> => {
    const api = AppHooks.statelessApi.create<IFamilyModel[], IFamilyModel[]>();
    // make some api calls here

    const [families] = await Promise.all([await api.get('/familyadministration/getfamilies')]);
    return {
      api,
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

  return <FamilyAdministration {...initializationProps} />;
};

export default FamilyAdministrationContainer;
