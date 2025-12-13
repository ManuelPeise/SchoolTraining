/**
 * Generic form hook for any model type.
 * @param initialValues Initial values for the form model
 */

import { useEffect, useState, useRef } from 'react';

// Deep equality check for objects/arrays
function deepEqual(a: any, b: any): boolean {
  if (a === b) return true;
  if (typeof a !== typeof b) return false;
  if (Array.isArray(a) && Array.isArray(b)) {
    if (a.length !== b.length) return false;
    for (let i = 0; i < a.length; i++) {
      if (!deepEqual(a[i], b[i])) return false;
    }
    return true;
  }

  if (typeof a === 'object' && typeof b === 'object' && a && b) {
    const keysA = Object.keys(a);
    const keysB = Object.keys(b);
    if (keysA.length !== keysB.length) return false;
    for (let key of keysA) {
      if (!deepEqual(a[key], b[key])) return false;
    }
    return true;
  }
  return false;
}

export const useForm = <TModel extends Record<string, any>>(initialValues: TModel) => {
  const [values, setValues] = useState<TModel>(initialValues);
  const [isModified, setIsModified] = useState(false);
  const [isValid, setIsValid] = useState(true);
  const initialRef = useRef(initialValues);

  const isNumber = (val: any) =>
    typeof val === 'number' || (!isNaN(val) && val !== '' && /^-?\d+$/.test(val));

  // Enhanced validation: numbers, required, arrays, and objects
  const validate = (vals: any, refVals: any = initialRef.current): boolean => {
    for (const key in vals) {
      const value = vals[key];
      const refValue = refVals ? refVals[key] : undefined;
      if (typeof refValue === 'number') {
        if (!isNumber(value)) return false;
      }
      if (value === undefined || value === null) return false;
      if (typeof value === 'string' && value.trim() === '') return false;
      if (Array.isArray(value)) {
        if (value.length === 0) return false;
        for (let i = 0; i < value.length; i++) {
          const item = value[i];
          const refItem = Array.isArray(refValue) ? refValue[i] : undefined;
          if (typeof item === 'object' && item !== null) {
            if (!validate(item, refItem)) return false;
          } else if (item === undefined || item === null || item === '') {
            return false;
          }
        }
      }
      if (typeof value === 'object' && value !== null && !Array.isArray(value)) {
        if (!validate(value, refValue)) return false;
      }
    }
    return true;
  };

  useEffect(() => {
    setIsModified(!deepEqual(values, initialRef.current));
    setIsValid(validate(values));
  }, [values]);

  // Array helpers
  const addToArray = <K extends keyof TModel>(key: K, item: any) => {
    setValues((prev) => {
      const arr = Array.isArray(prev[key]) ? prev[key] : [];
      return { ...prev, [key]: [...arr, item] };
    });
  };

  const removeFromArray = <K extends keyof TModel>(key: K, index: number) => {
    setValues((prev) => {
      const arr = Array.isArray(prev[key]) ? prev[key] : undefined;
      if (!arr || index < 0 || index >= arr.length) return prev;
      return { ...prev, [key]: arr.filter((_: any, i: number) => i !== index) };
    });
  };

  const updateArrayItem = <K extends keyof TModel>(key: K, index: number, newItem: any) => {
    setValues((prev) => {
      const arr = Array.isArray(prev[key]) ? prev[key] : undefined;
      if (!arr || index < 0 || index >= arr.length) return prev;
      return {
        ...prev,
        [key]: arr.map((item: any, i: number) => (i === index ? newItem : item)),
      };
    });
  };

  const updateArrayItemIndex = (rowIndex: number, newItem: any) => {
    setValues((prev) => {
      if (rowIndex < 0 || rowIndex >= (prev as any).length) return prev;
      return (prev as any).map((item: any, i: number) => (i === rowIndex ? newItem : item));
    });
  };

  const handleChange = <K extends keyof TModel>(key: K, value: TModel[K]) => {
    setValues((prev) => {
      const prevValue = prev[key];
      if (typeof prevValue === 'number') {
        if (isNumber(value)) {
          return { ...prev, [key]: Number(value) };
        } else {
          // Ignore invalid number input
          return prev;
        }
      }
      return { ...prev, [key]: value };
    });
  };

  const setFieldValue = <K extends keyof TModel>(key: K, value: TModel[K]) => {
    setValues((prev) => {
      const prevValue = prev[key];
      if (typeof prevValue === 'number') {
        if (isNumber(value)) {
          return { ...prev, [key]: Number(value) };
        } else {
          return prev;
        }
      }
      return { ...prev, [key]: value };
    });
  };

  const resetForm = () => {
    setValues(initialRef.current);
  };

  return {
    values,
    isModified,
    isValid,
    setValues,
    handleChange,
    setFieldValue,
    resetForm,
    addToArray,
    removeFromArray,
    updateArrayItem,
    updateArrayItemIndex,
  };
};
