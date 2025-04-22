// app/property-management/page.tsx
'use client';

import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { PropertyFilterBar } from './_components/property-filter-bar';
import { PropertyTable } from './_components/property-table';
import { PropertyPagination } from './_components/property-pagination';
import { PropertyFormDialog } from './_components/property-form-dialog';
import { sampleProperties } from './_components/sample-property-data';
import { Property } from './_components/type';
import { useRouter } from 'next/navigation';

export default function PropertyManagement() {
  const router = useRouter();
  const [properties, setProperties] = useState<Property[]>(sampleProperties);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filterStatus, setFilterStatus] = useState<string>('all');
  const [filterType, setFilterType] = useState<string>('all');
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [isFormDialogOpen, setIsFormDialogOpen] = useState<boolean>(false);
  const [editingProperty, setEditingProperty] = useState<Property | null>(null);

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

  const handleEditProperty = (property: Property) => {
    setEditingProperty(property);
    setIsFormDialogOpen(true);
  };

  const handleSaveProperty = (formData: Omit<Property, 'id' | 'createdAt'> & { id?: string }) => {
    if (formData.id) {
      // Update existing property
      setProperties(
        properties.map((property) =>
          property.id === formData.id ? { ...property, ...formData } : property
        )
      );
    } else {
      // Add new property
      const newProperty: Property = {
        ...formData,
        id: `${properties.length + 1}`,
        createdAt: new Date().toISOString().split('T')[0],
      };
      setProperties([...properties, newProperty]);
    }

    setEditingProperty(null);
  };

  return (
    <div className="container mx-auto py-8">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-3xl font-bold">Quản Lý Bất Động Sản</h1>
        <Button onClick={() => router.push('/add')}>
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

      <PropertyFormDialog
        open={isFormDialogOpen}
        onOpenChange={setIsFormDialogOpen}
        onSave={handleSaveProperty}
        editProperty={editingProperty}
      />
    </div>
  );
}
