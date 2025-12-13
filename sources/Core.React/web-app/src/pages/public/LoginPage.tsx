import React from 'react';
import Box from '@mui/material/Box';
import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';
import FormCard from 'src/components/wrappers/FormCard';
import Title from 'src/components/labels/Title';
import FormTextInput from 'src/components/input/FormTextInput';
import { LoginModel } from 'src/lib/types/LoginModel';
import { useForm } from 'src/hooks/useForm';
import FormCheckbox from 'src/components/input/FormCheckbox';
import FormButton from 'src/components/input/FormButton';
import { useAuth } from 'src/hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import Background from './../../assets/images/waves.jpg';

const LoginPage: React.FC = () => {
  const { login } = useAuth();
  const navigate = useNavigate();

  const initialModel: LoginModel = {
    userName: process.env.REACT_APP_USER_NAME || '',
    secret: process.env.REACT_APP_USER_PASSWORD || '',
    rememberMe: false,
    error: null,
  };

  const { values: loginModelState, handleChange } = useForm<LoginModel>(initialModel);

  const isReadonly =
    loginModelState.userName.trim() === '' ||
    loginModelState.secret.trim() === '' ||
    loginModelState.secret.length < 8;

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
        <Stack spacing={4}>
          <Title text="Login" />
          <FormTextInput
            label="UserName"
            type="text"
            value={loginModelState.userName}
            onChange={(val) => handleChange('userName', val)}
          />
          <FormTextInput
            label="Password"
            type="password"
            value={loginModelState.secret}
            onChange={(val) => handleChange('secret', val)}
          />
          <FormCheckbox
            checked={loginModelState.rememberMe}
            label="Remember Me"
            disabled={isReadonly}
            onChange={(val) => handleChange('rememberMe', val)}
          />
          {loginModelState.error && <Alert severity="error">{loginModelState.error}</Alert>}
          <FormButton
            label="Login"
            disabled={isReadonly}
            onClick={handleLogin.bind(null, loginModelState)}
          />
        </Stack>
      </FormCard>
    </Box>
  );
};

export default LoginPage;
