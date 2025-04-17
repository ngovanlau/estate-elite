import Cookies from 'js-cookie';

export const cookieOptions = {
  path: '/',
  secure: process.env.NODE_ENV === 'production',
  sameSize: 'strict' as const,
  // HttpOnly cannot be set in client-side JavaScript
  // It must be set by the server
  expires: 30, // Days
};

export const setCookie = (name: string, value: string, options = {}) => {
  Cookies.set(name, value, { ...cookieOptions, ...options });
};

export const getCookie = (name: string) => {
  return Cookies.get(name);
};

export const removeCookie = (name: string) => {
  Cookies.remove(name, { path: cookieOptions.path });
};
