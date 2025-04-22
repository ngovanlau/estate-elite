import { getCookie, removeCookie } from '@/lib/cookies';
import { setCookie } from '@/lib/cookies';
import { CurrentUser } from '@/types/response/identity-response';
import { Middleware } from '@reduxjs/toolkit';
import { loginSuccess, logout } from '../slices/auth-slice';
import { ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME, USER_KEY } from '@/lib/constant';

// Triển khai middleware với kiểu được định nghĩa từ trước
export const authMiddleware: Middleware = () => (next) => (action) => {
  // Xử lý trước khi dispatch action
  const result = next(action);

  // Xử lý sau khi dispatch action
  if (loginSuccess.match(action)) {
    // Lưu thông tin khi đăng nhập thành công
    setCookie(ACCESS_TOKEN_NAME, action.payload.accessToken);
    setCookie(REFRESH_TOKEN_NAME, action.payload.refreshToken);
    setCookie(USER_KEY, JSON.stringify(action.payload.currentUser));
  } else if (logout.match(action)) {
    // Xóa thông tin khi đăng xuất
    removeCookie(ACCESS_TOKEN_NAME);
    removeCookie(REFRESH_TOKEN_NAME);
    removeCookie(USER_KEY);
  }

  return result;
};

// Hàm khôi phục trạng thái auth từ Cookie khi làm mới trang
export const rehydrateAuthState = () => {
  try {
    const accessToken = getCookie(ACCESS_TOKEN_NAME);
    const refreshToken = getCookie(REFRESH_TOKEN_NAME);

    const userString = getCookie(USER_KEY);
    const currentUser: CurrentUser = userString ? JSON.parse(userString) : undefined;

    if (accessToken && refreshToken && currentUser) {
      return {
        auth: {
          currentUser,
          accessToken,
          refreshToken,
          isAuthenticated: true,
          loading: false,
          error: null,
        },
      };
    }
  } catch (e) {
    console.error('Error rehydrating auth state:', e);
  }

  return undefined;
};
