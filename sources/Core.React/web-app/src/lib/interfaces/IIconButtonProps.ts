export interface IIconButtonProps {
  icon: string;
  tooltip?: string;
  disabled?: boolean;
  size?: number;
  color?: string;
  inputRef?: React.RefObject<HTMLInputElement | null>;
  fileUploadCallback?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onClick?: () => Promise<void> | void;
}
