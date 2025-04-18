// pages/profile/index.tsx
'use client';

import { useState, useRef } from 'react';
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
import { Pencil, Camera, Loader2 } from 'lucide-react';
import Image from 'next/image';
import { toast } from 'sonner';

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
});

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
  const avatarInputRef = useRef<HTMLInputElement>(null);
  const backgroundInputRef = useRef<HTMLInputElement>(null);

  const profileForm = useForm<z.infer<typeof profileFormSchema>>({
    resolver: zodResolver(profileFormSchema),
    defaultValues: {
      name: 'Nguyễn Văn A',
      email: 'nguyenvana@example.com',
      phone: '0912345678',
      bio: 'Chuyên gia tư vấn bất động sản với hơn 5 năm kinh nghiệm.',
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

  function onProfileSubmit(values: z.infer<typeof profileFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast('Thông tin đã được cập nhật', {
        description: 'Thông tin cá nhân của bạn đã được lưu thành công.',
      });
    }, 1000);
  }

  function onPasswordSubmit(values: z.infer<typeof passwordFormSchema>) {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      console.log(values);
      setIsLoading(false);
      toast('Mật khẩu đã được cập nhật', {
        description: 'Mật khẩu của bạn đã được thay đổi thành công.',
      });
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
          toast('Cập nhật avatar', {
            description: 'Avatar của bạn đã được cập nhật thành công.',
          });
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
          toast('Cập nhật ảnh bìa', {
            description: 'Ảnh bìa của bạn đã được cập nhật thành công.',
          });
        }
      };
      reader.readAsDataURL(file);
    }
  }

  function handleResetPassword() {
    setIsLoading(true);
    // Giả lập API call
    setTimeout(() => {
      setIsLoading(false);
      toast('Yêu cầu đặt lại mật khẩu', {
        description: 'Email với hướng dẫn đặt lại mật khẩu đã được gửi đến email của bạn.',
      });
    }, 1000);
  }

  return (
    <div className="container mx-auto py-8">
      <div className="relative mb-6 h-64 w-full overflow-hidden rounded-xl bg-gray-100">
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
          <Card>
            <CardHeader className="pb-2">
              <CardTitle>Hồ sơ cá nhân</CardTitle>
              <CardDescription>Quản lý thông tin cá nhân của bạn</CardDescription>
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
              <p className="text-sm text-gray-500">ID: #12345</p>
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
                  <TabsTrigger
                    value="password"
                    className={`w-full justify-start ${activeTab === 'password' ? 'bg-accent' : ''}`}
                    onClick={() => setActiveTab('password')}
                  >
                    Đổi mật khẩu
                  </TabsTrigger>
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
                  <CardDescription>Thông tin này sẽ được hiển thị công khai.</CardDescription>
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
                      </div>
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
          </Tabs>
        </div>
      </div>
    </div>
  );
}
