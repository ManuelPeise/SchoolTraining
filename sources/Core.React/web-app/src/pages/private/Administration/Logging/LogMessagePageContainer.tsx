import React from 'react';
import { ILogComponentInitializationProps } from './interfaces/ILogComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import LogPage from './LogPage';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { ILogMessage } from './interfaces/ILogMessage';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const LogMessagePageContainer: React.FC<IProps> = (props: IProps) => {
  const { isLoading, setIsLoading, getResource } = props;
  const { accessRights } = useAccessRights();

  const initializeAsync = React.useCallback(async (): Promise<ILogComponentInitializationProps> => {
    const messageLogApi = AppHooks.statelessApi.create<ILogMessage[], void>();
    const messageLogDeleteApi = AppHooks.statelessApi.create<
      INotificationDataResponse<ILogMessage[]>,
      void
    >();

    const [logMessages] = await Promise.all([messageLogApi.get('/messagelog/getmessagelogs')]);

    return {
      messageLogDeleteApi,
      logMessages,
      isReadonly: !accessRights.isSystemAdmin,
      isLoading,
      setIsLoading,
      getResource,
    };
  }, [accessRights, getResource, isLoading, setIsLoading]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<ILogComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }

  return <LogPage {...initializationProps} />;
};

export default LogMessagePageContainer;
