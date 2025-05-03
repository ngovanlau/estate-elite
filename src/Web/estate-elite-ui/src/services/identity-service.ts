import {
  ConfirmRequest,
  LoginRequest,
  RegisterRequest,
  UpdateSellerProfileRequest,
  UpdateUserRequest,
} from '@/types/request/identity-request';
import { ApiResponse } from '@/types/response/base-response';
import { CurrentUser, Token } from '@/types/response/identity-response';
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

  public login = (request: LoginRequest): Promise<ApiResponse<Token>> => {
    return this.instance.post('authentication/login', request);
  };

  public googleLogin = (idToken: string): Promise<ApiResponse<Token>> => {
    return this.instance.post('authentication/google-login', { idToken });
  };

  public uploadAvatar = (formData: FormData): Promise<ApiResponse<string>> => {
    return this.instance.patchForm('user/upload-avatar', formData);
  };

  public uploadBackground = (formData: FormData): Promise<ApiResponse<string>> => {
    return this.instance.patchForm('user/upload-background', formData);
  };

  public getCurrentUser = (): Promise<ApiResponse<CurrentUser>> => {
    return this.instance.get('user/current-user');
  };

  public updateUser = (request: UpdateUserRequest): Promise<ApiResponse<boolean>> => {
    return this.instance.patch('user/update-user', request);
  };

  public updateSellerProfile = (
    request: UpdateSellerProfileRequest
  ): Promise<ApiResponse<boolean>> => {
    return this.instance.patch('user/update-seller-profile', request);
  };
}

const identityService = new IdentityService();
export default identityService;
