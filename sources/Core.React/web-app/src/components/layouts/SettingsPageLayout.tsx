import React from 'react';
import { useAccessRights } from 'src/hooks/useAccessRights';
import NavBar from './navigation/NavBar';
import styles from './layout.module.css';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';

export interface ISettingsPageLayoutProps extends ILocationProps {
  isLoading: boolean;
  setIsLoading: (isLoading: boolean) => void;
}
export interface INavigationListItem {
  key: string;
  title: string;
  subTitle: string;
  isReadonly?: boolean;
  component: React.FC<ISettingsPageLayoutProps>;
}

interface IProps extends ILocationProps {
  isLoading: boolean;
  setIsLoading: (isLoading: boolean) => void;
  listItems: INavigationListItem[];
}

const SettingsPageLayout: React.FC<IProps> = (props: IProps) => {
  const { listItems, isLoading } = props;
  const { appUser } = useAccessRights();

  const [selectedItem, setSelectedItem] = React.useState<INavigationListItem>(listItems[0]);

  const handleItemClick = React.useCallback((item: INavigationListItem) => {
    if (!item.isReadonly) {
      setSelectedItem(item);
    }
  }, []);

  const Component = selectedItem.component;

  return (
    <div className={styles.pageLayout}>
      <div className={styles.header}>
        <NavBar user={appUser} isLoading={isLoading} />
      </div>
      <div className={styles.settingsPageMainContent}>
        {/* start style this */}
        <div id="settings-page-navigation" className={styles.settingsPageNavigation}>
          <ul className={styles.navList}>
            {listItems.map((item) => (
              <li
                key={item.key}
                onClick={() => handleItemClick(item)}
                className={`${styles.navListItem} ${
                  selectedItem.key === item.key ? styles.selected : ''
                } ${item.isReadonly ? styles.readonly : ''}`}
              >
                <h4 className={styles.navItemTitle}>{item.title}</h4>
                <h6 className={styles.navItemSubtitle}>{item.subTitle}</h6>
              </li>
            ))}
          </ul>
        </div>
        <div id="settings-page-content" className={styles.settingsPageContent}>
          <Component {...props} />
        </div>
        {/* end style this */}
      </div>
    </div>
  );
};

export default SettingsPageLayout;
