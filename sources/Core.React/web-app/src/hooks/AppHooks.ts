import React from 'react';
import { StatelessApi } from './StatelessApi';
import { useAsyncComponentInitialization } from './useComponentMounting';
import { useLocationProps } from './useLocationProps';
import isEqual from 'lodash/isEqual';

type SubScription<TModel> = (state: TModel) => void;
type ReducerAction<TModel> = Partial<TModel> | ((s: TModel) => Partial<TModel>);

// Store hook with subscription capability
function reducer<TState>(state: TState, update: ReducerAction<TState>): TState {
  const stateUpdate = typeof update === 'function' ? update(state) : update;

  const keys = Object.keys(stateUpdate) as Array<keyof TState>;

  const isChanged = state != null && keys.some((key) => !isEqual(state[key], stateUpdate[key]));

  if (!isChanged) {
    return state;
  }

  return stateUpdate ? { ...state, ...stateUpdate } : state;
}

export const useStore = <TModel>(initialModel: TModel) => {
  const listeners = React.useRef(new Set<SubScription<TModel>>());
  const [state, dispatch] = React.useReducer(
    reducer as React.Reducer<TModel, ReducerAction<TModel>>,
    initialModel
  );

  const subscribe = React.useCallback((listener: SubScription<TModel>) => {
    listeners.current.add(listener);
    return () => {
      listeners.current.delete(listener);
    };
  }, []);

  React.useEffect(() => {
    listeners.current.forEach((listener) => listener(state));
  }, [state]);

  return { state, dispatch, subscribe };
};

export const AppHooks = {
  statelessApi: StatelessApi,
  useLocalisationProps: useLocationProps,
  useComponentMounting: useAsyncComponentInitialization,
  useStore: useStore,
};
