// app/login/page.tsx
'use client';

import { useState } from 'react';
import Image from 'next/image';
import Link from 'next/link';
import { Eye, EyeOff, Mail, Lock } from 'lucide-react';
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
import Background from '@/public/images/background-login-register.jpg';
import styles from './styles.module.scss';
import identityService from '@/services/identity-service';

export default function LoginPage() {
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [rememberMe, setRememberMe] = useState<boolean>(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    // Handle login logic here
    console.log('Login attempt with:', { email, password, rememberMe });
    // For demonstration purposes
    // router.push("/dashboard");

    const response = await identityService.login({
      email,
      password,
    });

    console.log(response);
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

      {/* Right side - Login form */}
      <div className="flex w-full items-center justify-center p-6 md:w-1/2">
        <div className="w-full max-w-md">
          <Card className="w-full border-0 shadow-md">
            <form onSubmit={handleLogin}>
              <CardHeader>
                <CardTitle className="text-center text-2xl font-semibold">Đăng nhập</CardTitle>
                <CardDescription className="text-center">
                  Đăng nhập vào tài khoản của bạn để quản lý bất động sản
                </CardDescription>
              </CardHeader>

              <CardContent className="mt-4 space-y-4">
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

              <CardFooter className="mt-4 flex flex-col gap-4">
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
        </div>
      </div>
    </div>
  );
}
