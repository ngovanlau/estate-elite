import { LISTING_TYPE, RENT_PERIOD } from '@/lib/enum';

export interface CreatePropertyRequest {
  title: string;
  description: string;
  listingType: LISTING_TYPE;
  rentPeriod?: RENT_PERIOD;
  area: number;
  landArea: number;
  buildDate: string;
  price: number;
  propertyTypeId: string;
  address: {
    country: string;
    province: string;
    district: string;
    ward: string;
    details: string;
  };
  rooms?: {
    id: string;
    quantity: number;
  }[];
  utilityIds?: string[];
  images: File[];
}
