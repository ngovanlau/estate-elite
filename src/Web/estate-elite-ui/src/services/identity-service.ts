import { LoginRequest } from '@/types/request/identity-request';
import { ApiResponse } from '@/types/response/base-response';
import { TokenResponseData } from '@/types/response/identity-response';
import BaseService from './base-service';
import { environment } from '@/lib/environment';

class IdentityService extends BaseService {
  public constructor() {
    super(environment.identityServiceApi + '/api/');
  }

  public login = (request: LoginRequest): Promise<ApiResponse<TokenResponseData>> => {
    return this.instance.post('authentication/login', request);
  };
}

export default new IdentityService();
