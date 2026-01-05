import React from 'react';
import Box from '@mui/material/Box';
import List from 'src/components/lists/List';
import HeaderListItem from 'src/components/lists/HeaderListItem';
import { IModuleComponentInitializationProps } from './models/IModuleComponentInitializationProps';
import FormListItem from 'src/components/lists/FormListItem';
import FormContainer, {
  DeleteButtonProps,
  SaveCancelButtonProps,
} from 'src/components/forms/FormContainer';
import { DropdownItem } from 'src/components/input/Dropdown';
import { IModule } from '../models/module';
import FormRow from 'src/components/forms/FormRow';
import FormAutoComplete from 'src/components/forms/FormAutoComplete';
import FormTextInput from 'src/components/forms/FormTextInput';
import Notification from 'src/components/Notification';
import { INotificationBadgeState } from 'src/lib/interfaces/INotificationBadgeState';
import { IFormChipProps } from 'src/components/forms/FormChip';
import FormChipContainer from 'src/components/forms/FormChipContainer';
import { FormLabel } from '@mui/material';
import { isEqual } from 'lodash';

const defaultModule: IModule = {
  moduleId: 0,
  idExternal: '',
  title: '',
  description: '',
  lastUpdateAt: null,
  lastUpdateBy: null,
  subModules: [],
};

interface IModuleConfigurationProps extends IModuleComponentInitializationProps {}

const ModuleConfigurationForm: React.FC<IModuleConfigurationProps> = (
  props: IModuleConfigurationProps
) => {
  const { saveApi, deleteApi, isReadonly, modules, getResource, setIsLoading } = props;

  const originalModuleRef = React.useRef<IModule | null>(null);

  const [initialState, setIntialState] = React.useState<IModule[]>(
    Array.isArray(modules) ? modules : []
  );
  const [selectedModule, setSelectedModule] = React.useState<IModule | null>(null);
  const [notificationBadge, setNotificationBadge] = React.useState<INotificationBadgeState>({
    show: false,
    message: '',
    color: 'success',
    duration: 3000,
  });

  const moduleDropdownItems = React.useMemo((): DropdownItem[] => {
    return initialState.map((module) => ({
      id: module.moduleId,
      label: module.title,
    }));
  }, [initialState]);

  const handleSelectModule = React.useCallback(
    (_: keyof IModule | string, id: number | null) => {
      if (id !== null) {
        const model = initialState.find((model) => model.moduleId === id) ?? null;

        if (model != null) {
          setSelectedModule(model);
          originalModuleRef.current = model;
        }
      } else {
        setSelectedModule(defaultModule);
        originalModuleRef.current = defaultModule;
      }
    },
    [initialState]
  );

  const handleSelectedModuleChange = React.useCallback(
    (key: keyof IModule, value: any) => {
      if (selectedModule === null) return;

      setSelectedModule({ ...selectedModule, [key]: value });
    },
    [selectedModule]
  );

  const handleSaveOrUpdate = React.useCallback(async () => {
    setIsLoading(true);

    try {
      if (selectedModule === null) return;

      await saveApi
        .post('/moduleconfiguration/saveorupdatemodule', selectedModule)
        .then((response) => {
          setIntialState(response.data);
          const selectedModuleFromResponse =
            response.data.find((x: IModule) => x.moduleId === selectedModule.moduleId) || null;
          setSelectedModule(selectedModuleFromResponse);
          originalModuleRef.current = selectedModuleFromResponse;
          setNotificationBadge({
            show: true,
            message: getResource(response.resourceKey),
            color: response.success ? 'success' : 'error',
            duration: 3000,
          });
        });
    } finally {
      setIsLoading(false);
    }
  }, [saveApi, selectedModule, setIsLoading, getResource]);

  const handleDelete = React.useCallback(async () => {
    setIsLoading(true);

    try {
      const response = await deleteApi.post(
        `/moduleconfiguration/deleteModule?moduleId=${selectedModule?.moduleId}`
      );
      setIntialState((prevState) => (response.success ? response.data : prevState));
      setSelectedModule(null);
      originalModuleRef.current = null;
      setNotificationBadge({
        show: true,
        message: getResource(response.resourceKey),
        color: response.success ? 'success' : 'error',
        duration: 3000,
      });
    } finally {
      setIsLoading(false);
    }
  }, [deleteApi, selectedModule, setIsLoading, getResource]);

  const revertChanges = React.useCallback(() => {
    setSelectedModule((prevState) => {
      return originalModuleRef.current != null ? originalModuleRef.current : prevState;
    });
  }, []);

  const isModified = React.useMemo((): boolean => {
    return !isEqual(selectedModule, originalModuleRef.current);
  }, [selectedModule]);

  const saveCancelButtonProps = React.useMemo((): SaveCancelButtonProps => {
    return {
      isModified: isModified,
      cancelLabel: getResource('common.labelCancel'),
      isCancelDisabled: isReadonly,
      onCancel: revertChanges,
      saveLabel: getResource('common.labelSave'),
      isSaveDisabled: isReadonly,
      onSave: handleSaveOrUpdate,
    };
  }, [isModified, isReadonly, revertChanges, getResource, handleSaveOrUpdate]);

  const deleteButtonProps = React.useMemo((): DeleteButtonProps => {
    return {
      isModified: isModified,
      deleteLabel: getResource('common.labelDelete'),
      isDisabled:
        isReadonly || isModified || selectedModule?.title === '' || selectedModule === null,
      onDelete: handleDelete,
    };
  }, [isModified, selectedModule, isReadonly, getResource, handleDelete]);

  const handleResetNotification = React.useCallback(() => {
    setNotificationBadge((prev) => ({
      ...prev,
      show: false,
    }));
  }, []);

  const lastUpdateLabel = React.useMemo((): string | null => {
    if (selectedModule?.lastUpdateAt == null || selectedModule?.lastUpdateBy == null) {
      return null;
    }

    return `${getResource('common.labelLastUpdateAtBy')
      .replace('{AT}', selectedModule.lastUpdateAt)
      .replace('{BY}', selectedModule.lastUpdateBy)}`;
  }, [selectedModule?.lastUpdateAt, selectedModule?.lastUpdateBy, getResource]);

  const formChipsProps: IFormChipProps[] = React.useMemo((): IFormChipProps[] => {
    if (selectedModule?.moduleId === 0) {
      return [];
    }
    return (
      selectedModule?.subModules.map((subModule) => ({
        label: subModule.title,
        isReadOnly: isReadonly,
      })) ?? []
    );
  }, [selectedModule, isReadonly]);

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
                  <FormAutoComplete<IModule>
                    isRequired={true}
                    propertyKey={'title'}
                    options={moduleDropdownItems}
                    value={selectedModule?.title ?? ''}
                    placeholder={getResource('common.labelSelectOrEnterTitle')}
                    isReadOnly={isReadonly}
                    onChange={(_, value) => handleSelectedModuleChange('title', value)}
                    onSelectionChange={handleSelectModule}
                  />
                </Box>
                <Box width="100%">
                  <FormTextInput<IModule>
                    isRequired={true}
                    propertyKey={'description'}
                    label={getResource('common.labelDescription')}
                    value={selectedModule?.description ?? ''}
                    placeholder={getResource('common.labelEnterDescription')}
                    isReadOnly={
                      isReadonly || (selectedModule?.moduleId === 0 && selectedModule?.title === '')
                    }
                    onChange={(_, value) => handleSelectedModuleChange('description', value)}
                  />
                </Box>
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

export default ModuleConfigurationForm;
