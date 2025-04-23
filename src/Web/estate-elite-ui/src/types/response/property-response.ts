import { CURRENCY_UNIT, LISTING_TYPE, PROPERTY_STATUS, RENT_PERIOD } from '@/lib/enum';

export interface PropertyType {
  id: string;
  name: string;
}

export interface Utility {
  id: string;
  name: string;
}

export interface Room {
  id: string;
  name: string;
}

export interface Property {
  id: string;
  title: string;
  address: string;
  listingType: LISTING_TYPE;
  price: number;
  rentPeriod?: RENT_PERIOD;
  area: number;
  type: string;
  currencyUnit: CURRENCY_UNIT;
  imageUrl: string;
}

export interface OwnerProperty extends Omit<Property, 'ImageUrl'> {
  status: PROPERTY_STATUS;
}
