// app/property-management/page.tsx
'use client';

import { useState } from 'react';
import { Search, Plus, Filter, MoreHorizontal, Edit, Trash2 } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import { Label } from '@/components/ui/label';
import { Badge } from '@/components/ui/badge';
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from '@/components/ui/pagination';

interface Property {
  id: string;
  title: string;
  address: string;
  price: number;
  status: 'Đang bán' | 'Đang cho thuê' | 'Đã bán' | 'Đã cho thuê';
  type: 'Căn hộ' | 'Nhà phố' | 'Biệt thự' | 'Đất nền';
  area: number;
  bedrooms: number;
  bathrooms: number;
  createdAt: string;
}

export default function PropertyManagement() {
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState('all');
  const [filterType, setFilterType] = useState('all');

  // Sample data for properties
  const [properties, setProperties] = useState<Property[]>([
    {
      id: '1',
      title: 'Căn hộ Vinhomes Central Park',
      address: '208 Nguyễn Hữu Cảnh, Bình Thạnh, TP.HCM',
      price: 3500000000,
      status: 'Đang bán',
      type: 'Căn hộ',
      area: 85,
      bedrooms: 2,
      bathrooms: 2,
      createdAt: '2025-03-15',
    },
    {
      id: '2',
      title: 'Nhà phố Thảo Điền',
      address: '12 Quốc Hương, Thảo Điền, Quận 2, TP.HCM',
      price: 12000000000,
      status: 'Đang cho thuê',
      type: 'Nhà phố',
      area: 200,
      bedrooms: 4,
      bathrooms: 3,
      createdAt: '2025-03-10',
    },
    {
      id: '3',
      title: 'Biệt thự Palm City',
      address: 'Palm City, Quận 9, TP.HCM',
      price: 18000000000,
      status: 'Đã bán',
      type: 'Biệt thự',
      area: 350,
      bedrooms: 5,
      bathrooms: 5,
      createdAt: '2025-02-28',
    },
    {
      id: '4',
      title: 'Đất nền Nhơn Trạch',
      address: 'Nhơn Trạch, Đồng Nai',
      price: 2500000000,
      status: 'Đang bán',
      type: 'Đất nền',
      area: 120,
      bedrooms: 0,
      bathrooms: 0,
      createdAt: '2025-02-15',
    },
    {
      id: '5',
      title: 'Căn hộ Masteri An Phú',
      address: 'An Phú, Quận 2, TP.HCM',
      price: 4200000000,
      status: 'Đã cho thuê',
      type: 'Căn hộ',
      area: 70,
      bedrooms: 2,
      bathrooms: 2,
      createdAt: '2025-01-22',
    },
  ]);

  // Filter properties based on search term and filters
  const filteredProperties = properties.filter((property) => {
    const matchesSearch =
      property.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      property.address.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || property.status === filterStatus;
    const matchesType = filterType === 'all' || property.type === filterType;

    return matchesSearch && matchesStatus && matchesType;
  });

  // Format price to VND
  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
  };

  const handleDeleteProperty = (id: string) => {
    setProperties(properties.filter((property) => property.id !== id));
  };

  return (
    <div className="container mx-auto py-8">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-3xl font-bold">Quản Lý Bất Động Sản</h1>
        <Dialog
          open={isAddDialogOpen}
          onOpenChange={setIsAddDialogOpen}
        >
          <DialogTrigger asChild>
            <Button>
              <Plus className="mr-2 h-4 w-4" /> Thêm bất động sản
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-xl">
            <DialogHeader>
              <DialogTitle>Thêm bất động sản mới</DialogTitle>
              <DialogDescription>
                Điền thông tin chi tiết về bất động sản vào form dưới đây.
              </DialogDescription>
            </DialogHeader>
            <div className="grid gap-4 py-4">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="title">Tiêu đề</Label>
                  <Input
                    id="title"
                    placeholder="Nhập tiêu đề bất động sản"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="address">Địa chỉ</Label>
                  <Input
                    id="address"
                    placeholder="Nhập địa chỉ"
                  />
                </div>
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="price">Giá</Label>
                  <Input
                    id="price"
                    type="number"
                    placeholder="Nhập giá"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="area">Diện tích (m²)</Label>
                  <Input
                    id="area"
                    type="number"
                    placeholder="Nhập diện tích"
                  />
                </div>
              </div>
              <div className="grid grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="type">Loại bất động sản</Label>
                  <Select>
                    <SelectTrigger id="type">
                      <SelectValue placeholder="Chọn loại" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="Căn hộ">Căn hộ</SelectItem>
                      <SelectItem value="Nhà phố">Nhà phố</SelectItem>
                      <SelectItem value="Biệt thự">Biệt thự</SelectItem>
                      <SelectItem value="Đất nền">Đất nền</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="bedrooms">Phòng ngủ</Label>
                  <Input
                    id="bedrooms"
                    type="number"
                    placeholder="Số phòng ngủ"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="bathrooms">Phòng tắm</Label>
                  <Input
                    id="bathrooms"
                    type="number"
                    placeholder="Số phòng tắm"
                  />
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="status">Trạng thái</Label>
                <Select>
                  <SelectTrigger id="status">
                    <SelectValue placeholder="Chọn trạng thái" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Đang bán">Đang bán</SelectItem>
                    <SelectItem value="Đang cho thuê">Đang cho thuê</SelectItem>
                    <SelectItem value="Đã bán">Đã bán</SelectItem>
                    <SelectItem value="Đã cho thuê">Đã cho thuê</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
            <DialogFooter>
              <Button
                variant="outline"
                onClick={() => setIsAddDialogOpen(false)}
              >
                Hủy
              </Button>
              <Button onClick={() => setIsAddDialogOpen(false)}>Lưu</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      <Card className="mb-6">
        <CardContent className="pt-6">
          <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
            <div className="flex flex-1 items-center space-x-2">
              <Search className="h-4 w-4" />
              <Input
                placeholder="Tìm kiếm theo tên, địa chỉ..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-md"
              />
            </div>
            <div className="flex flex-wrap gap-2">
              <Select
                value={filterStatus}
                onValueChange={setFilterStatus}
              >
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Trạng thái" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Tất cả trạng thái</SelectItem>
                  <SelectItem value="Đang bán">Đang bán</SelectItem>
                  <SelectItem value="Đang cho thuê">Đang cho thuê</SelectItem>
                  <SelectItem value="Đã bán">Đã bán</SelectItem>
                  <SelectItem value="Đã cho thuê">Đã cho thuê</SelectItem>
                </SelectContent>
              </Select>
              <Select
                value={filterType}
                onValueChange={setFilterType}
              >
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Loại BĐS" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Tất cả loại</SelectItem>
                  <SelectItem value="Căn hộ">Căn hộ</SelectItem>
                  <SelectItem value="Nhà phố">Nhà phố</SelectItem>
                  <SelectItem value="Biệt thự">Biệt thự</SelectItem>
                  <SelectItem value="Đất nền">Đất nền</SelectItem>
                </SelectContent>
              </Select>
              <Button
                variant="outline"
                size="icon"
              >
                <Filter className="h-4 w-4" />
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Danh sách bất động sản</CardTitle>
          <CardDescription>Quản lý tất cả các bất động sản trong hệ thống của bạn.</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="w-[250px]">Tên BĐS</TableHead>
                <TableHead>Địa chỉ</TableHead>
                <TableHead className="text-right">Giá</TableHead>
                <TableHead>Trạng thái</TableHead>
                <TableHead>Loại</TableHead>
                <TableHead className="text-right">Diện tích</TableHead>
                <TableHead className="text-center">Thao tác</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredProperties.length > 0 ? (
                filteredProperties.map((property) => (
                  <TableRow key={property.id}>
                    <TableCell className="font-medium">{property.title}</TableCell>
                    <TableCell className="max-w-[200px] truncate">{property.address}</TableCell>
                    <TableCell className="text-right">{formatPrice(property.price)}</TableCell>
                    <TableCell>
                      <Badge
                        variant={
                          property.status === 'Đang bán'
                            ? 'default'
                            : property.status === 'Đang cho thuê'
                              ? 'secondary'
                              : property.status === 'Đã bán'
                                ? 'destructive'
                                : 'outline'
                        }
                      >
                        {property.status}
                      </Badge>
                    </TableCell>
                    <TableCell>{property.type}</TableCell>
                    <TableCell className="text-right">{property.area} m²</TableCell>
                    <TableCell className="text-center">
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button
                            variant="ghost"
                            size="icon"
                          >
                            <MoreHorizontal className="h-4 w-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuLabel>Thao tác</DropdownMenuLabel>
                          <DropdownMenuSeparator />
                          <DropdownMenuItem>
                            <Edit className="mr-2 h-4 w-4" /> Chỉnh sửa
                          </DropdownMenuItem>
                          <DropdownMenuItem
                            className="text-red-600"
                            onClick={() => handleDeleteProperty(property.id)}
                          >
                            <Trash2 className="mr-2 h-4 w-4" /> Xóa
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell
                    colSpan={7}
                    className="py-6 text-center text-gray-500"
                  >
                    Không tìm thấy bất động sản nào
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>

          <div className="mt-4 flex items-center justify-end">
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
                  <PaginationEllipsis />
                </PaginationItem>
                <PaginationItem>
                  <PaginationNext href="#" />
                </PaginationItem>
              </PaginationContent>
            </Pagination>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
