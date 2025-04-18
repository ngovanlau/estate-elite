// src/app/projects/page.tsx
'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { PlusCircle, Search, SlidersHorizontal, ArrowUpDown, MoreHorizontal } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from '@/components/ui/pagination';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import { Badge } from '@/components/ui/badge';
import { Checkbox } from '@/components/ui/checkbox';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import Image from 'next/image';

// Types
type ProjectStatus = 'active' | 'upcoming' | 'completed' | 'archived';
type ProjectType = 'sale' | 'rent' | 'both';
type ProjectCategory = 'residential' | 'commercial' | 'industrial' | 'land';

interface Project {
  id: string;
  name: string;
  description: string;
  address: string;
  city: string;
  district: string;
  price: {
    min: number;
    max: number;
    currency: string;
  };
  area: {
    min: number;
    max: number;
    unit: string;
  };
  type: ProjectType;
  category: ProjectCategory;
  status: ProjectStatus;
  developer: string;
  completionDate: string;
  featured: boolean;
  imageUrl: string;
  tags: string[];
  unitCount: number;
  availableUnits: number;
  createdAt: string;
  updatedAt: string;
}

// Mock data
const projectsData: Project[] = [
  {
    id: 'p001',
    name: 'Sunshine Paradise',
    description: 'Khu căn hộ cao cấp với đầy đủ tiện nghi hiện đại',
    address: '123 Nguyễn Văn Linh',
    city: 'Hồ Chí Minh',
    district: 'Quận 7',
    price: {
      min: 1500000000,
      max: 4500000000,
      currency: 'VND',
    },
    area: {
      min: 50,
      max: 150,
      unit: 'm²',
    },
    type: 'sale',
    category: 'residential',
    status: 'active',
    developer: 'Sunshine Group',
    completionDate: '2026-06-30',
    featured: true,
    imageUrl: '/api/placeholder/400/200',
    tags: ['premium', 'view sông', 'gần trung tâm'],
    unitCount: 500,
    availableUnits: 150,
    createdAt: '2024-01-15T00:00:00Z',
    updatedAt: '2024-03-20T00:00:00Z',
  },
  {
    id: 'p002',
    name: 'Golden Office Tower',
    description: 'Tòa văn phòng hạng A tại trung tâm thành phố',
    address: '456 Lê Duẩn',
    city: 'Hà Nội',
    district: 'Hoàn Kiếm',
    price: {
      min: 25000000,
      max: 50000000,
      currency: 'VND',
    },
    area: {
      min: 100,
      max: 500,
      unit: 'm²',
    },
    type: 'rent',
    category: 'commercial',
    status: 'active',
    developer: 'VinGroup',
    completionDate: '2023-12-15',
    featured: true,
    imageUrl: '/api/placeholder/400/200',
    tags: ['hạng A', 'view thành phố', 'smart office'],
    unitCount: 100,
    availableUnits: 25,
    createdAt: '2023-10-05T00:00:00Z',
    updatedAt: '2024-04-01T00:00:00Z',
  },
  {
    id: 'p003',
    name: 'Green Valley Villa',
    description: 'Khu biệt thự sinh thái với không gian xanh rộng lớn',
    address: '789 Nguyễn Hữu Thọ',
    city: 'Hồ Chí Minh',
    district: 'Nhà Bè',
    price: {
      min: 12000000000,
      max: 30000000000,
      currency: 'VND',
    },
    area: {
      min: 200,
      max: 500,
      unit: 'm²',
    },
    type: 'sale',
    category: 'residential',
    status: 'upcoming',
    developer: 'Novaland',
    completionDate: '2026-12-31',
    featured: false,
    imageUrl: '/api/placeholder/400/200',
    tags: ['biệt thự', 'sinh thái', 'an ninh'],
    unitCount: 50,
    availableUnits: 50,
    createdAt: '2024-02-18T00:00:00Z',
    updatedAt: '2024-03-05T00:00:00Z',
  },
  {
    id: 'p004',
    name: 'Tech Park Industrial',
    description: 'Khu công nghiệp công nghệ cao với cơ sở hạ tầng hiện đại',
    address: '101 Xa lộ Hà Nội',
    city: 'Hồ Chí Minh',
    district: 'Thủ Đức',
    price: {
      min: 100000000,
      max: 500000000,
      currency: 'VND',
    },
    area: {
      min: 500,
      max: 10000,
      unit: 'm²',
    },
    type: 'both',
    category: 'industrial',
    status: 'active',
    developer: 'Becamex',
    completionDate: '2023-06-15',
    featured: false,
    imageUrl: '/api/placeholder/400/200',
    tags: ['công nghệ cao', 'logistics', 'xuất khẩu'],
    unitCount: 30,
    availableUnits: 8,
    createdAt: '2023-07-10T00:00:00Z',
    updatedAt: '2024-01-15T00:00:00Z',
  },
];

// Utility functions
const formatCurrency = (amount: number, currency: string): string => {
  if (currency === 'VND') {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND',
      maximumFractionDigits: 0,
    }).format(amount);
  }
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: currency,
  }).format(amount);
};

const formatArea = (area: number, unit: string): string => {
  return `${area} ${unit}`;
};

const getStatusColor = (status: ProjectStatus): string => {
  switch (status) {
    case 'active':
      return 'bg-green-100 text-green-800';
    case 'upcoming':
      return 'bg-blue-100 text-blue-800';
    case 'completed':
      return 'bg-purple-100 text-purple-800';
    case 'archived':
      return 'bg-gray-100 text-gray-800';
    default:
      return 'bg-gray-100 text-gray-800';
  }
};

const getTypeColor = (type: ProjectType): string => {
  switch (type) {
    case 'sale':
      return 'bg-amber-100 text-amber-800';
    case 'rent':
      return 'bg-indigo-100 text-indigo-800';
    case 'both':
      return 'bg-teal-100 text-teal-800';
    default:
      return 'bg-gray-100 text-gray-800';
  }
};

const getCategoryColor = (category: ProjectCategory): string => {
  switch (category) {
    case 'residential':
      return 'bg-pink-100 text-pink-800';
    case 'commercial':
      return 'bg-sky-100 text-sky-800';
    case 'industrial':
      return 'bg-orange-100 text-orange-800';
    case 'land':
      return 'bg-lime-100 text-lime-800';
    default:
      return 'bg-gray-100 text-gray-800';
  }
};

const statusOptions = [
  { value: 'active', label: 'Đang hoạt động' },
  { value: 'upcoming', label: 'Sắp ra mắt' },
  { value: 'completed', label: 'Đã hoàn thành' },
  { value: 'archived', label: 'Đã lưu trữ' },
];

const typeOptions = [
  { value: 'sale', label: 'Bán' },
  { value: 'rent', label: 'Cho thuê' },
  { value: 'both', label: 'Cả hai' },
];

const categoryOptions = [
  { value: 'residential', label: 'Nhà ở' },
  { value: 'commercial', label: 'Thương mại' },
  { value: 'industrial', label: 'Công nghiệp' },
  { value: 'land', label: 'Đất' },
];

export default function ProjectsPage() {
  const router = useRouter();
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<string>('');
  const [typeFilter, setTypeFilter] = useState<string>('');
  const [categoryFilter, setCategoryFilter] = useState<string>('');
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [selectedProjects, setSelectedProjects] = useState<string[]>([]);
  const [currentView, setCurrentView] = useState<'grid' | 'list'>('grid');

  // Filter projects based on search and filters
  const filteredProjects = projectsData.filter((project) => {
    const matchesSearch =
      searchTerm === '' ||
      project.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      project.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
      project.address.toLowerCase().includes(searchTerm.toLowerCase()) ||
      project.city.toLowerCase().includes(searchTerm.toLowerCase()) ||
      project.district.toLowerCase().includes(searchTerm.toLowerCase());

    const matchesStatus = statusFilter === '' || project.status === statusFilter;
    const matchesType = typeFilter === '' || project.type === typeFilter;
    const matchesCategory = categoryFilter === '' || project.category === categoryFilter;

    return matchesSearch && matchesStatus && matchesType && matchesCategory;
  });

  const toggleProjectSelection = (projectId: string) => {
    setSelectedProjects((prev) =>
      prev.includes(projectId) ? prev.filter((id) => id !== projectId) : [...prev, projectId]
    );
  };

  const selectAllProjects = () => {
    if (selectedProjects.length === filteredProjects.length) {
      setSelectedProjects([]);
    } else {
      setSelectedProjects(filteredProjects.map((project) => project.id));
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-2xl font-bold">Danh sách dự án bất động sản</h1>
        <Button onClick={() => router.push('/projects/new')}>
          <PlusCircle className="mr-2 h-4 w-4" />
          Thêm dự án mới
        </Button>
      </div>

      <div className="mb-6 rounded-lg bg-white p-4 shadow-sm">
        <div className="mb-4 flex flex-col gap-4 md:flex-row">
          <div className="relative flex-1">
            <Search className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
            <Input
              placeholder="Tìm kiếm dự án theo tên, mô tả, địa chỉ..."
              className="pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <Dialog
            open={isFilterOpen}
            onOpenChange={setIsFilterOpen}
          >
            <DialogTrigger asChild>
              <Button variant="outline">
                <SlidersHorizontal className="mr-2 h-4 w-4" />
                Bộ lọc
              </Button>
            </DialogTrigger>
            <DialogContent className="max-w-lg">
              <DialogHeader>
                <DialogTitle>Bộ lọc nâng cao</DialogTitle>
                <DialogDescription>
                  Điều chỉnh các bộ lọc để tìm dự án phù hợp với nhu cầu của bạn.
                </DialogDescription>
              </DialogHeader>
              <div className="grid gap-4 py-4">
                <div className="grid grid-cols-1 gap-4">
                  <div>
                    <label className="mb-1 block text-sm font-medium">Trạng thái</label>
                    <Select
                      value={statusFilter}
                      onValueChange={(value) => setStatusFilter(value)}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Tất cả trạng thái" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="">Tất cả trạng thái</SelectItem>
                        {statusOptions.map((option) => (
                          <SelectItem
                            key={option.value}
                            value={option.value}
                          >
                            {option.label}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                  <div>
                    <label className="mb-1 block text-sm font-medium">Loại giao dịch</label>
                    <Select
                      value={typeFilter}
                      onValueChange={(value) => setTypeFilter(value)}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Tất cả loại giao dịch" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="">Tất cả loại giao dịch</SelectItem>
                        {typeOptions.map((option) => (
                          <SelectItem
                            key={option.value}
                            value={option.value}
                          >
                            {option.label}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                  <div>
                    <label className="mb-1 block text-sm font-medium">Loại bất động sản</label>
                    <Select
                      value={categoryFilter}
                      onValueChange={(value) => setCategoryFilter(value)}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Tất cả loại bất động sản" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="">Tất cả loại bất động sản</SelectItem>
                        {categoryOptions.map((option) => (
                          <SelectItem
                            key={option.value}
                            value={option.value}
                          >
                            {option.label}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                </div>
              </div>
              <div className="flex justify-between">
                <Button
                  variant="outline"
                  onClick={() => {
                    setStatusFilter('');
                    setTypeFilter('');
                    setCategoryFilter('');
                  }}
                >
                  Xóa bộ lọc
                </Button>
                <Button onClick={() => setIsFilterOpen(false)}>Áp dụng</Button>
              </div>
            </DialogContent>
          </Dialog>
          <Tabs
            value={currentView}
            onValueChange={(value) => setCurrentView(value as 'grid' | 'list')}
          >
            <TabsList className="h-10">
              <TabsTrigger value="grid">Grid</TabsTrigger>
              <TabsTrigger value="list">List</TabsTrigger>
            </TabsList>
          </Tabs>
        </div>

        <div className="mb-4 flex items-center justify-between">
          <div className="flex items-center">
            <Checkbox
              checked={
                selectedProjects.length > 0 && selectedProjects.length === filteredProjects.length
              }
              onCheckedChange={selectAllProjects}
              id="select-all"
              className="mr-2"
            />
            <label
              htmlFor="select-all"
              className="text-sm"
            >
              Chọn tất cả
            </label>
          </div>
          {selectedProjects.length > 0 && (
            <div className="flex gap-2">
              <span className="text-sm text-gray-500">Đã chọn {selectedProjects.length} dự án</span>
              <Button
                variant="ghost"
                size="sm"
                className="text-red-500 hover:text-red-700"
              >
                Xóa đã chọn
              </Button>
            </div>
          )}
        </div>

        <Tabs
          value={currentView}
          className="w-full"
        >
          <TabsContent
            value="grid"
            className="mt-0"
          >
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
              {filteredProjects.map((project) => (
                <Card
                  key={project.id}
                  className="overflow-hidden transition-shadow hover:shadow-md"
                >
                  <div className="relative">
                    <Image
                      src={project.imageUrl}
                      alt={project.name}
                      fill
                      className="h-48 w-full object-cover"
                    />
                    <div className="absolute top-2 left-2">
                      <Checkbox
                        checked={selectedProjects.includes(project.id)}
                        onCheckedChange={() => toggleProjectSelection(project.id)}
                        className="h-5 w-5 rounded-sm bg-white/80"
                      />
                    </div>
                    {project.featured && (
                      <Badge className="absolute top-2 right-2 bg-yellow-500">Nổi bật</Badge>
                    )}
                  </div>
                  <CardHeader className="pb-2">
                    <div className="flex items-start justify-between">
                      <CardTitle className="text-lg">{project.name}</CardTitle>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button
                            variant="ghost"
                            size="sm"
                            className="h-8 w-8 p-0"
                          >
                            <MoreHorizontal className="h-4 w-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuLabel>Hành động</DropdownMenuLabel>
                          <DropdownMenuItem onClick={() => router.push(`/projects/${project.id}`)}>
                            Xem chi tiết
                          </DropdownMenuItem>
                          <DropdownMenuItem
                            onClick={() => router.push(`/projects/${project.id}/edit`)}
                          >
                            Chỉnh sửa
                          </DropdownMenuItem>
                          <DropdownMenuSeparator />
                          <DropdownMenuItem className="text-red-500">Xóa</DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                    <CardDescription className="line-clamp-2 text-sm">
                      {project.description}
                    </CardDescription>
                  </CardHeader>
                  <CardContent className="pb-2">
                    <div className="mb-2 flex flex-wrap gap-2">
                      <Badge
                        variant="secondary"
                        className={getStatusColor(project.status)}
                      >
                        {statusOptions.find((option) => option.value === project.status)?.label}
                      </Badge>
                      <Badge
                        variant="secondary"
                        className={getTypeColor(project.type)}
                      >
                        {typeOptions.find((option) => option.value === project.type)?.label}
                      </Badge>
                      <Badge
                        variant="secondary"
                        className={getCategoryColor(project.category)}
                      >
                        {categoryOptions.find((option) => option.value === project.category)?.label}
                      </Badge>
                    </div>
                    <div className="text-sm">
                      <div className="mb-1 flex justify-between">
                        <span className="text-gray-500">Giá:</span>
                        <span>
                          {formatCurrency(project.price.min, project.price.currency)} -{' '}
                          {formatCurrency(project.price.max, project.price.currency)}
                        </span>
                      </div>
                      <div className="mb-1 flex justify-between">
                        <span className="text-gray-500">Diện tích:</span>
                        <span>
                          {formatArea(project.area.min, project.area.unit)} -{' '}
                          {formatArea(project.area.max, project.area.unit)}
                        </span>
                      </div>
                      <div className="flex justify-between">
                        <span className="text-gray-500">Vị trí:</span>
                        <span>
                          {project.district}, {project.city}
                        </span>
                      </div>
                    </div>
                  </CardContent>
                  <CardFooter className="pt-2">
                    <div className="flex w-full items-center justify-between">
                      <span className="text-sm text-gray-500">
                        {project.availableUnits}/{project.unitCount} đơn vị còn trống
                      </span>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => router.push(`/projects/${project.id}`)}
                      >
                        Chi tiết
                      </Button>
                    </div>
                  </CardFooter>
                </Card>
              ))}
            </div>
          </TabsContent>

          <TabsContent
            value="list"
            className="mt-0"
          >
            <div className="overflow-hidden rounded-lg bg-white">
              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead>
                    <tr className="border-b">
                      <th className="px-4 py-3 text-left">
                        <Checkbox
                          checked={
                            selectedProjects.length > 0 &&
                            selectedProjects.length === filteredProjects.length
                          }
                          onCheckedChange={selectAllProjects}
                        />
                      </th>
                      <th className="px-4 py-3 text-left">
                        <div className="flex items-center">
                          <span>Tên dự án</span>
                          <ArrowUpDown className="ml-1 h-4 w-4" />
                        </div>
                      </th>
                      <th className="px-4 py-3 text-left">Vị trí</th>
                      <th className="px-4 py-3 text-left">Loại</th>
                      <th className="px-4 py-3 text-left">Trạng thái</th>
                      <th className="px-4 py-3 text-right">Giá</th>
                      <th className="px-4 py-3 text-right">Diện tích</th>
                      <th className="px-4 py-3 text-right">Số lượng</th>
                      <th className="px-4 py-3"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {filteredProjects.map((project) => (
                      <tr
                        key={project.id}
                        className="border-b hover:bg-gray-50"
                      >
                        <td className="px-4 py-3">
                          <Checkbox
                            checked={selectedProjects.includes(project.id)}
                            onCheckedChange={() => toggleProjectSelection(project.id)}
                          />
                        </td>
                        <td className="px-4 py-3">
                          <div className="flex items-center">
                            <Image
                              src={project.imageUrl}
                              alt={project.name}
                              fill
                              className="mr-2 h-10 w-10 rounded object-cover"
                            />
                            <div>
                              <div className="font-medium">{project.name}</div>
                              <div className="max-w-xs truncate text-xs text-gray-500">
                                {project.description}
                              </div>
                            </div>
                          </div>
                        </td>
                        <td className="px-4 py-3">
                          <div className="text-sm">{project.district}</div>
                          <div className="text-xs text-gray-500">{project.city}</div>
                        </td>
                        <td className="px-4 py-3">
                          <Badge
                            variant="secondary"
                            className={getTypeColor(project.type)}
                          >
                            {typeOptions.find((option) => option.value === project.type)?.label}
                          </Badge>
                          <div className="mt-1">
                            <Badge
                              variant="secondary"
                              className={getCategoryColor(project.category)}
                            >
                              {
                                categoryOptions.find((option) => option.value === project.category)
                                  ?.label
                              }
                            </Badge>
                          </div>
                        </td>
                        <td className="px-4 py-3">
                          <Badge
                            variant="secondary"
                            className={getStatusColor(project.status)}
                          >
                            {statusOptions.find((option) => option.value === project.status)?.label}
                          </Badge>
                        </td>
                        <td className="px-4 py-3 text-right">
                          <div className="text-sm">
                            {formatCurrency(project.price.min, project.price.currency)}
                          </div>
                          <div className="text-xs text-gray-500">
                            đến {formatCurrency(project.price.max, project.price.currency)}
                          </div>
                        </td>
                        <td className="px-4 py-3 text-right">
                          <div className="text-sm">
                            {formatArea(project.area.min, project.area.unit)}
                          </div>
                          <div className="text-xs text-gray-500">
                            đến {formatArea(project.area.max, project.area.unit)}
                          </div>
                        </td>
                        <td className="px-4 py-3 text-right">
                          <div className="text-sm">
                            {project.availableUnits}/{project.unitCount}
                          </div>
                          <div className="text-xs text-gray-500">đơn vị còn trống</div>
                        </td>
                        <td className="px-4 py-3">
                          <DropdownMenu>
                            <DropdownMenuTrigger asChild>
                              <Button
                                variant="ghost"
                                size="sm"
                                className="h-8 w-8 p-0"
                              >
                                <MoreHorizontal className="h-4 w-4" />
                              </Button>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent align="end">
                              <DropdownMenuLabel>Hành động</DropdownMenuLabel>
                              <DropdownMenuItem
                                onClick={() => router.push(`/projects/${project.id}`)}
                              >
                                Xem chi tiết
                              </DropdownMenuItem>
                              <DropdownMenuItem
                                onClick={() => router.push(`/projects/${project.id}/edit`)}
                              >
                                Chỉnh sửa
                              </DropdownMenuItem>
                              <DropdownMenuSeparator />
                              <DropdownMenuItem className="text-red-500">Xóa</DropdownMenuItem>
                            </DropdownMenuContent>
                          </DropdownMenu>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </TabsContent>
        </Tabs>

        {filteredProjects.length === 0 && (
          <div className="py-8 text-center">
            <div className="mb-2 text-lg text-gray-400">Không tìm thấy dự án nào</div>
            <div className="text-gray-500">
              Vui lòng thử điều chỉnh bộ lọc hoặc tìm kiếm với từ khóa khác
            </div>
          </div>
        )}

        <div className="mt-6">
          <Pagination>
            <PaginationContent>
              <PaginationItem>
                <PaginationPrevious href="#" />
              </PaginationItem>
              <PaginationItem>
                <PaginationLink
                  href="#"
                  isActive
                >
                  1
                </PaginationLink>
              </PaginationItem>
              <PaginationItem>
                <PaginationLink href="#">2</PaginationLink>
              </PaginationItem>
              <PaginationItem>
                <PaginationLink href="#">3</PaginationLink>
              </PaginationItem>
              <PaginationItem>
                <PaginationNext href="#" />
              </PaginationItem>
            </PaginationContent>
          </Pagination>
        </div>
      </div>
    </div>
  );
}
