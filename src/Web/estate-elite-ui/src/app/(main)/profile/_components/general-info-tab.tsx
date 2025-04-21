'use client';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { House, Loader2, Mail, Phone, User } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import toast from 'react-hot-toast';
import { z } from 'zod';
import { profileFormSchema } from './_validation';
import { useAppSelector } from '@/lib/hooks';
import { selectUser } from '@/redux/slices/auth-slice';
import { useState } from 'react';
import { InputField } from '@/components/form-fields/input-field';

export function GeneralInfoTab() {
  const [isLoading, setIsLoading] = useState(false);
  const currentUser = useAppSelector(selectUser);

  const profileForm = useForm<z.infer<typeof profileFormSchema>>({
    resolver: zodResolver(profileFormSchema),
    defaultValues: {
      fullName: currentUser?.fullName,
      email: currentUser?.email,
      phone: currentUser?.phone,
      address: currentUser?.address,
    },
  });

  function onProfileSubmit(values: z.infer<typeof profileFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast.success('Thông tin cá nhân của bạn đã được lưu thành công.');
    }, 1000);
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Thông tin chung</CardTitle>
        <CardDescription>Cập nhật thông tin cá nhân và loại tài khoản của bạn.</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...profileForm}>
          <form
            onSubmit={profileForm.handleSubmit(onProfileSubmit)}
            className="space-y-6"
          >
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              <InputField
                control={profileForm.control}
                name="fullName"
                required
                icon={User}
                label="Họ và tên"
                placeholder="Họ và tên"
              />
              <InputField
                control={profileForm.control}
                name="email"
                required
                icon={Mail}
                label="Email"
                placeholder="name@example.com"
              />
              <InputField
                control={profileForm.control}
                name="phone"
                icon={Phone}
                label="Số điện thoại"
                placeholder="Số điện thoại"
              />
              <InputField
                control={profileForm.control}
                name="address"
                icon={House}
                label="Địa chỉ"
                placeholder="Địa chỉ"
              />
            </div>
            <Button
              type="submit"
              disabled={isLoading}
            >
              {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              Lưu thay đổi
            </Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
