import React from 'react';
import {
  FormAutoCompleteProps,
  FormFieldSetup,
  FormNumberFieldProps,
  FormTextFieldProps,
  FormValidationStatus,
  UseFormResult,
} from './FormTypes';
import { AppHooks } from 'src/hooks/AppHooks';
import { createFormFieldFactory } from './formFieldFactory';
import { FormComponents } from './FormComponents';
import { isEqual } from 'lodash';
import { DropdownItem } from 'src/components/input/Dropdown';

type FormState<TModel> = {
  originalModel: TModel | null;
  model: TModel | null;
};

function useForm<TModel>(
  formFieldSetup: (
    factory: ReturnType<typeof createFormFieldFactory<TModel>>
  ) => FormFieldSetup<TModel>
): UseFormResult<TModel> {
  const originalRef = React.useRef<FormState<TModel> | null>({} as FormState<TModel>);

  const { state, dispatch } = AppHooks.useStore<FormState<TModel>>(originalRef.current!);
  const fieldFactory = createFormFieldFactory<TModel>();
  const fieldsRef = React.useRef(formFieldSetup(fieldFactory));

  const isValidModel = React.useMemo((): boolean => {
    const fieldsArray =
      typeof fieldsRef.current === 'function' ? fieldsRef.current() : fieldsRef.current;

    if (fieldsArray.length === 0 || !state.model) {
      return true;
    }

    return fieldsArray.every((field) => {
      const callback = (field as any).validationCallback as
        | undefined
        | ((key: keyof TModel, value: any) => boolean);

      if (callback) {
        if (!callback(field.key, state.model![field.key])) {
          return false;
        }
      }
      return true;
    });
  }, [state.model, fieldsRef.current]);

  const isModified = React.useMemo((): boolean => {
    if (!originalRef.current?.originalModel) {
      return false;
    }

    return !isEqual(state.model, originalRef.current.originalModel);
  }, [originalRef.current, state.model]);

  const useModel = React.useCallback(
    (formModel: TModel) => {
      originalRef.current = {
        originalModel: formModel,
        model: formModel,
      };
      dispatch(() => ({
        originalModel: formModel,
        model: formModel,
      }));
    },
    [originalRef, dispatch]
  );

  const getUpdatedModel = React.useCallback((): TModel | null => {
    return state.model;
  }, [state.model]);

  const revertChanges = React.useCallback(() => {
    dispatch((state) => ({ model: state.originalModel }));
  }, [dispatch]);

  const useSubscription = React.useCallback(
    (cb: (s: TModel) => Partial<TModel>) => {
      return cb(state.model!);
    },
    [state.model]
  );

  const useStatusSubscription = React.useCallback(
    (
      callback: (status: Partial<FormValidationStatus>) => Partial<FormValidationStatus>
    ): Partial<FormValidationStatus> => {
      return callback({ isDirty: isModified, canSave: isValidModel });
    },
    [state]
  );

  const onFieldChanged = React.useCallback((key: keyof TModel, value: any) => {
    console.log('onFieldChanged called', key, value);
    dispatch((state) => {
      const updatedModel = { ...state.model, [key]: value } as TModel;
      const isDirty = !originalRef.current?.originalModel
        ? false
        : JSON.stringify(updatedModel) !== JSON.stringify(originalRef.current.originalModel);
      return {
        model: updatedModel,
        isDirty,
      };
    });
  }, []);

  const onDropdownChanged = React.useCallback(
    (item: DropdownItem) => {
      dispatch((state) => {
        const updatedModel = { ...state.model, model: item } as TModel;
        const isDirty = !originalRef.current?.originalModel
          ? false
          : JSON.stringify(updatedModel) !== JSON.stringify(originalRef.current.originalModel);
        return {
          model: updatedModel,
          isDirty,
        };
      });
    },
    [dispatch]
  );

  const onSelectionChanged = React.useCallback(
    (key: keyof TModel, id: number) => {
      console.log('onSelectionChanged called', key, id);
      onFieldChanged(key, id);
    },
    [onFieldChanged]
  );

  const getFieldPropsByKey = React.useCallback(
    (
      key: keyof TModel
    ):
      | FormTextFieldProps<TModel>
      | FormNumberFieldProps<TModel>
      | FormAutoCompleteProps<TModel> => {
      const fieldsArray =
        typeof fieldsRef.current === 'function' ? fieldsRef.current() : fieldsRef.current;
      const field = fieldsArray.find((f) => f.key === key);

      if (!field) {
        throw new Error(`Field with key "${String(key)}" not found in form setup.`);
      }

      if (!state.model) {
        return {
          ...field,
          value: '',
          onChange: onFieldChanged,
          onSelectionChange: onSelectionChanged,
        } as
          | FormTextFieldProps<TModel>
          | FormNumberFieldProps<TModel>
          | FormAutoCompleteProps<TModel>;
      }

      return {
        ...field,
        value: state.model[key] as any,
        onChange: onFieldChanged,
        onSelectionChange: onSelectionChanged,
      } as
        | FormTextFieldProps<TModel>
        | FormNumberFieldProps<TModel>
        | FormAutoCompleteProps<TModel>;
    },
    [fieldsRef.current, state.model, onFieldChanged, onSelectionChanged]
  );

  const usePartialForm = <TModel, TModel2 extends Partial<TModel>>(
    formFieldSetup: (
      factory: ReturnType<typeof createFormFieldFactory<TModel2>>
    ) => FormFieldSetup<TModel2>
  ): UseFormResult<TModel2> => {
    // Create the field factory for the full model type
    const fieldFactory = createFormFieldFactory<TModel2>();
    // Pass the setup function to useNewForm, but cast the result to TModel2
    return useForm<TModel2>(() => formFieldSetup(fieldFactory));
  };

  console.log('Form state', state);
  return {
    useModel,
    revertChanges,
    getUpdatedModel,
    useSubscription,
    useStatusSubscription,
    getFieldPropsByKey,
    onFieldChanged,
    onSelectionChanged,
    onDropdownChanged,
    usePartialForm,
    TextField: FormComponents.FormTextField<TModel>,
    NumberField: FormComponents.FormNumberField<TModel>,
    AutoComplete: FormComponents.FormAutoCompleteField<TModel>,
    Dropdown: FormComponents.FormDropdownField<TModel>,
  };
}

export const Form = {
  create: useForm,
};
