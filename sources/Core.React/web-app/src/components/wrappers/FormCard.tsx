import React from 'react';
import styles from './Card.module.css';

interface IProps extends React.PropsWithChildren {
  minwidth?: string;
  padding?: string;
}

const FormCard: React.FC<IProps> = (props: IProps) => {
  return (
    <div className={styles.formCard} style={{ minWidth: props.minwidth, padding: props.padding }}>
      {props.children}
    </div>
  );
};

export default FormCard;
