import React, { PropsWithChildren } from 'react';
import ListItem from '@mui/material/ListItem';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

interface IProps extends PropsWithChildren {
  title?: string;
  subTitle?: string;
}

const FormListItem: React.FC<IProps> = (props: IProps) => {
  const { title, subTitle, children } = props;

  return (
    <ListItem
      sx={{
        width: '100%',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        borderBottom: '1px solid',
        borderColor: 'divider',
        px: 2,
        py: 1.5,
      }}
      disableGutters
    >
      {title && (
        <Box width="100%" sx={{ flex: 0 }}>
          <Typography variant="subtitle1" component="div" noWrap sx={{ maxWidth: '100%' }}>
            {title}
          </Typography>
          {subTitle && (
            <Typography variant="body2" color="text.secondary" noWrap sx={{ maxWidth: '100%' }}>
              {subTitle}
            </Typography>
          )}
        </Box>
      )}
      <Box width="100%">{children}</Box>
    </ListItem>
  );
};

export default React.memo(FormListItem);
