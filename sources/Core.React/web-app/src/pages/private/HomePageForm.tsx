import React from 'react';

import { TestModel } from './HomePage';
import { UseFormResult } from 'src/hooks/form/FormTypes';

interface IProps {
  form: UseFormResult<TestModel>;
}

const HomePageForm: React.FC<IProps> = (props) => {
  const { form } = props;

  return (
    <div>
      <form.TextField fieldKey="name" fieldPropsCallback={form.getFieldPropsByKey} />
      <form.TextField fieldKey="lastName" fieldPropsCallback={form.getFieldPropsByKey} />
    </div>
  );
};

export default React.memo(HomePageForm);
