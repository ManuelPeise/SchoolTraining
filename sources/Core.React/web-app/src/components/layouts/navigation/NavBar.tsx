import React, { useState } from 'react';
import { IAppUser } from 'src/lib/interfaces/IAppUser';
import { useNavigate } from 'react-router-dom';
import { useAuth } from 'src/hooks/useAuth';
import LoadingIndicator from '../loading/LoadingIndicator';
import { UserRoleEnum } from 'src/lib/enums/UserRoleEnum';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Avatar from '@mui/material/Avatar';
import Tooltip from '@mui/material/Tooltip';
import Divider from '@mui/material/Divider';
import { useLocationProps } from 'src/hooks/useLocationProps';

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
  const { getResource } = useLocationProps(['common']);
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
        title: getResource('common.labelAdministration'),
        iconClassName: 'bi bi-gear',
        link: '/administration',
        sortOrder: 0,
      });
    }

    if (userRoleValue === UserRoleEnum.Admin || userRoleValue === UserRoleEnum.SystemAdmin) {
      items.push({
        title: getResource('common.labelConfiguration'),
        iconClassName: 'bi bi-database-gear',
        link: '/configuration',
        sortOrder: 0,
      });
    }
    return items.sort((a, b) => a.sortOrder - b.sortOrder);
  }, [user, getResource]);

  if (user == null) {
    navigate('/auth');
    return null;
  }

  return (
    <>
      <AppBar
        position="static"
        color="default"
        enableColorOnDark
        elevation={2}
        sx={{ zIndex: 1201 }}
      >
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', minHeight: 64 }}>
          {/* Left: Hamburger and Brand */}
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <IconButton
              edge="start"
              color="inherit"
              aria-label="menu"
              onClick={toggleMenu}
              sx={{ mr: 2 }}
            >
              <i className="bi bi-list"></i>
            </IconButton>
            <Box
              sx={{ display: 'flex', alignItems: 'center', cursor: 'pointer' }}
              onClick={() => navigateTo('/home')}
            >
              <i className="bi bi-house" style={{ fontSize: 22, marginRight: 8 }}></i>
              <Typography variant="h6" noWrap sx={{ fontWeight: 700 }}>
                AppName
              </Typography>
            </Box>
          </Box>
          {/* Right: User section */}
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <Tooltip title={user.userName} arrow>
              <Avatar sx={{ bgcolor: 'primary.main', width: 36, height: 36 }}>
                <i className="bi bi-person-circle" style={{ fontSize: 22 }}></i>
              </Avatar>
            </Tooltip>
            <IconButton color="inherit" onClick={handleLogout} aria-label="Logout">
              <i className="bi bi-box-arrow-right" style={{ fontSize: 22 }}></i>
            </IconButton>
          </Box>
        </Toolbar>
      </AppBar>
      {/* Drawer for menu */}
      <Drawer
        anchor="left"
        open={isMenuOpen}
        onClose={toggleMenu}
        ModalProps={{ keepMounted: true }}
        sx={{
          '& .MuiDrawer-paper': { width: 260, boxSizing: 'border-box' },
        }}
      >
        <Box sx={{ p: 2, pb: 0 }}>
          <Typography variant="h6" sx={{ fontWeight: 700 }}>
            Menu
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Subtitle
          </Typography>
        </Box>
        <Divider sx={{ my: 1 }} />
        <List>
          {sideMenuItems.map((item) => (
            <ListItem key={item.link} disablePadding>
              {/* colloapsible? */}
              <ListItemButton onClick={() => navigateTo(item.link)}>
                <ListItemIcon>
                  <i className={item.iconClassName} style={{ fontSize: 20 }}></i>
                </ListItemIcon>
                <ListItemText primary={item.title} />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
      </Drawer>
      <LoadingIndicator isLoading={isLoading} />
    </>
  );
};

export default NavBar;
