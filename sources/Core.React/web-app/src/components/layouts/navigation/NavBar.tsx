import React, { useState } from 'react';
import { IAppUser } from 'src/lib/interfaces/IAppUser';
import styles from './navbar.module.css';
import { useNavigate } from 'react-router-dom';
import { useAuth } from 'src/hooks/useAuth';
import LoadingIndicator from '../loading/LoadingIndicator';

interface IProps {
  user: IAppUser | null;
  isLoading: boolean;
}

const NavBar: React.FC<IProps> = (props: IProps) => {
  const { user, isLoading } = props;
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  if (user == null) {
    navigate('/auth');
    return null;
  }

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <nav className={styles.navbar}>
      {/* brand aligned start */}
      <div className={styles.brand}>
        <a href="/home" className={styles.brandLink}>
          <i className="bi bi-house"></i>
          <span className={styles.brandText}>AppName</span>
        </a>
      </div>

      {/* Hamburger menu button for mobile */}
      <button className={styles.hamburger} onClick={toggleMenu} aria-label="Toggle menu">
        <span className={styles.hamburgerLine}></span>
        <span className={styles.hamburgerLine}></span>
        <span className={styles.hamburgerLine}></span>
      </button>

      {/* Nav item container */}
      <div className={`${styles.navItems} ${isMenuOpen ? styles.navItemsOpen : ''}`}>
        <ul className={styles.navList}>
          <li className={styles.navItem}>
            <a href="/home" className={styles.navLink}>
              Item-1
            </a>
          </li>
          <li className={styles.navItem}>
            <a href="/home" className={styles.navLink}>
              Item-2
            </a>
          </li>
          <li className={styles.navItem}>
            <a href="/home" className={styles.navLink}>
              Item-3
            </a>
          </li>
        </ul>
      </div>

      {/* user section aligned end*/}
      <div className={styles.userSection}>
        <div className={styles.userName}>
          <i className="bi bi-person-circle"></i>
          <span>{user.userName}</span>
        </div>
        <div className={styles.logoutBtn} onClick={logout}>
          <i className="bi bi-box-arrow-right"></i>
        </div>
      </div>
      <LoadingIndicator isLoading={isLoading} />
    </nav>
  );
};

export default NavBar;
