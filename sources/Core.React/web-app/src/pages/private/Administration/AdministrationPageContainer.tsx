import React from 'react';
import SettingsPageLayout, { INavigationListItem } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';
import FamilyAdministrationContainer from './FamilyAdministration/FamilyAdministrationContainer';
import FileImportContainer from './FileImport/FileImportContainer';

const AdministrationPageContainer: React.FC = () => {
  const { accessRights } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState(false);

  const localizationProps = AppHooks.useLocalisationProps(['common']);

  const listItems: INavigationListItem[] = [
    {
      key: 'family-administration',
      title: localizationProps.getResource('common.captionFamilyAdministration'),
      subTitle: localizationProps.getResource('common.labelManageFamilySettings'),
      isReadonly: !accessRights.accessRights.familyAdministration.view,
      component: FamilyAdministrationContainer,
    },
    {
      key: 'file-import',
      title: localizationProps.getResource('common.captionFileImport'),
      subTitle: localizationProps.getResource('common.labelManageFileImports'),
      isReadonly: !accessRights.accessRights.userAdministration.view,
      component: FileImportContainer,
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
