import React, { useEffect } from 'react';
import { FormComponentProps } from './type';
import { SelectField } from '@/components/form-fields/select-field';
import { InputField } from '@/components/form-fields/input-field';
import { useLocationData } from '@/lib/hooks/use-location-data';

export const LocationSection = ({ form }: FormComponentProps) => {
  const { provinces, districts, wards, setDistricts, setWards } = useLocationData();

  const provinceOptions = provinces.map((province) => ({
    value: province.Name,
    label: province.FullName,
  }));

  const districtOptions = districts.map((district) => ({
    value: district.Name,
    label: district.FullName,
  }));

  const wardOptions = wards.map((ward) => ({
    value: ward.Name,
    label: ward.FullName,
  }));

  // Handle province change
  useEffect(() => {
    const currentProvince = form.watch('province');
    if (!currentProvince) return;

    const provinceData = provinces.find((p) => p.Name === currentProvince);
    if (!provinceData) return;

    console.log(provinceData.District);

    setDistricts(provinceData.District || []);

    // Reset dependent fields
    form.setValue('district', '');
    form.setValue('ward', '');
    setWards([]);
  }, [form.watch('province'), provinces, setDistricts, setWards]);

  // Handle district schange
  useEffect(() => {
    const currentDistrict = form.watch('district');
    if (!currentDistrict) return;

    const currentProvince = form.watch('province');
    const provinceData = provinces.find((p) => p.Name === currentProvince);
    const districtData = provinceData?.District?.find((d) => d.Name === currentDistrict);

    setWards(districtData?.Ward || []);

    // Reset ward
    form.setValue('ward', '');
  }, [form.watch('district'), provinces, setWards]);

  return (
    <div className="space-y-4">
      <div className="text-lg font-medium">Vị trí</div>
      <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
        <SelectField
          control={form.control}
          name="province"
          options={provinceOptions}
          label="Tỉnh/Thành phố"
          placeholder="Chọn tỉnh/thành phố"
          required
        />

        <SelectField
          control={form.control}
          name="district"
          options={districtOptions}
          label="Quận/Huyện"
          placeholder="Chọn quận/huyện"
          required
          disabled={!form.watch('province')}
        />

        <SelectField
          control={form.control}
          name="ward"
          options={wardOptions}
          label="Phường/Xã"
          placeholder="Chọn phường/xã"
          required
          disabled={!form.watch('district')}
        />
      </div>

      <div>
        <InputField
          control={form.control}
          name="address"
          label="Chi tiết địa chỉ"
          placeholder="Chi tiết địa chỉ"
          required
          disabled={!form.watch('ward')}
        />
      </div>
    </div>
  );
};
