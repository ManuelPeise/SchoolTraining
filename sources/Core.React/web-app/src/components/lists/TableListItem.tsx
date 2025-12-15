import React, { PropsWithChildren } from 'react';
import ListItem from '@mui/material/ListItem';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

interface IProps extends PropsWithChildren {
  title: string;
  subTitle?: string;
}

const TableListItem: React.FC<IProps> = (props: IProps) => {
  const { title, subTitle, children } = props;

  return (
    <ListItem
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'flex-start',
        px: 2,
        py: 1.5,
        borderColor: 'divider',
      }}
      disableGutters
    >
      <Box sx={{ mb: 1 }}>
        <Typography variant="subtitle1" component="div">
          {title}
        </Typography>
        {subTitle && (
          <Typography variant="body2" color="text.secondary">
            {subTitle}
          </Typography>
        )}
      </Box>
      <Box sx={{ width: '100%', py: 2 }}>{children}</Box>
    </ListItem>
  );
};

export default React.memo(TableListItem);
