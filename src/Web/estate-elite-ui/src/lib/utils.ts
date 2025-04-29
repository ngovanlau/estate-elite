import { clsx, type ClassValue } from 'clsx';

import { twMerge } from 'tailwind-merge';
import { CURRENCY_UNIT, LISTING_TYPE, RENT_PERIOD } from './enum';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const formatCurrency = (
  value: number,
  currency: CURRENCY_UNIT,
  options?: {
    rentPeriod: RENT_PERIOD;
    locale?: string;
    maximumFractionDigits?: number;
    minimumFractionDigits?: number;
  }
): string => {
  const { locale = 'en-US', maximumFractionDigits = 2, minimumFractionDigits } = options || {};

  const formatOptions: Intl.NumberFormatOptions = {
    style: 'currency',
    currency,
    maximumFractionDigits,
  };

  if (minimumFractionDigits !== undefined) {
    formatOptions.minimumFractionDigits = minimumFractionDigits;
  }

  return new Intl.NumberFormat(locale, formatOptions).format(value);
};

export const rentPeriodMap = {
  [RENT_PERIOD.DAY]: 'Ngày',
  [RENT_PERIOD.MONTH]: 'Tháng',
  [RENT_PERIOD.YEAR]: 'Năm',
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
