import { Box, ListItem, Typography } from '@mui/material';
import React from 'react';
import FilterDropdown from 'src/components/input/FilterDropdown';
import FilterInput from 'src/components/input/FilterInput';
import FormCheckbox from 'src/components/input/FormCheckbox';
import { LogLevelEnum } from 'src/lib/enums/LogLevelEnum';

export type LogFilterState = {
  filterText: string;
  selectedItemId: number | null;
  selectedLogLevels: number[];
};

export interface ILogMessageFilterTextProps {
  placeholder: string;
}

export interface ILogMessageFilterDropdownProps {
  items: { id: number; label: string }[];
  placeholder: string;
}

interface IProps {
  title: string;
  subTitle?: string;
  isReadonly: boolean;
  logFilterState: LogFilterState;
  filterTextProps: ILogMessageFilterTextProps;
  filterDropdownProps: ILogMessageFilterDropdownProps;
  onChange: (newState: Partial<LogFilterState>) => void;
  getResource: (value: string) => string;
}

const LogMessageFilterListItem: React.FC<IProps> = (props: IProps) => {
  const {
    title,
    subTitle,
    isReadonly,
    filterTextProps,
    filterDropdownProps,
    logFilterState,
    onChange,
    getResource,
  } = props;

  const handleLogLevelChanged = React.useCallback(
    (logLevel: LogLevelEnum) => {
      const logLevels = [...logFilterState.selectedLogLevels];

      if (logLevels.includes(logLevel)) {
        const index = logLevels.indexOf(logLevel);
        logLevels.splice(index, 1);
      } else {
        logLevels.push(logLevel);
      }

      onChange({ selectedLogLevels: logLevels });
    },
    [logFilterState.selectedLogLevels, onChange]
  );

  const onFilterTextChange = React.useCallback(
    (newValue: string) => {
      onChange({ filterText: newValue });
    },
    [onChange]
  );

  const onFilterDropdownChange = React.useCallback(
    (newValue: number | null) => {
      onChange({ selectedItemId: newValue });
    },
    [onChange]
  );

  const onClearFilterDropdown = React.useCallback(() => {
    onChange({ selectedItemId: null });
  }, [onChange]);

  const onClearFilterText = React.useCallback(() => {
    onChange({ filterText: '' });
  }, [onChange]);

  return (
    <ListItem
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'flex-start',
        justifyContent: 'space-between',
        px: 2,
        py: 1.5,
        borderBottom: '1px solid',
        borderColor: 'divider',
      }}
      disableGutters
    >
      <Box sx={{ mb: 1 }}>
        <Typography variant="subtitle1" component="div">
          {title}
        </Typography>
        {subTitle && (
          <Typography variant="body2" color="text.secondary">
            {subTitle}
          </Typography>
        )}
      </Box>
      <Box
        display="flex"
        width="100%"
        justifyContent="space-between"
        mt={2}
        sx={{
          flexDirection: { md: 'column', xl: 'row' },
        }}
      >
        <Box display="flex" gap={2} alignItems="baseline">
          <FilterInput
            {...filterTextProps}
            filterText={logFilterState.filterText}
            isReadonly={isReadonly}
            onFilterTextChange={onFilterTextChange}
            onClearFilter={onClearFilterText}
          />
          <FilterDropdown
            {...filterDropdownProps}
            selectedItemId={logFilterState.selectedItemId}
            isReadonly={isReadonly}
            onSelectItem={onFilterDropdownChange}
            onClearSelection={onClearFilterDropdown}
          />
        </Box>
        <Box
          display="flex"
          alignItems="baseline"
          gap={2}
          sx={{ justifyContent: { md: 'flex-start', xl: 'flex-end' }, mt: { md: 2, xl: 2 } }}
        >
          <FormCheckbox
            disabled={isReadonly}
            label={getResource('common.labelInfo')}
            checked={logFilterState.selectedLogLevels.includes(LogLevelEnum.Info)}
            onChange={handleLogLevelChanged.bind(null, LogLevelEnum.Info)}
          />
          <FormCheckbox
            disabled={isReadonly}
            label={getResource('common.labelWarning')}
            checked={logFilterState.selectedLogLevels.includes(LogLevelEnum.Warning)}
            onChange={handleLogLevelChanged.bind(null, LogLevelEnum.Warning)}
          />
          <FormCheckbox
            disabled={isReadonly}
            label={getResource('common.labelError')}
            checked={logFilterState.selectedLogLevels.includes(LogLevelEnum.Error)}
            onChange={handleLogLevelChanged.bind(null, LogLevelEnum.Error)}
          />
        </Box>
      </Box>
    </ListItem>
  );
};

export default React.memo(LogMessageFilterListItem);
