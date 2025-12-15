import React from 'react';
import { IFileImportComponentInitializationProps } from './interfaces/IFileImportComponentInitializationProps';
import List from 'src/components/lists/List';
import HeaderListItem from 'src/components/lists/HeaderListItem';
import TableListItem from 'src/components/lists/TableListItem';
import TableLabel from 'src/components/table/components/TableLabel';
import { IFileImportModel } from './interfaces/IFileImportModel';
import { Column, Table } from 'src/components/table/Table';
import TableStatusIcon from 'src/components/table/components/TableStatusIcon';
import { FileImportStatusEnum } from 'src/lib/enums/FileImportStatusEnum';
import TableIconGroup from 'src/components/table/components/TableIconGroup';
import { FileImportTypeEnum } from 'src/lib/enums/FileImportTypeEnum';
import { INotificationBadgeState } from 'src/lib/interfaces/INotificationBadgeState';
import Notification from 'src/components/Notification';

interface IFileImportProps extends IFileImportComponentInitializationProps {
  isReadonly: boolean;
}

const FileImport: React.FC<IFileImportProps> = (props: IFileImportProps) => {
  const { fileModels, isReadonly, fileImportApi, executeFileImportApi, getResource } = props;

  const [files, setFiles] = React.useState<IFileImportModel[]>(fileModels);
  const [notificationBadge, setNotificationBadge] = React.useState<INotificationBadgeState>({
    show: false,
    message: '',
    color: 'success',
    duration: 3000,
  });

  const getStatus = React.useCallback(
    (status: FileImportStatusEnum): 'success' | 'pending' | 'error' => {
      switch (status) {
        case FileImportStatusEnum.Success:
          return 'success';
        case FileImportStatusEnum.Pending:
          return 'pending';
        case FileImportStatusEnum.Failed:
          return 'error';
        default:
          return 'pending';
      }
    },
    []
  );

  const getFileTypeLabel = React.useCallback(
    (type: FileImportTypeEnum): string => {
      switch (type) {
        case FileImportTypeEnum.Family:
          return getResource('common.labelFileTypeFamily');
        default:
          return getResource('common.labelUnknown');
      }
    },
    [getResource]
  );

  const getFileToolTipLabel = React.useCallback(
    (status: FileImportStatusEnum): string => {
      switch (status) {
        case FileImportStatusEnum.Success:
          return getResource('common.labelStatusSuccess');
        case FileImportStatusEnum.Pending:
          return getResource('common.labelStatusPending');
        case FileImportStatusEnum.Failed:
          return getResource('common.labelStatusFailed');
        default:
          return '';
      }
    },
    [getResource]
  );

  const handleDeleteAllImportedFiles = React.useCallback(async (): Promise<void> => {
    fileImportApi.post('/fileimport/deletefiles', undefined).then((response) => {
      setFiles(response);
    });
  }, [fileImportApi]);

  const handleImportFile = React.useCallback(
    async (fileId: number): Promise<void> => {
      const response = await executeFileImportApi.post(
        `/fileimport/importfile?id=${fileId}`,
        undefined
      );
      setFiles(response.Data);

      setNotificationBadge({
        show: true,
        message: getResource(response.ResourceKey),
        color: response.success ? 'success' : 'error',
        duration: 5000,
      });
    },
    [executeFileImportApi, getResource]
  );

  const handleDeleteFile = React.useCallback(
    async (fileId: number): Promise<void> => {
      await fileImportApi
        .post(`/fileimport/deletefile/?id=${fileId}`, undefined)
        .then((response) => {
          setFiles(response);
        });
    },
    [fileImportApi]
  );

  const handleResetNotification = React.useCallback(() => {
    setNotificationBadge((prev) => ({
      ...prev,
      show: false,
    }));
  }, []);

  const columns = React.useMemo((): Column<IFileImportModel>[] => {
    return [
      {
        key: 'fileId',
        header: '',
        align: 'left',
        hidden: true,
        minWidth: 0,
        render: (row: IFileImportModel) => <TableLabel value={row.fileId.toString()} />,
      },
      {
        key: 'fileName',
        header: getResource('common.captionFileName'),
        align: 'left',
        hidden: false,
        minWidth: 200,
        render: (row: IFileImportModel) => <TableLabel value={row.fileName} />,
      },
      {
        key: 'fileDate',
        header: getResource('common.captionFileImportDate'),
        align: 'center',
        minWidth: 100,
        render: (row: IFileImportModel) => <TableLabel value={row.fileDate} />,
      },
      {
        key: 'fileType',
        header: getResource('common.captionFileType'),
        align: 'left',
        minWidth: 100,
        render: (row: IFileImportModel) => <TableLabel value={getFileTypeLabel(row.fileType)} />,
      },
      {
        key: 'status',
        header: getResource('common.captionStatus'),
        align: 'center',
        minWidth: 50,
        render: (row: IFileImportModel) => (
          <TableStatusIcon
            status={getStatus(row.status)}
            size={18}
            toolTipText={getFileToolTipLabel(row.status)}
          />
        ),
      },
      {
        key: 'lastUpdate',
        header: getResource('common.captionLastUpdate'),
        align: 'center',
        minWidth: 100,
        render: (row: IFileImportModel) => <TableLabel value={row.lastUpdate} />,
      },
      {
        key: 'lastUpdateBy',
        header: getResource('common.captionLastUpdateBy'),
        align: 'left',
        minWidth: 100,
        render: (row: IFileImportModel) => <TableLabel value={row.lastUpdateBy} />,
      },
      {
        key: 'actions',
        header: getResource('common.captionActions'),
        align: 'center',
        minWidth: 100,
        render: (row: IFileImportModel) => (
          <TableIconGroup
            spacing={1}
            icons={[
              {
                id: row.fileId,
                size: 16,
                color: '#2196f3',
                iconClassName: 'bi bi-file-earmark-arrow-up',
                disabled: isReadonly || row.status === FileImportStatusEnum.Success,
                onClick: handleImportFile.bind(null, row.fileId),
              },
              {
                id: row.fileId,
                size: 16,
                color: '#f44336',
                iconClassName: 'bi bi-trash',
                disabled: isReadonly || row.status === FileImportStatusEnum.Pending,
                onClick: handleDeleteFile.bind(null, row.fileId),
              },
            ]}
          />
        ),
      },
    ];
  }, [
    isReadonly,
    getResource,
    getFileToolTipLabel,
    getFileTypeLabel,
    getStatus,
    handleDeleteFile,
    handleImportFile,
  ]);

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
          title={getResource('common.captionFileImport')}
          subTitle={getResource('common.labelManageFileImports')}
          iconButtonProps={[
            {
              icon: 'bi bi-trash',
              size: 20,
              color: '#f44336',
              tooltip: getResource('common.labelDeleteImportedFiles'),
              disabled: isReadonly || files.length === 0,
              onClick: handleDeleteAllImportedFiles,
            },
          ]}
        />
        <TableListItem
          title={getResource('common.captionImportedFiles')}
          subTitle={getResource('common.subTitleImportedFiles')}
        >
          <Table<IFileImportModel> columns={columns} data={files} maxHeight="600px" />
        </TableListItem>
      </List>

      <Notification {...notificationBadge} handleResetNotification={handleResetNotification} />
    </div>
  );
};

export default React.memo(FileImport);
