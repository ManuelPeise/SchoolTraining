import React from 'react';
import styles from './Title.module.css';

interface IProps {
  text: string;
}
const Title: React.FC<IProps> = (props: IProps) => {
  return <p className={styles.title}>{props.text}</p>;
};

export default Title;
