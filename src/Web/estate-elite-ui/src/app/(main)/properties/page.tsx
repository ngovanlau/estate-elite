// src/app/properties/page.tsx
'use client';

import { useEffect, useState } from 'react';
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
  CardDescription,
} from '@/components/ui/card';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Slider } from '@/components/ui/slider';
import { Badge } from '@/components/ui/badge';
import { HeartIcon, MapPin, ArrowUpDown, Search, Bed, Bath, Filter, Ruler } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

// Định nghĩa kiểu dữ liệu cho bất động sản
interface Property {
  id: string;
  title: string;
  description: string;
  price: number;
  location: string;
  type: 'sale' | 'rent';
  category: 'apartment' | 'house' | 'villa' | 'land';
  bedrooms: number;
  bathrooms: number;
  area: number;
  image: string;
  featured: boolean;
}

// Dữ liệu mẫu
const sampleProperties: Property[] = [
  {
    id: '1',
    title: 'Căn hộ cao cấp Vinhomes Central Park',
    description: 'Căn hộ 3 phòng ngủ view sông và công viên tuyệt đẹp',
    price: 5500000000,
    location: 'Quận Bình Thạnh, TP.HCM',
    type: 'sale',
    category: 'apartment',
    bedrooms: 3,
    bathrooms: 2,
    area: 110,
    image: '/api/placeholder/800/500',
    featured: true,
  },
  {
    id: '2',
    title: 'Nhà phố liền kề The Sun Avenue',
    description: 'Nhà phố thiết kế hiện đại, nội thất cao cấp',
    price: 35000000,
    location: 'Quận 2, TP.HCM',
    type: 'rent',
    category: 'house',
    bedrooms: 4,
    bathrooms: 3,
    area: 180,
    image: '/api/placeholder/800/500',
    featured: false,
  },
  {
    id: '3',
    title: 'Biệt thự nghỉ dưỡng Phú Quốc',
    description: 'Biệt thự view biển với hồ bơi riêng',
    price: 12000000000,
    location: 'Phú Quốc, Kiên Giang',
    type: 'sale',
    category: 'villa',
    bedrooms: 5,
    bathrooms: 4,
    area: 350,
    image: '/api/placeholder/800/500',
    featured: true,
  },
  {
    id: '4',
    title: 'Đất nền Bảo Lộc',
    description: 'Đất nền view đồi, khí hậu mát mẻ quanh năm',
    price: 2300000000,
    location: 'Bảo Lộc, Lâm Đồng',
    type: 'sale',
    category: 'land',
    bedrooms: 0,
    bathrooms: 0,
    area: 500,
    image: '/api/placeholder/800/500',
    featured: false,
  },
  {
    id: '5',
    title: 'Căn hộ studio Masteri An Phú',
    description: 'Căn hộ studio hiện đại, tiện nghi',
    price: 8000000,
    location: 'Quận 2, TP.HCM',
    type: 'rent',
    category: 'apartment',
    bedrooms: 1,
    bathrooms: 1,
    area: 45,
    image: '/api/placeholder/800/500',
    featured: false,
  },
  {
    id: '6',
    title: 'Nhà phố Thảo Điền',
    description: 'Nhà phố khu compound an ninh 24/7',
    price: 45000000,
    location: 'Quận 2, TP.HCM',
    type: 'rent',
    category: 'house',
    bedrooms: 4,
    bathrooms: 4,
    area: 200,
    image: '/api/placeholder/800/500',
    featured: true,
  },
];

export default function PropertyListingPage() {
  const [properties, setProperties] = useState<Property[]>(sampleProperties);
  const [filterType, setFilterType] = useState<string>('');
  const [filterCategory, setFilterCategory] = useState<string>('');
  const [filterLocation, setFilterLocation] = useState<string>('');
  const [priceRange, setPriceRange] = useState<number[]>([0, 15000000000]);
  const [sortOption, setSortOption] = useState<string>('');
  const [searchTerm, setSearchTerm] = useState<string>('');

  // Xử lý lọc và sắp xếp dữ liệu
  const filteredProperties = properties
    .filter((property) => {
      return (
        (filterType === '' || property.type === filterType) &&
        (filterCategory === '' || property.category === filterCategory) &&
        (filterLocation === '' || property.location.includes(filterLocation)) &&
        property.price >= priceRange[0] &&
        property.price <= priceRange[1] &&
        (searchTerm === '' ||
          property.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
          property.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
          property.location.toLowerCase().includes(searchTerm.toLowerCase()))
      );
    })
    .sort((a, b) => {
      if (sortOption === 'price-asc') return a.price - b.price;
      if (sortOption === 'price-desc') return b.price - a.price;
      if (sortOption === 'area-asc') return a.area - b.area;
      if (sortOption === 'area-desc') return b.area - a.area;
      return 0;
    });

  // Format giá tiền
  const formatPrice = (price: number, type: 'sale' | 'rent') => {
    if (type === 'sale') {
      if (price >= 1000000000) {
        return `${(price / 1000000000).toFixed(1)} tỷ`;
      } else {
        return `${(price / 1000000).toFixed(0)} triệu`;
      }
    } else {
      return `${(price / 1000000).toFixed(1)} triệu/tháng`;
    }
  };

  useEffect(() => {
    setProperties(sampleProperties);
  }, []);

  return (
    <div className="container mx-auto py-8">
      <h1 className="mb-8 text-3xl font-bold">Danh sách bất động sản</h1>

      {/* Thanh tìm kiếm và lọc */}
      <div className="mb-8 rounded-lg bg-white p-6 shadow-md">
        <div className="mb-4 grid grid-cols-1 gap-4 md:grid-cols-5">
          <div className="relative">
            <Search className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
            <Input
              placeholder="Tìm kiếm bất động sản..."
              className="pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>

          <Select
            value={filterType}
            onValueChange={setFilterType}
          >
            <SelectTrigger>
              <SelectValue placeholder="Loại giao dịch" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">Tất cả</SelectItem>
              <SelectItem value="sale">Bán</SelectItem>
              <SelectItem value="rent">Cho thuê</SelectItem>
            </SelectContent>
          </Select>

          <Select
            value={filterCategory}
            onValueChange={setFilterCategory}
          >
            <SelectTrigger>
              <SelectValue placeholder="Loại BĐS" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">Tất cả</SelectItem>
              <SelectItem value="apartment">Căn hộ</SelectItem>
              <SelectItem value="house">Nhà phố</SelectItem>
              <SelectItem value="villa">Biệt thự</SelectItem>
              <SelectItem value="land">Đất nền</SelectItem>
            </SelectContent>
          </Select>

          <Select
            value={filterLocation}
            onValueChange={setFilterLocation}
          >
            <SelectTrigger>
              <SelectValue placeholder="Vị trí" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">Tất cả</SelectItem>
              <SelectItem value="Quận 2">Quận 2</SelectItem>
              <SelectItem value="Quận Bình Thạnh">Quận Bình Thạnh</SelectItem>
              <SelectItem value="Phú Quốc">Phú Quốc</SelectItem>
              <SelectItem value="Bảo Lộc">Bảo Lộc</SelectItem>
            </SelectContent>
          </Select>

          <Select
            value={sortOption}
            onValueChange={setSortOption}
          >
            <SelectTrigger>
              <SelectValue placeholder="Sắp xếp" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="price-asc">Giá: Thấp đến cao</SelectItem>
              <SelectItem value="price-desc">Giá: Cao đến thấp</SelectItem>
              <SelectItem value="area-asc">Diện tích: Nhỏ đến lớn</SelectItem>
              <SelectItem value="area-desc">Diện tích: Lớn đến nhỏ</SelectItem>
            </SelectContent>
          </Select>
        </div>

        <div className="mt-4">
          <p className="mb-2 text-sm text-gray-500">
            Khoảng giá: {formatPrice(priceRange[0], 'sale')} - {formatPrice(priceRange[1], 'sale')}
          </p>
          <Slider
            defaultValue={[0, 15000000000]}
            max={15000000000}
            step={100000000}
            value={priceRange}
            onValueChange={setPriceRange}
            className="my-4"
          />
        </div>
      </div>

      {/* Hiển thị kết quả */}
      <div className="mb-4 flex items-center justify-between">
        <p className="text-gray-600">Hiển thị {filteredProperties.length} bất động sản</p>
        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="sm"
          >
            <Filter className="mr-2 h-4 w-4" />
            Bộ lọc
          </Button>
          <Button
            variant="outline"
            size="sm"
          >
            <ArrowUpDown className="mr-2 h-4 w-4" />
            Sắp xếp
          </Button>
        </div>
      </div>

      {/* Danh sách bất động sản */}
      <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
        {filteredProperties.map((property) => (
          <Link
            href={`/properties/${property.id}`}
            key={property.id}
            className="group"
          >
            <Card className="group h-full overflow-hidden transition-shadow duration-300 hover:shadow-lg">
              <div className="relative h-48 w-full overflow-hidden">
                <Image
                  src={property.image}
                  alt={property.title}
                  fill
                  className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
                />
                {property.featured && (
                  <Badge className="absolute top-2 left-2 bg-amber-500">Nổi bật</Badge>
                )}
                <Badge
                  className={`absolute top-2 right-2 ${property.type === 'sale' ? 'bg-blue-500' : 'bg-green-500'}`}
                >
                  {property.type === 'sale' ? 'Bán' : 'Cho thuê'}
                </Badge>
                <Button
                  size="icon"
                  variant="ghost"
                  className="absolute right-2 bottom-2 h-8 w-8 rounded-full bg-white/70 hover:bg-white"
                >
                  <HeartIcon className="h-4 w-4 text-gray-600 hover:text-red-500" />
                </Button>
              </div>

              <CardHeader className="pb-2">
                <CardTitle className="line-clamp-1 text-lg transition-colors group-hover:text-blue-600">
                  {property.title}
                </CardTitle>
                <CardDescription className="flex items-center text-sm">
                  <MapPin className="mr-1 h-3.5 w-3.5 text-gray-500" />
                  {property.location}
                </CardDescription>
              </CardHeader>

              <CardContent className="pb-3">
                <p className="text-lg font-semibold text-blue-600">
                  {formatPrice(property.price, property.type)}
                </p>
                <p className="mt-1 line-clamp-2 text-sm text-gray-600">{property.description}</p>
              </CardContent>

              <CardFooter className="border-t pt-3">
                <div className="flex w-full justify-between text-sm text-gray-600">
                  {property.category !== 'land' && (
                    <>
                      <div className="flex items-center">
                        <Bed className="mr-1 h-4 w-4" />
                        {property.bedrooms} phòng ngủ
                      </div>
                      <div className="flex items-center">
                        <Bath className="mr-1 h-4 w-4" />
                        {property.bathrooms} phòng tắm
                      </div>
                    </>
                  )}
                  <div className="ml-auto flex items-center">
                    <Ruler className="mr-1 h-4 w-4" />
                    {property.area} m²
                  </div>
                </div>
              </CardFooter>
            </Card>
          </Link>
        ))}
      </div>

      {/* Phân trang */}
      <div className="mt-8 flex justify-center">
        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="sm"
            disabled
          >
            Trước
          </Button>
          <Button
            variant="default"
            size="sm"
          >
            1
          </Button>
          <Button
            variant="outline"
            size="sm"
          >
            2
          </Button>
          <Button
            variant="outline"
            size="sm"
          >
            3
          </Button>
          <Button
            variant="outline"
            size="sm"
          >
            Sau
          </Button>
        </div>
      </div>
    </div>
  );
}
