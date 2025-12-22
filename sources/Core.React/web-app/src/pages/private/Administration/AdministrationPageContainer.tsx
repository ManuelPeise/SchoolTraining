import React from 'react';
import SettingsPageLayout, { INavigationListItem } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';
import FamilyAdministrationContainer from './FamilyAdministration/FamilyAdministrationContainer';
import FileImportContainer from './FileImport/FileImportContainer';
import LogMessagePageContainer from './Logging/LogMessagePageContainer';

const AdministrationPageContainer: React.FC = () => {
  const { userRights } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState(false);

  const localizationProps = AppHooks.useLocalisationProps(['common']);

  const listItems: INavigationListItem[] = [
    {
      key: 'family-administration',
      title: localizationProps.getResource('common.captionFamilyAdministration'),
      subTitle: localizationProps.getResource('common.labelManageFamilySettings'),
      isReadonly: !userRights.familyAdministrationRight.view,
      component: FamilyAdministrationContainer,
    },
    {
      key: 'file-import',
      title: localizationProps.getResource('common.captionFileImport'),
      subTitle: localizationProps.getResource('common.labelManageFileImports'),
      isReadonly: !userRights.familyAdministrationRight.view,
      component: FileImportContainer,
    },
    {
      key: 'message-log',
      title: localizationProps.getResource('common.captionMessageLog'),
      subTitle: localizationProps.getResource('common.labelManageMessageLogs'),
      isReadonly: !userRights.familyAdministrationRight.view,
      component: LogMessagePageContainer,
    },
  ];

  return (
    <SettingsPageLayout
      isLoading={isLoading}
      {...localizationProps}
      setIsLoading={setIsLoading}
      listItems={listItems}
    />
  );
};

export default AdministrationPageContainer;
