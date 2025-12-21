import React from 'react';
import PageLayout from 'src/components/layouts/PageLayout';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import HomePageForm from './HomePageForm';
import { Form } from 'src/hooks/form/Form';

type Friend = {
  name: string;
  lastName: string;
};

export type TestModel = {
  name: string;
  lastName: string;
  friend: Friend | null;
};

const HomePage: React.FC = () => {
  const form = Form.create<TestModel>((factory) => [
    factory.createStringSettings({ key: 'name', type: 'text', isRequired: true }, 'First Name'),
    factory.createStringSettings({ key: 'lastName', type: 'text' }, 'Last Name'),
  ]);

  const friendForm = form.usePartialForm<TestModel, Friend>((factory) => [
    factory.createStringSettings(
      { key: 'name', type: 'text', isRequired: true },
      'Friend First Name'
    ),
    factory.createStringSettings(
      { key: 'lastName', type: 'text', isRequired: true },
      'Friend Last Name'
    ),
  ]);

  React.useEffect(() => {
    form.useModel({
      name: 'John',
      lastName: 'Doe',
      friend: { name: 'Jane', lastName: 'Smith' },
    });

    friendForm.useModel({ name: 'Jane', lastName: 'Smith' });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <PageLayout isLoading={false}>
      <Box display="flex" flexDirection="column" gap={3}>
        <Box p={3}>
          <Typography variant="h4" component="h1">
            Home Page
          </Typography>
        </Box>
        <Box p={3}>
          <HomePageForm form={form} />
          <friendForm.TextField
            fieldKey="name"
            fieldPropsCallback={friendForm.getFieldPropsByKey}
          />
        </Box>
      </Box>
    </PageLayout>
  );
};

export default HomePage;
