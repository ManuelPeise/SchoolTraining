import React from 'react';
import styles from './Input.module.css';

interface IProps {
  label: string;
  checked: boolean;
  disabled?: boolean;
  onChange: (checked: boolean) => void;
}

const FormCheckbox: React.FC<IProps> = (props: IProps) => {
  const { label, checked, disabled, onChange } = props;

  return (
    <div className={styles.formCheckGroup}>
      <input
        className={styles.formCheckbox}
        type="checkbox"
        disabled={disabled}
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
      />
      <label className={styles.formCheckLabel}>{label}</label>
    </div>
  );
};

export default FormCheckbox;
