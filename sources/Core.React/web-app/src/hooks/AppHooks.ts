import React from 'react';
import { StatelessApi } from './StatelessApi';
import { useAsyncComponentInitialization } from './useComponentMounting';
import { useLocationProps } from './useLocationProps';
import isEqual from 'lodash/isEqual';

type FormState<TModel extends {}> = {
  model: TModel;
  isModified: boolean;
  readonlyFields?: (keyof TModel)[];
};

type FormResult<TModel extends {}> = {
  updatedModel: TModel;
  isModified: boolean;
  readonlyFields?: (keyof TModel)[];
  updateFormModelExternal: (newModel: TModel) => void;
  disabledFields: DisabledProp<TModel, keyof TModel>;
  onFieldChanged: (key: keyof TModel | string, value: any) => void;
  revertChanges: () => void;
};

type SubScription<TModel> = (state: TModel) => void;
type ReducerAction<TModel> = Partial<TModel> | ((s: TModel) => Partial<TModel>);

function toDisabledModel<TModel extends {}>(
  model: TModel,
  disabledFields: (keyof TModel)[]
): DisabledProp<TModel, keyof TModel> {
  const disabledModel: Record<string, boolean> = {};

  (Object.keys(model) as Array<keyof TModel>).forEach((key) => {
    const disabledKey = `${String(key)}Disabled`;
    disabledModel[disabledKey] = disabledFields.includes(key);
  });

  return disabledModel as DisabledProp<TModel, keyof TModel>;
}

function setNestedValue(obj: any, path: string, value: any) {
  const keys = path.split('.');
  let current = obj;
  for (let i = 0; i < keys.length - 1; i++) {
    if (typeof current[keys[i]] !== 'object' || current[keys[i]] === null) {
      current[keys[i]] = {};
    }
    current = current[keys[i]];
  }
  current[keys[keys.length - 1]] = value;
}

const useForm = <TModel extends {}>(
  initialState: TModel,
  readonlyFields?: (keyof TModel)[]
): TModel & FormResult<TModel> => {
  const originalModel = React.useRef<TModel>(initialState);
  const [formState, setFormState] = React.useState<FormState<TModel>>({
    model: initialState,
    isModified: false,
    readonlyFields,
  });

  const updateFormModelExternal = React.useCallback(
    (newModel: TModel) => {
      originalModel.current = newModel;
      setFormState({
        ...formState,
        model: newModel,
        isModified: !isEqual(newModel, originalModel.current),
      });
    },
    [originalModel, formState]
  );

  const checkForModifications = React.useCallback(
    (newState: FormState<TModel>) => {
      return !isEqual(originalModel.current, newState.model);
    },
    [originalModel]
  );

  const onFieldChanged = React.useCallback(
    (key: keyof TModel | string, value: any) => {
      const newState = { ...formState, model: { ...formState.model, [key]: value } };

      if (!readonlyFields?.includes(key as keyof TModel)) {
        setNestedValue(newState.model, key as string, value);
        setFormState((prevFormState) => ({
          ...prevFormState,
          model: newState.model,
          isModified: checkForModifications(newState),
        }));
      }
    },
    [checkForModifications, formState, readonlyFields]
  );

  // const onFieldChanged = React.useCallback(
  //   (key: keyof TModel | string, value: any) => {
  //     if (!readonlyFields?.includes(key as keyof TModel)) {
  //       setFormState((prevState) => {
  //         // Deep copy model and nested objects for immutability
  //         const newModel = { ...prevState.model };
  //         // If the key is nested (contains a dot), copy the first-level object
  //         const keys = (key as string).split('.');
  //         if (keys.length > 1) {
  //           // Shallow copy the first-level object to avoid mutating original
  //           (newModel as any)[keys[0]] = { ...(newModel as any)[keys[0]] };
  //         }
  //         setNestedValue(newModel, key as string, value);
  //         const newState = { ...prevState, model: newModel };
  //         return {
  //           ...newState,
  //           isModified: checkForModifications(newState),
  //         };
  //       });
  //     }
  //   },
  //   [checkForModifications, readonlyFields]
  // );

  const revertChanges = React.useCallback(() => {
    setFormState({
      model: originalModel.current,
      isModified: false,
    });
  }, []);

  return {
    ...formState.model,
    disabledFields: toDisabledModel(formState.model, formState.readonlyFields || []),
    updatedModel: formState.model,
    isModified: formState.isModified,
    readonlyFields: formState.readonlyFields,
    updateFormModelExternal,
    revertChanges,
    onFieldChanged,
  };
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

export const AppHooks = {
  statelessApi: StatelessApi,
  useLocalisationProps: useLocationProps,
  useComponentMounting: useAsyncComponentInitialization,
  useForm: useForm,
  useStore: useStore,
};
