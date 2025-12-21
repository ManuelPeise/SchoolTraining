import React from 'react';
import Box from '@mui/material/Box';
import List from 'src/components/lists/List';
import HeaderListItem from 'src/components/lists/HeaderListItem';
import FormListItem from 'src/components/lists/FormListItem';
import FormContainer, {
  DeleteButtonProps,
  SaveCancelButtonProps,
} from 'src/components/forms/FormContainer';
import FormRow from 'src/components/forms/FormRow';
import { ISubModuleComoponentInitializationProps } from './models/ISubModuleComponentInitializationProps';
import { DropdownItem } from 'src/components/input/Dropdown';
import FormAutoComplete from 'src/components/forms/FormAutoComplete';
import { ISubModuleBase, ISubModuleDataCollection } from '../models/subModule';
import { VocabularyDirectionEnum } from 'src/lib/enums/VocabularyDirectionEnum';
import { IModule } from '../models/module';
import FormTextInput from 'src/components/forms/FormTextInput';
import FormChipContainer from 'src/components/forms/FormChipContainer';
import { IFormChipProps } from 'src/components/forms/FormChip';
import { FormLabel } from '@mui/material';
import FormDropdown from 'src/components/forms/FormDropdown';
import { vocabularyDirectionTypeDropdownItems } from '../helper';
import { isEqual } from 'lodash';
import { INotificationBadgeState } from 'src/lib/interfaces/INotificationBadgeState';
import Notification from 'src/components/Notification';
import { ISubModuleConfigurationInitializationModel } from './models/ISubModuleConfigurationInitializationModel';

const defaultSubMenuModule: ISubModuleBase = {
  subModuleId: 0,
  idExternal: '',
  title: '',
  description: '',
  direction: VocabularyDirectionEnum.GermanEnglish,
  lastUpdateAt: null,
  lastUpdateBy: null,
  moduleId: 0,
  module: {} as IModule,
};

interface IModuleConfigurationProps extends ISubModuleComoponentInitializationProps {}

const SubModuleConfigurationForm: React.FC<IModuleConfigurationProps> = (
  props: IModuleConfigurationProps
) => {
  const { saveApi, deleteApi, model, isReadonly, getResource, setIsLoading } = props;

  const [initialState, setIntialState] =
    React.useState<ISubModuleConfigurationInitializationModel>(model);

  const originalSubModuleRef = React.useRef<ISubModuleBase | null>(null);
  const [selectedModule, setSelectedModule] = React.useState<DropdownItem | null>(null);
  const [selectedSubModule, setSelectedSubModule] = React.useState<ISubModuleBase | null>(null);
  const [notificationBadge, setNotificationBadge] = React.useState<INotificationBadgeState>({
    show: false,
    message: '',
    color: 'success',
    duration: 3000,
  });

  const availableSubModules = React.useMemo<ISubModuleDataCollection | null>(() => {
    if (selectedModule == null) {
      return null;
    }
    const correspondingSubModuleFormModel = initialState.subModuleDataCollection.find(
      (x) => x.moduleId === selectedModule?.id
    ) || { moduleId: selectedModule?.id ?? 0, subModuleDropdownItems: [], subModules: [] };
    return correspondingSubModuleFormModel;
  }, [initialState.subModuleDataCollection, selectedModule]);

  const handleSelectSubModule = React.useCallback((subModule: ISubModuleBase | null) => {
    originalSubModuleRef.current = subModule;
    setSelectedSubModule(subModule);
  }, []);

  const handleModuleChanged = React.useCallback((module: DropdownItem | null) => {
    setSelectedModule(module);
    setSelectedSubModule(null);
  }, []);

  const handleModuleChange = React.useCallback(
    (value: string) => {
      const dropdownItem: DropdownItem = { id: -1, label: value };

      handleModuleChanged(dropdownItem);
    },
    [handleModuleChanged]
  );

  const handleSubModuleChange = React.useCallback(
    (key: keyof ISubModuleBase, value: any) => {
      const subModuleItem: ISubModuleBase =
        selectedSubModule === null
          ? { ...defaultSubMenuModule, moduleId: selectedModule?.id ?? 0 }
          : {
              ...selectedSubModule,
            };

      setSelectedSubModule({ ...subModuleItem, [key]: value });
    },
    [selectedModule, selectedSubModule]
  );

  const handleSaveSubModule = React.useCallback(async () => {
    setIsLoading(true);

    try {
      if (selectedSubModule == null) {
        return;
      }

      const requestModel: ISubModuleBase = {
        ...selectedSubModule,
        moduleId: selectedModule?.id ?? 0,
      };

      const response = await saveApi.post(
        '/moduleconfiguration/saveorupdatesubmodule',
        requestModel
      );

      setIntialState(response.data);

      setNotificationBadge({
        show: true,
        message: getResource(response.resourceKey),
        color: response.success ? 'success' : 'error',
        duration: 3000,
      });

      const subModuleResponseData = response.data.subModuleDataCollection
        .find((x) => x.moduleId === requestModel.moduleId)
        ?.subModules.find(
          (x) => x.title === requestModel.title && x.description === requestModel.description
        );

      originalSubModuleRef.current = subModuleResponseData || null;
      setSelectedSubModule(subModuleResponseData || null);
    } finally {
      setIsLoading(false);
    }
  }, [selectedSubModule, saveApi, selectedModule, getResource, setIsLoading]);

  const handleDeleteSubModule = React.useCallback(async () => {
    setIsLoading(true);

    try {
      const response = await deleteApi.post(
        `/moduleconfiguration/deletesubmodule?subModuleid=${selectedSubModule?.subModuleId}`
      );

      setIntialState(response.data);
      originalSubModuleRef.current = null;
      setNotificationBadge({
        show: true,
        message: getResource(response.resourceKey),
        color: response.success ? 'success' : 'error',
        duration: 3000,
      });
      setSelectedSubModule(null);
    } finally {
      setIsLoading(false);
    }
  }, [deleteApi, selectedSubModule, setIsLoading, getResource]);

  const handleResetNotification = React.useCallback(() => {
    setNotificationBadge((prev) => ({
      ...prev,
      show: false,
    }));
  }, []);

  const formChipsProps = React.useMemo((): IFormChipProps[] => {
    const moduleLabels: string[] = initialState.subModuleDataCollection.map((subModuleData) => {
      const module = subModuleData.subModuleDropdownItems
        .map((item) => item)
        .find((x) => x.id === selectedModule?.id);

      if (module == null) {
        return '';
      }

      return module.label;
    });

    return moduleLabels
      .filter((x) => x !== '')
      .map((label, index) => {
        return {
          label: label,
          value: index.toString(),
          isReadOnly: isReadonly,
        };
      });
  }, [selectedModule, initialState.subModuleDataCollection, isReadonly]);

  const lastUpdateLabel = React.useMemo((): string | null => {
    if (selectedSubModule?.lastUpdateAt == null || selectedSubModule?.lastUpdateBy == null) {
      return null;
    }

    return `${getResource('common.labelLastUpdateAtBy')
      .replace('{AT}', selectedSubModule.lastUpdateAt)
      .replace('{BY}', selectedSubModule.lastUpdateBy)}`;
  }, [selectedSubModule?.lastUpdateAt, selectedSubModule?.lastUpdateBy, getResource]);

  const isModified = React.useMemo((): boolean => {
    return !isEqual(originalSubModuleRef.current, selectedSubModule);
  }, [selectedSubModule]);

  const saveCancelButtonProps: SaveCancelButtonProps = React.useMemo(() => {
    return {
      saveLabel: getResource('common.labelSave'),
      cancelLabel: getResource('common.labelCancel'),
      isReadOnly: isReadonly,
      isModified: isModified,
      onSave: handleSaveSubModule,
      onCancel: () => setSelectedSubModule(originalSubModuleRef.current),
    };
  }, [isReadonly, isModified, originalSubModuleRef, getResource, handleSaveSubModule]);

  const deleteButtonProps = React.useMemo((): DeleteButtonProps => {
    return {
      deleteLabel: getResource('common.labelDelete'),
      isModified: !isModified,
      onDelete: handleDeleteSubModule,
      isDisabled: isReadonly || isModified,
    };
  }, [getResource, isModified, handleDeleteSubModule, isReadonly]);

  return (
    <Box
      sx={{
        height: '100%',
        display: 'flex',
        minWidth: '600px',
        width: '100%',
        flexDirection: 'column',
        justifyContent: 'space-between',
      }}
    >
      <List minHeight={200} maxHeight={500}>
        <HeaderListItem
          title={getResource('common.captionModuleConfiguration')}
          subTitle={getResource('common.labelManageYourModules')}
        />
        <FormListItem>
          <Box display="flex" minWidth="600px" width="100%">
            <FormContainer
              saveCancelButtonProps={saveCancelButtonProps}
              deleteButtonProps={deleteButtonProps}
            >
              <FormRow numberOfColumns={2} gapSize={2}>
                <Box width="100%">
                  <FormAutoComplete<DropdownItem>
                    key="module-select"
                    isRequired={true}
                    propertyKey={'label'}
                    options={initialState.parentModuleDropdownItems}
                    value={selectedModule?.label ?? ''}
                    placeholder={getResource('common.labelSelectParentModule')}
                    onChange={(_, value) => handleModuleChange(value)}
                    onSelectionChange={(_, id) => {
                      const selected =
                        initialState.parentModuleDropdownItems.find((item) => item.id === id) ||
                        null;
                      handleModuleChanged(selected);
                    }}
                  />
                </Box>
              </FormRow>
              <FormRow numberOfColumns={2} gapSize={2}>
                <Box width="100%">
                  <FormAutoComplete<ISubModuleBase>
                    key={
                      selectedModule ? `submodule-select-${selectedModule.id}` : 'submodule-select'
                    }
                    isRequired={true}
                    propertyKey={'title'}
                    options={availableSubModules?.subModuleDropdownItems || []}
                    value={selectedSubModule?.title ?? ''}
                    placeholder={getResource('common.labelSelectOrEnterTitle')}
                    isReadOnly={selectedModule == null}
                    onChange={(_, value) => handleSubModuleChange('title', value)}
                    onSelectionChange={(_, id) => {
                      const subModule =
                        availableSubModules?.subModules.find((item) => item.subModuleId === id) ||
                        null;
                      handleSelectSubModule(subModule);
                    }}
                  />
                </Box>
                <Box width="100%">
                  <FormTextInput<ISubModuleBase>
                    isRequired={true}
                    propertyKey={'description'}
                    label={getResource('common.labelDescription')}
                    value={selectedSubModule?.description ?? ''}
                    placeholder={getResource('common.labelEnterDescription')}
                    isReadOnly={(isReadonly && selectedModule == null) || selectedSubModule == null}
                    onChange={(_, value) => handleSubModuleChange('description', value)}
                  />
                </Box>
              </FormRow>
              <FormRow>
                <FormDropdown<ISubModuleBase>
                  isRequired={true}
                  propertyKey={'direction'}
                  label={getResource('common.labelSelectLanguageDirection')}
                  items={vocabularyDirectionTypeDropdownItems.map((item) => ({
                    id: item.id,
                    label: getResource(item.label),
                  }))}
                  selectedItemId={
                    selectedSubModule?.direction ?? VocabularyDirectionEnum.GermanEnglish
                  }
                  isReadOnly={(isReadonly && selectedModule == null) || selectedSubModule == null}
                  onChange={(_, value) => handleSubModuleChange('direction', value)}
                />
              </FormRow>
              <FormRow numberOfColumns={1} gapSize={2}>
                <FormChipContainer
                  label={getResource('common.labelAvailableSubModules')}
                  models={formChipsProps}
                />
              </FormRow>
              <FormRow>
                {lastUpdateLabel && (
                  <FormLabel
                    sx={{ fontWeight: 'normal', fontStyle: 'italic', fontSize: '16px', my: 2 }}
                  >
                    {lastUpdateLabel}
                  </FormLabel>
                )}
              </FormRow>
            </FormContainer>
          </Box>
        </FormListItem>
      </List>
      <Notification {...notificationBadge} handleResetNotification={handleResetNotification} />
    </Box>
  );
};

export default SubModuleConfigurationForm;
