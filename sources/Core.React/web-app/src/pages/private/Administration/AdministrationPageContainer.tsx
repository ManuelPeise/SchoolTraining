import React from 'react';
import SettingsPageLayout, { INavigationListItem } from 'src/components/layouts/SettingsPageLayout';
import { useAccessRights } from 'src/hooks/useAccessRights';

const AdministrationPageContainer: React.FC = () => {
  const { accessRights } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState(false);

  console.log('AdministrationPageContainer render', { accessRights });
  const listItems: INavigationListItem[] = [
    {
      key: 'family-administration',
      title: 'Family Administration',
      subTitle: 'Manage family settings',

      isReadonly: !accessRights.accessRights.familyAdministration.view,
      component: () => <div>Family Administration Component</div>,
    },
    {
      key: '/user-administration',
      title: 'User Administration',
      subTitle: 'Manage user accounts',
      isReadonly: !accessRights.accessRights.userAdministration.view,
      component: () => <div>User Administration Component</div>,
    },
  ];

  return <SettingsPageLayout isLoading={isLoading} listItems={listItems} />;
};

export default AdministrationPageContainer;
