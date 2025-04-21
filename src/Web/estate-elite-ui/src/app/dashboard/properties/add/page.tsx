'use client';

import React, { useEffect, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { useRouter } from 'next/navigation';
import { z } from 'zod';
import Image from 'next/image';
import { X, ImagePlus } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Checkbox } from '@/components/ui/checkbox';
import { Separator } from '@/components/ui/separator';
import toast from 'react-hot-toast';
import { InputField } from '@/components/form-fields/input-field';
import { LISTING_TYPE } from '@/lib/enum';
import { District, Province, Ward } from '@/types';

const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
const ACCEPTED_IMAGE_TYPES = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp'];

const propertySchema = z.object({
  title: z.string().min(5, { message: 'Tiêu đề phải có ít nhất 5 ký tự' }),
  description: z.string().min(20, { message: 'Mô tả phải có ít nhất 20 ký tự' }),
  propertyType: z.string({ required_error: 'Vui lòng chọn loại bất động sản' }),
  listingType: z.string({ required_error: 'Vui lòng chọn kiểu giao dịch' }),
  price: z.string().min(1, { message: 'Vui lòng nhập giá' }),
  area: z.string().min(1, { message: 'Vui lòng nhập diện tích' }),
  address: z.string().min(5, { message: 'Địa chỉ phải có ít nhất 5 ký tự' }),
  city: z.string().min(1, { message: 'Vui lòng chọn thành phố' }),
  district: z.string().min(1, { message: 'Vui lòng chọn quận/huyện' }),
  ward: z.string().min(1, { message: 'Vui lòng chọn phường/xã' }),
  bedrooms: z.string().optional(),
  bathrooms: z.string().optional(),
  features: z.array(z.string()).optional(),
  images: z
    .array(
      z
        .instanceof(File)
        .refine((file) => file.size <= MAX_FILE_SIZE, `Kích thước tệp tối đa là 5MB`)
        .refine(
          (file) => ACCEPTED_IMAGE_TYPES.includes(file.type),
          'Chỉ chấp nhận định dạng .jpg, .jpeg, .png và .webp'
        )
    )
    .optional(),
});

export default function AddPropertyPage() {
  const router = useRouter();
  const [previewImages, setPreviewImages] = useState<string[]>([]);
  const [provinces, setProvinces] = useState<Province[]>([]);
  const [selectedCity, setSelectedCity] = useState<string>();
  const [selectedDistrict, setSelectedDistrict] = useState<string>();
  const [selectedWard, setSelectedWard] = useState<string>();
  const [districts, setDistricts] = useState<District[]>([]);
  const [wards, setWards] = useState<Ward[]>([]);

  const form = useForm<z.infer<typeof propertySchema>>({
    resolver: zodResolver(propertySchema),
    defaultValues: {
      title: '',
      description: '',
      price: '',
      area: '',
      address: '',
      bedrooms: '',
      bathrooms: '',
      features: [],
      images: [],
    },
  });

  // Lắng nghe sự thay đổi của field city
  useEffect(() => {
    const subscription = form.watch((value, { name }) => {
      if (name === 'city') {
        setSelectedCity(value.city);
      }
      if (name === 'district') {
        setSelectedDistrict(value.district);
      }
      if (name === 'ward') {
        setSelectedWard(value.ward);
      }
    });

    return () => subscription.unsubscribe();
  }, [form]);

  // Cập nhật districts khi city thay đổi
  useEffect(() => {
    if (selectedCity) {
      const province = provinces.find((p) => p.Name === selectedCity);
      setDistricts(province?.District || []);

      // Reset district và ward khi city thay đổi
      form.setValue('district', '');
      form.setValue('ward', '');
      setWards([]);
    }
  }, [selectedCity, provinces, form]);

  // Cập nhật wards khi district thay đổi
  useEffect(() => {
    if (selectedDistrict && districts.length > 0) {
      const district = districts.find((d) => d.Name === selectedDistrict);
      setWards(district?.Ward || []);

      // Reset ward khi district thay đổi
      form.setValue('ward', '');
    }
  }, [selectedDistrict, districts, form]);

  const features = [
    { id: 'air-conditioning', label: 'Điều hòa' },
    { id: 'parking', label: 'Bãi đỗ xe' },
    { id: 'swimming-pool', label: 'Hồ bơi' },
    { id: 'garden', label: 'Sân vườn' },
    { id: 'security', label: 'An ninh 24/7' },
    { id: 'elevator', label: 'Thang máy' },
    { id: 'gym', label: 'Phòng tập gym' },
  ];

  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files || files.length === 0) return;

    const imageFiles: File[] = Array.from(files);
    const currentImages = form.getValues('images') || [];

    // Kiểm tra kích thước và loại file
    const validFiles = imageFiles.filter(
      (file) => file.size <= MAX_FILE_SIZE && ACCEPTED_IMAGE_TYPES.includes(file.type)
    );

    if (validFiles.length !== imageFiles.length) {
      toast.error('Một số hình ảnh không hợp lệ');
    }

    // Cập nhật giá trị trong form
    const newImages = [...currentImages, ...validFiles];
    form.setValue('images', newImages);

    // Tạo URL preview cho các hình ảnh
    const newPreviewUrls = validFiles.map((file) => URL.createObjectURL(file));
    setPreviewImages((prev) => [...prev, ...newPreviewUrls]);
  };

  const removeImage = (index: number) => {
    // Xóa khỏi form values
    const currentImages = form.getValues('images') || [];
    const newImages = [...currentImages];
    newImages.splice(index, 1);
    form.setValue('images', newImages);

    // Xóa khỏi preview
    const newPreviews = [...previewImages];
    URL.revokeObjectURL(newPreviews[index]); // Giải phóng bộ nhớ
    newPreviews.splice(index, 1);
    setPreviewImages(newPreviews);
  };

  // In the onSubmit function, modify the form data submission section:

  const onSubmit = (data: z.infer<typeof propertySchema>) => {
    console.log(data);

    // Create FormData to send both form data and files
    const formData = new FormData();

    // Add form data fields to formData
    Object.entries(data).forEach(([key, value]) => {
      if (key !== 'images' && key !== 'features') {
        // Ensure we're only passing string values
        if (value !== undefined && value !== null) {
          formData.append(key, String(value));
        }
      }
    });

    // Add features with the correct type handling
    if (data.features && data.features.length > 0) {
      data.features.forEach((feature) => {
        formData.append('features', feature);
      });
    }

    // Add image files with the correct type handling
    if (data.images && data.images.length > 0) {
      data.images.forEach((image) => {
        formData.append('images', image);
      });
    }

    toast.success('Thông tin bất động sản đã được gửi');

    // Simulate redirect
    setTimeout(() => {
      router.push('/dashboard/properties');
    }, 1500);
  };

  useEffect(() => {
    const fetchProvinces = async () => {
      try {
        const response = await fetch('/api/provinces');
        if (!response.ok) {
          throw new Error('Failed to fetch provinces');
        }
        const data = await response.json();
        setProvinces(data);
      } catch (error) {
        console.error('Error fetching provinces:', error);
        toast.error('Không thể tải danh sách tỉnh/thành phố');
      } finally {
        // setIsLoading(false);s
      }
    };

    fetchProvinces();
  }, []);

  return (
    <div className="container mx-auto py-6">
      <Card className="w-full">
        <CardHeader>
          <CardTitle>Thêm bất động sản mới</CardTitle>
          <CardDescription>
            Nhập thông tin chi tiết về bất động sản để đăng lên hệ thống
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(onSubmit)}
              className="space-y-8"
            >
              <div className="space-y-6">
                <div className="text-lg font-medium">Thông tin cơ bản</div>
                <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                  <InputField
                    control={form.control}
                    name="title"
                    label="Tiêu đề"
                    placeholder="Tiêu đề"
                    required
                  />

                  <FormField
                    control={form.control}
                    name="propertyType"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Loại bất động sản</FormLabel>
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Chọn loại bất động sản" />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            <SelectItem value="apartment">Căn hộ</SelectItem>
                            <SelectItem value="house">Nhà phố</SelectItem>
                            <SelectItem value="villa">Biệt thự</SelectItem>
                            <SelectItem value="office">Văn phòng</SelectItem>
                            <SelectItem value="land">Đất nền</SelectItem>
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="listingType"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Loại giao dịch</FormLabel>
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Chọn loại giao dịch" />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            <SelectItem value={LISTING_TYPE.SALE}>Bán</SelectItem>
                            <SelectItem value={LISTING_TYPE.RENT}>Cho thuê</SelectItem>
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="price"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Giá</FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            placeholder="Nhập giá"
                            {...field}
                          />
                        </FormControl>
                        <FormDescription>Giá tính bằng triệu VNĐ</FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="area"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Diện tích</FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            placeholder="Nhập diện tích"
                            {...field}
                          />
                        </FormControl>
                        <FormDescription>Diện tích tính bằng m²</FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>

                <FormField
                  control={form.control}
                  name="description"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Mô tả</FormLabel>
                      <FormControl>
                        <Textarea
                          placeholder="Mô tả chi tiết về bất động sản"
                          className="min-h-32"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <Separator />

                {/* Phần upload hình ảnh */}
                <div>
                  <div className="mb-4 text-lg font-medium">Hình ảnh bất động sản</div>
                  <div className="flex flex-col space-y-4">
                    <div className="grid grid-cols-2 gap-4 md:grid-cols-3 lg:grid-cols-4">
                      {previewImages.map((url, index) => (
                        <div
                          key={index}
                          className="relative aspect-square overflow-hidden rounded-md border border-gray-200"
                        >
                          <Image
                            src={url}
                            alt={`Preview ${index + 1}`}
                            fill
                            className="object-cover"
                          />
                          <button
                            type="button"
                            onClick={() => removeImage(index)}
                            className="bg-opacity-50 hover:bg-opacity-70 absolute top-1 right-1 rounded-full bg-black p-1 text-white"
                          >
                            <X className="h-4 w-4" />
                          </button>
                        </div>
                      ))}

                      <label
                        htmlFor="image-upload"
                        className="relative flex aspect-square cursor-pointer flex-col items-center justify-center rounded-md border border-dashed border-gray-300 text-sm text-gray-500 hover:border-gray-400"
                      >
                        <ImagePlus className="h-8 w-8" />
                        <span className="mt-2">Thêm ảnh</span>
                        <input
                          id="image-upload"
                          type="file"
                          multiple
                          accept=".jpg,.jpeg,.png,.webp"
                          className="sr-only"
                          onChange={handleImageUpload}
                        />
                      </label>
                    </div>

                    <FormDescription>
                      Tải lên tối đa 10 hình ảnh về bất động sản, định dạng .jpg, .jpeg, .png hoặc
                      .webp, kích thước tối đa 5MB mỗi ảnh.
                    </FormDescription>
                  </div>
                </div>

                <Separator />

                <div className="text-lg font-medium">Vị trí</div>
                <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                  <FormField
                    control={form.control}
                    name="city"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Tỉnh/Thành phố</FormLabel>
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Chọn tỉnh/thành phố" />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {provinces.map((province) => (
                              <SelectItem
                                key={province.Code}
                                value={province.Name}
                              >
                                {province.FullName}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="district"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Quận/Huyện</FormLabel>
                        <Select
                          disabled={selectedCity === undefined}
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Chọn quận/huyện" />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {districts.map((district) => (
                              <SelectItem
                                key={district.Code}
                                value={district.Name}
                              >
                                {district.FullName}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="ward"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Phường/Xã</FormLabel>
                        <Select
                          disabled={selectedDistrict === undefined}
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Chọn phường/xã" />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {wards.map((ward) => (
                              <SelectItem
                                key={ward.Code}
                                value={ward.Name}
                              >
                                {ward.FullName}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="address"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Địa chỉ</FormLabel>
                        <FormControl>
                          <Input
                            disabled={selectedWard === undefined}
                            placeholder="Số nhà, tên đường"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>

                <Separator />

                <div className="text-lg font-medium">Thông tin thêm</div>
                <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                  <FormField
                    control={form.control}
                    name="bedrooms"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Số phòng ngủ</FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            placeholder="Nhập số phòng ngủ"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="bathrooms"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Số phòng tắm</FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            placeholder="Nhập số phòng tắm"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>

                <FormField
                  control={form.control}
                  name="features"
                  render={() => (
                    <FormItem>
                      <div className="mb-4">
                        <FormLabel>Tiện ích</FormLabel>
                        <FormDescription>Chọn các tiện ích có sẵn cho bất động sản</FormDescription>
                      </div>
                      <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
                        {features.map((feature) => (
                          <FormField
                            key={feature.id}
                            control={form.control}
                            name="features"
                            render={({ field }) => {
                              return (
                                <FormItem
                                  key={feature.id}
                                  className="flex flex-row items-start space-y-0 space-x-3"
                                >
                                  <FormControl>
                                    <Checkbox
                                      checked={field.value?.includes(feature.id)}
                                      onCheckedChange={(checked) => {
                                        return checked
                                          ? field.onChange([...(field.value || []), feature.id])
                                          : field.onChange(
                                              field.value?.filter((value) => value !== feature.id)
                                            );
                                      }}
                                    />
                                  </FormControl>
                                  <FormLabel className="text-sm font-normal">
                                    {feature.label}
                                  </FormLabel>
                                </FormItem>
                              );
                            }}
                          />
                        ))}
                      </div>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <div className="flex justify-end space-x-4">
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => router.back()}
                >
                  Hủy
                </Button>
                <Button type="submit">Thêm bất động sản</Button>
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
