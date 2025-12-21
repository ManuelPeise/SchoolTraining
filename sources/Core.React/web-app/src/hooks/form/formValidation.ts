export const validateString = (value: string): boolean => {
  if (!value || value.trim().length === 0) {
    return false;
  }
  return true;
};
