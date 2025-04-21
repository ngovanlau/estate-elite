'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { Loader2 } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import toast from 'react-hot-toast';
import { z } from 'zod';
import { passwordFormSchema } from './_validation';
import { PasswordField } from '@/components/form-fields/password-field';

export function PasswordTab() {
  const [isLoading, setIsLoading] = useState(false);

  const passwordForm = useForm<z.infer<typeof passwordFormSchema>>({
    resolver: zodResolver(passwordFormSchema),
    defaultValues: {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    },
  });

  function onPasswordSubmit(values: z.infer<typeof passwordFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast.success('Mật khẩu của bạn đã được thay đổi thành công.');
      passwordForm.reset();
    }, 1000);
  }

  function handleResetPassword() {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      setIsLoading(false);
      toast.success('Email với hướng dẫn đặt lại mật khẩu đã được gửi đến email của bạn.');
    }, 1000);
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Đổi mật khẩu</CardTitle>
        <CardDescription>Thay đổi mật khẩu đăng nhập của bạn.</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...passwordForm}>
          <form
            onSubmit={passwordForm.handleSubmit(onPasswordSubmit)}
            className="space-y-6"
          >
            <PasswordField
              control={passwordForm.control}
              name="currentPassword"
              label="Mật khẩu hiện tại"
              required
            />
            <PasswordField
              control={passwordForm.control}
              name="newPassword"
              label="Mật khẩu mới"
              required
            />
            <PasswordField
              control={passwordForm.control}
              name="confirmPassword"
              label="Xác nhận mật khẩu mới"
              required
            />
            <div className="flex flex-col items-start justify-between gap-4 sm:flex-row sm:items-center">
              <Button
                type="submit"
                disabled={isLoading}
              >
                {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                Cập nhật mật khẩu
              </Button>
              <Button
                variant="ghost"
                type="button"
                onClick={handleResetPassword}
                disabled={isLoading}
              >
                Quên mật khẩu?
              </Button>
            </div>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
