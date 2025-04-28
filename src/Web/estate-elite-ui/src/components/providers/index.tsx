'use client';

import { AppStore, makeStore } from '@/redux/store';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useRef } from 'react';
import { Provider } from 'react-redux';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { PayPalScriptProvider } from '@paypal/react-paypal-js';
import { environment } from '@/lib/environment';

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
        <PayPalScriptProvider
          options={{
            clientId: environment.paypalClientId,
          }}
        >
          {children}
          {process.env.NODE_ENV !== 'production' && <ReactQueryDevtools />}
        </PayPalScriptProvider>
      </QueryClientProvider>
    </Provider>
  );
}
