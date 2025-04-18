// pages/profile/index.tsx
'use client';

import { useState, useRef, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  Pencil,
  Camera,
  Loader2,
  Building2,
  Home,
  Briefcase,
  Users,
  CheckCircle,
  Award,
} from 'lucide-react';
import { Separator } from '@/components/ui/separator';
import { Badge } from '@/components/ui/badge';
import Image from 'next/image';
import toast from 'react-hot-toast';

const userTypes = ['buyer', 'seller', 'developer'] as const;
type UserType = (typeof userTypes)[number];

// Schema cho thông tin chung
const profileFormSchema = z.object({
  name: z.string().min(2, {
    message: 'Tên phải có ít nhất 2 ký tự.',
  }),
  email: z.string().email({
    message: 'Email không hợp lệ.',
  }),
  phone: z.string().min(10, {
    message: 'Số điện thoại phải có ít nhất 10 số.',
  }),
  bio: z.string().max(500, {
    message: 'Giới thiệu không được vượt quá 500 ký tự.',
  }),
  address: z.string().min(5, {
    message: 'Địa chỉ phải có ít nhất 5 ký tự.',
  }),
  userType: z.enum(userTypes),
});

// Schema cho thông tin doanh nghiệp (seller & developer)
const businessFormSchema = z.object({
  companyName: z.string().min(2, {
    message: 'Tên công ty phải có ít nhất 2 ký tự.',
  }),
  taxId: z.string().min(10, {
    message: 'Mã số thuế phải có ít nhất 10 ký tự.',
  }),
  website: z
    .string()
    .url({
      message: 'Website không hợp lệ.',
    })
    .or(z.string().length(0)),
  businessType: z.string().min(1, {
    message: 'Vui lòng chọn loại hình kinh doanh.',
  }),
  establishedYear: z.string().refine(
    (val) => {
      const year = parseInt(val);
      const currentYear = new Date().getFullYear();
      return !isNaN(year) && year > 1900 && year <= currentYear;
    },
    {
      message: 'Năm thành lập không hợp lệ.',
    }
  ),
});

// Schema cho đổi mật khẩu
const passwordFormSchema = z
  .object({
    currentPassword: z.string().min(8, {
      message: 'Mật khẩu phải có ít nhất 8 ký tự.',
    }),
    newPassword: z.string().min(8, {
      message: 'Mật khẩu mới phải có ít nhất 8 ký tự.',
    }),
    confirmPassword: z.string().min(8, {
      message: 'Mật khẩu xác nhận phải có ít nhất 8 ký tự.',
    }),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: 'Mật khẩu xác nhận không khớp.',
    path: ['confirmPassword'],
  });

export default function ProfilePage() {
  const [activeTab, setActiveTab] = useState('general');
  const [isLoading, setIsLoading] = useState(false);
  const [avatarSrc, setAvatarSrc] = useState('/api/placeholder/100/100');
  const [backgroundSrc, setBackgroundSrc] = useState('/api/placeholder/1200/300');
  const [userType, setUserType] = useState<UserType>('buyer');
  const [isVerified, setIsVerified] = useState(false);

  const avatarInputRef = useRef<HTMLInputElement>(null);
  const backgroundInputRef = useRef<HTMLInputElement>(null);
  const licenseInputRef = useRef<HTMLInputElement>(null);

  const profileForm = useForm<z.infer<typeof profileFormSchema>>({
    resolver: zodResolver(profileFormSchema),
    defaultValues: {
      name: 'Nguyễn Văn A',
      email: 'nguyenvana@example.com',
      phone: '0912345678',
      bio: 'Tham gia từ năm 2023. Quan tâm đến bất động sản khu vực TP.HCM.',
      address: '123 Nguyễn Văn Linh, Quận 7, TP.HCM',
      userType: 'buyer',
    },
  });

  const businessForm = useForm<z.infer<typeof businessFormSchema>>({
    resolver: zodResolver(businessFormSchema),
    defaultValues: {
      companyName: '',
      taxId: '',
      website: '',
      businessType: '',
      establishedYear: '',
    },
  });

  const passwordForm = useForm<z.infer<typeof passwordFormSchema>>({
    resolver: zodResolver(passwordFormSchema),
    defaultValues: {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    },
  });

  // Cập nhật userType khi người dùng thay đổi loại tài khoản
  useEffect(() => {
    const subscription = profileForm.watch((value, { name }) => {
      if (name === 'userType' && value.userType) {
        setUserType(value.userType as UserType);
      }
    });
    return () => subscription.unsubscribe();
  }, [profileForm]); // Add profileForm to the dependency array

  function onProfileSubmit(values: z.infer<typeof profileFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast.success('Thông tin cá nhân của bạn đã được lưu thành công.t');
    }, 1000);
  }

  function onBusinessSubmit(values: z.infer<typeof businessFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast.success('Thông tin doanh nghiệp của bạn đã được lưu thành công.');
    }, 1000);
  }

  function onPasswordSubmit(values: z.infer<typeof passwordFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast.success('Mật khẩu của bạn đã được thay đổi thành công.');
      passwordForm.reset();
    }, 1000);
  }

  function handleAvatarChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target?.result) {
          setAvatarSrc(e.target.result as string);
          toast.success('Avatar của bạn đã được cập nhật thành công.');
        }
      };
      reader.readAsDataURL(file);
    }
  }

  function handleBackgroundChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target?.result) {
          setBackgroundSrc(e.target.result as string);
          toast.success('Ảnh bìa của bạn đã được cập nhật thành công.');
        }
      };
      reader.readAsDataURL(file);
    }
  }

  function handleLicenseUpload(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (file) {
      setIsLoading(true);
      // Giả lập API call
      setTimeout(() => {
        setIsLoading(false);
        toast.success('Giấy phép kinh doanh của bạn đã được gửi để xác minh.');
      }, 1500);
    }
  }

  function handleResetPassword() {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      setIsLoading(false);
      toast.success('Email với hướng dẫn đặt lại mật khẩu đã được gửi đến email của bạn.');
    }, 1000);
  }

  function requestVerification() {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      setIsLoading(false);
      setIsVerified(true);
      toast.success('Tài khoản của bạn đã được xác minh thành công.');
    }, 1500);
  }

  // Render icon theo loại người dùng
  const getUserTypeIcon = () => {
    switch (userType) {
      case 'buyer':
        return <Home className="h-4 w-4" />;
      case 'seller':
        return <Briefcase className="h-4 w-4" />;
      case 'developer':
        return <Building2 className="h-4 w-4" />;
      default:
        return <Users className="h-4 w-4" />;
    }
  };

  // Render nhãn theo loại người dùng
  const getUserTypeLabel = () => {
    switch (userType) {
      case 'buyer':
        return 'Người mua';
      case 'seller':
        return 'Người bán';
      case 'developer':
        return 'Chủ dự án';
      default:
        return 'Người dùng';
    }
  };

  return (
    <div className="container mx-auto py-8">
      <div className="relative mb-8 h-64 w-full overflow-hidden rounded-xl bg-gray-100">
        <Image
          src={backgroundSrc}
          alt="Cover"
          fill
          className="object-cover"
        />
        <Button
          onClick={() => backgroundInputRef.current?.click()}
          variant="secondary"
          size="icon"
          className="absolute right-4 bottom-4 rounded-full"
        >
          <Camera className="h-4 w-4" />
        </Button>
        <input
          ref={backgroundInputRef}
          type="file"
          accept="image/*"
          className="hidden"
          onChange={handleBackgroundChange}
        />
      </div>

      <div className="flex flex-col gap-8 md:flex-row">
        <div className="w-full md:w-1/4">
          <Card className="sticky top-4">
            <CardHeader className="pb-2">
              <CardTitle>Hồ sơ cá nhân</CardTitle>
              <CardDescription>Quản lý thông tin tài khoản của bạn</CardDescription>
            </CardHeader>
            <CardContent className="flex flex-col items-center pt-6">
              <div className="relative mb-6">
                <Avatar className="h-24 w-24">
                  <AvatarImage
                    src={avatarSrc}
                    alt="Avatar"
                  />
                  <AvatarFallback>NA</AvatarFallback>
                </Avatar>
                <Button
                  onClick={() => avatarInputRef.current?.click()}
                  variant="secondary"
                  size="icon"
                  className="absolute right-0 bottom-0 rounded-full"
                >
                  <Pencil className="h-4 w-4" />
                </Button>
                <input
                  ref={avatarInputRef}
                  type="file"
                  accept="image/*"
                  className="hidden"
                  onChange={handleAvatarChange}
                />
              </div>

              <h3 className="text-lg font-medium">Nguyễn Văn A</h3>
              <p className="text-sm text-gray-500">nguyenvana@example.com</p>

              <div className="mt-2 flex items-center gap-1">
                <Badge
                  variant="outline"
                  className="flex items-center gap-1"
                >
                  {getUserTypeIcon()}
                  <span>{getUserTypeLabel()}</span>
                </Badge>
                {isVerified && (
                  <Badge className="flex items-center gap-1 bg-green-500">
                    <CheckCircle className="h-3 w-3" />
                    <span>Đã xác minh</span>
                  </Badge>
                )}
              </div>

              <div className="mt-6 w-full">
                <Separator className="my-4" />
                <div className="flex items-center justify-between text-sm">
                  <span className="text-gray-500">ID người dùng:</span>
                  <span className="font-medium">#12345</span>
                </div>
                <div className="mt-2 flex items-center justify-between text-sm">
                  <span className="text-gray-500">Ngày tham gia:</span>
                  <span className="font-medium">15/06/2023</span>
                </div>
                {(userType === 'seller' || userType === 'developer') && (
                  <div className="mt-2 flex items-center justify-between text-sm">
                    <span className="text-gray-500">Đánh giá:</span>
                    <span className="flex items-center font-medium">
                      <Award className="mr-1 h-4 w-4 text-yellow-500" />
                      4.8/5
                    </span>
                  </div>
                )}
                <Separator className="my-4" />
              </div>
            </CardContent>
            <CardFooter>
              <Tabs className="flex w-full flex-col space-y-1">
                <TabsList>
                  <TabsTrigger
                    value="general"
                    className={`w-full justify-start ${activeTab === 'general' ? 'bg-accent' : ''}`}
                    onClick={() => setActiveTab('general')}
                  >
                    Thông tin chung
                  </TabsTrigger>

                  {(userType === 'seller' || userType === 'developer') && (
                    <TabsTrigger
                      value="business"
                      className={`w-full justify-start ${
                        activeTab === 'business' ? 'bg-accent' : ''
                      }`}
                      onClick={() => setActiveTab('business')}
                    >
                      Thông tin doanh nghiệp
                    </TabsTrigger>
                  )}

                  <TabsTrigger
                    value="password"
                    className={`w-full justify-start ${activeTab === 'password' ? 'bg-accent' : ''}`}
                    onClick={() => setActiveTab('password')}
                  >
                    Đổi mật khẩu
                  </TabsTrigger>

                  {!isVerified && (
                    <TabsTrigger
                      value="verification"
                      className={`w-full justify-start ${
                        activeTab === 'verification' ? 'bg-accent' : ''
                      }`}
                      onClick={() => setActiveTab('verification')}
                    >
                      Xác minh tài khoản
                    </TabsTrigger>
                  )}
                </TabsList>
              </Tabs>
            </CardFooter>
          </Card>
        </div>

        <div className="w-full md:w-3/4">
          <Tabs
            value={activeTab}
            onValueChange={setActiveTab}
          >
            <TabsContent value="general">
              <Card>
                <CardHeader>
                  <CardTitle>Thông tin chung</CardTitle>
                  <CardDescription>
                    Cập nhật thông tin cá nhân và loại tài khoản của bạn.
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <Form {...profileForm}>
                    <form
                      onSubmit={profileForm.handleSubmit(onProfileSubmit)}
                      className="space-y-6"
                    >
                      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                        <FormField
                          control={profileForm.control}
                          name="name"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Họ và tên</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Họ và tên"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={profileForm.control}
                          name="email"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Email</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Email"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={profileForm.control}
                          name="phone"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Số điện thoại</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Số điện thoại"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={profileForm.control}
                          name="address"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Địa chỉ</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Địa chỉ liên hệ"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                      </div>

                      <FormField
                        control={profileForm.control}
                        name="userType"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Loại tài khoản</FormLabel>
                            <Select
                              onValueChange={field.onChange}
                              defaultValue={field.value}
                            >
                              <FormControl>
                                <SelectTrigger>
                                  <SelectValue placeholder="Chọn loại tài khoản" />
                                </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                <SelectItem value="buyer">Người mua</SelectItem>
                                <SelectItem value="seller">Người bán</SelectItem>
                                <SelectItem value="developer">Chủ dự án</SelectItem>
                              </SelectContent>
                            </Select>
                            <FormMessage />
                          </FormItem>
                        )}
                      />

                      <FormField
                        control={profileForm.control}
                        name="bio"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Giới thiệu</FormLabel>
                            <FormControl>
                              <Textarea
                                placeholder="Viết một vài dòng về bạn"
                                className="resize-none"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <Button
                        type="submit"
                        disabled={isLoading}
                      >
                        {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                        Lưu thay đổi
                      </Button>
                    </form>
                  </Form>
                </CardContent>
              </Card>
            </TabsContent>

            <TabsContent value="business">
              <Card>
                <CardHeader>
                  <CardTitle>Thông tin doanh nghiệp</CardTitle>
                  <CardDescription>
                    Thông tin này sẽ được sử dụng cho mục đích kinh doanh và xác minh.
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <Form {...businessForm}>
                    <form
                      onSubmit={businessForm.handleSubmit(onBusinessSubmit)}
                      className="space-y-6"
                    >
                      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                        <FormField
                          control={businessForm.control}
                          name="companyName"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Tên công ty/doanh nghiệp</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Tên công ty/doanh nghiệp"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={businessForm.control}
                          name="taxId"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Mã số thuế</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Mã số thuế"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={businessForm.control}
                          name="website"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Website (nếu có)</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="https://example.com"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={businessForm.control}
                          name="establishedYear"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel>Năm thành lập</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="2020"
                                  {...field}
                                />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                      </div>

                      <FormField
                        control={businessForm.control}
                        name="businessType"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Loại hình kinh doanh</FormLabel>
                            <Select
                              onValueChange={field.onChange}
                              defaultValue={field.value}
                            >
                              <FormControl>
                                <SelectTrigger>
                                  <SelectValue placeholder="Chọn loại hình kinh doanh" />
                                </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                <SelectItem value="individual">Cá nhân kinh doanh</SelectItem>
                                <SelectItem value="company">Công ty TNHH</SelectItem>
                                <SelectItem value="jointStock">Công ty cổ phần</SelectItem>
                                <SelectItem value="agency">Đại lý môi giới</SelectItem>
                                <SelectItem value="developer">Chủ đầu tư</SelectItem>
                              </SelectContent>
                            </Select>
                            <FormMessage />
                          </FormItem>
                        )}
                      />

                      <div>
                        <FormLabel>Giấy phép kinh doanh</FormLabel>
                        <div className="mt-2 flex items-center">
                          <Button
                            type="button"
                            variant="outline"
                            onClick={() => licenseInputRef.current?.click()}
                          >
                            Tải lên giấy phép
                          </Button>
                          <input
                            ref={licenseInputRef}
                            type="file"
                            accept="image/*,.pdf"
                            className="hidden"
                            onChange={handleLicenseUpload}
                          />
                          <p className="ml-4 text-sm text-gray-500">
                            Chấp nhận file PDF, JPG, PNG (tối đa 5MB)
                          </p>
                        </div>
                      </div>

                      <Button
                        type="submit"
                        disabled={isLoading}
                      >
                        {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                        Lưu thông tin doanh nghiệp
                      </Button>
                    </form>
                  </Form>
                </CardContent>
              </Card>
            </TabsContent>

            <TabsContent value="password">
              <Card>
                <CardHeader>
                  <CardTitle>Đổi mật khẩu</CardTitle>
                  <CardDescription>Thay đổi mật khẩu đăng nhập của bạn.</CardDescription>
                </CardHeader>
                <CardContent>
                  <Form {...passwordForm}>
                    <form
                      onSubmit={passwordForm.handleSubmit(onPasswordSubmit)}
                      className="space-y-6"
                    >
                      <FormField
                        control={passwordForm.control}
                        name="currentPassword"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Mật khẩu hiện tại</FormLabel>
                            <FormControl>
                              <Input
                                type="password"
                                placeholder="••••••••"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={passwordForm.control}
                        name="newPassword"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Mật khẩu mới</FormLabel>
                            <FormControl>
                              <Input
                                type="password"
                                placeholder="••••••••"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={passwordForm.control}
                        name="confirmPassword"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Xác nhận mật khẩu mới</FormLabel>
                            <FormControl>
                              <Input
                                type="password"
                                placeholder="••••••••"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <div className="flex flex-col items-start justify-between gap-4 sm:flex-row sm:items-center">
                        <Button
                          type="submit"
                          disabled={isLoading}
                        >
                          {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                          Cập nhật mật khẩu
                        </Button>
                        <Button
                          variant="ghost"
                          type="button"
                          onClick={handleResetPassword}
                          disabled={isLoading}
                        >
                          Quên mật khẩu?
                        </Button>
                      </div>
                    </form>
                  </Form>
                </CardContent>
              </Card>
            </TabsContent>

            <TabsContent value="verification">
              <Card>
                <CardHeader>
                  <CardTitle>Xác minh tài khoản</CardTitle>
                  <CardDescription>
                    Xác minh tài khoản của bạn để nhận được nhiều ưu đãi và nâng cao uy tín.
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="space-y-6">
                    <div className="rounded-lg border border-yellow-100 bg-yellow-50 p-4">
                      <h3 className="text-sm font-medium text-yellow-800">
                        Tại sao phải xác minh tài khoản?
                      </h3>
                      <ul className="mt-2 space-y-2 text-sm text-yellow-700">
                        <li className="flex gap-2">
                          <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
                          <span>Tăng độ tin cậy và uy tín với khách hàng/đối tác</span>
                        </li>
                        <li className="flex gap-2">
                          <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
                          <span>Được ưu tiên hiển thị trong kết quả tìm kiếm</span>
                        </li>
                        <li className="flex gap-2">
                          <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
                          <span>Mở khóa các tính năng nâng cao dành cho tài khoản đã xác minh</span>
                        </li>
                        <li className="flex gap-2">
                          <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
                          <span>Tăng cơ hội giao dịch thành công</span>
                        </li>
                      </ul>
                    </div>

                    <div className="space-y-4">
                      <div>
                        <h3 className="text-base font-medium">
                          Để xác minh tài khoản, vui lòng cung cấp:
                        </h3>
                        <ul className="mt-2 space-y-2 text-sm">
                          <li className="flex items-center gap-2">
                            <div className="flex h-5 w-5 items-center justify-center rounded-full bg-gray-200 text-xs">
                              1
                            </div>
                            <span>CMND/CCCD/Hộ chiếu còn hiệu lực</span>
                          </li>
                          <li className="flex items-center gap-2">
                            <div className="flex h-5 w-5 items-center justify-center rounded-full bg-gray-200 text-xs">
                              2
                            </div>
                            <span>Ảnh chân dung rõ nét</span>
                          </li>
                          {(userType === 'seller' || userType === 'developer') && (
                            <li className="flex items-center gap-2">
                              <div className="flex h-5 w-5 items-center justify-center rounded-full bg-gray-200 text-xs">
                                3
                              </div>
                              <span>Giấy phép kinh doanh hoặc giấy tờ pháp lý liên quan</span>
                            </li>
                          )}
                        </ul>
                      </div>

                      <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                        <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
                          <Camera className="mb-2 h-8 w-8 text-gray-400" />
                          <p className="text-center text-sm text-gray-500">
                            Tải lên mặt trước CMND/CCCD
                          </p>
                          <Button
                            variant="outline"
                            size="sm"
                            className="mt-2"
                          >
                            Chọn file
                          </Button>
                        </div>

                        <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
                          <Camera className="mb-2 h-8 w-8 text-gray-400" />
                          <p className="text-center text-sm text-gray-500">
                            Tải lên mặt sau CMND/CCCD
                          </p>
                          <Button
                            variant="outline"
                            size="sm"
                            className="mt-2"
                          >
                            Chọn file
                          </Button>
                        </div>
                      </div>

                      <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
                        <Camera className="mb-2 h-8 w-8 text-gray-400" />
                        <p className="text-center text-sm text-gray-500">
                          Tải lên ảnh chân dung rõ nét
                        </p>
                        <p className="mt-1 text-center text-xs text-gray-400">
                          Chụp ảnh chân dung với góc nhìn thẳng, ánh sáng rõ ràng
                        </p>
                        <Button
                          variant="outline"
                          size="sm"
                          className="mt-2"
                        >
                          Chọn file
                        </Button>
                      </div>

                      {(userType === 'seller' || userType === 'developer') && (
                        <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
                          <Camera className="mb-2 h-8 w-8 text-gray-400" />
                          <p className="text-center text-sm text-gray-500">
                            Tải lên giấy phép kinh doanh/giấy tờ pháp lý
                          </p>
                          <Button
                            variant="outline"
                            size="sm"
                            className="mt-2"
                          >
                            Chọn file
                          </Button>
                        </div>
                      )}
                    </div>

                    <div className="rounded-lg border border-blue-100 bg-blue-50 p-4">
                      <h3 className="text-sm font-medium text-blue-800">Lưu ý quan trọng:</h3>
                      <ul className="mt-2 space-y-1 text-sm text-blue-700">
                        <li>
                          • Thông tin của bạn sẽ được bảo mật và chỉ sử dụng cho mục đích xác minh
                        </li>
                        <li>• Quy trình xác minh thường mất 1-3 ngày làm việc</li>
                        <li>• Bạn sẽ nhận được thông báo qua email khi hoàn tất xác minh</li>
                      </ul>
                    </div>

                    <Button
                      className="w-full"
                      onClick={requestVerification}
                      disabled={isLoading}
                    >
                      {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                      Gửi yêu cầu xác minh
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>
          </Tabs>
        </div>
      </div>
    </div>
  );
}
