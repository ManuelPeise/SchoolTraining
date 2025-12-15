import React from 'react';
import { ILogComponentInitializationProps } from './interfaces/ILogComponentInitializationProps';

const LogPage: React.FC<ILogComponentInitializationProps> = (
  props: ILogComponentInitializationProps
) => {
  console.log(props);
  return <div>Log Page</div>;
};

export default React.memo(LogPage);
