import React from 'react';
import 'src/App.css';
import FormCard from 'src/components/wrappers/FormCard';
import Title from 'src/components/labels/Title';
import FormTextInput from 'src/components/input/FormTextInput';
import { LoginModel } from 'src/lib/types/LoginModel';
import FormCheckbox from 'src/components/input/FormCheckbox';
import FormButton from 'src/components/input/FormButton';
import { useAuth } from 'src/hooks/useAuth';
import { useNavigate } from 'react-router-dom';

const LoginPage: React.FC = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [loginModelState, setLoginModelState] = React.useState<LoginModel>({
    userName: '',
    secret: '',
    rememberMe: false,
    error: null,
  });

  const isReadonly =
    loginModelState.userName.trim() === '' ||
    loginModelState.secret.trim() === '' ||
    loginModelState.secret.length < 8;

  const onChangeField = (field: keyof LoginModel, value: string | boolean) => {
    setLoginModelState((prevState) => ({
      ...prevState,
      [field]: value,
    }));
  };

  const handleLogin = React.useCallback(async (model: LoginModel) => {
    await login(model);
    navigate('/home');
  }, []);

  return (
    <div className="loginPage">
      <FormCard minwidth="300px" padding="30px 25px">
        <Title text="Login" />
        <FormTextInput
          label="UserName"
          type="text"
          value={loginModelState.userName}
          onChange={onChangeField.bind(null, 'userName')}
        />
        <FormTextInput
          label="Password"
          type="password"
          value={loginModelState.secret}
          onChange={onChangeField.bind(null, 'secret')}
        />
        <FormCheckbox
          checked={loginModelState.rememberMe}
          label="Remember Me"
          disabled={isReadonly}
          onChange={onChangeField.bind(null, 'rememberMe')}
        />
        {loginModelState.error && <div className="errorMessage">{loginModelState.error}</div>}
        <FormButton
          label="Login"
          disabled={isReadonly}
          onClick={handleLogin.bind(null, loginModelState)}
        />
      </FormCard>
    </div>
  );
};

export default LoginPage;
