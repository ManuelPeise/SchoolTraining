import React from 'react';
import { useAccessRights } from 'src/hooks/useAccessRights';
import NavBar from './navigation/NavBar';
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import List from '@mui/material/List';

import Typography from '@mui/material/Typography';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';

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
    <Box
      sx={{
        minHeight: '100vh',
        height: '100vh',
        display: 'flex',
        flexDirection: 'column',
        bgcolor: 'background.default',
        flex: 1,
      }}
    >
      <Box component="header" sx={{ width: '100%', boxShadow: 1, zIndex: 1100 }}>
        <NavBar user={appUser} isLoading={isLoading} />
      </Box>
      <Container
        maxWidth={false}
        disableGutters
        sx={{
          flex: 1,
          minHeight: 0,
          p: { xs: 1, sm: 2, md: 3 },
          display: 'flex',
          flexDirection: { xs: 'column', md: 'row' },
          gap: { xs: 2, md: 3 },
          alignItems: { xs: 'stretch', md: 'stretch' },
          boxSizing: 'border-box',
        }}
      >
        {/* Navigation */}
        <Box
          id="settings-page-navigation"
          sx={{
            width: { xs: '100%', md: 300 },
            maxWidth: { xs: '100%', md: 300 },
            minWidth: { md: 250 },
            flexShrink: 0,
            display: 'flex',
            flexDirection: 'column',
            bgcolor: 'background.paper',
            borderRadius: 2,
            boxShadow: 2,
            mb: { xs: 2, md: 0 },
            height: { xs: 'auto', md: '100%' },
          }}
        >
          <List sx={{ p: 0, m: 0, width: '100%' }}>
            {listItems.map((item) => (
              <ListItem key={item.key} disablePadding sx={{ width: '100%', padding: 0, m: 0 }}>
                <ListItemButton
                  onClick={() => handleItemClick(item)}
                  selected={selectedItem.key === item.key}
                  disabled={item.isReadonly}
                  sx={{
                    width: '100%',
                    bgcolor: selectedItem.key === item.key ? 'primary.main' : 'inherit',
                    color: selectedItem.key === item.key ? 'primary.contrastText' : 'text.primary',
                    opacity: item.isReadonly ? 0.5 : 1,
                    cursor: item.isReadonly ? 'not-allowed' : 'pointer',
                    '&:hover': {
                      bgcolor:
                        !item.isReadonly && selectedItem.key !== item.key
                          ? 'action.hover'
                          : undefined,
                    },
                  }}
                >
                  <Box>
                    <Typography variant="h6" sx={{ fontWeight: 600 }}>
                      {item.title}
                    </Typography>
                    <Typography variant="subtitle2" sx={{ color: 'text.secondary' }}>
                      {item.subTitle}
                    </Typography>
                  </Box>
                </ListItemButton>
              </ListItem>
            ))}
          </List>
        </Box>
        {/* Content */}
        <Box
          id="settings-page-content"
          sx={{
            flex: 1,
            bgcolor: 'background.paper',
            borderRadius: 2,
            boxShadow: 2,
            p: { xs: 2, sm: 3, md: 4 },
            minWidth: 0,
            minHeight: 300,
            height: '100%',
            overflow: 'auto',
          }}
        >
          <Component {...props} />
        </Box>
      </Container>
    </Box>
  );
};

export default SettingsPageLayout;
