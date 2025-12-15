import { useEffect, useRef, useState } from 'react';

export const useAsyncComponentInitialization = <TInitProps>(
  initializeAsync: () => Promise<TInitProps>
) => {
  const [isInitialized, setIsInitialized] = useState(false);
  const [initializationProps, setInitializationProps] = useState<TInitProps>();
  const isMounted = useRef(true);

  useEffect(() => {
    isMounted.current = true;
    (async () => {
      const result = await initializeAsync();
      if (isMounted.current) {
        setInitializationProps(result);
        setIsInitialized(true);
      }
    })();
    return () => {
      isMounted.current = false;
      // Add any cleanup logic here if needed
    };
  }, [initializeAsync]);

  return { isInitialized, initializationProps: initializationProps! };
};
