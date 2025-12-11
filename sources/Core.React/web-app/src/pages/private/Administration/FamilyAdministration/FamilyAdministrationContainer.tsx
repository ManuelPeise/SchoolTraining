import React from 'react';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

interface IComponentInitializationProps extends IProps {
  getResource: (key: string) => string;
  api: IStatelessApi<any, any>;
}

const FamilyAdministrationContainer: React.FC<IProps> = (props: IProps) => {
  const initializeAsync = React.useCallback(async (): Promise<IComponentInitializationProps> => {
    // create a api service client
    const api = AppHooks.statelessApi.create<any, any>();
    // make some api calls here
    // const [] = await Promise.all([]);

    return {
      api,
      isLoading: props.isLoading,
      setIsLoading: props.setIsLoading,
      getResource: props.getResource,
    };
  }, [props.isLoading, props.setIsLoading, props.getResource]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<IComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }
  return (
    <div {...initializationProps}>
      <p>Family Administration Container</p>
      <p>{initializationProps.isLoading ? 'Loading...' : 'Loaded'}</p>
    </div>
  );
};

export default FamilyAdministrationContainer;
