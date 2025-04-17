import { USER_ROLE } from '@/lib/enum';

export interface TokenData {
  accessToken: string;
  refreshToken: string;
}

export interface CurrentUserData {
  id: string;
  username: string;
  email: string;
  fullName: string;
  role: USER_ROLE;
  phone?: string;
  address?: string;
  avatar?: string;
  background?: string;
  sellerProfile?: SellerProfile;
}

interface SellerProfile {
  companyName: string;
  licenseNumber?: string;
  taxIdentificationNumber: string;
  professionalLicense: string;
  biography: string;
  isVerified: boolean;
}
