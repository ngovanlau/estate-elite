import { USER_ROLE } from '@/lib/enum';

export interface Token {
  accessToken: string;
  refreshToken: string;
}

export interface CurrentUser {
  id: string;
  username: string;
  email: string;
  fullName: string;
  role: USER_ROLE;
  phone?: string;
  address?: string;
  avatar?: string;
  background?: string;
  createdOn: Date;
  sellerProfile?: SellerProfile;
}

interface SellerProfile {
  companyName: string;
  licenseNumber?: string;
  taxIdentificationNumber: string;
  professionalLicense?: string;
  biography?: string;
  establishedYear: number;
  isVerified: boolean;
}
