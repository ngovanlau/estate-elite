'use client';

import Link from 'next/link';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { PasswordField } from '../../../../components/form-fields/password-field';
import { z } from 'zod';
import { UsernameOrEmailField } from '@/components/form-fields/username-or-email-field';
import { loginFormSchema } from './_validation';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import { useState } from 'react';
import identityService from '@/services/identity-service';
import { LoginRequest } from '@/types/request/identity-request';
import { setCookie } from '@/lib/cookies';
import { ACCESS_TOKEN_NAME, REFRESH_TOKEN_NAME } from '@/lib/constant';
import { useAppDispatch } from '@/lib/hooks';
import { loginFailure, loginStart, loginSuccess } from '@/redux/slices/auth-slice';
import { useRouter } from 'next/navigation';
import toast from 'react-hot-toast';
import { CredentialResponse, GoogleLogin } from '@react-oauth/google';
import { Token } from '@/types/response/identity-response';

export const LoginForm = () => {
  const dispatch = useAppDispatch();
  const router = useRouter();
  const [rememberMe, setRememberMe] = useState<boolean>(false);
  const form = useForm<z.infer<typeof loginFormSchema>>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      usernameOrEmail: '',
      password: '',
    },
  });

  const handleLogin = async (values: z.infer<typeof loginFormSchema>) => {
    const request: LoginRequest = {
      password: values.password,
    };

    if (values.usernameOrEmail.includes('@')) request.email = values.usernameOrEmail;
    else request.username = values.usernameOrEmail;

    try {
      dispatch(loginStart());

      const loginResponse = await identityService.login(request);
      if (loginResponse.succeeded && loginResponse.data) {
        handleAfterLogin(loginResponse.data);
      } else {
        if (loginResponse.code === 'E114') {
          form.setError('password', {
            message: 'Mật khẩu không chính xác',
          });
        } else {
          toast.error('Đăng nhập thất bại');
        }

        throw new Error('message' in loginResponse ? loginResponse.message : 'Đăng nhập thất bại');
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Đăng nhập thất bại';
      dispatch(loginFailure(errorMessage));
      throw error;
    }
  };

  const handleGoogleLogin = async (credentialResponse: CredentialResponse) => {
    try {
      const response = await identityService.googleLogin(credentialResponse.credential || '');
      if (response.succeeded && response.data) {
        handleAfterLogin(response.data);
      } else {
        toast.error('Đăng nhập thất bại');
        throw new Error('message' in response ? response.message : 'Đăng nhập thất bại');
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Đăng nhập thất bại';
      dispatch(loginFailure(errorMessage));
      throw error;
    }
  };

  const handleAfterLogin = async (response: Token) => {
    try {
      setCookie(ACCESS_TOKEN_NAME, response.accessToken);
      setCookie(REFRESH_TOKEN_NAME, response.refreshToken);

      const getCurrentUserResponse = await identityService.getCurrentUser();
      if (getCurrentUserResponse.succeeded && getCurrentUserResponse.data) {
        const currentUser = getCurrentUserResponse.data;

        dispatch(
          loginSuccess({
            currentUser,
            accessToken: response.accessToken,
            refreshToken: response.refreshToken,
          })
        );
      } else {
        form.setError('password', {
          type: 'onBlur',
          message: 'Mật khẩu không chính xác',
        });

        toast.error('Đăng nhập thất bại');

        throw new Error(
          'message' in getCurrentUserResponse
            ? getCurrentUserResponse.message
            : 'Đăng nhập thất bại'
        );
      }

      router.push('/');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Đăng nhập thất bại';
      dispatch(loginFailure(errorMessage));
      throw error;
    }
  };

  return (
    <Card className="w-full border-0 shadow-md">
      <CardHeader>
        <CardTitle className="text-center text-2xl font-semibold">Đăng nhập</CardTitle>
        <CardDescription className="text-center">
          Đăng nhập vào tài khoản của bạn để quản lý bất động sản
        </CardDescription>
      </CardHeader>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(handleLogin)}>
          <CardContent className="space-y-4">
            <UsernameOrEmailField control={form.control} />
            <PasswordField control={form.control} />

            <div className="flex items-center space-x-2">
              <Checkbox
                id="remember"
                checked={rememberMe}
                onCheckedChange={(checked) => setRememberMe(checked as boolean)}
              />
              <Label
                htmlFor="remember"
                className="text-sm"
              >
                Ghi nhớ đăng nhập
              </Label>
            </div>
          </CardContent>

          <CardFooter className="mt-4 flex flex-col gap-4">
            <Button
              type="submit"
              className="w-full bg-blue-600 hover:bg-blue-700"
            >
              Đăng nhập
            </Button>

            <div className="text-center text-sm">
              Chưa có tài khoản?{' '}
              <Link
                href="/register"
                className="font-medium text-blue-600 hover:text-blue-800"
              >
                Đăng ký ngay
              </Link>
            </div>
          </CardFooter>
        </form>
      </Form>
      <div className="mx-6">
        <GoogleLogin
          shape="square"
          logo_alignment="center"
          onSuccess={handleGoogleLogin}
          onError={() => {
            console.log('Login Failed');
          }}
        />
      </div>
    </Card>
  );
};
