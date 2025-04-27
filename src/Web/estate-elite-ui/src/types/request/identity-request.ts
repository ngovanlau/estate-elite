import { USER_ROLE } from '@/lib/enum';

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
  role: USER_ROLE;
  password: string;
  confirmationPassword: string;
}

export interface ConfirmRequest {
  userId: string;
  code: string;
}

export interface UpdateUserRequest {
  fullName?: string;
  email?: string;
  phone?: string;
  address?: string;
}

export interface UpdateSellerProfileRequest {
  companyName: string;
  licenseNumber?: string;
  taxIdentificationNumber: string;
  professionalLicense?: string;
  biography?: string;
  establishedYear: number;
  acceptsPaypal: boolean;
  paypalEmail?: string;
  paypalMerchantId?: string;
}
