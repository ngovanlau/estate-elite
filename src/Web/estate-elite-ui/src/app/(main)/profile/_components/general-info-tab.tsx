'use client';

import { useEffect, useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { House, Loader2, Mail, Phone, User } from 'lucide-react';
import toast from 'react-hot-toast';
import { useAppSelector } from '@/lib/hooks';
import { selectUser } from '@/redux/slices/auth-slice';
import { InputField } from '@/components/form-fields/input-field';
import identityService from '@/services/identity-service';
import { useCurrentUser } from '@/lib/hooks/use-current-user';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { profileFormSchema } from './_validation';

export function GeneralInfoTab() {
  const [isLoading, setIsLoading] = useState(false);
  const currentUser = useAppSelector(selectUser);
  const { refresh } = useCurrentUser();

  const profileForm = useForm<z.infer<typeof profileFormSchema>>({
    resolver: zodResolver(profileFormSchema),
    defaultValues: {
      fullName: undefined,
      email: undefined,
      phone: undefined,
      address: undefined,
    },
  });

  const { control, handleSubmit, reset, formState } = profileForm;
  const { isDirty } = formState;

  // Reset form when currentUser changes
  useEffect(() => {
    if (currentUser) {
      reset({
        fullName: currentUser.fullName || undefined,
        email: currentUser.email || undefined,
        phone: currentUser.phone || undefined,
        address: currentUser.address || undefined,
      });
    } else {
      reset();
    }
  }, [currentUser, reset]);

  const onProfileSubmit = async (values: z.infer<typeof profileFormSchema>) => {
    setIsLoading(true);

    try {
      const response = await identityService.updateUser({
        fullName: values.fullName,
        email: values.email,
        phone: values.phone,
        address: values.address,
      });

      if (!response.succeeded || !response.data) {
        toast.error('Cập nhật thất bại, vui lòng thử lại');
        return;
      }

      toast.success('Cập nhật thông tin thành công');
      refresh();
    } catch (error) {
      toast.error('Đã xảy ra lỗi, vui lòng thử lại');
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Thông tin chung</CardTitle>
        <CardDescription>Cập nhật thông tin cá nhân và loại tài khoản của bạn.</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...profileForm}>
          <form
            onSubmit={handleSubmit(onProfileSubmit)}
            className="space-y-6"
          >
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              <InputField
                control={control}
                name="fullName"
                icon={User}
                label="Họ và tên"
                placeholder="Họ và tên"
              />
              <InputField
                control={control}
                name="email"
                icon={Mail}
                label="Email"
                placeholder="name@example.com"
              />
              <InputField
                control={control}
                name="phone"
                icon={Phone}
                label="Số điện thoại"
                placeholder="Số điện thoại"
              />
              <InputField
                control={control}
                name="address"
                icon={House}
                label="Địa chỉ"
                placeholder="Địa chỉ"
              />
            </div>
            <Button
              type="submit"
              disabled={isLoading || !isDirty}
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
