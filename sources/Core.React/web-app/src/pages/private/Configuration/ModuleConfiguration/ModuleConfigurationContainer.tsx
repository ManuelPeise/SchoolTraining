import React from 'react';
import ModuleConfiguration from './ModuleConfiguration';
import { ISettingsPageLayoutProps } from 'src/components/layouts/SettingsPageLayout';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IModuleComponentInitializationProps } from './models/IModuleComponentInitializationProps';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';

interface IProps extends ISettingsPageLayoutProps, ILocationProps {}

const ModuleConfigurationContainer: React.FC<IProps> = (props: IProps) => {
  const { getResource, setIsLoading } = props;
  const { accessRights } = useAccessRights();

  const initializeAsync =
    React.useCallback(async (): Promise<IModuleComponentInitializationProps> => {
      return {
        isReadonly: !accessRights.accessRights.moduleConfiguration.edit,
        modules: [],
        modulesDropdownItems: [
          { id: 1, label: 'Module 1' },
          { id: 2, label: 'Module 2' },
        ],
        subModules: [],
        subModulesDropdownItems: [
          { id: 1, label: 'SubModule 1' },
          { id: 2, label: 'SubModule 2' },
        ],
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
