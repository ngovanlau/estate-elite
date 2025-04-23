'use client';

import { useEffect, useState } from 'react';
import { Plus } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { PropertyFilterBar } from './_components/property-filter-bar';
import { PropertyTable } from './_components/property-table';
import { PropertyPagination } from './_components/property-pagination';
import { useRouter } from 'next/navigation';
import { OwnerProperty } from '@/types/response/property-response';
import propertyService from '@/services/property-service';
import toast from 'react-hot-toast';

export default function PropertyManagement() {
  const router = useRouter();
  const [properties, setProperties] = useState<OwnerProperty[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  const [filterType, setFilterType] = useState<string>('all');
  const [currentPage, setCurrentPage] = useState<number>(1);

  // Filter properties based on search term and filters
  const filteredProperties = properties.filter((property) => {
    const matchesSearch =
      property.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      property.address.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || property.status === filterStatus;
    const matchesType = filterType === 'all' || property.type === filterType;

    return matchesSearch && matchesStatus && matchesType;
  });

  const handleDeleteProperty = (id: string) => {
    setProperties(properties.filter((property) => property.id !== id));
  };

  const handleEditProperty = () => {
    // setEditingProperty(property);
    // setIsFormDialogOpen(true);
  };

  // const handleSaveProperty = (
  //   formData: Omit<OwnerProperty, 'id' | 'createdAt'> & { id?: string }
  // ) => {
  //   if (formData.id) {
  //     // Update existing property
  //     setProperties(
  //       properties.map((property) =>
  //         property.id === formData.id ? { ...property, ...formData } : property
  //       )
  //     );
  //   } else {
  //     // Add new property
  //     const newProperty: OwnerProperty = {
  //       ...formData,
  //       id: `${properties.length + 1}`,
  //       createdAt: new Date().toISOString().split('T')[0],
  //     };
  //     setProperties([...properties, newProperty]);
  //   }

  //   setEditingProperty(null);
  // };

  const fetchOwnerProperties = async () => {
    try {
      const response = await propertyService.getOwnerProperties();
      if (!response.succeeded) {
        toast.error('Lấy danh sách bất động sản thất bại');
        throw new Error(response.message);
      }

      setProperties(response.data);
    } catch (error) {
      console.error('Error fetching properties:', error);
    }
  };

  useEffect(() => {
    fetchOwnerProperties();
  }, []);

  return (
    <div className="container mx-auto py-8">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-3xl font-bold">Quản Lý Bất Động Sản</h1>
        <Button onClick={() => router.push('properties/add')}>
          <Plus className="mr-2 h-4 w-4" /> Thêm bất động sản
        </Button>
      </div>

      <PropertyFilterBar
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        filterStatus={filterStatus}
        setFilterStatus={setFilterStatus}
        filterType={filterType}
        setFilterType={setFilterType}
      />

      <Card>
        <CardHeader>
          <CardTitle>Danh sách bất động sản</CardTitle>
          <CardDescription>Quản lý tất cả các bất động sản trong hệ thống của bạn.</CardDescription>
        </CardHeader>
        <CardContent>
          <PropertyTable
            properties={filteredProperties}
            onDelete={handleDeleteProperty}
            onEdit={handleEditProperty}
          />

          <PropertyPagination
            currentPage={currentPage}
            totalPages={Math.ceil(filteredProperties.length / 10)}
            onPageChange={setCurrentPage}
          />
        </CardContent>
      </Card>

      {/* <PropertyFormDialog
        open={isFormDialogOpen}
        onOpenChange={setIsFormDialogOpen}
        onSave={handleSaveProperty}
        editProperty={editingProperty}
      /> */}
    </div>
  );
}
