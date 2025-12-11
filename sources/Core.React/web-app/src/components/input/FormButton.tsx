import React from 'react';
import styles from './Input.module.css';

interface IProps {
  label: string;
  disabled?: boolean;
  onClick: () => void | Promise<void>;
}

const FormButton: React.FC<IProps> = (props: IProps) => {
  const { label, disabled, onClick } = props;

  return (
    <div className={styles.fomButtonGroup}>
      <button disabled={disabled} onClick={onClick} className={styles.formButton}>
        {label}
      </button>
    </div>
  );
};

export default FormButton;
