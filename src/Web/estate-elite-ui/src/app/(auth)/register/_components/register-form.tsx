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
import { registerFormSchema } from './validation';
import { NameField } from '../../../../components/form-fields/name-field';
import { UsernameField } from '../../../../components/form-fields/username-field';
import { EmailField } from '../../../../components/form-fields/email-field';
import { PasswordField } from '../../../../components/form-fields/password-field';
import { z } from 'zod';
import { USER_ROLE } from '@/lib/enum';
import { RoleField } from '../../../../components/form-fields/role-field';
import { ConfirmPasswordField } from '../../../../components/form-fields/comfirmation-password-field';
import { RegisterFormValues } from '../../../../lib/types';

type RegisterFormProps = {
  onSubmit: (values: RegisterFormValues) => void;
};

export const RegisterForm = ({ onSubmit }: RegisterFormProps) => {
  const form = useForm<z.infer<typeof registerFormSchema>>({
    resolver: zodResolver(registerFormSchema),
    defaultValues: {
      fullName: '',
      username: '',
      email: '',
      role: USER_ROLE.BUYER,
      password: '',
      confirmationPassword: '',
    },
  });

  return (
    <Card className="w-full border-0 shadow-md">
      <CardHeader>
        <CardTitle className="text-center text-2xl font-semibold">Đăng ký</CardTitle>
        <CardDescription className="text-center">
          Tạo tài khoản mới để trải nghiệm dịch vụ bất động sản chuyên nghiệp
        </CardDescription>
      </CardHeader>

      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <NameField control={form.control} />
              <UsernameField control={form.control} />
            </div>

            <EmailField control={form.control} />
            <RoleField control={form.control} />
            <PasswordField control={form.control} />
            <ConfirmPasswordField control={form.control} />
          </CardContent>

          <CardFooter className="mt-4 flex flex-col gap-4">
            <Button
              type="submit"
              className="w-full bg-blue-600 hover:bg-blue-700"
            >
              Đăng ký
            </Button>

            <div className="text-center text-sm">
              Đã có tài khoản?{' '}
              <Link
                href="/login"
                className="font-medium text-blue-600 hover:text-blue-800"
              >
                Đăng nhập
              </Link>
            </div>
          </CardFooter>
        </form>
      </Form>
    </Card>
  );
};
