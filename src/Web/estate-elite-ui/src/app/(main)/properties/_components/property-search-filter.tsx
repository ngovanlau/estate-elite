'use client';

import React from 'react';
import { Input } from '@/components/ui/input';
import { Slider } from '@/components/ui/slider';
import { Search } from 'lucide-react';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';

interface PropertySearchFilterProps {
  searchTerm: string;
  setSearchTerm: (value: string) => void;
  filterType: string;
  setFilterType: (value: string) => void;
  filterCategory: string;
  setFilterCategory: (value: string) => void;
  filterLocation: string;
  setFilterLocation: (value: string) => void;
  priceRange: number[];
  setPriceRange: (value: number[]) => void;
  sortOption: string;
  setSortOption: (value: string) => void;
  formatPrice: (price: number, type: 'sale' | 'rent') => string;
}

export function PropertySearchFilter({
  searchTerm,
  setSearchTerm,
  filterType,
  setFilterType,
  filterCategory,
  setFilterCategory,
  filterLocation,
  setFilterLocation,
  priceRange,
  setPriceRange,
  sortOption,
  setSortOption,
  formatPrice,
}: PropertySearchFilterProps) {
  return (
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
  );
}
