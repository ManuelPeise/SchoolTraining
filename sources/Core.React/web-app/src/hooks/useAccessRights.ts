import { useContext } from 'react';
import { AccessRightsContext } from 'src/lib/context/AccessRightContext';
import { IAccessRightsContext } from 'src/lib/interfaces/IAccessRightsContext';

export const useAccessRights = (): IAccessRightsContext => {
  const context = useContext(AccessRightsContext);

  if (!context) {
    throw new Error('useAccessRights must be used within an AccessRightsContextProvider');
  }

  return context;
};
