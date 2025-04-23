import { LISTING_TYPE, PROPERTY_STATUS } from '@/lib/enum';

// types/property.ts
export interface Property {
  id: string;
  title: string;
  address: string;
  price: number;
  status: PROPERTY_STATUS;
  type: 'Căn hộ' | 'Nhà phố' | 'Biệt thự' | 'Đất nền';
  area: number;
  bedrooms: number;
  bathrooms: number;
  createdAt: string;
  image: string;
  listingType: LISTING_TYPE;
}

export type PropertyType = Property['type'];

// Format helpers
export const formatPrice = (price: number): string => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
};
