import React from 'react';
import { useTranslation } from 'react-i18next';
import { ILocationProps } from '../lib/interfaces/ILocationProps';

export const useLocationProps = (namespaces: string[] = []): ILocationProps => {
  const { t, i18n } = useTranslation(namespaces);

  React.useEffect(() => {
    i18n.loadNamespaces(namespaces);
  }, [i18n, namespaces]);

  const getResource = React.useCallback(
    (key: string): string => {
      if (key.includes('.')) {
        const [namespace, ...resourceParts] = key.split('.');
        const resource = resourceParts.join('.');
        return t(resource, { ns: namespace });
      }
      return t(key);
    },
    [t]
  );

  return {
    getResource,
  };
};
