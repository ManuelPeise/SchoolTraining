import React from 'react';
import ListItem from '@mui/material/ListItem';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import Icon from '@mui/material/Icon';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import { IIconButtonProps } from 'src/lib/interfaces/IIconButtonProps';

interface IHeaderListItemProps {
  title: string;
  subTitle?: string;
  iconButtonProps?: IIconButtonProps[];
}

const HeaderListItem: React.FC<IHeaderListItemProps> = (props: IHeaderListItemProps) => {
  const { title, subTitle, iconButtonProps } = props;

  return (
    <ListItem
      sx={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        px: 2,
        py: 1.5,
        borderBottom: '1px solid',
        borderColor: 'divider',
      }}
      disableGutters
      secondaryAction={
        iconButtonProps && (
          <Stack direction="row" sx={{ paddingRight: '2rem' }}>
            {iconButtonProps.map((iconButtonProp, index) => (
              <Box key={index}>
                <IconButton
                  onClick={iconButtonProp.onClick}
                  size="small"
                  sx={{ p: 1 }}
                  disabled={iconButtonProp.disabled}
                  title={iconButtonProp.tooltip}
                >
                  <Icon
                    className={iconButtonProp.icon}
                    sx={{ fontSize: iconButtonProp.size ?? 16, color: iconButtonProp.color }}
                  />
                </IconButton>
                {iconButtonProp.inputRef && (
                  <input
                    type="file"
                    ref={iconButtonProp.inputRef}
                    style={{ display: 'none' }}
                    onChange={iconButtonProp.fileUploadCallback}
                  />
                )}
              </Box>
            ))}
          </Stack>
        )
      }
    >
      <Box>
        <Typography variant="h6" component="div">
          {title}
        </Typography>
        {subTitle && (
          <Typography variant="subtitle2" color="text.secondary">
            {subTitle}
          </Typography>
        )}
      </Box>
    </ListItem>
  );
};

export default React.memo(HeaderListItem);
