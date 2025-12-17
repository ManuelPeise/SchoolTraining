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
  const {
    isReadonly,
    modules,
    modulesDropdownItems,
    subModules,
    subModulesDropdownItems,
    getResource,
  } = props;

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
            isReadOnly={isReadonly}
            modules={modules}
            modulesDropdownItems={modulesDropdownItems}
            subModules={subModules}
            subModulesDropdownItems={subModulesDropdownItems}
            getResource={getResource}
          />
        </FormListItem>
      </List>
    </Box>
  );
};

export default React.memo(ModuleConfiguration);
