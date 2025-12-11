import React from 'react';
import { useTranslation } from 'react-i18next';
import { ILocationProps } from '../lib/interfaces/ILocationProps';

export const useLocationProps = (namespaces: string[] = []): ILocationProps => {
  const { t, i18n } = useTranslation(namespaces);

  React.useEffect(() => {
    i18n.loadNamespaces(namespaces);
  }, [i18n, namespaces]);

  return {
    getResource: (key: string, options?: any) => String(t(key, options)),
  };
};
