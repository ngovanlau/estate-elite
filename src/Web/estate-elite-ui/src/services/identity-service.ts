import { ConfirmRequest, LoginRequest, RegisterRequest } from '@/types/request/identity-request';
import { ApiResponse } from '@/types/response/base-response';
import { CurrentUserData, TokenData } from '@/types/response/identity-response';
import BaseService from './base-service';
import { environment } from '@/lib/environment';

class IdentityService extends BaseService {
  public constructor() {
    super(environment.identityServiceApi + '/api/');
  }

  public register = (request: RegisterRequest): Promise<ApiResponse<string>> => {
    return this.instance.post('authentication/register', request);
  };

  public confirm = (request: ConfirmRequest): Promise<ApiResponse<boolean | number>> => {
    return this.instance.post('authentication/confirm', request);
  };

  public resendCode = (userId: string): Promise<ApiResponse<string>> => {
    return this.instance.post('authentication/resend-code', { userId });
  };

  public login = (request: LoginRequest): Promise<ApiResponse<TokenData>> => {
    return this.instance.post('authentication/login', request);
  };

  public uploadAvatar = (image: FormData): Promise<ApiResponse<string>> => {
    return this.instance.patchForm('user/upload-avatar', image);
  };

  public uploadBackground = (image: FormData): Promise<ApiResponse<string>> => {
    return this.instance.patchForm('user/upload-background', image);
  };

  public getCurrentUser = (): Promise<ApiResponse<CurrentUserData>> => {
    return this.instance.get('user/current-user');
  };
}

const identityService = new IdentityService();
export default identityService;
