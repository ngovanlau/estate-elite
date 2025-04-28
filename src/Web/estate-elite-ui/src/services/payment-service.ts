import { environment } from '@/lib/environment';
import BaseService from './base-service';
import { CaptureOrderRequest, RentPropertyRequest } from '@/types/request/payment-request';
import { ApiResponse } from '@/types/response/base-response';
import { CaptureOrderResponse, RentPropertyResponse } from '@/types/response/payment-response';

class PaymentService extends BaseService {
  public constructor() {
    super(environment.paymentServiceApi + '/api/');
  }

  public rentProperty = (
    request: RentPropertyRequest
  ): Promise<ApiResponse<RentPropertyResponse>> => {
    return this.instance.post('transaction/rent-property', request);
  };

  public captureOrder = (
    request: CaptureOrderRequest
  ): Promise<ApiResponse<CaptureOrderResponse>> => {
    return this.instance.post('transaction/capture-order', request);
  };
}

const paymentService = new PaymentService();
export default paymentService;
