import React from 'react';
import SettingsPageLayout, { INavigationListItem } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';
import ModuleConfigurationContainer from './ModuleConfiguration/ModuleConfigurationContainer';

const ConfigurationContainer: React.FC = () => {
  const { accessRights } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState(false);
  const localizationProps = AppHooks.useLocalisationProps(['common']);

  const listItems: INavigationListItem[] = [];

  if (accessRights.accessRights.moduleConfiguration.view) {
    listItems.push({
      key: 'module-configuration',
      title: localizationProps.getResource('common.captionModuleConfiguration'),
      subTitle: localizationProps.getResource('common.labelManageModuleConfiguration'),
      isReadonly: !accessRights.accessRights.familyAdministration.view,
      component: ModuleConfigurationContainer,
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
