import React from 'react';

type PartialForm<TModel> = {
  nestedFormModel: TModel;
  resetForm: () => void;
  handleNestedFormFieldChanged: (key: string, value: any) => void;
};

type NestedForm<TModel> = {
  formModel: TModel;
  resetForm: () => void;
  handleFormFieldChanged: (key: keyof TModel, value: TModel[keyof TModel]) => void;
};

const usePartialForm = <TModel>(initialModel: TModel): PartialForm<TModel> => {
  const originalRef = React.useRef<TModel>(initialModel);
  const [nestedFormModel, setNestedFormModel] = React.useState<TModel>(initialModel);

  const resetForm = React.useCallback(() => {
    setNestedFormModel(originalRef.current);
  }, []);

  const handleNestedFormFieldChanged = React.useCallback((key: string, value: any) => {
    setNestedFormModel((prevModel) => ({
      ...prevModel,
      [key]: value,
    }));
  }, []);

  return {
    nestedFormModel,
    resetForm,
    handleNestedFormFieldChanged,
  };
};

export const useNestedForm = <TModel extends object>(
  initialModel: TModel,
  partialFormKeys?: keyof TModel[]
): NestedForm<TModel> => {
  const originalRef = React.useRef<TModel>(initialModel);
  const [formModel, setFormModel] = React.useState<TModel>(initialModel);

  const [partialForms, setPartialForms] = React.useState<{
    [K in keyof TModel]?: PartialForm<TModel[K]>;
  }>({});

  const createPartialForm = React.useCallback((key: keyof TModel) => {
    const partialForm = usePartialForm(formModel[key]);

    setPartialForms((prevPartialForms) => ({
      ...prevPartialForms,
      [key]: partialForm,
    }));
  }, []);

  const resetForm = React.useCallback(() => {
    Object.values(partialForms).forEach((key) => {
      partialForms[key as keyof typeof partialForms]?.resetForm();
    });

    setFormModel(originalRef.current);
  }, []);

  const handleFormFieldChanged = React.useCallback(
    (key: keyof TModel, value: TModel[keyof TModel]) => {
      if (partialForms[key]) {
        partialForms[key].handleNestedFormFieldChanged(key as string, value);
      }
      setFormModel((prevModel) => ({
        ...prevModel,
        [key]: value,
      }));
    },
    []
  );

  React.useEffect(() => {
    if (partialFormKeys) {
      Object.keys(partialFormKeys).forEach((key) => {
        if (!partialForms[key as keyof typeof partialForms]) {
          createPartialForm(key as keyof TModel);
        }
      });
    }
  }, [partialFormKeys]);

  return {
    formModel,
    resetForm,
    handleFormFieldChanged,
  };
};
