import { ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME } from '@/lib/constant';
import { getCookie, removeCookie, setCookie } from '@/lib/cookies';
import { environment } from '@/lib/environment';
import { ApiResponse } from '@/types/response/base-response';
import { Token } from '@/types/response/identity-response';
import axios, {
  AxiosError,
  AxiosInstance,
  AxiosResponse,
  InternalAxiosRequestConfig,
  CancelTokenSource,
} from 'axios';

const ROUTES = {
  LOGIN: '/login',
  REFRESH_TOKEN: '/authentication/refresh-token',
};

interface AxiosRequestConfigWithRetry extends InternalAxiosRequestConfig {
  _retry?: boolean;
  metadata?: {
    startTime: number;
  };
}

// Create a queue for requests that fail due to token expiration
interface QueueItem {
  resolve: (value: unknown) => void;
  reject: (reason?: unknown) => void;
  config: AxiosRequestConfigWithRetry;
}

enum ErrorCode {
  UNAUTHORIZED = 'UNAUTHORIZED',
  FORBIDDEN = 'FORBIDDEN',
  NOT_FOUND = 'NOT_FOUND',
  SERVER_ERROR = 'SERVER_ERROR',
  NETWORK_ERROR = 'NETWORK_ERROR',
  TIMEOUT = 'TIMEOUT',
}

// Custom error type with code
interface ServiceError {
  code: ErrorCode;
  message: string;
  data?: unknown;
}

interface RequestParams {
  [key: string]: string | number | boolean | null | undefined;
}

export default class BaseService {
  protected instance: AxiosInstance;
  protected baseURL: string;
  private TIME_OUT = Number(environment.apiTimeout);
  private static isRefreshing = false;
  private static failedQueue: QueueItem[] = [];

  public constructor(baseURL: string) {
    if (!baseURL) throw new Error('Base URL cannot be empty');
    this.baseURL = baseURL;

    this.instance = axios.create({ baseURL, timeout: this.TIME_OUT });
    this.initializeRequestInterceptor();
    this.initializeResponseInterceptor();
  }

  /**
   * Set the refreshing state
   */
  private static setRefreshingState(state: boolean): void {
    BaseService.isRefreshing = state;
  }

  /**
   * Get the refreshing state
   */
  private static getRefreshingState(): boolean {
    return BaseService.isRefreshing;
  }

  /**
   * Instance method to set refreshing state
   */
  private setRefreshingState(state: boolean): void {
    BaseService.setRefreshingState(state);
  }

  /**
   * Instance method to get refreshing state
   */
  private getRefreshingState(): boolean {
    return BaseService.getRefreshingState();
  }

  /**
   * Process the queue of failed requests
   */
  private processQueue = (error: AxiosError | null, token: string | null = null): void => {
    BaseService.failedQueue.forEach(({ resolve, reject, config }) => {
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
    BaseService.failedQueue = [];
  };

  /**
   * Initialize request interceptor
   */
  private initializeRequestInterceptor = (): void => {
    this.instance.interceptors.request.use((config) => {
      // Add timestamp for performance tracking
      const configWithMetadata = config as AxiosRequestConfigWithRetry;
      configWithMetadata.metadata = { startTime: new Date().getTime() };
      return this.handleRequest(configWithMetadata);
    }, this.handleRequestError);
  };

  /**
   * Handle request configuration
   */
  private handleRequest = async (
    config: InternalAxiosRequestConfig
  ): Promise<InternalAxiosRequestConfig> => {
    if (typeof window !== 'undefined') {
      const accessToken = getCookie(ACCESS_TOKEN_NAME);
      if (accessToken && config.headers) {
        config.headers.Authorization = `Bearer ${accessToken}`;
      }
    }

    return config;
  };

  /**
   * Handle request error
   */
  private handleRequestError = (error: AxiosError): Promise<AxiosError> => {
    console.error('Request error:', error.message);
    return Promise.reject(error);
  };

  /**
   * Initialize response interceptor
   */
  private initializeResponseInterceptor = (): void => {
    this.instance.interceptors.response.use(this.handleResponse, this.handleResponseError);
  };

  /**
   * Handle successful response
   */
  private handleResponse = async (response: AxiosResponse): Promise<AxiosResponse> => {
    // Log performance metrics
    const config = response.config as AxiosRequestConfigWithRetry;
    if (config?.metadata?.startTime) {
      const duration = new Date().getTime() - config.metadata.startTime;
      console.debug(`Request to ${config.url} took ${duration}ms`);
    }

    return response.data;
  };

  /**
   * Get error code from HTTP status
   */
  private getErrorCodeFromStatus = (status: number | undefined): ErrorCode => {
    switch (status) {
      case 401:
        return ErrorCode.UNAUTHORIZED;
      case 403:
        return ErrorCode.FORBIDDEN;
      case 404:
        return ErrorCode.NOT_FOUND;
      case 500:
        return ErrorCode.SERVER_ERROR;
      default:
        return ErrorCode.NETWORK_ERROR;
    }
  };

  /**
   * Refresh authentication token
   */
  private async refreshAuthToken(): Promise<string> {
    try {
      const accessToken = getCookie(ACCESS_TOKEN_NAME);
      const refreshToken = getCookie(REFRESH_TOKEN_NAME);

      if (!refreshToken) {
        throw new Error('No refresh token available');
      }

      const response = await this.instance.post<unknown, ApiResponse<Token>>(ROUTES.REFRESH_TOKEN, {
        accessToken,
        refreshToken,
      });

      if (!response.succeeded) {
        throw new Error(response.message || 'Failed to refresh token');
      }

      const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data;

      setCookie(ACCESS_TOKEN_NAME, newAccessToken);
      setCookie(REFRESH_TOKEN_NAME, newRefreshToken);

      return newAccessToken;
    } catch (error) {
      console.error('Failed to refresh token:', error);
      throw error;
    }
  }

  /**
   * Retry request with exponential backoff
   */
  private async retryWithBackoff(
    config: AxiosRequestConfigWithRetry,
    retryCount = 0
  ): Promise<unknown> {
    const maxRetries = 3;
    const waitTime = Math.pow(2, retryCount) * 1000; // 1s, 2s, 4s, ...

    if (retryCount >= maxRetries) {
      return Promise.reject(new Error('Max retries reached'));
    }

    try {
      await new Promise<void>((resolve) => setTimeout(() => resolve(), waitTime));
      return this.instance(config);
    } catch {
      return this.retryWithBackoff(config, retryCount + 1);
    }
  }

  /**
   * Handle response error
   */
  private handleResponseError = async (error: AxiosError): Promise<unknown> => {
    const errorMessage = error.message || 'Unknown error';
    const status = error.response?.status;
    const errorCode = this.getErrorCodeFromStatus(status);
    const originalConfig = error.config as AxiosRequestConfigWithRetry;

    // Log detailed error information
    const requestInfo = {
      url: originalConfig?.url,
      method: originalConfig?.method,
      status: error.response?.status,
      errorCode,
      data: error.response?.data,
    };

    console.error('API Error:', errorMessage, requestInfo);

    if (!error.response) {
      if (error.code === 'ECONNABORTED') {
        console.error('Request timeout:', errorMessage);
        const serviceError: ServiceError = { code: ErrorCode.TIMEOUT, message: errorMessage };
        return Promise.reject(serviceError);
      } else if (axios.isCancel(error)) {
        console.error('Request cancelled:', errorMessage);
        return Promise.reject(error);
      } else {
        console.error('Network error:', errorMessage);
        const serviceError: ServiceError = { code: ErrorCode.NETWORK_ERROR, message: errorMessage };
        return Promise.reject(serviceError);
      }
    }

    // Skip refresh token process for refresh token endpoint to avoid infinite loop
    if (originalConfig.url?.includes(ROUTES.REFRESH_TOKEN)) {
      this.removeAuthentication();
      return Promise.reject(error);
    }

    // Handle 401 Unauthorized - Token expired
    if (status === 401 && !originalConfig._retry) {
      if (this.getRefreshingState()) {
        // If refreshing is in progress, queue the request
        return new Promise((resolve, reject) => {
          BaseService.failedQueue.push({ resolve, reject, config: originalConfig });
        });
      }

      originalConfig._retry = true;
      this.setRefreshingState(true);

      try {
        // Refresh the token
        const newAccessToken = await this.refreshAuthToken();

        // Update Authorization header
        if (this.instance.defaults.headers) {
          this.instance.defaults.headers.common['Authorization'] = `Bearer ${newAccessToken}`;
        }

        if (originalConfig.headers) {
          originalConfig.headers.Authorization = `Bearer ${newAccessToken}`;
        }

        // Process queued requests
        this.processQueue(null, newAccessToken);
        this.setRefreshingState(false);

        // Retry the original request with new token
        return this.instance(originalConfig);
      } catch (refreshError) {
        // If refresh token is invalid, logout the user
        this.processQueue(refreshError as AxiosError);
        this.setRefreshingState(false);
        this.removeAuthentication();
        return Promise.reject(refreshError);
      }
    }

    // Additional error handling based on status codes
    if (status === 429) {
      // Too Many Requests
      return this.retryWithBackoff(originalConfig);
    }

    return Promise.reject(error);
  };

  /**
   * Remove authentication and redirect to login
   */
  private removeAuthentication = (): void => {
    removeCookie(ACCESS_TOKEN_NAME);
    removeCookie(REFRESH_TOKEN_NAME);
    BaseService.failedQueue = [];

    if (typeof window !== 'undefined') {
      window.location.href = ROUTES.LOGIN;
    }
  };

  /**
   * Create cancel token for request cancellation
   */
  public createCancelToken(): CancelTokenSource {
    return axios.CancelToken.source();
  }

  /**
   * GET request
   */
  public async get<R>(url: string, params?: RequestParams): Promise<ApiResponse<R>> {
    return this.instance.get<R, ApiResponse<R>>(url, { params });
  }

  /**
   * POST request
   */
  public async post<R, D = Record<string, unknown>>(
    url: string,
    data?: D
  ): Promise<ApiResponse<R>> {
    return this.instance.post<R, ApiResponse<R>>(url, data);
  }

  /**
   * PUT request
   */
  public async put<R, D = Record<string, unknown>>(url: string, data?: D): Promise<ApiResponse<R>> {
    return this.instance.put<R, ApiResponse<R>>(url, data);
  }

  /**
   * DELETE request
   */
  public async delete<R>(url: string, params?: RequestParams): Promise<ApiResponse<R>> {
    return this.instance.delete<R, ApiResponse<R>>(url, { params });
  }
}
