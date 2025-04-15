import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import { ACCESS_TOKEN_NAME, getCookie } from "./cookies";

const createAxiosInstance = (baseURL: string): AxiosInstance => {
    const config: AxiosRequestConfig = {
        baseURL,
        headers: {
            "Content-Type": 'application/json'
        },
        timeout: 30000,
    }

    const instance = axios.create(config);

    // Interceptors
    instance.interceptors.request.use(
        (config) => {
            if (typeof window !== 'undefined') {
                const token = getCookie(ACCESS_TOKEN_NAME);
                if (token && config.headers) {
                    config.headers.Authorization = `Bearer ${token}`;
                }
            }

            return config;
        },
        (error) => Promise.reject(error)
    )

    instance.interceptors.response.use(
        (response) => response,
        (error) => {
            // Xử lý lỗi (ví dụ: refresh token, redirect đến trang login)
            if (error.response?.status === 401) {
            // Handle unauthorized
            }
            return Promise.reject(error);
        }
    );

    return instance;
}

// Create instance for services
export const identityService = createAxiosInstance(
  process.env.NEXT_PUBLIC_IDENTITY_SERVICE_URL || "http://localhost:5001" // Default fallback
);
