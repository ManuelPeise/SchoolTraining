import { DropdownItem } from 'src/components/input/Dropdown';
import {
  FormFieldPropsBase,
  FormTextFieldProps,
  FormNumberFieldProps,
  FormAutoCompleteProps,
  ValidationFormMemberCallback,
  FormCheckboxFieldProps,
  FormArraySettingsProps,
  FormDropdownFieldProps,
} from './FormTypes';

export interface FormFieldFactory<TModel> {
  createCheckboxSettings(
    options: FormFieldPropsBase<TModel>,
    label?: string,
    validationCallback?: ValidationFormMemberCallback
  ): FormCheckboxFieldProps<TModel>;
  createStringSettings(
    options: FormFieldPropsBase<TModel>,
    label?: string,
    validationCallback?: ValidationFormMemberCallback
  ): FormTextFieldProps<TModel>;
  createNumberSettings(
    options: FormFieldPropsBase<TModel>,
    label?: string,
    validationCallback?: ValidationFormMemberCallback
  ): FormNumberFieldProps<TModel>;
  createAutoCompleteSettings(
    options: FormFieldPropsBase<TModel>,
    items: DropdownItem[],
    label?: string,
    validationCallback?: ValidationFormMemberCallback
  ): FormAutoCompleteProps<TModel>;
  createArraySettings(
    options: FormFieldPropsBase<TModel>,
    items: any[],
    validationCallback?: ValidationFormMemberCallback
  ): FormArraySettingsProps<TModel>;
  createDropdownSettings(
    options: FormFieldPropsBase<TModel>,
    items: DropdownItem[],
    label?: string,
    validationCallback?: ValidationFormMemberCallback
  ): FormDropdownFieldProps<TModel>;
}

export function createFormFieldFactory<TModel>(): FormFieldFactory<TModel> {
  return {
    createCheckboxSettings: (
      options: FormFieldPropsBase<TModel>,
      label?: string,
      validationCallback?: ValidationFormMemberCallback
    ): FormCheckboxFieldProps<TModel> => ({
      key: options.key,
      type: options.type,
      isRequired: options?.isRequired ?? false,
      isReadonly: options?.isReadonly ?? false,
      value: false,
      label: label || '',
      onChange: () => {},
      validationCallback: validationCallback,
    }),
    createStringSettings: (
      options: FormFieldPropsBase<TModel>,
      label?: string,
      validationCallback?: ValidationFormMemberCallback
    ): FormTextFieldProps<TModel> => ({
      key: options.key,
      type: options.type,
      isRequired: options?.isRequired ?? false,
      isReadonly: options?.isReadonly ?? false,
      value: '',
      label: label || '',
      onChange: () => {},
      validationCallback: validationCallback,
    }),
    createNumberSettings: (
      options: FormFieldPropsBase<TModel>,
      label?: string,
      validationCallback?: ValidationFormMemberCallback
    ): FormNumberFieldProps<TModel> => ({
      key: options.key,
      type: 'number',
      isRequired: options.isRequired,
      isReadonly: options.isReadonly,
      label: label || '',
      value: 0,
      onChange: () => {},
      validationCallback: validationCallback,
    }),
    createAutoCompleteSettings: (
      options: FormFieldPropsBase<TModel>,
      items: DropdownItem[],
      label?: string,
      validationCallback?: ValidationFormMemberCallback
    ): FormAutoCompleteProps<TModel> => ({
      key: options.key,
      type: options.type,
      isRequired: options.isRequired,
      isReadonly: options.isReadonly,
      value: '',
      label: label || '',
      options: items,
      onChange: () => {},
      onSelectionChange: () => {},
      validationCallback: validationCallback,
    }),
    createArraySettings: (
      options: FormFieldPropsBase<TModel>,
      items: any[],
      validationCallback?: ValidationFormMemberCallback
    ): FormArraySettingsProps<TModel> => ({
      key: options.key,
      type: options.type,
      isRequired: options.isRequired,
      isReadonly: options.isReadonly,
      items: items,
      onChange: () => {},
      validationCallback: validationCallback,
    }),
    createDropdownSettings: (
      options: FormFieldPropsBase<TModel>,
      items: DropdownItem[],
      label?: string,
      validationCallback?: ValidationFormMemberCallback
    ): FormDropdownFieldProps<TModel> => ({
      key: options.key,
      type: options.type,
      isRequired: options.isRequired,
      isReadonly: options.isReadonly,
      value: null,
      label: label || '',
      options: items,
      onChange: () => {},
      validationCallback: validationCallback,
    }),
  };
}
