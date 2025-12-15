import React from 'react';
import PageLayout from 'src/components/layouts/PageLayout';
import { useForm } from 'src/hooks/useForm';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import Checkbox from '@mui/material/Checkbox';

const HomePage: React.FC = () => {
  const [text, setText] = React.useState('');

  const user: User = {
    id: '1',
    name: 'John Doe',
    email: '0',
    secret: {
      id: 's1',
      content: 'My secret content',
    },
    friends: [
      { id: 'f1', name: 'Alice', checked: true },
      { id: 'f2', name: 'Bob', checked: false },
    ],
  };

  const { values, handleChange, resetForm, updateArrayItem } = useForm<User>(user);

  const handleAddFriend = React.useCallback(() => {
    handleChange('friends', [
      ...values.friends,
      { id: `f${values.friends.length + 1}`, name: text, checked: false },
    ]);
    setText('');
  }, [handleChange, values.friends, text]);

  return (
    <PageLayout isLoading={false}>
      <Box display="flex" flexDirection="column" gap={3}>
        <Box p={3}>
          <Typography variant="h4" component="h1">
            Home Page
          </Typography>
        </Box>
        <Box display="flex" gap={2} alignItems="center">
          <TextField
            label="Friend name"
            value={text}
            onChange={(e) => setText(e.target.value)}
            size="small"
            sx={{ minWidth: 200 }}
          />
          <Button variant="contained" onClick={handleAddFriend}>
            Add
          </Button>
          <Button variant="outlined" color="secondary" onClick={resetForm}>
            Revert
          </Button>
        </Box>
        <Box>
          <List>
            {values.friends.map((friend, idx) => (
              <ListItem
                key={friend.id}
                secondaryAction={
                  <Checkbox
                    checked={friend.checked}
                    onChange={(e) => {
                      updateArrayItem('friends', idx, {
                        ...friend,
                        checked: e.target.checked,
                      });
                    }}
                  />
                }
              >
                <ListItemText primary={friend.name} />
              </ListItem>
            ))}
          </List>
        </Box>
      </Box>
    </PageLayout>
  );
};

export default HomePage;

type Secret = {
  id: string;
  content: string;
};

type Friend = {
  id: string;
  name: string;
  checked: boolean;
};

type User = {
  id: string;
  name: string;
  email: string;
  secret: Secret;
  friends: Friend[];
};
