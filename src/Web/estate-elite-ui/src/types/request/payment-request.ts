export interface RentPropertyRequest {
  propertyId: string;
  rentalPeriod: number;
  returnUrl: string;
  cancelUrl: string;
}

export interface CaptureOrderRequest {
  transactionId: string;
  orderId: string;
}
