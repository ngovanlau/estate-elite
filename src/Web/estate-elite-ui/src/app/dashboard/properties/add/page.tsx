'use client';

import React, { useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { useRouter } from 'next/navigation';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { Form } from '@/components/ui/form';
import { propertySchema } from './_components/type';
import { submitPropertyForm } from './_components/uititls';
import { BasicInfoSection } from './_components/basic-info-section';
import { ImageUploadSection } from './_components/images-upload-section';
import { LocationSection } from './_components/location-section';
import { RoomsSection } from './_components/rooms-section';
import { UtilitiesSection } from './_components/utilities-section';
import { z } from 'zod';

// const DEFAULT_FORM_VALUES: z.infer<typeof propertySchema> = {
//   title: '',
//   description: '',
//   listingType: LISTING_TYPE.SALE,
//   price: '',
//   area: '',
//   landArea: '',
//   address: '',
//   bedrooms: '',
//   bathrooms: '',
//   features: [],
//   images: [],
// };

export default function AddPropertyPage() {
  const router = useRouter();
  const [previewImages, setPreviewImages] = useState<string[]>([]);

  const form = useForm<z.infer<typeof propertySchema>>({
    resolver: zodResolver(propertySchema),
    // defaultValues: DEFAULT_FORM_VALUES,
  });

  const onSubmit = (values: z.infer<typeof propertySchema>) => {
    submitPropertyForm(values, () => {
      // Redirect after successful submission
      setTimeout(() => {
        router.push('/dashboard/properties');
      }, 1500);
    });
  };

  return (
    <div className="container mx-auto py-6">
      <Card className="w-full">
        <CardHeader>
          <CardTitle>Thêm bất động sản mới</CardTitle>
          <CardDescription>
            Nhập thông tin chi tiết về bất động sản để đăng lên hệ thống
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(onSubmit)}
              className="space-y-8"
            >
              <div className="space-y-6">
                <BasicInfoSection form={form} />
                <Separator />

                <ImageUploadSection
                  form={form}
                  previewImages={previewImages}
                  setPreviewImages={setPreviewImages}
                />
                <Separator />

                <LocationSection form={form} />
                <Separator />

                <RoomsSection form={form} />

                <UtilitiesSection form={form} />
              </div>

              <div className="flex justify-end space-x-4">
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => {
                    form.reset();
                    router.back();
                  }}
                >
                  Hủy
                </Button>
                <Button type="submit">Thêm bất động sản</Button>
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
