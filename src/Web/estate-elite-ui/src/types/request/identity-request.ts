export interface LoginRequest {
  username?: string;
  email?: string;
  password: string;
}

export interface RefreshTokenRequest {
  accessToken: string;
  refreshToken: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  fullname: string;
  password: string;
  confirmationPassword: string;
}

export interface ConfirmRequest {
  userId: string;
  code: string;
}
