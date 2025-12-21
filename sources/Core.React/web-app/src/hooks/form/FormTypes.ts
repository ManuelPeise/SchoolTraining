import { JSX } from 'react';
import { DropdownItem } from 'src/components/input/Dropdown';
import { createFormFieldFactory } from './formFieldFactory';

export type FormValidationStatus = {
  isDirty: boolean;
  canSave: boolean;
};

export type FormField<TModel> = {
  key: keyof TModel;
  type: 'text' | 'password' | 'number' | 'boolean';
  isRequired?: boolean;
  isReadonly?: boolean;
};

export type FormFieldPropsBase<TModel> = {
  key: keyof TModel;
  type: 'text' | 'password' | 'number' | 'boolean';
  isRequired?: boolean;
  isReadonly?: boolean;
  validationCallback?: ValidationFormMemberCallback;
};

export type FormTextFieldProps<TModel> = FormFieldPropsBase<TModel> & {
  isPassword?: boolean;
  value: string;
  label: string;
  onChange: (key: keyof TModel, newValue: string) => void;
};

export type FormDropdownFieldProps<TModel> = FormFieldPropsBase<TModel> & {
  value: number | null;
  label: string;
  options: DropdownItem[];
  onChange: (key: keyof TModel, newValue: number) => void;
};

export type FormNumberFieldProps<TModel> = FormFieldPropsBase<TModel> & {
  min?: number;
  max?: number;
  step?: number;
  value: number;
  label: string;
  onChange: (key: keyof TModel, newValue: number) => void;
};

export type FormAutoCompleteProps<TModel> = FormFieldPropsBase<TModel> & {
  value: string;
  label: string;
  options: DropdownItem[];
  onChange: (key: keyof TModel, newValue: string) => void;
  onSelectionChange: (key: keyof TModel, id: number) => void;
};

export type FormCheckboxFieldProps<TModel> = FormFieldPropsBase<TModel> & {
  value: boolean;
  label: string;
  onChange: (key: keyof TModel, newValue: boolean) => void;
};

export type FormArraySettingsProps<TModel> = FormFieldPropsBase<TModel> & {
  items: any[];
  onChange: (key: keyof TModel, newValue: any[]) => void;
};

export type FormAutocompleteFieldProps<TModel> = TextFieldProps<TModel> & {};

export type FormFieldSetup<TModel> =
  | FormFieldPropsBase<TModel>[]
  | (() => FormFieldPropsBase<TModel>[]);

export type TextFieldProps<TModel> = {
  fieldKey: keyof TModel;
  fieldPropsCallback: FieldPropsCallback<TModel>;
};

export type DropdownFieldProps<TModel> = {
  fieldKey: keyof TModel;
  fieldPropsCallback: FieldPropsCallback<TModel>;
};

export type BooleanFieldProps<TModel> = {
  fieldKey: keyof TModel;
  fieldPropsCallback: FieldPropsCallback<TModel>;
};

export type ValidationFormMemberCallback = (value: any) => boolean;

export type FieldPropsCallback<TModel> = (
  key: keyof TModel
) =>
  | FormTextFieldProps<TModel>
  | FormNumberFieldProps<TModel>
  | FormAutoCompleteProps<TModel>
  | FormCheckboxFieldProps<TModel>
  | DropdownFieldProps<TModel>;

export type UseFormResult<TModel> = {
  useModel: (formModel: TModel) => void;
  revertChanges: () => void;
  getUpdatedModel: () => TModel | null;
  useSubscription: (cb: (s: TModel) => Partial<TModel>) => Partial<TModel>;
  useStatusSubscription: (
    callback: (status: Partial<FormValidationStatus>) => Partial<FormValidationStatus>
  ) => Partial<FormValidationStatus>;
  getFieldPropsByKey: (
    key: keyof TModel
  ) =>
    | FormTextFieldProps<TModel>
    | FormNumberFieldProps<TModel>
    | FormAutoCompleteProps<TModel>
    | FormCheckboxFieldProps<TModel>;
  onFieldChanged: (key: keyof TModel, value: any) => void;
  onSelectionChanged: (key: keyof TModel, id: number) => void;
  onDropdownChanged: (item: DropdownItem) => void;
  usePartialForm: <TModel, TModel2 extends Partial<TModel>>(
    formFieldSetup: (
      factory: ReturnType<typeof createFormFieldFactory<TModel2>>
    ) => FormFieldSetup<TModel2>
  ) => UseFormResult<TModel2>;
  TextField: ({ fieldKey, fieldPropsCallback }: TextFieldProps<TModel>) => JSX.Element;
  NumberField: ({ fieldKey, fieldPropsCallback }: TextFieldProps<TModel>) => JSX.Element;
  AutoComplete: ({
    fieldKey,
    fieldPropsCallback,
  }: FormAutocompleteFieldProps<TModel>) => JSX.Element;
  Dropdown: ({ fieldKey, fieldPropsCallback }: DropdownFieldProps<TModel>) => JSX.Element;
  Checkbox: ({ fieldKey, fieldPropsCallback }: BooleanFieldProps<TModel>) => JSX.Element;
};
