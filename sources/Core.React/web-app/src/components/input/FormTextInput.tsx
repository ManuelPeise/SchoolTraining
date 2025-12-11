import React from 'react';
import styles from './Input.module.css';

interface IProps {
  label?: string;
  placeholder?: string;
  disabled?: boolean;
  type: string;

  value: string;
  onChange: (value: string) => void;
}

const FormTextInput: React.FC<IProps> = (props: IProps) => {
  const { label, placeholder, disabled, type, value, onChange } = props;

  return (
    <div className={styles.formGroup}>
      {label && <label className={styles.formLabel}>{label}</label>}
      <input
        className={styles.formInput}
        type={type}
        disabled={disabled}
        placeholder={placeholder}
        value={value}
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
};

export default FormTextInput;
