import React from 'react';
import SettingsPageLayout, { INavigationListItem } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';
import SubModuleConfigurationContainer from './SubModuleConfiguration/SubModuleConfigurationContainer';
import ModuleConfigurationContainer from './ModuleConfiguration/ModuleConfigurationContainer';

const ConfigurationContainer: React.FC = () => {
  const { userRights } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState(false);
  const localizationProps = AppHooks.useLocalisationProps(['common']);

  const listItems: INavigationListItem[] = [];

  if (userRights.moduleAdministrationRight.view) {
    listItems.push({
      key: 'module-configuration',
      title: localizationProps.getResource('common.captionModules'),
      subTitle: localizationProps.getResource('common.labelManageModules'),
      isReadonly: !userRights.moduleAdministrationRight.view,
      component: ModuleConfigurationContainer,
    });

    listItems.push({
      key: 'submodule-configuration',
      title: localizationProps.getResource('common.captionSubModules'),
      subTitle: localizationProps.getResource('common.labelManageSubModules'),
      isReadonly: !userRights.subModuleAdministrationRight.view,
      component: SubModuleConfigurationContainer,
    });
  }

  return (
    <SettingsPageLayout
      isLoading={isLoading}
      {...localizationProps}
      setIsLoading={setIsLoading}
      listItems={listItems}
    />
  );
};

export default ConfigurationContainer;
