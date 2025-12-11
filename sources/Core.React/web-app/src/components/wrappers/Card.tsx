import React from 'react';
import styles from './Card.module.css';

interface IProps extends React.PropsWithChildren {}

const Card: React.FC<IProps> = (props: IProps) => {
  return <div className={styles.card}>{props.children}</div>;
};

export default Card;
