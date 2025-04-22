// types/property.ts
export interface Property {
  id: string;
  title: string;
  address: string;
  price: number;
  status: 'Đang bán' | 'Đang cho thuê' | 'Đã bán' | 'Đã cho thuê';
  type: 'Căn hộ' | 'Nhà phố' | 'Biệt thự' | 'Đất nền';
  area: number;
  bedrooms: number;
  bathrooms: number;
  createdAt: string;
}

export type PropertyStatus = Property['status'];
export type PropertyType = Property['type'];

// Format helpers
export const formatPrice = (price: number): string => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
};
