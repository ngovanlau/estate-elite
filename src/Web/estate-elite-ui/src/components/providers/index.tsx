'use client';

import { AppStore, makeStore } from '@/redux/store';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useRef } from 'react';
import { Provider } from 'react-redux';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

export default function Providers({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const storeRef = useRef<AppStore>(undefined);
  if (!storeRef.current) {
    storeRef.current = makeStore();
  }

  const queryClientRef = useRef<QueryClient>(undefined);
  if (!queryClientRef.current) {
    queryClientRef.current = new QueryClient({
      defaultOptions: {
        queries: {
          staleTime: 60 * 1000,
          retry: 1,
          refetchOnWindowFocus: false,
        },
      },
    });
  }

  return (
    <Provider store={storeRef.current}>
      <QueryClientProvider client={queryClientRef.current}>
        {children}
        {process.env.NODE_ENV !== 'production' && <ReactQueryDevtools />}
      </QueryClientProvider>
    </Provider>
  );
}
