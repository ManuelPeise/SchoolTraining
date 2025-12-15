import React, { PropsWithChildren } from 'react';
import { useAccessRights } from 'src/hooks/useAccessRights';
import NavBar from './navigation/NavBar';
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';

interface IProps extends PropsWithChildren {
  isLoading: boolean;
}

const PageLayout: React.FC<IProps> = (props: IProps) => {
  const { isLoading } = props;
  const { appUser } = useAccessRights();

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        flexDirection: 'column',
        bgcolor: 'background.default',
      }}
    >
      {/* Header */}
      <Box component="header" sx={{ width: '100%', boxShadow: 1, zIndex: 1100 }}>
        <NavBar user={appUser} isLoading={isLoading} />
      </Box>
      {/* Main content */}
      <Container maxWidth="lg" sx={{ flex: 1, py: 3 }}>
        {props.children}
      </Container>
    </Box>
  );
};

export default PageLayout;
