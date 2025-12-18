import React from 'react';
import Box from '@mui/material/Box';
import List from 'src/components/lists/List';
import HeaderListItem from 'src/components/lists/HeaderListItem';
import { IModuleComponentInitializationProps } from './models/IModuleComponentInitializationProps';
import FormListItem from 'src/components/lists/FormListItem';
import ModuleConfigurationForm from './components/ModuleConfigurationForm';

interface IModuleConfigurationProps extends IModuleComponentInitializationProps {}

const ModuleConfiguration: React.FC<IModuleConfigurationProps> = (
  props: IModuleConfigurationProps
) => {
  const { api, isReadonly, moduleInitializationModel, getResource, setIsLoading } = props;

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
          subTitle={getResource('common.labelManageModuleConfiguration')}
          iconButtonProps={[
            {
              icon: 'bi bi-arrow-clockwise',
              size: 20,
              tooltip: getResource('common.labelDeleteImportedFiles'),
              disabled: isReadonly,
              onClick: () => {},
            },
          ]}
        />
        <FormListItem
          title={getResource('common.labelModuleConfiguration')}
          subTitle={getResource('common.subTitleModuleConfiguration')}
        >
          <ModuleConfigurationForm
            api={api}
            isReadOnly={isReadonly}
            moduleInitializationModel={moduleInitializationModel}
            getResource={getResource}
            setIsLoading={setIsLoading}
          />
        </FormListItem>
      </List>
    </Box>
  );
};

export default React.memo(ModuleConfiguration);
