import React from 'react';
import { ILocationProps } from 'src/lib/interfaces/ILocationProps';
import { IFamilyModel } from './interfaces/IFamilyModel';
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
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';
import { INotificationResponse } from 'src/lib/interfaces/INotificationResponse';
import Notification from 'src/components/Notification';
import { INotificationBadgeState } from 'src/lib/interfaces/INotificationBadgeState';

interface IProps extends ILocationProps {
  isReadonly?: boolean;
  families: IFamilyModel[];
  fileApi: IStatelessApi<INotificationResponse, any>;
  familyApi: IStatelessApi<IFamilyModel[], IFamilyModel[]>;
  getResource: (key: string) => string;
  setIsLoading: (isLoading: boolean) => void;
}

const FamilyAdministration: React.FC<IProps> = (props: IProps) => {
  const { isReadonly, fileApi, familyApi, families, getResource, setIsLoading } = props;
  const originalFamilies = React.useRef<IFamilyModel[]>(families);
  const fileInputRef = React.useRef<HTMLInputElement>(null);
  const [familiesState, setFamiliesState] = React.useState<IFamilyModel[]>(families);

  const [notificationBadge, setNotificationBadge] = React.useState<INotificationBadgeState>({
    show: false,
    message: '',
    color: 'success',
    duration: 3000,
  });

  const isModified = React.useMemo((): boolean => {
    let hasModifcations = false;

    originalFamilies.current.forEach((originalFamily) => {
      const currentFamily = familiesState.find((f) => f.familyId === originalFamily.familyId);

      if (currentFamily?.isActive !== originalFamily.isActive) {
        hasModifcations = true;
      }
    });
    return hasModifcations;
  }, [originalFamilies, familiesState]);

  const handleReset = React.useCallback(() => {
    setFamiliesState(originalFamilies.current);
  }, []);

  const getModifiedModels = React.useCallback((): IFamilyModel[] => {
    const modifiedFamilies: IFamilyModel[] = [];

    originalFamilies.current.forEach((originalFamily) => {
      const currentFamily = familiesState.find((f) => f.familyId === originalFamily.familyId);

      if (currentFamily !== undefined && currentFamily?.isActive !== originalFamily.isActive) {
        modifiedFamilies.push(currentFamily);
      }
    });

    return modifiedFamilies;
  }, [familiesState, originalFamilies]);

  const handleSave = React.useCallback(async () => {
    const modifiedFamilies = getModifiedModels();

    setIsLoading(true);

    if (modifiedFamilies.length > 0) {
      const response = await familyApi.post(
        '/familyadministration/updatefamilies',
        modifiedFamilies
      );

      setFamiliesState(response);
      originalFamilies.current = response;
      setNotificationBadge({
        show: true,
        message: getResource('common.labelFamiliesSavedSuccessfully'),
        color: 'success',
        duration: 3000,
      });
      setIsLoading(false);
    }
  }, [familyApi, getModifiedModels, getResource, setIsLoading]);

  const onIsActiveChanged = React.useCallback((familyId: number) => {
    setFamiliesState((prevFamilies) => {
      const updatedFamilies = prevFamilies.map((family) => {
        return family.familyId === familyId ? { ...family, isActive: !family.isActive } : family;
      });
      return updatedFamilies;
    });
  }, []);

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
            onChange={() => onIsActiveChanged(row.familyId)}
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
        render: (row: IFamilyModel) => <TableLabel value={row.contactMailAddress} />,
      },
      {
        key: 'lastUpdatedBy',
        header: getResource('common.captionLastUpdateBy'),
        align: 'left',
        minWidth: 150,
        render: (row: IFamilyModel) => <TableLabel value={row.lastUpdateBy} />,
      },
      {
        key: 'lastUpdatedAt',
        header: getResource('common.captionLastUpdateAt'),
        align: 'left',
        minWidth: 150,
        render: (row: IFamilyModel) => <TableLabel value={row.lastUpdateAt} />,
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
  }, [getResource, onIsActiveChanged, isReadonly]);

  const saveCancelButtonProps = React.useMemo((): ISaveCancelButtonsProps => {
    return {
      labelSave: getResource('common.labelSave'),
      saveDisabled: !isModified,
      labelCancel: getResource('common.labelCancel'),
      saveAction: handleSave,
      cancelAction: handleReset,
    };
  }, [isModified, handleReset, handleSave, getResource]);

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
    await fileApi.downloadFile('/familyadministration/downloadfamilyimporttemplate');
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
              onClick: () => {
                if (fileInputRef.current) {
                  fileInputRef.current.click();
                }
              },
              fileUploadCallback: handleFileUpload,
            },
          ]}
        />
        <TableListItem
          title={getResource('common.captionFamiliesInSystem')}
          subTitle={getResource('common.subTitleFamiliesInSystem')}
        >
          <Table<IFamilyModel> columns={columns} data={familiesState} maxHeight="400px" />
        </TableListItem>
      </List>
      <SaveCancelButtons {...saveCancelButtonProps} />
      <Notification {...notificationBadge} handleResetNotification={handleResetNotification} />
    </div>
  );
};

export default React.memo(FamilyAdministration);
