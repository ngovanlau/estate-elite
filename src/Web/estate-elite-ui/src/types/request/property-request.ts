import { LISTING_TYPE, RENT_PERIOD } from '@/lib/enum';

export interface CreatePropertyRequest {
  title: string;
  description: string;
  listingType: LISTING_TYPE;
  rentPeriod: RENT_PERIOD;
  area: number;
  landArea: number;
  buildDate: string;
  price: number;
  propertyId: string;
  address: {
    country: string;
    province: string;
    district: string;
    ward: string;
    details: string;
  };
  rooms: {
    id: string;
    quantity: string;
  }[];
  utilityIds: string[];
  images: File[];
}

// interface Address {
//   country: string;
//   province: string;
//   district: string;
//   ward: string;
//   details: string;
// }

// interface Room {
//   id: string;
//   quantity: string;
// }
