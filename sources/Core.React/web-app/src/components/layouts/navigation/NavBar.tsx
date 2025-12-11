import React from 'react';
import { IAppUser } from 'src/lib/interfaces/IAppUser';
import styles from '../layout.module.css';
import { useNavigate } from 'react-router-dom';

interface IProps {
  user: IAppUser | null;
}

const NavBar: React.FC<IProps> = (props: IProps) => {
  const { user } = props;
  const navigate = useNavigate();

  if (user == null) {
    navigate('/auth');
    return null;
  }

  return (
    <div className={styles.navbar}>
      <div className={styles.navbarBrand}>
        <a href="/home">
          <i className="bi bi-house"></i>
          <span>AppName</span>
        </a>
      </div>
      <div></div>
      {/* Right side user info and logout TODO style*/}
      <div>
        <div className={styles.navbarUser}>
          <i className="bi bi-person-circle"></i>
          <span>{user.userName}</span>
        </div>
        <div>
          <i className="bi bi-box-arrow-right"></i>
        </div>
      </div>
    </div>
  );
};

export default NavBar;
