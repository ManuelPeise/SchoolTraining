import { Box } from '@mui/material';
import React from 'react';
import FormContainer from 'src/components/forms/FormContainer';
import { getModuleConfiguration, ModuleConfigurationModel } from '../moduleFormConfiguration';
import { AppHooks } from 'src/hooks/AppHooks';
import { Module, ModuleServerModel, SubModuleServermodel } from '../models/Module';
import FormTextInput from 'src/components/forms/FormTextInput';
import FormRow from 'src/components/forms/FormRow';
import {
  moduleConfigurationDropdownItems,
  vocabularyDirectionTypeDropdownItems,
} from '../../helper';
import FormDropdown from 'src/components/forms/FormDropdown';
import { ModuleConfigurationTypeEnum } from '../enums/ModuleConfigurationTypeEnum';
import { DropdownItem } from 'src/components/input/Dropdown';
import FormAutoComplete from 'src/components/forms/FormAutoComplete';

interface IProps {
  isReadOnly: boolean;
  modules: ModuleServerModel[];
  modulesDropdownItems: DropdownItem[];
  subModules: SubModuleServermodel[];
  subModulesDropdownItems: DropdownItem[];
  getResource: (key: string) => string;
}

const ModuleConfigurationForm: React.FC<IProps> = (props: IProps) => {
  const { modulesDropdownItems, subModulesDropdownItems, getResource } = props;

  const [selectedModuleType, setSelectedModuleType] = React.useState<ModuleConfigurationTypeEnum>(
    ModuleConfigurationTypeEnum.ModuleConfiguration
  );

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

  const { moduleId, title, description, direction, isModified, onFieldChanged, revertChanges } =
    AppHooks.useForm<Module>({
      moduleId: 0,
      title: '',
      description: '',
      direction: null,
    });

  const selectedModule = React.useMemo((): DropdownItem | null => {
    return modulesDropdownItems.find((module) => module.id === moduleId) || null;
  }, [moduleId, modulesDropdownItems]);

  return (
    <Box display="flex" minWidth="600px" width="100%">
      <FormContainer
        saveCancelButtonProps={{
          isModified: isModified,
          cancelLabel: getResource('common.labelCancel'),
          isCancelDisabled: false,
          onCancel: revertChanges,
          saveLabel: getResource('common.labelSave'),
          isSaveDisabled: false,
          onSave: () => {},
        }}
      >
        <Box display="flex" minWidth="600px" width="100%" flexDirection="column" gap={2}>
          <FormRow>
            <FormDropdown
              label={getResource('common.labelModuleType')}
              isReadOnly={isModified}
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
                    onFieldChanged(key, selected ? selected.id : 0);
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
                  value={title}
                  placeholder={getResource('common.labelSelectOrEnterTitle')}
                  isReadOnly={false}
                  onChange={onFieldChanged}
                />
              </Box>
              <Box width="100%">
                <FormTextInput<Module>
                  isRequired={true}
                  propertyKey={'description'}
                  label={getResource('common.labelDescription')}
                  value={description}
                  placeholder={getResource('common.labelEnterDescription')}
                  disabled={false}
                  onChange={onFieldChanged}
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
                selectedItemId={direction}
                isReadOnly={false}
                placeholder={getResource('common.labelSelectLanguageDirection')}
                items={vocabularyDirectionDropdownItems}
                onChange={onFieldChanged}
              />
            </FormRow>
          )}
        </Box>
      </FormContainer>
    </Box>
  );
};

export default React.memo(ModuleConfigurationForm);
