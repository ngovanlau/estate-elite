import { ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME, USER_KEY } from '@/lib/constant';
import { removeCookie, setCookie } from '@/lib/cookies';
import { CurrentUser } from '@/types/response/identity-response';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

export interface AuthState {
  currentUser: CurrentUser | null;
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

const initialState: AuthState = {
  currentUser: null,
  accessToken: null,
  refreshToken: null,
  isAuthenticated: false,
  loading: false,
  error: null,
};

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    // Action bắt đầu đăng nhập
    loginStart: (state) => {
      state.loading = true;
      state.error = null;
    },

    // Action đăng nhập thành công
    loginSuccess: (
      state,
      action: PayloadAction<{
        currentUser: CurrentUser;
        accessToken: string;
        refreshToken: string;
      }>
    ) => {
      state.loading = false;
      state.isAuthenticated = true;
      state.currentUser = action.payload.currentUser;
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.error = null;
    },

    // Action đăng nhập thất bại
    loginFailure: (state, action: PayloadAction<string>) => {
      state.loading = false;
      state.isAuthenticated = false;
      state.currentUser = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.error = action.payload;
    },

    // Action đăng xuất
    logout: (state) => {
      state.currentUser = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.isAuthenticated = false;
      state.error = null;

      removeCookie(ACCESS_TOKEN_NAME);
      removeCookie(REFRESH_TOKEN_NAME);
      removeCookie(USER_KEY);
    },

    // Action cập nhật thông tin người dùng
    updateUser: (state, action: PayloadAction<CurrentUser>) => {
      state.currentUser = action.payload;

      setCookie(USER_KEY, JSON.stringify(state.currentUser));
    },
  },
});

// Export các actions
export const { loginStart, loginSuccess, loginFailure, logout, updateUser } = authSlice.actions;

// Export reducer
export default authSlice.reducer;

// Selectors
export const selectAuth = (state: { auth: AuthState }) => state.auth;
export const selectUser = (state: { auth: AuthState }) => state.auth.currentUser;
export const selectIsAuthenticated = (state: { auth: AuthState }) => state.auth.isAuthenticated;
