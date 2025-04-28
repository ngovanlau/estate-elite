'use client';

import { useEffect, useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { Building2, Calendar, IdCard, Loader2, Mail } from 'lucide-react';
import toast from 'react-hot-toast';
import { useAppSelector } from '@/lib/hooks';
import { selectUser } from '@/redux/slices/auth-slice';
import { TextareaField } from '@/components/form-fields/textarea-field';
import { InputField } from '@/components/form-fields/input-field';
import { CheckboxField } from '@/components/form-fields/checkbox-field';
import { useCurrentUser } from '@/lib/hooks/use-current-user';
import identityService from '@/services/identity-service';
import { UpdateSellerProfileRequest } from '@/types/request/identity-request';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { businessFormSchema } from './_validation';

export function BusinessInfoTab() {
  const [isLoading, setIsLoading] = useState(false);
  const { sellerProfile } = useAppSelector(selectUser) || {};
  const { refresh } = useCurrentUser();

  const businessForm = useForm<z.infer<typeof businessFormSchema>>({
    resolver: zodResolver(businessFormSchema),
    defaultValues: {
      companyName: undefined,
      taxId: undefined,
      licenseNumber: undefined,
      professionalLicense: undefined,
      establishedYear: undefined,
      biography: undefined,
      acceptsPaypal: false,
      paypalEmail: undefined,
      paypalMerchantId: undefined,
    },
  });

  const { control, handleSubmit, watch, reset, resetField, clearErrors, formState } = businessForm;
  const { isDirty } = formState;
  const acceptsPaypal = watch('acceptsPaypal');
  const paypalEmail = watch('paypalEmail');
  const paypalMerchantId = watch('paypalMerchantId');

  // Load initial data from sellerProfile
  useEffect(() => {
    if (sellerProfile) {
      reset({
        companyName: sellerProfile.companyName || undefined,
        taxId: sellerProfile.taxIdentificationNumber || undefined,
        licenseNumber: sellerProfile.licenseNumber || undefined,
        professionalLicense: sellerProfile.professionalLicense || undefined,
        establishedYear: sellerProfile.establishedYear || undefined,
        biography: sellerProfile.biography || undefined,
        acceptsPaypal: sellerProfile.acceptsPaypal || false,
        paypalEmail: sellerProfile.paypalEmail || undefined,
        paypalMerchantId: sellerProfile.paypalMerchantId || undefined,
      });
    } else {
      reset();
    }
  }, [sellerProfile, reset]);

  // Reset Paypal fields when acceptsPaypal changes
  useEffect(() => {
    if (!acceptsPaypal) {
      resetField('paypalEmail');
      resetField('paypalMerchantId');
    }
  }, [acceptsPaypal, resetField]);

  // Clear acceptsPaypal errors when paypal fields are filled
  useEffect(() => {
    if (paypalEmail || paypalMerchantId) {
      clearErrors('acceptsPaypal');
    }
  }, [paypalEmail, paypalMerchantId, clearErrors]);

  const onBusinessSubmit = async (values: z.infer<typeof businessFormSchema>) => {
    setIsLoading(true);
    try {
      const request: UpdateSellerProfileRequest = {
        companyName: values.companyName,
        licenseNumber: values.licenseNumber,
        taxIdentificationNumber: values.taxId,
        professionalLicense: values.professionalLicense,
        biography: values.biography,
        establishedYear: values.establishedYear,
        acceptsPaypal: values.acceptsPaypal,
      };

      if (values.acceptsPaypal) {
        request.paypalEmail = values.paypalEmail;
        request.paypalMerchantId = values.paypalMerchantId;
      }

      const response = await identityService.updateSellerProfile(request);

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
        <CardTitle>Thông tin doanh nghiệp</CardTitle>
        <CardDescription>
          Thông tin này sẽ được sử dụng cho mục đích kinh doanh và xác minh.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...businessForm}>
          <form
            onSubmit={handleSubmit(onBusinessSubmit)}
            className="space-y-6"
          >
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              <InputField
                control={control}
                name="companyName"
                icon={Building2}
                label="Tên công ty/doanh nghiệp"
                placeholder="Tên công ty/doanh nghiệp"
                required
              />
              <InputField
                control={control}
                name="taxId"
                icon={IdCard}
                label="Mã số thuế"
                placeholder="Mã số thuế"
                required
              />
              <InputField
                control={control}
                name="establishedYear"
                type="number"
                icon={Calendar}
                label="Năm thành lập"
                placeholder="Năm thành lập"
                required
              />
              <InputField
                control={control}
                name="licenseNumber"
                icon={IdCard}
                label="Mã giấy phép kinh doanh"
                placeholder="Nhập mã giấy phép kinh doanh"
              />
              <InputField
                control={control}
                name="professionalLicense"
                icon={IdCard}
                label="Mã giấy phép hành nghề"
                placeholder="Nhập mã giấy phép hành nghề"
              />
            </div>

            <CheckboxField
              control={control}
              name="acceptsPaypal"
              label="Cho phép thanh toán bằng Paypal"
            />

            {acceptsPaypal && (
              <div className="grid grid-cols-1 gap-4">
                <InputField
                  control={control}
                  name="paypalEmail"
                  icon={Mail}
                  label="Email của tài khoản Paypal"
                  placeholder="Nhập email của tài khoản Paypal"
                  required
                />
                <InputField
                  control={control}
                  name="paypalMerchantId"
                  icon={IdCard}
                  label="Merchant ID của tài khoản Paypal"
                  placeholder="Nhập merchant ID của tài khoản Paypal"
                  required
                />
              </div>
            )}

            <TextareaField
              control={control}
              name="biography"
              label="Giới thiệu"
              placeholder="Viết một vài dòng về công ty"
              className="resize-none"
            />

            <Button
              type="submit"
              disabled={isLoading || !isDirty}
            >
              {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              Lưu thông tin doanh nghiệp
            </Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
