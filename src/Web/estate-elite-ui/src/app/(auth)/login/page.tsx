// app/login/page.tsx
'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import Image from 'next/image';
import Link from 'next/link';
import { Eye, EyeOff, Building, Mail, Lock } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Checkbox } from '@/components/ui/checkbox';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';

export default function LoginPage() {
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [rememberMe, setRememberMe] = useState<boolean>(false);
  const router = useRouter();

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle login logic here
    console.log('Login attempt with:', { email, password, rememberMe });
    // For demonstration purposes
    // router.push("/dashboard");
  };

  return (
    <div className="flex min-h-screen flex-col md:flex-row">
      {/* Left side - Image */}
      <div className="relative hidden bg-blue-600 md:block md:w-1/2">
        <div className="absolute inset-0 z-10 bg-gradient-to-br from-blue-600/90 to-blue-800/90"></div>
        <div className="absolute inset-0 z-20 flex items-center justify-center">
          <div className="space-y-6 px-10 text-white">
            <Building className="h-16 w-16" />
            <h2 className="text-3xl font-bold">RealEstate Pro</h2>
            <p className="text-xl">
              Nền tảng quản lý bất động sản hàng đầu cho doanh nghiệp của bạn
            </p>
            <div className="pt-6">
              <p className="font-medium">Quản lý dễ dàng:</p>
              <ul className="mt-2 list-inside list-disc space-y-2">
                <li>Đăng tin bán/cho thuê bất động sản</li>
                <li>Quản lý khách hàng và giao dịch</li>
                <li>Theo dõi hiệu quả hoạt động kinh doanh</li>
                <li>Báo cáo thống kê trực quan</li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      {/* Right side - Login form */}
      <div className="flex w-full items-center justify-center bg-gray-50 p-6 md:w-1/2">
        <div className="w-full max-w-md">
          <div className="mb-8 flex justify-center md:hidden">
            <div className="flex items-center gap-2">
              <Building className="h-8 w-8 text-blue-600" />
              <h1 className="text-2xl font-bold text-blue-600">RealEstate Pro</h1>
            </div>
          </div>

          <Card className="w-full border-0 shadow-md">
            <form onSubmit={handleLogin}>
              <CardHeader>
                <CardTitle className="text-center text-2xl font-semibold">Đăng nhập</CardTitle>
                <CardDescription className="text-center">
                  Đăng nhập vào tài khoản của bạn để quản lý bất động sản
                </CardDescription>
              </CardHeader>

              <CardContent className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <div className="relative">
                    <Mail className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                    <Input
                      id="email"
                      type="email"
                      placeholder="name@example.com"
                      className="pl-10"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      required
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <div className="flex items-center justify-between">
                    <Label htmlFor="password">Mật khẩu</Label>
                    <Link
                      href="/forgot-password"
                      className="text-xs font-medium text-blue-600 hover:text-blue-800"
                    >
                      Quên mật khẩu?
                    </Link>
                  </div>
                  <div className="relative">
                    <Lock className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                    <Input
                      id="password"
                      type={showPassword ? 'text' : 'password'}
                      placeholder="••••••••"
                      className="pr-10 pl-10"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
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

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="remember"
                    checked={rememberMe}
                    onCheckedChange={(checked) => setRememberMe(checked as boolean)}
                  />
                  <Label
                    htmlFor="remember"
                    className="text-sm"
                  >
                    Ghi nhớ đăng nhập
                  </Label>
                </div>
              </CardContent>

              <CardFooter className="flex flex-col gap-4">
                <Button
                  type="submit"
                  className="w-full bg-blue-600 hover:bg-blue-700"
                >
                  Đăng nhập
                </Button>

                <div className="text-center text-sm">
                  Chưa có tài khoản?{' '}
                  <Link
                    href="/register"
                    className="font-medium text-blue-600 hover:text-blue-800"
                  >
                    Đăng ký ngay
                  </Link>
                </div>
              </CardFooter>
            </form>
          </Card>

          <div className="mt-6 text-center text-sm text-gray-600">
            <Link
              href="/partner-login"
              className="text-blue-600 hover:text-blue-800"
            >
              Đăng nhập với tư cách đối tác
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}
