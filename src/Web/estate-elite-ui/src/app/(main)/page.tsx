'use client';

import { Button } from '@/components/ui/button';
import propertyService from '@/services/property-service';
import { Property, PropertyType } from '@/types/response/property-response';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import PropertyCard from '../_components/property-card';

export default function HomePage() {
  const [propertyTypes, setPropertyTypes] = useState<PropertyType[]>([]);
  const [mostViewProperties, setMostViewProperties] = useState<Property[]>([]);
  const [selectPropertyType, setSelectPropertyType] = useState<string>('');
  const [address, setAddress] = useState<string>('');
  const router = useRouter();

  const handleSearchClick = () => {
    router.push(`properties?address=${address}&property-type-id=${selectPropertyType}`);
  };

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

  const fetchMostViewProperties = async () => {
    try {
      const response = await propertyService.getMostViewProperties(3);
      if (response.succeeded) {
        setMostViewProperties(response.data);
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    fetchPropertyTypes();
    fetchMostViewProperties();
  }, []);

  useEffect(() => {
    setSelectPropertyType(propertyTypes[0]?.id);
  }, [propertyTypes]);

  return (
    <div className="space-y-16">
      {/* Hero Section */}
      <section className="pt-12 pb-24 text-center">
        <h1 className="mb-6 text-4xl font-bold md:text-6xl">Tìm Ngôi Nhà Mơ Ước Của Bạn</h1>
        <p className="mx-auto mb-10 max-w-3xl text-xl text-gray-600">
          Khám phá hàng ngàn bất động sản đang chờ đợi bạn trên nền tảng của chúng tôi
        </p>

        <div className="mx-auto w-full max-w-4xl rounded-lg bg-white p-4 shadow-lg">
          <div className="grid grid-cols-1 gap-4 md:grid-cols-4">
            <div className="col-span-2">
              <input
                type="text"
                placeholder="Nhập địa điểm, khu vực..."
                className="w-full rounded-md border p-3"
                onChange={(e) => setAddress(e.target.value)}
              />
            </div>
            <div>
              <select
                onChange={(e) => setSelectPropertyType(e.target.value)}
                className="w-full rounded-md border p-3"
              >
                {propertyTypes.map((type) => (
                  <option
                    key={type.id}
                    value={type.id}
                  >
                    {type.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <Button
                onClick={handleSearchClick}
                className="h-12 w-full p-3"
              >
                Tìm kiếm
              </Button>
            </div>
          </div>
        </div>
      </section>

      {/* Featured Properties */}
      <section>
        <div className="mb-8 flex items-center justify-between">
          <h2 className="text-3xl font-bold">Bất Động Sản Nổi Bật</h2>
          <Link href="/properties">
            <Button variant="outline">Xem tất cả</Button>
          </Link>
        </div>

        <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
          {mostViewProperties.map((property) => (
            <PropertyCard
              key={property.id}
              property={property}
            />
          ))}
        </div>
      </section>

      {/* Services */}
      <section className="-mx-4 bg-gray-50 px-4 py-12">
        <div className="container mx-auto">
          <h2 className="mb-12 text-center text-3xl font-bold">Dịch Vụ Của Chúng Tôi</h2>

          <div className="grid grid-cols-1 gap-8 md:grid-cols-3">
            <div className="rounded-lg bg-white p-6 text-center shadow-md">
              <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-blue-100">
                <span className="text-2xl text-blue-600">🏠</span>
              </div>
              <h3 className="mb-3 text-xl font-bold">Mua Bất Động Sản</h3>
              <p className="text-gray-600">
                Tìm và mua bất động sản phù hợp với nhu cầu và ngân sách của bạn
              </p>
            </div>

            <div className="rounded-lg bg-white p-6 text-center shadow-md">
              <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-green-100">
                <span className="text-2xl text-green-600">🔑</span>
              </div>
              <h3 className="mb-3 text-xl font-bold">Thuê Bất Động Sản</h3>
              <p className="text-gray-600">
                Khám phá các lựa chọn thuê nhà với giá cả hợp lý và vị trí thuận tiện
              </p>
            </div>

            <div className="rounded-lg bg-white p-6 text-center shadow-md">
              <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-purple-100">
                <span className="text-2xl text-purple-600">📊</span>
              </div>
              <h3 className="mb-3 text-xl font-bold">Đầu Tư Bất Động Sản</h3>
              <p className="text-gray-600">
                Tìm hiểu cơ hội đầu tư bất động sản sinh lời với sự tư vấn chuyên nghiệp
              </p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
