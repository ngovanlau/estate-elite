import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/auth-slice';
import { authMiddleware, rehydrateAuthState } from './middlewares/auth-middleware';

export const makeStore = () => {
  // Khôi phục trạng thái từ cookie (nếu có)
  const preloadedState = typeof window !== 'undefined' ? rehydrateAuthState() : undefined;

  return configureStore({
    reducer: {
      auth: authReducer,
    },
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware({
        serializableCheck: false,
      }).concat(authMiddleware),
    preloadedState,
    devTools: process.env.NODE_ENV !== 'production',
  });
};

// Infer the type of makeStore
export type AppStore = ReturnType<typeof makeStore>;
// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<AppStore['getState']>;
export type AppDispatch = AppStore['dispatch'];
