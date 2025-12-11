import { StatelessApi } from './StatelessApi';
import { useAsyncComponentInitialization } from './useComponentMounting';
import { useLocationProps } from './useLocationProps';

export const AppHooks = {
  statelessApi: StatelessApi,
  useLocalisationProps: useLocationProps,
  useComponentMounting: useAsyncComponentInitialization,
};
