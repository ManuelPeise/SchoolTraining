import React from 'react';
import { ILogComponentInitializationProps } from './interfaces/ILogComponentInitializationProps';
import List from 'src/components/lists/List';
import HeaderListItem from 'src/components/lists/HeaderListItem';
import TableListItem from 'src/components/lists/TableListItem';
import LogMessageFilterListItem from './components/LogMessageFilterListItem';
import { Column, Table } from 'src/components/table/Table';
import { ILogMessage } from './interfaces/ILogMessage';
import TableLabel from 'src/components/table/components/TableLabel';
import { LogLevelEnum } from 'src/lib/enums/LogLevelEnum';
import TableIconGroup from 'src/components/table/components/TableIconGroup';
import { FilterDropdownItem } from 'src/components/input/FilterDropdown';

type LogFilterState = {
  filterText: string;
  selectedItemId: number | null;
  selectedLogLevels: number[];
};

const LogPage: React.FC<ILogComponentInitializationProps> = (
  props: ILogComponentInitializationProps
) => {
  const { logMessages, isReadonly, getResource } = props;

  const [logFilterState, setLogFilterState] = React.useState<LogFilterState>({
    filterText: '',
    selectedItemId: null,
    selectedLogLevels: [],
  });

  const handleLogFilterStateChanged = React.useCallback((newState: Partial<LogFilterState>) => {
    setLogFilterState((prevState) => ({
      ...prevState,
      ...newState,
    }));
  }, []);

  const getLogLevelLabel = React.useCallback(
    (logLevel: number): string => {
      switch (logLevel) {
        case LogLevelEnum.Info:
          return getResource('common.labelLogLevelInfo');
        case LogLevelEnum.Warning:
          return getResource('common.labelLogLevelWarning');
        case LogLevelEnum.Error:
          return getResource('common.labelLogLevelError');
        case LogLevelEnum.CriticalError:
          return getResource('common.labelLogLevelCriticalError');
        default:
          return '';
      }
    },
    [getResource]
  );

  const modules = React.useMemo((): FilterDropdownItem[] => {
    const moduleSet: FilterDropdownItem[] = [];
    logMessages.forEach((msg) => {
      if (
        msg.module !== undefined &&
        msg.module !== null &&
        !moduleSet.find((m) => m.label === msg.module)
      ) {
        moduleSet.push({ id: moduleSet.length, label: msg.module });
      }
    });
    return moduleSet;
  }, [logMessages]);

  const filteredLogMessages = React.useMemo((): ILogMessage[] => {
    let allMessages = [...logMessages];

    if (logFilterState.filterText.length > 0) {
      allMessages = allMessages.filter((msg) =>
        msg.message.toLocaleLowerCase().startsWith(logFilterState.filterText.toLocaleLowerCase())
      );
    }

    if (logFilterState.selectedItemId !== null) {
      allMessages = allMessages.filter(
        (msg) => modules.find((m) => m.label === msg.module)?.id === logFilterState.selectedItemId
      );
    }

    if (logFilterState.selectedLogLevels.length > 0) {
      allMessages = allMessages.filter((msg) =>
        logFilterState.selectedLogLevels.includes(msg.logLevel)
      );
    }
    return allMessages;
  }, [
    logMessages,
    logFilterState.selectedItemId,
    logFilterState.selectedLogLevels,
    logFilterState.filterText,
    modules,
  ]);

  const columns = React.useMemo((): Column<ILogMessage>[] => {
    return [
      {
        key: 'id',
        header: '',
        align: 'left',
        hidden: true,
        minWidth: 0,
        render: (row: ILogMessage) => <TableLabel value={row.id.toString()} />,
      },
      {
        key: 'timeStamp',
        header: getResource('common.captionTimeStamp'),
        align: 'left',
        minWidth: 100,
        render: (row: ILogMessage) => <TableLabel value={row.timeStamp} />,
      },
      {
        key: 'logLevel',
        header: getResource('common.captionLogLevel'),
        align: 'left',
        minWidth: 100,
        render: (row: ILogMessage) => <TableLabel value={getLogLevelLabel(row.logLevel)} />,
      },
      {
        key: 'module',
        header: getResource('common.captionModule'),
        align: 'left',
        minWidth: 100,
        render: (row: ILogMessage) => <TableLabel value={row.module?.toString() ?? ''} />,
      },
      {
        key: 'message',
        header: getResource('common.captionMessage'),
        align: 'left',
        hidden: false,
        minWidth: 200,
        render: (row: ILogMessage) => <TableLabel value={row.message} />,
      },
      {
        key: 'exeptionMessage',
        header: getResource('common.captionExceptionMessage'),
        align: 'center',
        minWidth: 100,
        render: (row: ILogMessage) => <TableLabel value={row.exeptionMessage} />,
      },
      {
        key: 'stackTrace',
        header: getResource('common.captionStackTrace'),
        align: 'left',
        minWidth: 100,
        render: (row: ILogMessage) => (
          <TableIconGroup
            spacing={1}
            icons={[
              {
                id: row.id,
                size: 16,
                color: '#2196f3',
                iconClassName: 'bi bi-file-earmark-arrow-up',
                tooltip: row.stackTrace,
                disabled: false,
                onClick: () => {},
              },
            ]}
          />
        ),
      },
    ];
  }, [getLogLevelLabel, getResource]);

  return (
    <div
      style={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-between',
      }}
    >
      <List minHeight={200} maxHeight={500}>
        <HeaderListItem
          title={getResource('common.captionLogMessages')}
          subTitle={getResource('common.labelManageLogMessages')}
          iconButtonProps={[
            {
              icon: 'bi bi-eraser',
              size: 20,
              color: '#f44336',
              tooltip: getResource('common.labelCleanupLogmessages'),
              disabled: isReadonly || logMessages.length === 0,
              onClick: () => {},
            },
          ]}
        />
        <LogMessageFilterListItem
          title={getResource('common.captionLogMessageFilter')}
          subTitle={getResource('common.labelFilterLogMessages')}
          isReadonly={isReadonly || logMessages.length === 0}
          logFilterState={logFilterState}
          filterTextProps={{
            placeholder: getResource('common.placeholderFilterLogMessages'),
          }}
          filterDropdownProps={{
            placeholder: getResource('common.placeholderSelectModule'),
            items: [],
          }}
          getResource={getResource}
          onChange={handleLogFilterStateChanged}
        />
        <TableListItem
          title={getResource('common.captionAvailableLogMessages')}
          subTitle={getResource('common.subTitleAvailableLogMessages')}
        >
          <Table<ILogMessage> columns={columns} data={filteredLogMessages} maxHeight="600px" />
        </TableListItem>
      </List>

      {/* <Notification {...notificationBadge} handleResetNotification={handleResetNotification} /> */}
    </div>
  );
};

export default React.memo(LogPage);
