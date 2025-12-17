import React from 'react';
import { StatelessApi } from './StatelessApi';
import { useAsyncComponentInitialization } from './useComponentMounting';
import { useLocationProps } from './useLocationProps';

type FormState<TModel extends {}> = {
  model: TModel;
  isModified: boolean;
  readonlyFields?: (keyof TModel)[];
};

type FormResult<TModel extends {}> = {
  updatedModel: TModel;
  isModified: boolean;
  readonlyFields?: (keyof TModel)[];
  disabledFields: DisabledProp<TModel, keyof TModel>;
  onFieldChanged: (key: keyof TModel, value: any) => void;
  revertChanges: () => void;
};

// Deep equality check for objects (no arrays)
function deepEqualObj(a: any, b: any): boolean {
  if (a === b) return true;
  if (typeof a !== typeof b) return false;
  if (typeof a !== 'object' || a === null || b === null) return false;
  const keysA = Object.keys(a);
  const keysB = Object.keys(b);
  if (keysA.length !== keysB.length) return false;
  return keysA.every((key) => deepEqualObj(a[key], b[key]));
}

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

  const checkForModifications = React.useCallback(
    (newState: FormState<TModel>) => {
      return !deepEqualObj(originalModel.current, newState.model);
    },
    [originalModel]
  );

  const onFieldChanged = React.useCallback(
    (key: keyof TModel, value: any) => {
      const newState = { ...formState, model: { ...formState.model, [key]: value } };

      if (!readonlyFields?.includes(key)) {
        setFormState((prevFormState) => ({
          ...prevFormState,
          model: newState.model,
          isModified: checkForModifications(newState),
        }));
      }
    },
    [checkForModifications, formState, readonlyFields]
  );

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
    revertChanges,
    onFieldChanged,
  };
};

export const AppHooks = {
  statelessApi: StatelessApi,
  useLocalisationProps: useLocationProps,
  useComponentMounting: useAsyncComponentInitialization,
  useForm: useForm,
};
