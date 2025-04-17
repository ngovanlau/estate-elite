'use client';

import { useState } from 'react';
import { Search, Plus, MoreHorizontal } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Badge } from '@/components/ui/badge';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import Image from 'next/image';

// Định nghĩa kiểu cho dự án bất động sản
type ProjectStatus = 'Đang mở bán' | 'Sắp mở bán' | 'Đã bán hết' | 'Đang cho thuê';
type PropertyType = 'Căn hộ' | 'Biệt thự' | 'Nhà phố' | 'Đất nền' | 'Văn phòng' | 'Khác';

interface PropertyProject {
  id: number;
  name: string;
  type: PropertyType;
  location: string;
  price: string;
  status: ProjectStatus;
  progress: number;
  imageUrl: string;
  units: number;
  unitsSold: number;
}

// Loại dự án bất động sản
const propertyTypes = [
  'Tất cả',
  'Căn hộ',
  'Biệt thự',
  'Nhà phố',
  'Đất nền',
  'Văn phòng',
  'Khác',
] as const;
type PropertyTypeFilter = (typeof propertyTypes)[number];

// Danh sách dự án mẫu
const propertyProjects: PropertyProject[] = [
  {
    id: 1,
    name: 'Vinhomes Ocean Park',
    type: 'Căn hộ',
    location: 'Hà Nội',
    price: '1.5 - 4.5 tỷ',
    status: 'Đang mở bán',
    progress: 75,
    imageUrl: '/api/placeholder/300/200',
    units: 120,
    unitsSold: 89,
  },
  {
    id: 2,
    name: 'Sunshine City',
    type: 'Căn hộ',
    location: 'TP.HCM',
    price: '2.8 - 8.5 tỷ',
    status: 'Đang mở bán',
    progress: 65,
    imageUrl: '/api/placeholder/300/200',
    units: 250,
    unitsSold: 167,
  },
  {
    id: 3,
    name: 'Palm Garden',
    type: 'Biệt thự',
    location: 'Đà Nẵng',
    price: '12 - 18 tỷ',
    status: 'Sắp mở bán',
    progress: 40,
    imageUrl: '/api/placeholder/300/200',
    units: 48,
    unitsSold: 12,
  },
  {
    id: 4,
    name: 'Diamond Plaza',
    type: 'Văn phòng',
    location: 'TP.HCM',
    price: '35 - 120 triệu/m²',
    status: 'Đang cho thuê',
    progress: 100,
    imageUrl: '/api/placeholder/300/200',
    units: 80,
    unitsSold: 65,
  },
  {
    id: 5,
    name: 'Green Valley',
    type: 'Đất nền',
    location: 'Bình Dương',
    price: '1.2 - 3.5 tỷ',
    status: 'Đã bán hết',
    progress: 100,
    imageUrl: '/api/placeholder/300/200',
    units: 150,
    unitsSold: 150,
  },
  {
    id: 6,
    name: 'Eco Smart City',
    type: 'Nhà phố',
    location: 'Hà Nội',
    price: '6.5 - 12 tỷ',
    status: 'Đang mở bán',
    progress: 55,
    imageUrl: '/api/placeholder/300/200',
    units: 72,
    unitsSold: 35,
  },
];

// Định nghĩa kiểu cho các props của component
interface ProgressBarProps {
  value: number;
}

interface StatusBadgeProps {
  status: ProjectStatus;
}

export default function PropertyProjectsDashboard() {
  const [selectedFilter, setSelectedFilter] = useState<PropertyTypeFilter>('Tất cả');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [viewMode, setViewMode] = useState<'grid' | 'list'>('grid');

  // Lọc dự án theo bộ lọc và từ khóa tìm kiếm
  const filteredProjects = propertyProjects.filter((project) => {
    const matchesFilter = selectedFilter === 'Tất cả' || project.type === selectedFilter;
    const matchesSearch =
      project.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      project.location.toLowerCase().includes(searchTerm.toLowerCase());
    return matchesFilter && matchesSearch;
  });

  // Render thanh tiến độ
  const ProgressBar: React.FC<ProgressBarProps> = ({ value }) => (
    <div className="h-2 w-full rounded-full bg-gray-200">
      <div
        className="h-2 rounded-full bg-blue-600"
        style={{ width: `${value}%` }}
      ></div>
    </div>
  );

  // Render status badge
  const StatusBadge: React.FC<StatusBadgeProps> = ({ status }) => {
    let colorClass = 'bg-blue-500';

    if (status === 'Đang mở bán') colorClass = 'bg-green-500';
    else if (status === 'Sắp mở bán') colorClass = 'bg-yellow-500';
    else if (status === 'Đã bán hết') colorClass = 'bg-gray-500';
    else if (status === 'Đang cho thuê') colorClass = 'bg-purple-500';

    return (
      <span className={`rounded-full px-2 py-1 text-xs font-medium text-white ${colorClass}`}>
        {status}
      </span>
    );
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="text-3xl font-bold">Quản lý dự án bất động sản</h2>
        <Button>
          <Plus className="mr-2 h-4 w-4" />
          Thêm dự án mới
        </Button>
      </div>

      <div className="flex items-center justify-between space-x-4">
        <div className="relative flex-1">
          <Search className="absolute top-3 left-3 h-4 w-4 text-gray-500" />
          <Input
            placeholder="Tìm kiếm dự án, địa điểm..."
            className="pl-9"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>

        <Select
          value={selectedFilter}
          onValueChange={(value: PropertyTypeFilter) => setSelectedFilter(value)}
        >
          <SelectTrigger className="w-44">
            <SelectValue placeholder="Loại dự án" />
          </SelectTrigger>
          <SelectContent>
            {propertyTypes.map((type) => (
              <SelectItem
                key={type}
                value={type}
              >
                {type}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        <div className="flex rounded-md bg-gray-100">
          <Button
            variant={viewMode === 'grid' ? 'default' : 'ghost'}
            size="sm"
            onClick={() => setViewMode('grid')}
            className="rounded-r-none"
          >
            Grid
          </Button>
          <Button
            variant={viewMode === 'list' ? 'default' : 'ghost'}
            size="sm"
            onClick={() => setViewMode('list')}
            className="rounded-l-none"
          >
            List
          </Button>
        </div>
      </div>

      <Tabs defaultValue="all">
        <TabsList>
          <TabsTrigger value="all">Tất cả dự án</TabsTrigger>
          <TabsTrigger value="selling">Đang mở bán</TabsTrigger>
          <TabsTrigger value="upcoming">Sắp mở bán</TabsTrigger>
          <TabsTrigger value="sold">Đã bán hết</TabsTrigger>
        </TabsList>

        <TabsContent
          value="all"
          className="pt-4"
        >
          {viewMode === 'grid' ? (
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
              {filteredProjects.map((project) => (
                <Card
                  key={project.id}
                  className="overflow-hidden transition-shadow hover:shadow-lg"
                >
                  <div className="relative h-48">
                    <Image
                      src={project.imageUrl}
                      alt={project.name}
                      className="h-full w-full object-cover"
                      width={48}
                      height={48}
                    />
                    <div className="absolute top-2 right-2">
                      <StatusBadge status={project.status} />
                    </div>
                  </div>
                  <CardHeader className="pb-2">
                    <CardTitle className="flex items-start justify-between">
                      <div>
                        {project.name}
                        <span className="block text-sm font-normal text-gray-500">
                          {project.location}
                        </span>
                      </div>
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
                          <DropdownMenuItem>Xem chi tiết</DropdownMenuItem>
                          <DropdownMenuItem>Chỉnh sửa</DropdownMenuItem>
                          <DropdownMenuItem className="text-red-600">Xóa</DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </CardTitle>
                    <CardDescription className="mt-1 flex items-center">
                      <Badge
                        variant="outline"
                        className="mr-2"
                      >
                        {project.type}
                      </Badge>
                      <span className="font-medium">{project.price}</span>
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <div className="space-y-3">
                      <div className="flex justify-between text-sm">
                        <span>Tiến độ dự án</span>
                        <span className="font-medium">{project.progress}%</span>
                      </div>
                      <ProgressBar value={project.progress} />
                      <div className="mt-2 flex justify-between text-sm">
                        <span>Đã bán</span>
                        <span className="font-medium">
                          {project.unitsSold}/{project.units} căn
                        </span>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          ) : (
            <div className="rounded-md border">
              <div className="grid grid-cols-12 gap-4 bg-gray-50 p-4 font-medium">
                <div className="col-span-4">Dự án</div>
                <div className="col-span-2">Loại</div>
                <div className="col-span-2">Giá</div>
                <div className="col-span-2">Tiến độ</div>
                <div className="col-span-1">Trạng thái</div>
                <div className="col-span-1"></div>
              </div>
              {filteredProjects.map((project) => (
                <div
                  key={project.id}
                  className="grid grid-cols-12 items-center gap-4 border-t p-4"
                >
                  <div className="col-span-4 flex items-center">
                    <Image
                      src={project.imageUrl}
                      alt={project.name}
                      className="mr-3 h-12 w-12 rounded-md object-cover"
                      width={48}
                      height={48}
                    />
                    <div>
                      <div className="font-medium">{project.name}</div>
                      <div className="text-sm text-gray-500">{project.location}</div>
                    </div>
                  </div>
                  <div className="col-span-2">
                    <Badge variant="outline">{project.type}</Badge>
                  </div>
                  <div className="col-span-2">{project.price}</div>
                  <div className="col-span-2">
                    <div className="flex items-center space-x-2">
                      <ProgressBar value={project.progress} />
                      <span className="text-sm font-medium">{project.progress}%</span>
                    </div>
                  </div>
                  <div className="col-span-1">
                    <StatusBadge status={project.status} />
                  </div>
                  <div className="col-span-1 text-right">
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
                        <DropdownMenuItem>Xem chi tiết</DropdownMenuItem>
                        <DropdownMenuItem>Chỉnh sửa</DropdownMenuItem>
                        <DropdownMenuItem className="text-red-600">Xóa</DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                </div>
              ))}
            </div>
          )}
        </TabsContent>

        <TabsContent
          value="selling"
          className="pt-4"
        >
          {viewMode === 'grid' ? (
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
              {filteredProjects
                .filter((project) => project.status === 'Đang mở bán')
                .map((project) => (
                  <Card
                    key={project.id}
                    className="overflow-hidden transition-shadow hover:shadow-lg"
                  >
                    <div className="relative h-48">
                      <Image
                        src={project.imageUrl}
                        alt={project.name}
                        className="h-full w-full object-cover"
                        width={48}
                        height={48}
                      />
                      <div className="absolute top-2 right-2">
                        <StatusBadge status={project.status} />
                      </div>
                    </div>
                    <CardHeader className="pb-2">
                      <CardTitle className="flex items-start justify-between">
                        <div>
                          {project.name}
                          <span className="block text-sm font-normal text-gray-500">
                            {project.location}
                          </span>
                        </div>
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
                            <DropdownMenuItem>Xem chi tiết</DropdownMenuItem>
                            <DropdownMenuItem>Chỉnh sửa</DropdownMenuItem>
                            <DropdownMenuItem className="text-red-600">Xóa</DropdownMenuItem>
                          </DropdownMenuContent>
                        </DropdownMenu>
                      </CardTitle>
                      <CardDescription className="mt-1 flex items-center">
                        <Badge
                          variant="outline"
                          className="mr-2"
                        >
                          {project.type}
                        </Badge>
                        <span className="font-medium">{project.price}</span>
                      </CardDescription>
                    </CardHeader>
                    <CardContent>
                      <div className="space-y-3">
                        <div className="flex justify-between text-sm">
                          <span>Tiến độ dự án</span>
                          <span className="font-medium">{project.progress}%</span>
                        </div>
                        <ProgressBar value={project.progress} />
                        <div className="mt-2 flex justify-between text-sm">
                          <span>Đã bán</span>
                          <span className="font-medium">
                            {project.unitsSold}/{project.units} căn
                          </span>
                        </div>
                      </div>
                    </CardContent>
                  </Card>
                ))}
            </div>
          ) : (
            <div className="rounded-md border">
              <div className="grid grid-cols-12 gap-4 bg-gray-50 p-4 font-medium">
                <div className="col-span-4">Dự án</div>
                <div className="col-span-2">Loại</div>
                <div className="col-span-2">Giá</div>
                <div className="col-span-2">Tiến độ</div>
                <div className="col-span-1">Trạng thái</div>
                <div className="col-span-1"></div>
              </div>
              {filteredProjects
                .filter((project) => project.status === 'Đang mở bán')
                .map((project) => (
                  <div
                    key={project.id}
                    className="grid grid-cols-12 items-center gap-4 border-t p-4"
                  >
                    <div className="col-span-4 flex items-center">
                      <Image
                        src={project.imageUrl}
                        alt={project.name}
                        className="mr-3 h-12 w-12 rounded-md object-cover"
                        width={48}
                        height={48}
                      />
                      <div>
                        <div className="font-medium">{project.name}</div>
                        <div className="text-sm text-gray-500">{project.location}</div>
                      </div>
                    </div>
                    <div className="col-span-2">
                      <Badge variant="outline">{project.type}</Badge>
                    </div>
                    <div className="col-span-2">{project.price}</div>
                    <div className="col-span-2">
                      <div className="flex items-center space-x-2">
                        <ProgressBar value={project.progress} />
                        <span className="text-sm font-medium">{project.progress}%</span>
                      </div>
                    </div>
                    <div className="col-span-1">
                      <StatusBadge status={project.status} />
                    </div>
                    <div className="col-span-1 text-right">
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
                          <DropdownMenuItem>Xem chi tiết</DropdownMenuItem>
                          <DropdownMenuItem>Chỉnh sửa</DropdownMenuItem>
                          <DropdownMenuItem className="text-red-600">Xóa</DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                  </div>
                ))}
            </div>
          )}
        </TabsContent>

        {/* Các tab còn lại (upcoming và sold) sẽ có cấu trúc tương tự */}
        <TabsContent
          value="upcoming"
          className="pt-4"
        >
          <div className="py-8 text-center text-gray-500">
            {filteredProjects.filter((project) => project.status === 'Sắp mở bán').length > 0 ? (
              <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
                {/* Nội dung tương tự như tab all nhưng chỉ hiển thị các dự án sắp mở bán */}
              </div>
            ) : (
              <p>Không có dự án sắp mở bán phù hợp với bộ lọc hiện tại</p>
            )}
          </div>
        </TabsContent>

        <TabsContent
          value="sold"
          className="pt-4"
        >
          <div className="py-8 text-center text-gray-500">
            {filteredProjects.filter((project) => project.status === 'Đã bán hết').length > 0 ? (
              <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
                {/* Nội dung tương tự như tab all nhưng chỉ hiển thị các dự án đã bán hết */}
              </div>
            ) : (
              <p>Không có dự án đã bán hết phù hợp với bộ lọc hiện tại</p>
            )}
          </div>
        </TabsContent>
      </Tabs>

      <div className="mt-6 flex items-center justify-between">
        <Button
          variant="outline"
          size="sm"
          disabled
        >
          Trước
        </Button>
        <div className="flex items-center space-x-2">
          <Button
            variant="outline"
            size="sm"
            className="h-10 w-10 bg-blue-50 p-0 font-medium"
          >
            1
          </Button>
          <Button
            variant="outline"
            size="sm"
            className="h-10 w-10 p-0 font-medium"
          >
            2
          </Button>
          <Button
            variant="outline"
            size="sm"
            className="h-10 w-10 p-0 font-medium"
          >
            3
          </Button>
        </div>
        <Button
          variant="outline"
          size="sm"
        >
          Tiếp
        </Button>
      </div>
    </div>
  );
}
