import React from 'react';
import Box from '@mui/material/Box';
import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';
import FormCard from 'src/components/wrappers/FormCard';
import Title from 'src/components/labels/Title';
import { LoginModel } from 'src/lib/types/LoginModel';
import FormButton from 'src/components/input/FormButton';
import { useAuth } from 'src/hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import Background from './../../assets/images/waves.jpg';
import { Form } from 'src/hooks/form/Form';
import { useLocationProps } from 'src/hooks/useLocationProps';
import FormRow from 'src/components/forms/FormRow';

const LoginPage: React.FC = () => {
  const { login } = useAuth();
  const { getResource } = useLocationProps(['common']);
  const navigate = useNavigate();

  const initialModel: LoginModel = {
    userName: process.env.REACT_APP_USER_NAME || '',
    secret: process.env.REACT_APP_USER_PASSWORD || '',
    rememberMe: false,
    error: null,
  };

  const form = Form.create<LoginModel>((factory) => [
    factory.createStringSettings(
      { key: 'userName', type: 'text', isRequired: true, isReadonly: false },
      getResource('common.labelUserName')
    ),
    factory.createStringSettings(
      { key: 'secret', type: 'password', isRequired: true, isReadonly: false },
      getResource('common.labelPassword')
    ),
    factory.createCheckboxSettings(
      { key: 'rememberMe', type: 'boolean', isRequired: false, isReadonly: false },
      getResource('common.labelRememberMe')
    ),
  ]);

  React.useEffect(() => {
    form.useModel(initialModel);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const { userName, secret, rememberMe, error } = form.useSubscription((state) => ({
    userName: state?.userName,
    secret: state?.secret,
    rememberMe: state?.rememberMe,
    error: state?.error,
  }));

  const isReadonly =
    userName?.trim() === '' || secret?.trim() === '' || (secret !== undefined && secret.length < 8);

  const handleLogin = React.useCallback(
    async (model: LoginModel) => {
      await login(model);
      navigate('/home');
    },
    [navigate, login]
  );

  return (
    <Box
      minHeight="100vh"
      minWidth="400px"
      width="100vw"
      display="flex"
      alignItems="center"
      justifyContent="center"
      sx={{
        backgroundImage: `url(${Background})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
      }}
    >
      <FormCard minwidth="320px" padding="32px 28px">
        <Stack width="100%" spacing={2}>
          <Title text="Login" />
          <FormRow numberOfColumns={1}>
            <form.TextField fieldKey="userName" fieldPropsCallback={form.getFieldPropsByKey} />
          </FormRow>
          <FormRow numberOfColumns={1}>
            <form.TextField fieldKey="secret" fieldPropsCallback={form.getFieldPropsByKey} />
          </FormRow>
          <FormRow numberOfColumns={1}>
            <form.Checkbox fieldKey="rememberMe" fieldPropsCallback={form.getFieldPropsByKey} />
          </FormRow>

          {error && <Alert severity="error">{error}</Alert>}
          <FormButton
            label="Login"
            disabled={isReadonly}
            onClick={handleLogin.bind(null, {
              userName: userName === undefined ? '' : userName,
              secret: secret === undefined ? '' : secret,
              rememberMe: rememberMe === undefined ? false : rememberMe,
              error: null,
            })}
          />
        </Stack>
      </FormCard>
    </Box>
  );
};

export default LoginPage;
