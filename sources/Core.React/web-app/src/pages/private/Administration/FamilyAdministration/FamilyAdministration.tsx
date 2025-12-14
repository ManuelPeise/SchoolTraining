import React from 'react';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IFamilyModel } from './interfaces/IFamilyModel';
import { useForm } from 'src/hooks/useForm';
import List from 'src/components/lists/List';
import HeaderListItem from 'src/components/lists/HeaderListItem';
import Table, { Column } from 'src/components/table/Table';
import TableListItem from 'src/components/lists/TableListItem';
import TableIconButton from 'src/components/table/components/TableIconButton';
import TableCheckbox from 'src/components/table/components/TableCheckbox';
import TableLabel from 'src/components/table/components/TableLabel';
import SaveCancelButtons, {
  ISaveCancelButtonsProps,
} from 'src/components/groups/SaveCancelButtons';
import { dummyFamilies } from './dummyFamilies';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationResponse } from 'src/lib/interfaces/INotificationResponse';
import Notification from 'src/components/Notification';

interface INotificationBadgeState {
  show: boolean;
  message: string;
  color: 'success' | 'error' | 'info' | 'warning';
  duration: number;
}
interface IProps extends ILocationProps {
  isReadonly?: boolean;
  families: IFamilyModel[];
  fileApi: IStatelessApi<INotificationResponse, any>;
  getResource: (key: string) => string;
}

const FamilyAdministration: React.FC<IProps> = (props: IProps) => {
  const { getResource, isReadonly, fileApi } = props;

  const [notificationBadge, setNotificationBadge] = React.useState<INotificationBadgeState>({
    show: false,
    message: '',
    color: 'success',
    duration: 5000,
  });

  const fileInputRef = React.useRef<HTMLInputElement>(null);

  const { isModified, isValid, values, updateArrayItemIndex, resetForm } =
    useForm<IFamilyModel[]>(dummyFamilies);

  const columns = React.useMemo((): Column<IFamilyModel>[] => {
    return [
      { key: 'familyId', header: getResource('common.familyId'), align: 'left', hidden: true },
      {
        key: 'isActive',
        header: getResource('common.captionIsActive'),
        align: 'center',
        minWidth: 100,
        render: (row: IFamilyModel, rowIndex: number) => (
          <TableCheckbox
            propertyName="isActive"
            rowIndex={rowIndex}
            model={row}
            disabled={isReadonly}
            checked={row.isActive}
            onChange={updateArrayItemIndex}
          />
        ),
      },
      {
        key: 'name',
        header: getResource('common.captionName'),
        align: 'left',
        minWidth: 200,
        render: (row: IFamilyModel) => <TableLabel value={row.name} />,
      },
      {
        key: 'contactMailAddress',
        header: getResource('common.captionContactMailAddress'),
        align: 'left',
        minWidth: 250,
      },
      {
        key: 'createdBy',
        header: getResource('common.captionCreatedBy'),
        align: 'left',
        minWidth: 150,
      },
      {
        key: 'createdAt',
        header: getResource('common.captionCreatedAt'),
        align: 'left',
        minWidth: 150,
      },
      {
        key: 'delete',
        header: getResource('common.captionDeleteFamily'),
        align: 'center',
        minWidth: 150,
        render: () => (
          <TableIconButton
            iconClassName="bi bi-trash"
            size={18}
            disabled={isReadonly}
            onClick={async () => {}}
          />
        ),
      },
    ];
  }, [getResource, updateArrayItemIndex, isReadonly]);

  const handleSave = React.useCallback(async () => {
    // Handle save logic here
  }, []);

  const saveCancelButtonProps = React.useMemo((): ISaveCancelButtonsProps => {
    return {
      labelSave: getResource('common.labelSave'),
      saveDisabled: !isModified || !isValid,
      labelCancel: getResource('common.labelCancel'),
      saveAction: handleSave,
      cancelAction: resetForm,
    };
  }, [isModified, isValid, handleSave, resetForm, getResource]);

  const handleFileUpload = React.useCallback(
    async (event: React.ChangeEvent<HTMLInputElement>) => {
      const files = event.target.files;
      if (files && files.length > 0 && files[0]) {
        const response = await fileApi.postFile(
          '/familyadministration/uploadfamilytemplatefile',
          files[0]
        );

        setNotificationBadge({
          show: true,
          message: getResource(response.resourceKey),
          color: response.success ? 'success' : 'error',
          duration: 5000,
        });
      }
    },
    [fileApi, getResource]
  );

  const handleFileDownloadClicked = React.useCallback(async () => {
    await fileApi.downloadFile(
      '/familyadministration/downloadfamilyimporttemplate'
      // 'FamilyImport_FamilyName_YYYYMMDD.json'
    );
  }, [fileApi]);

  const handleResetNotification = React.useCallback(() => {
    setNotificationBadge((prev) => ({
      ...prev,
      show: false,
    }));
  }, []);

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
          title={getResource('common.captionFamilies')}
          subTitle={getResource('common.subTitleFamilies')}
          iconButtonProps={[
            {
              icon: 'bi bi-download',
              size: 20,
              tooltip: getResource('common.labelDownloadFileTemplate'),
              disabled: isReadonly,
              onClick: async () => {
                await handleFileDownloadClicked().then(() => {
                  setNotificationBadge({
                    show: true,
                    message: getResource('common.labelDownloadFileTemplate'),
                    color: 'success',
                    duration: 3000,
                  });
                });
              },
            },
            {
              icon: 'bi bi-upload',
              inputRef: fileInputRef,
              tooltip: getResource('common.labelUploadFile'),
              disabled: isReadonly,
              size: 20,
              fileUploadCallback: handleFileUpload,
            },
          ]}
        />
        <TableListItem
          title={getResource('common.captionFamiliesInSystem')}
          subTitle={getResource('common.subTitleFamiliesInSystem')}
        >
          <Table<IFamilyModel> columns={columns} data={values} maxHeight="400px" />
        </TableListItem>
      </List>
      <SaveCancelButtons {...saveCancelButtonProps} />
      <Notification {...notificationBadge} handleResetNotification={handleResetNotification} />
    </div>
  );
};

export default React.memo(FamilyAdministration);
