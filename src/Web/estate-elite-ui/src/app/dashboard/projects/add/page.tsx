'use client';

import { useState } from 'react';
import { ChevronLeft, MapPin, Upload, X, Plus, Trash } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Button } from '@/components/ui/button';
import { Tabs, TabsList, TabsTrigger, TabsContent } from '@/components/ui/tabs';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Separator } from '@/components/ui/separator';
import { Switch } from '@/components/ui/switch';
import Image from 'next/image';

// Định nghĩa kiểu dữ liệu
// type ProjectStatus = 'draft' | 'published' | 'selling' | 'coming-soon' | 'sold-out';
// type PropertyType = 'apartment' | 'villa' | 'house' | 'land' | 'office' | 'commercial' | 'other';

interface PropertyUnit {
  id: string;
  name: string;
  type: string;
  area: string;
  price: string;
  bedrooms: string;
  bathrooms: string;
  direction: string;
  status: 'available' | 'pending' | 'sold';
}

// Component trang thêm dự án bất động sản
export default function AddPropertyProject() {
  const [activeTab, setActiveTab] = useState('basic-info');
  const [images, setImages] = useState<string[]>([]);
  const [amenities, setAmenities] = useState<string[]>([]);
  const [newAmenity, setNewAmenity] = useState('');
  const [propertyUnits, setPropertyUnits] = useState<PropertyUnit[]>([]);

  // Thêm hình ảnh mới
  const handleAddImage = () => {
    // Trong trường hợp thật, sẽ có xử lý upload ảnh
    // Ở đây chỉ thêm ảnh mẫu vào state
    setImages([...images, `/api/placeholder/500/300?text=Image ${images.length + 1}`]);
  };

  // Xóa hình ảnh
  const handleRemoveImage = (index: number) => {
    const newImages = [...images];
    newImages.splice(index, 1);
    setImages(newImages);
  };

  // Thêm tiện ích mới
  const handleAddAmenity = () => {
    if (newAmenity.trim()) {
      setAmenities([...amenities, newAmenity.trim()]);
      setNewAmenity('');
    }
  };

  // Xóa tiện ích
  const handleRemoveAmenity = (index: number) => {
    const newAmenities = [...amenities];
    newAmenities.splice(index, 1);
    setAmenities(newAmenities);
  };

  // Thêm đơn vị bất động sản mới
  const handleAddPropertyUnit = () => {
    const newUnit: PropertyUnit = {
      id: `unit-${Date.now()}`,
      name: '',
      type: 'apartment',
      area: '',
      price: '',
      bedrooms: '1',
      bathrooms: '1',
      direction: 'north',
      status: 'available',
    };
    setPropertyUnits([...propertyUnits, newUnit]);
  };

  // Xóa đơn vị bất động sản
  const handleRemovePropertyUnit = (id: string) => {
    setPropertyUnits(propertyUnits.filter((unit) => unit.id !== id));
  };

  // Cập nhật thông tin đơn vị bất động sản
  const handleUpdatePropertyUnit = (id: string, field: keyof PropertyUnit, value: string) => {
    setPropertyUnits(
      propertyUnits.map((unit) => (unit.id === id ? { ...unit, [field]: value } : unit))
    );
  };

  return (
    <div className="container mx-auto max-w-5xl py-8">
      <div className="mb-6 flex items-center">
        <Button
          variant="ghost"
          size="sm"
          className="mr-4"
        >
          <ChevronLeft className="mr-1 h-4 w-4" />
          Quay lại
        </Button>
        <h1 className="text-2xl font-bold">Thêm dự án bất động sản mới</h1>
      </div>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
        <div className="lg:col-span-2">
          <Tabs
            value={activeTab}
            onValueChange={setActiveTab}
            className="space-y-6"
          >
            <TabsList className="grid grid-cols-4">
              <TabsTrigger value="basic-info">Thông tin cơ bản</TabsTrigger>
              <TabsTrigger value="details">Chi tiết</TabsTrigger>
              <TabsTrigger value="media">Hình ảnh & Tài liệu</TabsTrigger>
              <TabsTrigger value="units">Danh sách BĐS</TabsTrigger>
            </TabsList>

            {/* Tab thông tin cơ bản */}
            <TabsContent
              value="basic-info"
              className="space-y-6"
            >
              <Card>
                <CardHeader>
                  <CardTitle>Thông tin cơ bản</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-1 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="project-name">Tên dự án</Label>
                      <Input
                        id="project-name"
                        placeholder="Nhập tên dự án"
                      />
                    </div>

                    <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                      <div className="space-y-2">
                        <Label htmlFor="project-type">Loại bất động sản</Label>
                        <Select defaultValue="apartment">
                          <SelectTrigger id="project-type">
                            <SelectValue placeholder="Chọn loại bất động sản" />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectItem value="apartment">Căn hộ</SelectItem>
                            <SelectItem value="villa">Biệt thự</SelectItem>
                            <SelectItem value="house">Nhà phố</SelectItem>
                            <SelectItem value="land">Đất nền</SelectItem>
                            <SelectItem value="office">Văn phòng</SelectItem>
                            <SelectItem value="commercial">Thương mại</SelectItem>
                            <SelectItem value="other">Khác</SelectItem>
                          </SelectContent>
                        </Select>
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="project-status">Trạng thái</Label>
                        <Select defaultValue="coming-soon">
                          <SelectTrigger id="project-status">
                            <SelectValue placeholder="Chọn trạng thái" />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectItem value="draft">Bản nháp</SelectItem>
                            <SelectItem value="coming-soon">Sắp mở bán</SelectItem>
                            <SelectItem value="selling">Đang mở bán</SelectItem>
                            <SelectItem value="sold-out">Đã bán hết</SelectItem>
                          </SelectContent>
                        </Select>
                      </div>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-description">Mô tả dự án</Label>
                      <Textarea
                        id="project-description"
                        placeholder="Nhập mô tả chi tiết về dự án..."
                        className="min-h-32"
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Vị trí</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="project-city">Tỉnh/Thành phố</Label>
                      <Select defaultValue="hcm">
                        <SelectTrigger id="project-city">
                          <SelectValue placeholder="Chọn tỉnh/thành phố" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="hcm">TP. Hồ Chí Minh</SelectItem>
                          <SelectItem value="hn">Hà Nội</SelectItem>
                          <SelectItem value="dn">Đà Nẵng</SelectItem>
                          <SelectItem value="hp">Hải Phòng</SelectItem>
                          <SelectItem value="bd">Bình Dương</SelectItem>
                          <SelectItem value="other">Khác</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-district">Quận/Huyện</Label>
                      <Select defaultValue="q1">
                        <SelectTrigger id="project-district">
                          <SelectValue placeholder="Chọn quận/huyện" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="q1">Quận 1</SelectItem>
                          <SelectItem value="q2">Quận 2</SelectItem>
                          <SelectItem value="q3">Quận 3</SelectItem>
                          <SelectItem value="q7">Quận 7</SelectItem>
                          <SelectItem value="other">Khác</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="project-address">Địa chỉ chi tiết</Label>
                    <div className="relative">
                      <MapPin className="absolute top-3 left-3 h-4 w-4 text-gray-500" />
                      <Input
                        id="project-address"
                        className="pl-9"
                        placeholder="Nhập địa chỉ chi tiết"
                      />
                    </div>
                  </div>

                  <div className="flex h-64 items-center justify-center rounded-md border border-dashed border-gray-300 bg-gray-100">
                    <div className="text-center">
                      <MapPin className="mx-auto h-10 w-10 text-gray-400" />
                      <p className="mt-2 text-sm text-gray-500">Bản đồ sẽ hiển thị ở đây</p>
                      <Button
                        variant="outline"
                        size="sm"
                        className="mt-4"
                      >
                        Chọn vị trí trên bản đồ
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Giá & Phí</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="project-price-min">Giá từ</Label>
                      <div className="relative">
                        <Input
                          id="project-price-min"
                          placeholder="Ví dụ: 1.5"
                        />
                        <div className="absolute top-3 right-3 text-gray-500">tỷ</div>
                      </div>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-price-max">Đến</Label>
                      <div className="relative">
                        <Input
                          id="project-price-max"
                          placeholder="Ví dụ: 5.2"
                        />
                        <div className="absolute top-3 right-3 text-gray-500">tỷ</div>
                      </div>
                    </div>
                  </div>

                  <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="project-area-min">Diện tích từ</Label>
                      <div className="relative">
                        <Input
                          id="project-area-min"
                          placeholder="Ví dụ: 50"
                        />
                        <div className="absolute top-3 right-3 text-gray-500">m²</div>
                      </div>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-area-max">Đến</Label>
                      <div className="relative">
                        <Input
                          id="project-area-max"
                          placeholder="Ví dụ: 120"
                        />
                        <div className="absolute top-3 right-3 text-gray-500">m²</div>
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Tab chi tiết */}
            <TabsContent
              value="details"
              className="space-y-6"
            >
              <Card>
                <CardHeader>
                  <CardTitle>Thông tin chi tiết</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="project-units">Tổng số căn</Label>
                      <Input
                        id="project-units"
                        type="number"
                        placeholder="Nhập tổng số căn"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-floors">Số tầng</Label>
                      <Input
                        id="project-floors"
                        type="number"
                        placeholder="Nhập số tầng"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-start-date">Ngày khởi công</Label>
                      <Input
                        id="project-start-date"
                        type="date"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-completion-date">Ngày hoàn thành dự kiến</Label>
                      <Input
                        id="project-completion-date"
                        type="date"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-developer">Chủ đầu tư</Label>
                      <Input
                        id="project-developer"
                        placeholder="Nhập tên chủ đầu tư"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="project-legal">Pháp lý</Label>
                      <Select defaultValue="red-book">
                        <SelectTrigger id="project-legal">
                          <SelectValue placeholder="Chọn loại pháp lý" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="red-book">Sổ đỏ/Sổ hồng</SelectItem>
                          <SelectItem value="contract">Hợp đồng mua bán</SelectItem>
                          <SelectItem value="pending">Đang chờ sổ</SelectItem>
                          <SelectItem value="other">Khác</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Tiện ích</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="flex space-x-2">
                    <Input
                      placeholder="Thêm tiện ích mới"
                      value={newAmenity}
                      onChange={(e) => setNewAmenity(e.target.value)}
                      onKeyDown={(e) => e.key === 'Enter' && handleAddAmenity()}
                      className="flex-1"
                    />
                    <Button
                      type="button"
                      onClick={handleAddAmenity}
                    >
                      <Plus className="mr-1 h-4 w-4" />
                      Thêm
                    </Button>
                  </div>

                  <div className="mt-4 flex flex-wrap gap-2">
                    {amenities.map((amenity, index) => (
                      <div
                        key={index}
                        className="flex items-center rounded-full bg-gray-100 px-3 py-1"
                      >
                        <span className="text-sm">{amenity}</span>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="ml-1 h-5 w-5 p-0"
                          onClick={() => handleRemoveAmenity(index)}
                        >
                          <X className="h-3 w-3" />
                        </Button>
                      </div>
                    ))}
                    {amenities.length === 0 && (
                      <p className="text-sm text-gray-500">Chưa có tiện ích nào được thêm</p>
                    )}
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Thông tin khác</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="project-additional-info">Thông tin bổ sung</Label>
                    <Textarea
                      id="project-additional-info"
                      placeholder="Nhập các thông tin bổ sung về dự án..."
                      className="min-h-32"
                    />
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Tab hình ảnh & tài liệu */}
            <TabsContent
              value="media"
              className="space-y-6"
            >
              <Card>
                <CardHeader>
                  <CardTitle>Hình ảnh dự án</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
                    {images.map((image, index) => (
                      <div
                        key={index}
                        className="relative aspect-video overflow-hidden rounded-md bg-gray-100"
                      >
                        <Image
                          src={image}
                          alt={`Project image ${index + 1}`}
                          className="h-full w-full object-cover"
                          width={100}
                          height={100}
                        />
                        <Button
                          variant="destructive"
                          size="sm"
                          className="absolute top-2 right-2 h-8 w-8 rounded-full p-0"
                          onClick={() => handleRemoveImage(index)}
                        >
                          <X className="h-4 w-4" />
                        </Button>
                      </div>
                    ))}

                    <div
                      className="flex aspect-video cursor-pointer items-center justify-center rounded-md border-2 border-dashed border-gray-300 bg-gray-100 hover:bg-gray-50"
                      onClick={handleAddImage}
                    >
                      <div className="text-center">
                        <Upload className="mx-auto h-8 w-8 text-gray-400" />
                        <p className="mt-2 text-sm font-medium">Tải hình ảnh lên</p>
                        <p className="text-xs text-gray-500">PNG, JPG, GIF lên đến 10MB</p>
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Tài liệu dự án</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="rounded-md border-2 border-dashed border-gray-300 p-8">
                    <div className="text-center">
                      <Upload className="mx-auto h-10 w-10 text-gray-400" />
                      <p className="mt-2 font-medium">Kéo thả tệp để tải lên</p>
                      <p className="mt-1 text-sm text-gray-500">hoặc</p>
                      <Button
                        variant="outline"
                        size="sm"
                        className="mt-2"
                      >
                        Chọn tệp
                      </Button>
                      <p className="mt-2 text-xs text-gray-500">PDF, DOCX lên đến 20MB</p>
                    </div>
                  </div>

                  <div className="space-y-2">
                    <p className="text-sm font-medium">Tài liệu đã tải lên</p>
                    <p className="text-sm text-gray-500">Chưa có tài liệu nào được tải lên</p>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Tab danh sách bất động sản */}
            <TabsContent
              value="units"
              className="space-y-6"
            >
              <Card>
                <CardHeader className="flex flex-row items-center justify-between">
                  <CardTitle>Danh sách bất động sản</CardTitle>
                  <Button onClick={handleAddPropertyUnit}>
                    <Plus className="mr-2 h-4 w-4" />
                    Thêm bất động sản
                  </Button>
                </CardHeader>
                <CardContent>
                  {propertyUnits.length === 0 ? (
                    <div className="rounded-md border-2 border-dashed border-gray-300 py-10 text-center">
                      <p className="text-gray-500">Chưa có bất động sản nào trong dự án</p>
                      <Button
                        onClick={handleAddPropertyUnit}
                        className="mt-4"
                      >
                        <Plus className="mr-2 h-4 w-4" />
                        Thêm bất động sản
                      </Button>
                    </div>
                  ) : (
                    <div className="space-y-6">
                      {propertyUnits.map((unit, index) => (
                        <div
                          key={unit.id}
                          className="rounded-md border p-4"
                        >
                          <div className="mb-4 flex items-center justify-between">
                            <h3 className="font-medium">Bất động sản #{index + 1}</h3>
                            <Button
                              variant="ghost"
                              size="sm"
                              className="h-8 w-8 p-0 text-red-500 hover:text-red-600"
                              onClick={() => handleRemovePropertyUnit(unit.id)}
                            >
                              <Trash className="h-4 w-4" />
                            </Button>
                          </div>

                          <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                            <div className="space-y-2">
                              <Label htmlFor={`unit-name-${unit.id}`}>Tên/Mã căn hộ</Label>
                              <Input
                                id={`unit-name-${unit.id}`}
                                placeholder="Ví dụ: A-1202"
                                value={unit.name}
                                onChange={(e) =>
                                  handleUpdatePropertyUnit(unit.id, 'name', e.target.value)
                                }
                              />
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-type-${unit.id}`}>Loại căn hộ</Label>
                              <Select
                                value={unit.type}
                                onValueChange={(value) =>
                                  handleUpdatePropertyUnit(unit.id, 'type', value)
                                }
                              >
                                <SelectTrigger id={`unit-type-${unit.id}`}>
                                  <SelectValue placeholder="Chọn loại căn hộ" />
                                </SelectTrigger>
                                <SelectContent>
                                  <SelectItem value="apartment">Căn hộ</SelectItem>
                                  <SelectItem value="penthouse">Penthouse</SelectItem>
                                  <SelectItem value="duplex">Duplex</SelectItem>
                                  <SelectItem value="studio">Studio</SelectItem>
                                  <SelectItem value="other">Khác</SelectItem>
                                </SelectContent>
                              </Select>
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-area-${unit.id}`}>Diện tích (m²)</Label>
                              <Input
                                id={`unit-area-${unit.id}`}
                                placeholder="Ví dụ: 85"
                                value={unit.area}
                                onChange={(e) =>
                                  handleUpdatePropertyUnit(unit.id, 'area', e.target.value)
                                }
                              />
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-price-${unit.id}`}>Giá (tỷ VNĐ)</Label>
                              <Input
                                id={`unit-price-${unit.id}`}
                                placeholder="Ví dụ: 3.5"
                                value={unit.price}
                                onChange={(e) =>
                                  handleUpdatePropertyUnit(unit.id, 'price', e.target.value)
                                }
                              />
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-bedrooms-${unit.id}`}>Số phòng ngủ</Label>
                              <Select
                                value={unit.bedrooms}
                                onValueChange={(value) =>
                                  handleUpdatePropertyUnit(unit.id, 'bedrooms', value)
                                }
                              >
                                <SelectTrigger id={`unit-bedrooms-${unit.id}`}>
                                  <SelectValue placeholder="Chọn số phòng ngủ" />
                                </SelectTrigger>
                                <SelectContent>
                                  <SelectItem value="0">Studio</SelectItem>
                                  <SelectItem value="1">1 phòng ngủ</SelectItem>
                                  <SelectItem value="2">2 phòng ngủ</SelectItem>
                                  <SelectItem value="3">3 phòng ngủ</SelectItem>
                                  <SelectItem value="4">4 phòng ngủ</SelectItem>
                                  <SelectItem value="5+">5+ phòng ngủ</SelectItem>
                                </SelectContent>
                              </Select>
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-bathrooms-${unit.id}`}>Số phòng tắm</Label>
                              <Select
                                value={unit.bathrooms}
                                onValueChange={(value) =>
                                  handleUpdatePropertyUnit(unit.id, 'bathrooms', value)
                                }
                              >
                                <SelectTrigger id={`unit-bathrooms-${unit.id}`}>
                                  <SelectValue placeholder="Chọn số phòng tắm" />
                                </SelectTrigger>
                                <SelectContent>
                                  <SelectItem value="1">1 phòng tắm</SelectItem>
                                  <SelectItem value="2">2 phòng tắm</SelectItem>
                                  <SelectItem value="3">3 phòng tắm</SelectItem>
                                  <SelectItem value="4">4 phòng tắm</SelectItem>
                                  <SelectItem value="5+">5+ phòng tắm</SelectItem>
                                </SelectContent>
                              </Select>
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-direction-${unit.id}`}>Hướng</Label>
                              <Select
                                value={unit.direction}
                                onValueChange={(value) =>
                                  handleUpdatePropertyUnit(unit.id, 'direction', value)
                                }
                              >
                                <SelectTrigger id={`unit-direction-${unit.id}`}>
                                  <SelectValue placeholder="Chọn hướng" />
                                </SelectTrigger>
                                <SelectContent>
                                  <SelectItem value="north">Bắc</SelectItem>
                                  <SelectItem value="east">Đông</SelectItem>
                                  <SelectItem value="south">Nam</SelectItem>
                                  <SelectItem value="west">Tây</SelectItem>
                                  <SelectItem value="northeast">Đông Bắc</SelectItem>
                                  <SelectItem value="northwest">Tây Bắc</SelectItem>
                                  <SelectItem value="southeast">Đông Nam</SelectItem>
                                  <SelectItem value="southwest">Tây Nam</SelectItem>
                                </SelectContent>
                              </Select>
                            </div>

                            <div className="space-y-2">
                              <Label htmlFor={`unit-status-${unit.id}`}>Trạng thái</Label>
                              <Select
                                value={unit.status}
                                onValueChange={(value: string) =>
                                  handleUpdatePropertyUnit(unit.id, 'status', value)
                                }
                              >
                                <SelectTrigger id={`unit-status-${unit.id}`}>
                                  <SelectValue placeholder="Chọn trạng thái" />
                                </SelectTrigger>
                                <SelectContent>
                                  <SelectItem value="available">Còn trống</SelectItem>
                                  <SelectItem value="pending">Đang giao dịch</SelectItem>
                                  <SelectItem value="sold">Đã bán</SelectItem>
                                </SelectContent>
                              </Select>
                            </div>
                          </div>
                        </div>
                      ))}
                    </div>
                  )}
                </CardContent>
              </Card>
            </TabsContent>
          </Tabs>
        </div>
        <div>
          <div className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle>Trạng thái dự án</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="flex items-center justify-between">
                    <Label
                      htmlFor="project-published"
                      className="cursor-pointer"
                    >
                      Xuất bản dự án
                    </Label>
                    <Switch id="project-published" />
                  </div>
                  <div className="flex items-center justify-between">
                    <Label
                      htmlFor="project-featured"
                      className="cursor-pointer"
                    >
                      Đánh dấu nổi bật
                    </Label>
                    <Switch id="project-featured" />
                  </div>
                  <Separator className="my-2" />
                  <p className="text-sm text-gray-500">
                    Dự án chưa xuất bản sẽ không hiển thị cho người dùng.
                  </p>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>SEO</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="seo-title">Tiêu đề SEO</Label>
                    <Input
                      id="seo-title"
                      placeholder="Tiêu đề hiển thị trên Google"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="seo-description">Mô tả SEO</Label>
                    <Textarea
                      id="seo-description"
                      placeholder="Mô tả ngắn hiển thị trên kết quả tìm kiếm"
                      className="h-20"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="seo-keywords">Từ khóa SEO</Label>
                    <Input
                      id="seo-keywords"
                      placeholder="Các từ khóa cách nhau bằng dấu phẩy"
                    />
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Thông tin liên hệ</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="contact-name">Tên người liên hệ</Label>
                    <Input
                      id="contact-name"
                      placeholder="Nhập tên người liên hệ"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="contact-phone">Số điện thoại</Label>
                    <Input
                      id="contact-phone"
                      placeholder="Nhập số điện thoại"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="contact-email">Email</Label>
                    <Input
                      id="contact-email"
                      placeholder="Nhập địa chỉ email"
                      type="email"
                    />
                  </div>
                </div>
              </CardContent>
            </Card>

            <div className="flex flex-col gap-2">
              <Button
                className="w-full"
                size="lg"
              >
                Lưu dự án
              </Button>
              <Button
                variant="outline"
                className="w-full"
                size="lg"
              >
                Lưu bản nháp
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
