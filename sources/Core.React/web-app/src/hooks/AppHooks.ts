import React from 'react';
import { StatelessApi } from './StatelessApi';
import { useAsyncComponentInitialization } from './useComponentMounting';
import { useLocationProps } from './useLocationProps';
import isEqual from 'lodash/isEqual';
import { LocalStorageKeyEnum } from 'src/lib/enums/LocalStorageKeyEnum';

type SubScription<TModel> = (state: TModel) => void;
type ReducerAction<TModel> = Partial<TModel> | ((s: TModel) => Partial<TModel>);
type LocalStorageResult<TModel> = {
  model: TModel | null;
  setItem: (value: TModel) => void;
  removeItem: () => void;
};

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

export const useLocalStorage = <TModel>(key: LocalStorageKeyEnum): LocalStorageResult<TModel> => {
  const [localStorageModel, setLocalStorageModel] = React.useState<TModel | null>(null);

  const setItem = React.useCallback(
    (value: TModel) => {
      localStorage.setItem(key, JSON.stringify(value));
      setLocalStorageModel(value);
    },
    [key]
  );

  const getItem = React.useCallback((): TModel | null => {
    const item = localStorage.getItem(key);
    return item ? (JSON.parse(item) as TModel) : null;
  }, [key]);

  const removeItem = React.useCallback((): void => {
    localStorage.removeItem(key);
    setLocalStorageModel(null);
  }, [key]);

  React.useEffect(() => {
    const item = getItem();
    setLocalStorageModel(item);
  }, [getItem]);

  return { model: localStorageModel, setItem, removeItem };
};

export const AppHooks = {
  statelessApi: StatelessApi,
  useLocalisationProps: useLocationProps,
  useComponentMounting: useAsyncComponentInitialization,
  useStore: useStore,
  useLocalStorage: useLocalStorage,
};
