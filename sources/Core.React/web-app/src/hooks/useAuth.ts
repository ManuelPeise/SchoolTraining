import { useContext } from 'react';
import AuthContext from 'src/lib/context/AuthContext';
import { IAuthContext } from 'src/lib/interfaces/IAuthContext';

export const useAuth = (): IAuthContext => {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }

  return context;
};
