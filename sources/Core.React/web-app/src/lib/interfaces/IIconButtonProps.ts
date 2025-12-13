export interface IIconButtonProps {
  icon: string;
  size?: number;
  inputRef?: React.RefObject<HTMLInputElement | null>;
  fileUploadCallback?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onClick?: () => Promise<void> | void;
}
