export interface LoginRequest {
  username?: string;
  email?: string;
  password: string;
}

export interface RefreshTokenRequest {
  accessToken: string;
  refreshToken: string;
}
