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

interface IProps extends ILocationProps {
  families: IFamilyModel[];
  getResource: (key: string) => string;
}

const FamilyAdministration: React.FC<IProps> = (props: IProps) => {
  const { getResource } = props;

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
          <TableIconButton iconClassName="bi bi-trash" size={18} onClick={async () => {}} />
        ),
      },
    ];
  }, [getResource]);

  const saveCancelButtonProps = React.useMemo((): ISaveCancelButtonsProps => {
    return {
      labelSave: getResource('common.labelSave'),
      saveDisabled: !isModified || !isValid,
      labelCancel: getResource('common.labelCancel'),
      saveAction: async () => {
        // Handle save action
      },
      cancelAction: resetForm,
    };
  }, [isModified, isValid, resetForm, getResource]);

  const handleFileUpload = React.useCallback(async (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (files && files.length > 0) {
      const file = files[0];
      // Handle file upload action
      console.log('Uploaded file:', file.name);
    }
  }, []);

  const handleFileDownloadClicked = React.useCallback(async () => {
    fileInputRef.current?.click();
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
              inputRef: fileInputRef,
              onClick: handleFileDownloadClicked,
              fileUploadCallback: handleFileUpload,
            },
            {
              icon: 'bi bi-upload',
              size: 20,
              onClick: async () => {
                // Handle upload action
              },
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
    </div>
  );
};

export default React.memo(FamilyAdministration);
