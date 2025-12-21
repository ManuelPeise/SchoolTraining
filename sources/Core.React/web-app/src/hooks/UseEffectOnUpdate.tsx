import { useEffect, useRef } from 'react';

export function useUpdateEffect(effect: () => void, deps: any[]) {
  const isInitialMount = useRef(true);

  useEffect(() => {
    if (isInitialMount.current) {
      isInitialMount.current = false;
    } else {
      effect();
    }
  }, deps);
}
