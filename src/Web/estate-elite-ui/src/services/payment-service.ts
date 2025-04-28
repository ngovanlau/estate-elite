import { environment } from '@/lib/environment';
import BaseService from './base-service';
import { RentPropertyRequest } from '@/types/request/payment-request';
import { ApiResponse } from '@/types/response/base-response';
import { RentPropertyResponse } from '@/types/response/payment-response';

class PaymentService extends BaseService {
  public constructor() {
    super(environment.paymentServiceApi + '/api/');
  }

  public rentProperty = (
    request: RentPropertyRequest
  ): Promise<ApiResponse<RentPropertyResponse>> => {
    return this.instance.post('transaction/rent-property', request);
  };
}

const paymentService = new PaymentService();
export default paymentService;
