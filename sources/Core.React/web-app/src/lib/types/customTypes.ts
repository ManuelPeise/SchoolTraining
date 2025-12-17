type DisabledProp<TModel, Key extends keyof TModel> = {
  [K in Key as `${string & K}Disabled`]: boolean;
};
