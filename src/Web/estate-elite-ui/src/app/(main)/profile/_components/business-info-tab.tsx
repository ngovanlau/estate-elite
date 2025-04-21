'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { Building2, Calendar, IdCard, Loader2 } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import toast from 'react-hot-toast';
import { z } from 'zod';
import { businessFormSchema } from './_validation';
import { useAppSelector } from '@/lib/hooks';
import { selectUser } from '@/redux/slices/auth-slice';
import { BiographyField } from '@/components/form-fields/biography-field';
import { InputField } from '@/components/form-fields/input-field';

export function BusinessInfoTab() {
  const [isLoading, setIsLoading] = useState(false);
  const sellerProfile = useAppSelector(selectUser)?.sellerProfile;

  const businessForm = useForm<z.infer<typeof businessFormSchema>>({
    resolver: zodResolver(businessFormSchema),
    defaultValues: {
      companyName: sellerProfile?.companyName,
      taxId: sellerProfile?.taxIdentificationNumber,
      licenseNumber: sellerProfile?.licenseNumber,
      professionalLicense: sellerProfile?.professionalLicense,
      establishedYear: sellerProfile?.establishedYear.toString(),
      biography: sellerProfile?.biography,
    },
  });

  function onBusinessSubmit(values: z.infer<typeof businessFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast.success('Thông tin doanh nghiệp của bạn đã được lưu thành công.');
    }, 1000);
  }

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
            onSubmit={businessForm.handleSubmit(onBusinessSubmit)}
            className="space-y-6"
          >
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              <InputField
                control={businessForm.control}
                name="companyName"
                icon={Building2}
                label="Tên công ty/doanh nghiệp"
                placeholder="Tên công ty/doanh nghiệp"
                required
              />
              <InputField
                control={businessForm.control}
                name="taxId"
                icon={IdCard}
                label="Mã số thuế"
                placeholder="Mã số thuế"
                required
              />
              <InputField
                control={businessForm.control}
                name="establishedYear"
                icon={Calendar}
                label="Năm thành lập"
                placeholder="Năm thành lập"
                required
              />
              <InputField
                control={businessForm.control}
                name="licenseNumber"
                icon={IdCard}
                label="Mã giấy phép kinh doanh"
                placeholder="Nhập mã giấy phép kinh doanh"
              />
              <InputField
                control={businessForm.control}
                name="professionalLicense"
                icon={IdCard}
                label="Mã giấy phép hành nghề"
                placeholder="Nhập mã giấy phép hành nghề"
              />
            </div>

            <BiographyField control={businessForm.control} />

            {/* TODO
            <div>
              <FormLabel>Giấy phép kinh doanh</FormLabel>
              <div className="mt-2 flex items-center">
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => licenseInputRef.current?.click()}
                >
                  Tải lên giấy phép
                </Button>
                <input
                  ref={licenseInputRef}
                  type="file"
                  accept="image/*,.pdf"
                  className="hidden"
                  onChange={handleLicenseUpload}
                />
                <p className="ml-4 text-sm text-gray-500">
                  Chấp nhận file PDF, JPG, PNG (tối đa 5MB)
                </p>
              </div>
            </div> */}

            <Button
              type="submit"
              disabled={isLoading}
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
