import { clsx, type ClassValue } from 'clsx';

import { twMerge } from 'tailwind-merge';
import { LISTING_TYPE } from './enum';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const formatCurrency = (value: number): string => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
    maximumFractionDigits: 0,
  }).format(value);
};

export function formatPrice(price: number, type: LISTING_TYPE) {
  if (type === LISTING_TYPE.SALE) {
    if (price >= 1000000000) {
      return `${(price / 1000000000).toFixed(1)} tỷ`;
    } else {
      return `${(price / 1000000).toFixed(0)} triệu`;
    }
  } else {
    return `${(price / 1000000).toFixed(1)} triệu/tháng`;
  }
}

// export const formatPrice = (price: number): string => {
//   return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
// };
