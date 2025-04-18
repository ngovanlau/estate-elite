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
import { LoginFormValues } from '@/lib/type';
import { UsernameOrEmailField } from '@/components/form-fields/username-or-email-field';
import { loginFormSchema } from './validation';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import { useState } from 'react';

type LoginFormProps = {
  onSubmit: (values: LoginFormValues) => void;
};

export const LoginForm = ({ onSubmit }: LoginFormProps) => {
  const [rememberMe, setRememberMe] = useState<boolean>(false);
  const form = useForm<z.infer<typeof loginFormSchema>>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      usernameOrEmail: '',
      password: '',
    },
  });

  return (
    <Card className="w-full border-0 shadow-md">
      <CardHeader>
        <CardTitle className="text-center text-2xl font-semibold">Đăng nhập</CardTitle>
        <CardDescription className="text-center">
          Đăng nhập vào tài khoản của bạn để quản lý bất động sản
        </CardDescription>
      </CardHeader>

      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)}>
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
    </Card>
  );
};
