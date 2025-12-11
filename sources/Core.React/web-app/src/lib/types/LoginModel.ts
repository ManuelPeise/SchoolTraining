export type LoginModel = {
  userName: string;
  secret: string;
  rememberMe: boolean;
  error: string | null;
};

export type LoginRequestModel = Omit<LoginModel, 'error'> & {};
