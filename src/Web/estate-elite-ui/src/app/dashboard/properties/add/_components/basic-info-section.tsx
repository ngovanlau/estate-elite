import React, { useEffect, useState } from 'react';
import { LISTING_TYPE } from '@/lib/enum';
import { InputField } from '@/components/form-fields/input-field';
import { propertySchema } from './type';
import { PropertyType } from '@/types/response/property-service';
import propertyService from '@/services/property-service';
import { SelectField } from '@/components/form-fields/select-field';
import { TextareaField } from '@/components/form-fields/textarea-field';
import { DatePickerField } from '@/components/form-fields/date-picker-field';
import { UseFormReturn } from 'react-hook-form';
import { z } from 'zod';

type BasicInfoSectionProps = {
  form: UseFormReturn<z.infer<typeof propertySchema>>;
};

export const BasicInfoSection = ({ form }: BasicInfoSectionProps) => {
  const [propertyTypes, setPropertyTypes] = useState<PropertyType[]>([]);

  const listingOptions = [
    {
      value: LISTING_TYPE.SALE,
      label: 'Bán',
    },
    {
      value: LISTING_TYPE.RENT,
      label: 'Cho thuê',
    },
  ];

  const propertyTypeOptions = propertyTypes.map((type) => ({
    value: type.id,
    label: type.name,
  }));

  const fetchPropertyTypes = async () => {
    try {
      const response = await propertyService.getPropertyType();
      if (response.succeeded) {
        setPropertyTypes(response.data);
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    fetchPropertyTypes();
  }, []);

  return (
    <div className="space-y-4">
      <div className="text-lg font-medium">Thông tin cơ bản</div>
      <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
        <InputField
          control={form.control}
          name="title"
          label="Tiêu đề"
          placeholder="Tiêu đề"
          required
        />

        <SelectField
          control={form.control}
          name="propertyType"
          label="Loại bất động sản"
          options={propertyTypeOptions}
          placeholder="Chọn loại bất động sản"
          required
        />

        <SelectField
          control={form.control}
          name="listingType"
          label="Loại giao dịch"
          options={listingOptions}
          placeholder="Chọn loại giao dịch"
          required
        />

        <InputField
          control={form.control}
          name="price"
          type="number"
          label="Giá"
          placeholder="Nhập giá"
          description="Giá tính bằng triệu VNĐ"
          required
        />

        <InputField
          control={form.control}
          name="area"
          type="number"
          label="Diện tích"
          placeholder="Nhập diện tích"
          description="Diện tích tính bằng m²"
          required
        />

        <InputField
          control={form.control}
          name="landArea"
          type="number"
          label="Diện tích đất"
          placeholder="Nhập diện tích đất"
          description="Diện tích tính bằng m²"
          required
        />

        <DatePickerField
          control={form.control}
          name="buildDate"
          label="Chọn ngày xây dựng"
          formatString="DD/MM/YYYY"
          required
        />
      </div>

      <TextareaField
        control={form.control}
        name="description"
        label="Mô tả"
        placeholder="Mô tả chi tiết về bất động sản"
        className="min-h-32 resize-none"
        required
      />
    </div>
  );
};
