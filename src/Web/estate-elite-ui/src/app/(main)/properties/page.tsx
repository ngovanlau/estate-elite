'use client';

import { useEffect, useState } from 'react';
import { PaginationSection } from '@/components/pagination-section';
import PropertyCard from '@/app/_components/property-card';
import propertyService from '@/services/property-service';
import toast from 'react-hot-toast';
import { Property } from '@/types/response/property-response';

export default function PropertyListingPage() {
  const [properties, setProperties] = useState<Property[]>([]);
  // const [filterType, setFilterType] = useState<string>('');
  // const [filterCategory, setFilterCategory] = useState<string>('');
  // const [filterLocation, setFilterLocation] = useState<string>('');
  // const [priceRange, setPriceRange] = useState<number[]>([0, 15000000000]);
  // const [sortOption, setSortOption] = useState<string>('');
  // const [searchTerm, setSearchTerm] = useState<string>('');

  // Xử lý lọc và sắp xếp dữ liệu
  // const filteredProperties = properties
  //   .filter((property) => {
  //     return (
  //       (filterType === '' || property.type === filterType) &&
  //       (filterCategory === '' || property.category === filterCategory) &&
  //       (filterLocation === '' || property.location.includes(filterLocation)) &&
  //       property.price >= priceRange[0] &&
  //       property.price <= priceRange[1] &&
  //       (searchTerm === '' ||
  //         property.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
  //         property.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
  //         property.location.toLowerCase().includes(searchTerm.toLowerCase()))
  //     );
  //   })
  //   .sort((a, b) => {
  //     if (sortOption === 'price-asc') return a.price - b.price;
  //     if (sortOption === 'price-desc') return b.price - a.price;
  //     if (sortOption === 'area-asc') return a.area - b.area;
  //     if (sortOption === 'area-desc') return b.area - a.area;
  //     return 0;
  //   });

  const fetchProperties = async () => {
    try {
      const response = await propertyService.getProperties();
      if (!response.succeeded) {
        toast.error('Lấy danh sách bất động sản thất bại');
        return;
      }
      setProperties(response.data);
    } catch (error) {
      console.error('Error fetching properties:', error);
    }
  };

  useEffect(() => {
    fetchProperties();
  }, []);

  return (
    <div className="container mx-auto py-8">
      <h1 className="mb-8 text-3xl font-bold">Danh sách bất động sản</h1>

      {/* Thanh tìm kiếm và lọc */}
      {/* <PropertySearchFilter
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        filterType={filterType}
        setFilterType={setFilterType}
        filterCategory={filterCategory}
        setFilterCategory={setFilterCategory}
        filterLocation={filterLocation}
        setFilterLocation={setFilterLocation}
        priceRange={priceRange}
        setPriceRange={setPriceRange}
        sortOption={sortOption}
        setSortOption={setSortOption}
        formatPrice={formatPrice}
      /> */}

      {/* Hiển thị kết quả */}
      {/* <PropertyListHeader count={filteredProperties.length} /> */}

      {/* Danh sách bất động sản */}
      <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
        {properties.map((property) => (
          // <PropertyCard
          //   key={property.id}
          //   property={property}
          //   formatPrice={formatPrice}
          // />
          <PropertyCard
            key={property.id}
            property={property}
          />
        ))}
      </div>

      {/* Phân trang */}
      <PaginationSection
        currentPage={1}
        onPageChange={() => {}}
        totalPages={10}
      />
    </div>
  );
}
