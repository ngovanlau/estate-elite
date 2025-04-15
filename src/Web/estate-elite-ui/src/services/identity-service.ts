import { identityService } from "@/lib/axios";
import { LoginRequest } from "@/types/request/identity-request";
import { ApiResponse } from "@/types/response/base-reponse";
import { LoginResponseData } from "@/types/response/identity-reponse";

export const IdentityService = {
    login: async (request: LoginRequest): Promise<ApiResponse<LoginResponseData>> => {
        const response = await identityService.post<ApiResponse<LoginResponseData>>(
            "api/authentication/login", 
            request
        );
        return response.data;
    }
};