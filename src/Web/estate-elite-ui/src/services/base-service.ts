import { ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME } from '@/lib/constant';
import { getCookie, removeCookie, setCookie } from '@/lib/cookies';
import { environment } from '@/lib/environment';
import { ApiResponse } from '@/types/response/base-response';
import { TokenResponseData } from '@/types/response/identity-response';
import axios, { AxiosError, AxiosInstance, AxiosResponse, InternalAxiosRequestConfig } from 'axios';

const ROUTES = {
  LOGIN: '/login',
  REFRESH_TOKEN: '/authentication/refresh-token',
};

interface AxiosRequestConfigWithRetry extends InternalAxiosRequestConfig {
  _retry?: boolean;
}

// Create a queue for requests that fail due to token expiration
interface QueueItem {
  resolve: (value: unknown) => void;
  reject: (reason?: unknown) => void;
  config: AxiosRequestConfigWithRetry;
}

export default class BaseService {
  protected instance: AxiosInstance;
  protected baseURL: string;
  private TIME_OUT = Number(environment.apiTimeout);
  private isRefreshing = false;
  private failedQueue: QueueItem[] = [];

  public constructor(baseURL: string) {
    if (!baseURL) throw new Error('Base URL can not empty');
    this.baseURL = baseURL;

    this.instance = axios.create({ baseURL, timeout: this.TIME_OUT });
    this.initializeRequestInterceptor();
    this.initializeResponseInterceptor();
  }

  private processQueue = (error: AxiosError | null, token: string | null = null) => {
    this.failedQueue.forEach(({ resolve, reject, config }) => {
      if (error) {
        reject(error);
        return;
      }

      // Update the token in the request headers
      if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      resolve(this.instance(config));
    });

    // Clear the queue
    this.failedQueue = [];
  };

  private initializeRequestInterceptor = async () => {
    this.instance.interceptors.request.use(this.handleRequest, this.handleRequestError);
  };

  private handleRequest = async (config: InternalAxiosRequestConfig) => {
    if (typeof window !== 'undefined') {
      const accessToken = getCookie(ACCESS_TOKEN_NAME);
      if (accessToken && config.headers) {
        config.headers.Authorization = `Bearer ${accessToken}`;
      }
    }

    return config;
  };

  private handleRequestError = (error: AxiosError) => {
    return Promise.reject(error);
  };

  private initializeResponseInterceptor = async () => {
    this.instance.interceptors.response.use(this.handleResponse, this.handleResponseError);
  };

  private handleResponse = async (response?: AxiosResponse) => {
    return response?.data;
  };

  private handleResponseError = async (error: AxiosError) => {
    const errorMessage = error.message || 'Unknown error';

    if (!error.response) {
      if (error.code === 'ECONNABORTED') {
        console.error('Request timeout:', errorMessage);
      } else if (axios.isCancel(error)) {
        console.error('Request cancelled:', errorMessage);
      } else {
        console.error('Network error:', errorMessage);
      }
      return Promise.reject(error);
    }

    const originalConfig = error.config as AxiosRequestConfigWithRetry;
    const status = error.response.status;

    // Skip refresh token process for refresh token endpoint to avoid infinite loop
    if (originalConfig.url?.includes('/authentication/refresh-token')) {
      this.removeAuthentication();
      return Promise.reject(error);
    }

    // Handle 401 Unauthorized - Token expired
    if (status === 401 && !originalConfig._retry) {
      if (this.isRefreshing) {
        // If refreshing is in progress, queue the request
        return new Promise((resolve, reject) => {
          this.failedQueue.push({ resolve, reject, config: originalConfig });
        });
      }

      originalConfig._retry = true;
      this.isRefreshing = true;

      try {
        const newAccessToken = await this.refreshAuthToken();

        // Update Authorization header
        this.instance.defaults.headers.common['Authorization'] = `Bearer ${newAccessToken}`;
        originalConfig.headers.Authorization = `Bearer ${newAccessToken}`;

        // Process queued requests
        this.processQueue(null, newAccessToken);
        this.isRefreshing = false;

        // Retry the original request with new token
        return this.instance(originalConfig);
      } catch (refreshError) {
        // If refresh token is invalid, logout the user
        this.processQueue(refreshError as AxiosError);
        this.isRefreshing = false;
        this.removeAuthentication();
        return Promise.reject(refreshError);
      }
    }

    switch (status) {
      case 403:
        console.error('Forbidden access:', error.message);
        break;
      case 404:
        console.error('Resource not found:', error.message);
        break;
      case 500:
        console.error('Server error:', error.message);
        break;
      default:
        console.error(`HTTP error ${status}:`, error.message);
    }

    return Promise.reject(error);
  };

  private async refreshAuthToken() {
    try {
      const accessToken = getCookie(ACCESS_TOKEN_NAME);
      const refreshToken = getCookie(REFRESH_TOKEN_NAME);

      if (!refreshToken) {
        throw new Error('No refresh token available');
      }

      const response = await this.instance.post<ApiResponse<TokenResponseData>>(
        ROUTES.REFRESH_TOKEN,
        { accessToken, refreshToken }
      );

      const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data.data;

      setCookie(ACCESS_TOKEN_NAME, newAccessToken);
      setCookie(REFRESH_TOKEN_NAME, newRefreshToken);

      return newAccessToken;
    } catch (error) {
      console.error('Failed to refresh token:', error);
      throw error;
    }
  }

  private removeAuthentication = () => {
    removeCookie(ACCESS_TOKEN_NAME);
    removeCookie(REFRESH_TOKEN_NAME);
    this.failedQueue = [];

    if (typeof window !== 'undefined') {
      window.location.href = ROUTES.LOGIN;
    }
    return;
  };
}
