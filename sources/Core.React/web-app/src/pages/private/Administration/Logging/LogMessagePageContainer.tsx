import React from 'react';
import { ILogComponentInitializationProps } from './interfaces/ILogComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import LogPage from './LogPage';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { useAccessRights } from 'src/hooks/useAccessRights';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const LogMessagePageContainer: React.FC<IProps> = (props: IProps) => {
  const { isLoading, setIsLoading, getResource } = props;
  const { accessRights } = useAccessRights();

  const initializeAsync = React.useCallback(async (): Promise<ILogComponentInitializationProps> => {
    return {
      isReadonly: !accessRights.isAdmin || !accessRights.isSystemAdmin,
      isLoading,
      setIsLoading,
      getResource,
    };
  }, []);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<ILogComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }

  return <LogPage {...initializationProps} />;
};

export default LogMessagePageContainer;
