import { Box, FormLabel } from '@mui/material';
import React from 'react';
import FormContainer, {
  DeleteButtonProps,
  SaveCancelButtonProps,
} from 'src/components/forms/FormContainer';
import { getModuleConfiguration, ModuleConfigurationModel } from '../moduleFormConfiguration';
import { AppHooks } from 'src/hooks/AppHooks';
import { Module } from '../models/Module';
import FormTextInput from 'src/components/forms/FormTextInput';
import FormRow from 'src/components/forms/FormRow';
import {
  mapDropdownItems,
  moduleConfigurationDropdownItems,
  vocabularyDirectionTypeDropdownItems,
} from '../../helper';
import FormDropdown from 'src/components/forms/FormDropdown';
import { ModuleConfigurationTypeEnum } from '../enums/ModuleConfigurationTypeEnum';
import { DropdownItem } from 'src/components/input/Dropdown';
import FormAutoComplete from 'src/components/forms/FormAutoComplete';
import { ModuleInitializationModel } from '../models/IModuleComponentInitializationProps';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationDataResponse } from 'src/lib/interfaces/INotificationDataResponse';
import moment from 'moment';

const defaultModule: Module = {
  moduleId: 0,
  title: '',
  description: '',
  direction: null,
};
interface IProps {
  api: IStatelessApi<INotificationDataResponse<ModuleInitializationModel>, Module>;
  isReadOnly: boolean;
  moduleInitializationModel: ModuleInitializationModel;
  getResource: (key: string) => string;
  setIsLoading: (isLoading: boolean) => void;
}

const ModuleConfigurationForm: React.FC<IProps> = (props: IProps) => {
  const { moduleInitializationModel, api, getResource, setIsLoading } = props;

  const [initializationModel, setInitializationModel] =
    React.useState<ModuleInitializationModel>(moduleInitializationModel);

  const [selectedModuleType, setSelectedModuleType] = React.useState<ModuleConfigurationTypeEnum>(
    ModuleConfigurationTypeEnum.ModuleConfiguration
  );

  const [selectedModel, setSelectedModel] = React.useState<Module>(defaultModule);

  const modulesDropdownItems = React.useMemo<DropdownItem[]>(() => {
    return mapDropdownItems(initializationModel.modules);
  }, [initializationModel.modules]);

  const subModulesDropdownItems = React.useMemo<DropdownItem[]>(() => {
    return mapDropdownItems(initializationModel.subModules);
  }, [initializationModel.subModules]);

  const configuration = React.useMemo<ModuleConfigurationModel>(() => {
    return getModuleConfiguration(selectedModuleType);
  }, [selectedModuleType]);

  const moduleDropdownItems = React.useMemo(() => {
    return moduleConfigurationDropdownItems.map((item) => {
      return { ...item, label: getResource(item.label) };
    });
  }, [getResource]);

  const vocabularyDirectionDropdownItems = React.useMemo(() => {
    return vocabularyDirectionTypeDropdownItems.map((item) => {
      return { ...item, label: getResource(item.label) };
    });
  }, [getResource]);

  const formModel = AppHooks.useForm<Module>(selectedModel);

  const selectedModule = React.useMemo((): DropdownItem | null => {
    return modulesDropdownItems.find((module) => module.id === formModel.moduleId) || null;
  }, [formModel.moduleId, modulesDropdownItems]);

  const handleSaveOrUpdateModule = React.useCallback(async () => {
    setIsLoading(true);
    const serverModel: Module = {
      moduleId: formModel.moduleId,
      idExternal: formModel.idExternal,
      title: formModel.title,
      description: formModel.description,
      direction: formModel.direction,
    };
    await api.post('/moduleconfiguration/saveorupdatemodule', serverModel).then((response) => {
      setInitializationModel((prevState) => ({
        ...prevState,
        modules: response.data.modules,
      }));
    });

    setIsLoading(false);
  }, [formModel, api, setIsLoading]);

  const handleSaveOrUpdateSubModule = React.useCallback(async () => {
    setIsLoading(true);
    const serverModel: Module = {
      moduleId: formModel.moduleId,
      idExternal: formModel.idExternal,
      title: formModel.title,
      description: formModel.description,
      direction: formModel.direction,
    };
    await api.post('/moduleconfiguration/saveorupdatesubmodule', serverModel).then((response) => {
      setInitializationModel((prevState) => ({
        ...prevState,
        subModules: response.data.subModules,
      }));
    });

    setIsLoading(false);
  }, [formModel, api, setIsLoading]);

  const handleDelete = React.useCallback(async () => {
    setIsLoading(true);
    const serverModel: Module = {
      moduleId: formModel.moduleId,
      title: formModel.title,
      description: formModel.description,
      direction: formModel.direction,
    };
    await api
      .post(`/moduleconfiguration/deleteModule?idExternal=${formModel.moduleId}`, serverModel)
      .then((response) => {
        setInitializationModel((prevState) => ({
          ...prevState,
          subModules: response.data.subModules,
        }));
      });

    setIsLoading(false);
  }, [
    formModel.description,
    formModel.direction,
    formModel.moduleId,
    api,
    formModel.title,
    setIsLoading,
  ]);

  const saveCancelButtonProps = React.useMemo((): SaveCancelButtonProps => {
    return {
      isModified: formModel.isModified,
      cancelLabel: getResource('common.labelCancel'),
      isCancelDisabled: false,
      onCancel: formModel.revertChanges,
      saveLabel: getResource('common.labelSave'),
      isSaveDisabled: false,
      onSave:
        selectedModuleType === ModuleConfigurationTypeEnum.ModuleConfiguration
          ? handleSaveOrUpdateModule
          : handleSaveOrUpdateSubModule,
    };
  }, [
    formModel.isModified,
    getResource,
    formModel.revertChanges,
    handleSaveOrUpdateModule,
    handleSaveOrUpdateSubModule,
    selectedModuleType,
  ]);

  const deleteButtonProps = React.useMemo((): DeleteButtonProps => {
    return {
      isModified: formModel.isModified,
      deleteLabel: getResource('common.labelDelete'),
      isDisabled: formModel.isModified || formModel.title === '' || selectedModel === null,
      onDelete: handleDelete,
    };
  }, [formModel.isModified, formModel.title, selectedModel, getResource, handleDelete]);

  const handleSelectModuleConfiguration = React.useCallback(
    (_: keyof Module, id: number | null) => {
      if (id !== null) {
        const model =
          selectedModuleType === ModuleConfigurationTypeEnum.ModuleConfiguration
            ? initializationModel.modules.find((module) => module.moduleId === id)
            : initializationModel.subModules.find((subModule) => subModule.moduleId === id);

        if (model != null) {
          setSelectedModel(model);
          formModel.updateFormModelExternal(model);
        }
      } else {
        setSelectedModel(defaultModule);
        formModel.updateFormModelExternal(defaultModule);
      }
    },
    [initializationModel.modules, initializationModel.subModules, selectedModuleType, formModel]
  );

  const lastUpdateLabel = React.useMemo((): string | null => {
    if (
      selectedModel &&
      selectedModel?.lastUpdateAt != null &&
      selectedModel?.lastUpdateBy != null
    ) {
      return `${getResource('common.labelLastUpdateAtBy')
        .replace('{AT}', moment(selectedModel.lastUpdateAt).format('LL'))
        .replace('{BY}', selectedModel.lastUpdateBy)}`;
    }
    return null;
  }, [selectedModel, getResource]);

  return (
    <Box display="flex" minWidth="600px" width="100%">
      <FormContainer
        saveCancelButtonProps={saveCancelButtonProps}
        deleteButtonProps={deleteButtonProps}
      >
        <Box display="flex" minWidth="600px" width="100%" flexDirection="column" gap={2}>
          <FormRow>
            <FormDropdown
              label={getResource('common.labelModuleType')}
              isReadOnly={formModel.isModified}
              selectedItemId={selectedModuleType}
              items={moduleDropdownItems}
              onChange={(_, val) => setSelectedModuleType(val)}
            />
          </FormRow>
          {configuration.isModuleIdRequired && (
            <FormRow>
              <Box width="100%">
                <FormAutoComplete<Module>
                  isRequired={true}
                  propertyKey={'moduleId'}
                  options={modulesDropdownItems}
                  value={selectedModule ? selectedModule.label : ''}
                  placeholder={getResource('common.labelSelectParentModule')}
                  isReadOnly={false}
                  onChange={(key, value) => {
                    const selected = modulesDropdownItems.find((module) => module.label === value);
                    formModel.onFieldChanged(key, selected ? selected.id : 0);
                  }}
                />
              </Box>
            </FormRow>
          )}
          {configuration.hasTitle && configuration.hasDescription && (
            <FormRow numberOfColumns={2} gapSize={2}>
              <Box width="100%">
                <FormAutoComplete<Module>
                  isRequired={true}
                  propertyKey={'title'}
                  options={
                    configuration.key === ModuleConfigurationTypeEnum.ModuleConfiguration
                      ? modulesDropdownItems
                      : subModulesDropdownItems
                  }
                  value={formModel.title}
                  placeholder={getResource('common.labelSelectOrEnterTitle')}
                  isReadOnly={false}
                  onChange={formModel.onFieldChanged}
                  onSelectionChange={handleSelectModuleConfiguration}
                />
              </Box>
              <Box width="100%">
                <FormTextInput<Module>
                  isRequired={true}
                  propertyKey={'description'}
                  label={getResource('common.labelDescription')}
                  value={formModel.description}
                  placeholder={getResource('common.labelEnterDescription')}
                  disabled={false}
                  onChange={formModel.onFieldChanged}
                />
              </Box>
            </FormRow>
          )}
          {configuration.hasVocabularyDirection && (
            <FormRow>
              <FormDropdown<Module>
                propertyKey={'direction'}
                isRequired={true}
                label={getResource('common.labelLanguageDirection')}
                selectedItemId={formModel.direction}
                isReadOnly={false}
                placeholder={getResource('common.labelSelectLanguageDirection')}
                items={vocabularyDirectionDropdownItems}
                onChange={formModel.onFieldChanged}
              />
            </FormRow>
          )}
          <FormRow>
            {lastUpdateLabel && (
              <FormLabel sx={{ fontWeight: 'normal', fontStyle: 'italic', fontSize: '16px' }}>
                {lastUpdateLabel}
              </FormLabel>
            )}
          </FormRow>
        </Box>
      </FormContainer>
    </Box>
  );
};

export default ModuleConfigurationForm;
