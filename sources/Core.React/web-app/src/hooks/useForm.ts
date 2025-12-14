/**
 * Generic form hook for any model type.
 * @param initialValues Initial values for the form model
 */

import React, { useEffect, useState, useRef } from 'react';

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
    for (const key of keysA) {
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

  const isNumber = React.useCallback(
    (val: any) => typeof val === 'number' || (!isNaN(val) && val !== '' && /^-?\d+$/.test(val)),
    []
  );

  // Enhanced validation: numbers, required, arrays, and objects
  const validate = React.useCallback(
    (vals: any, refVals: any = initialRef.current): boolean => {
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
    },
    [isNumber]
  );

  useEffect(() => {
    setIsModified(!deepEqual(values, initialRef.current));
    setIsValid(validate(values));
  }, [values, validate]);

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

  /**
   * Returns modified items in an array field of the form model, comparing by specified keys.
   * @param arrayFieldKey The key of the array field in the form model (e.g., 'families')
   * @param keys The keys of the model to compare for changes (e.g., ['isActive', 'name'])
   */
  const getModifiedArrayItems = <T>(keys: Array<keyof T>, propertyName?: keyof T): T[] => {
    const modifiedItems: T[] = [];
    const originalArray = propertyName
      ? ((initialRef.current as any)[propertyName] as T[])
      : (initialRef.current as any as T[]);
    const currentArray = propertyName
      ? ((values as any)[propertyName] as T[])
      : (values as any as T[]);

    if (!Array.isArray(originalArray) || !Array.isArray(currentArray)) {
      return modifiedItems;
    }

    currentArray.forEach((currentItem, index) => {
      const originalItem = originalArray[index];
      let isModified = false;
      keys.forEach((key) => {
        if (!deepEqual(currentItem[key], originalItem ? originalItem[key] : undefined)) {
          isModified = true;
        }
      });

      if (isModified) {
        modifiedItems.push(currentItem);
      }
    });
    return modifiedItems;
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
    getModifiedArrayItems,
  };
};
