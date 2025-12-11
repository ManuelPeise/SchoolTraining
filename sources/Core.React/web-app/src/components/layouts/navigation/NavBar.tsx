import React, { useState } from 'react';
import { IAppUser } from 'src/lib/interfaces/IAppUser';
import styles from './navbar.module.css';
import { useNavigate } from 'react-router-dom';
import { useAuth } from 'src/hooks/useAuth';
import LoadingIndicator from '../loading/LoadingIndicator';
import { UserRoleEnum } from 'src/lib/enums/UserRoleEnum';

export interface ISideMenuItem {
  title: string;
  iconClassName: string;
  link: string;
  sortOrder: number;
}

interface IProps {
  user: IAppUser | null;
  isLoading: boolean;
}

const NavBar: React.FC<IProps> = (props: IProps) => {
  const { user, isLoading } = props;
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const toggleMenu = React.useCallback(() => {
    setIsMenuOpen(!isMenuOpen);
  }, [isMenuOpen]);

  const navigateTo = React.useCallback(
    (link: string) => {
      navigate(link);
      setIsMenuOpen(false);
    },
    [navigate]
  );

  const handleLogout = React.useCallback(() => {
    logout();
    navigate('/auth');
  }, [logout, navigate]);

  const sideMenuItems: ISideMenuItem[] = React.useMemo(() => {
    const items: ISideMenuItem[] = [];

    if (user == null) {
      return items;
    }

    const userRoleValue =
      typeof user.userRole === 'string'
        ? UserRoleEnum[user.userRole as keyof typeof UserRoleEnum]
        : user.userRole;

    if (userRoleValue === UserRoleEnum.Admin || userRoleValue === UserRoleEnum.SystemAdmin) {
      items.push({
        title: 'Administration',
        iconClassName: 'bi bi-gear',
        link: '/administration',
        sortOrder: 0,
      });
    }
    return items.sort((a, b) => a.sortOrder - b.sortOrder);
  }, [user]);

  if (user == null) {
    navigate('/auth');
    return null;
  }

  return (
    <nav className={styles.navbar}>
      <div className={styles.navbarLeft}>
        {/* Hamburger menu button */}
        <button className={styles.hamburger} onClick={toggleMenu} aria-label="Toggle menu">
          <i className="bi bi-list"></i>
        </button>

        {/* brand aligned behind hamburger */}
        <div className={styles.brand} onClick={navigateTo.bind(null, '/home')}>
          <div className={styles.brandLink}>
            <i className="bi bi-house"></i>
            <span className={styles.brandText}>AppName</span>
          </div>
        </div>
      </div>
      {/* Nav item container */}
      <div className={`${styles.navItems} ${isMenuOpen ? styles.navItemsOpen : ''}`}>
        {/* Sidebar header */}
        <div className={styles.sidebarHeader}>
          <span className={styles.sidebarTitle}>Menu</span>
          <p>Subtitle</p>
        </div>

        <ul className={styles.navList}>
          {sideMenuItems.map((item) => (
            <li
              key={item.link}
              className={styles.navItem}
              onClick={navigateTo.bind(null, item.link)}
            >
              <div className={styles.navLink}>
                <i className={item.iconClassName}></i>
                <span className={styles.navLinkText}>{item.title}</span>
              </div>
            </li>
          ))}
        </ul>
      </div>

      {/* user section aligned end*/}
      <div className={styles.userSection}>
        <div className={styles.userName}>
          <i className="bi bi-person-circle"></i>
          <span>{user.userName}</span>
        </div>
        <div className={styles.logoutBtn} onClick={handleLogout}>
          <i className="bi bi-box-arrow-right"></i>
        </div>
      </div>
      <LoadingIndicator isLoading={isLoading} />
    </nav>
  );
};

export default NavBar;
