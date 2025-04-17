// app/register/page.tsx
'use client';

import { useState } from 'react';
import Link from 'next/link';
import { Eye, EyeOff, Mail, Lock, User, Phone } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import Image from 'next/image';
import Background from '@/public/images/background-login-register.jpg';
import styles from './styles.module.scss';
import { USER_ROLE } from '@/lib/enum';

export default function RegisterPage() {
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState<boolean>(false);

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle registration logic here
    console.log('Registration attempt');
    // router.push("/login");
  };

  return (
    <div className="flex min-h-screen flex-col md:flex-row">
      {/* Left side - Image */}
      <div className="relative hidden md:block md:w-1/2">
        <Image
          src={Background}
          alt="background"
          fill
          sizes="100vw"
          className={styles.image}
        />
      </div>

      {/* Right side - Registration form */}
      <div className="flex w-full items-center justify-center p-6 md:w-1/2">
        <div className="w-full max-w-fit">
          <Card className="w-full border-0 shadow-md">
            <form onSubmit={handleRegister}>
              <CardHeader>
                <CardTitle className="text-center text-2xl font-semibold">Đăng ký</CardTitle>
                <CardDescription className="text-center">
                  Tạo tài khoản mới để trải nghiệm dịch vụ bất động sản chuyên nghiệp
                </CardDescription>
              </CardHeader>

              <CardContent className="mt-4 space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="firstName">Họ và tên</Label>
                    <div className="relative">
                      <User className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                      <Input
                        id="fullName"
                        placeholder="Ngo Van Lau"
                        className="pl-10"
                        required
                      />
                    </div>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="lastName">Username</Label>
                    <Input
                      id="username"
                      placeholder="username"
                      required
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <div className="relative">
                    <Mail className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                    <Input
                      id="email"
                      type="email"
                      placeholder="name@example.com"
                      className="pl-10"
                      required
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="phone">Số điện thoại</Label>
                    <div className="relative">
                      <Phone className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                      <Input
                        id="phone"
                        type="tel"
                        placeholder="0912345678"
                        className="pl-10"
                        required
                      />
                    </div>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="role">Bạn là</Label>
                    <Select required>
                      <SelectTrigger className="w-full">
                        <SelectValue placeholder="Chọn vai trò của bạn" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value={USER_ROLE.BUYER}>Khách hàng tìm mua/thuê</SelectItem>
                        <SelectItem value={USER_ROLE.SELLER}>Chủ sở hữu bất động sản</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="password">Mật khẩu</Label>
                  <div className="relative">
                    <Lock className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                    <Input
                      id="password"
                      type={showPassword ? 'text' : 'password'}
                      placeholder="••••••••"
                      className="pr-10 pl-10"
                      required
                    />
                    <Button
                      type="button"
                      variant="ghost"
                      size="icon"
                      className="absolute top-0 right-0 h-full px-3 py-2"
                      onClick={() => setShowPassword(!showPassword)}
                    >
                      {showPassword ? (
                        <EyeOff className="h-4 w-4 text-gray-400" />
                      ) : (
                        <Eye className="h-4 w-4 text-gray-400" />
                      )}
                    </Button>
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="confirmPassword">Xác nhận mật khẩu</Label>
                  <div className="relative">
                    <Lock className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                    <Input
                      id="confirmPassword"
                      type={showConfirmPassword ? 'text' : 'password'}
                      placeholder="••••••••"
                      className="pr-10 pl-10"
                      required
                    />
                    <Button
                      type="button"
                      variant="ghost"
                      size="icon"
                      className="absolute top-0 right-0 h-full px-3 py-2"
                      onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                    >
                      {showConfirmPassword ? (
                        <EyeOff className="h-4 w-4 text-gray-400" />
                      ) : (
                        <Eye className="h-4 w-4 text-gray-400" />
                      )}
                    </Button>
                  </div>
                </div>
              </CardContent>

              <CardFooter className="mt-4 flex flex-col gap-4">
                <Button
                  type="submit"
                  className="w-full bg-blue-600 hover:bg-blue-700"
                >
                  Đăng ký
                </Button>

                <div className="text-center text-sm">
                  Đã có tài khoản?{' '}
                  <Link
                    href="/login"
                    className="font-medium text-blue-600 hover:text-blue-800"
                  >
                    Đăng nhập
                  </Link>
                </div>
              </CardFooter>
            </form>
          </Card>
        </div>
      </div>
    </div>
  );
}
