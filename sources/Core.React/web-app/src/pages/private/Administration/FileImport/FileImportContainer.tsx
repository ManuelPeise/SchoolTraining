import React from 'react';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { useAccessRights } from 'src/hooks/useAccessRights';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IFileImportComponentInitializationProps } from './interfaces/IFileImportComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import FileImport from './FileImport';
import { IFileImportModel } from './interfaces/IFileImportModel';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const FileImportContainer: React.FC<IProps> = (props: IProps) => {
  const { accessRights } = useAccessRights();

  const initializeAsync =
    React.useCallback(async (): Promise<IFileImportComponentInitializationProps> => {
      const fileImportApi = AppHooks.statelessApi.create<IFileImportModel[], void>();
      const executeFileImportApi = AppHooks.statelessApi.create<
        INotificationDataResponse<IFileImportModel[]>,
        void
      >();

      const [fileModels] = await Promise.all([await fileImportApi.get('/fileimport/getfiles')]);

      return {
        fileImportApi,
        executeFileImportApi,
        fileModels,
        isReadonly: !accessRights.isSystemAdmin,
        getResource: props.getResource,
      };
    }, [props.getResource, accessRights]);

  const { isInitialized, initializationProps } =
    AppHooks.useComponentMounting<IFileImportComponentInitializationProps>(initializeAsync);

  if (!isInitialized) {
    return null;
  }

  return <FileImport {...initializationProps} />;
};

export default FileImportContainer;
