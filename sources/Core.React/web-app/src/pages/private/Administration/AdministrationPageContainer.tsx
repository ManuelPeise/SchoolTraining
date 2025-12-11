import React from 'react';
import SettingsPageLayout, { INavigationListItem } from 'src/components/layouts/SettingsPageLayout';
import { AppHooks } from 'src/hooks/AppHooks';
import { useAccessRights } from 'src/hooks/useAccessRights';

const AdministrationPageContainer: React.FC = () => {
  const { accessRights } = useAccessRights();
  const [isLoading, setIsLoading] = React.useState(false);

  const localizationProps = AppHooks.UseLocalisationProps(['common']);

  const listItems: INavigationListItem[] = [
    {
      key: 'family-administration',
      title: 'Family Administration',
      subTitle: 'Manage family settings',
      isReadonly: !accessRights.accessRights.familyAdministration.view,
      component: () => (
        <div>
          <div>Family Administration Component</div>
          <div>
            <button onClick={setIsLoading.bind(null, !isLoading)}>Click me</button>
          </div>
        </div>
      ),
    },
    {
      key: '/user-administration',
      title: 'User Administration',
      subTitle: 'Manage user accounts',
      isReadonly: !accessRights.accessRights.userAdministration.view,
      component: () => <div>User Administration Component</div>,
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
