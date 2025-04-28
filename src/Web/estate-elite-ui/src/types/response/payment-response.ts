import { CURRENCY_UNIT, PAYMENT_METHOD, TRANSACTION_STATUS } from '@/lib/enum';

export interface RentPropertyResponse {
  orderId: string;
  transactionId: string;
  links: string[];
}

export interface CaptureOrderResponse {
  id: string;
  amount: number;
  currencyUnit: CURRENCY_UNIT;
  status: TRANSACTION_STATUS;
  paymentMethod: PAYMENT_METHOD;
  createdOn: string;
}
