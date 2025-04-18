'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { Button } from '@/components/ui/button';
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuList,
  NavigationMenuTrigger,
} from '@/components/ui/navigation-menu';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { cn } from '@/lib/utils';
import { useState, useEffect, JSX } from 'react';

// Define user interface
interface User {
  id: string;
  name: string;
  email: string;
  avatar?: string;
  phone?: string;
  role: 'user' | 'agent' | 'admin';
  createdAt: string;
  updatedAt: string;
}
const regularUser: User = {
  id: '1234567890',
  name: 'Nguyễn Văn A',
  email: 'nguyenvana@example.com',
  avatar: '/api/placeholder/150/150',
  phone: '0912345678',
  role: 'user',
  createdAt: '2023-12-01T08:30:00Z',
  updatedAt: '2024-04-15T14:22:00Z',
};

export function Header(): JSX.Element {
  const pathname = usePathname();
  // State với proper typing
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(true);
  const [user, setUser] = useState<User | null>(regularUser);

  // Effect để kiểm tra trạng thái đăng nhập
  useEffect(() => {
    const checkAuth = (): void => {
      try {
        const storedUser = localStorage.getItem('user');
        if (storedUser) {
          const parsedUser: User = JSON.parse(storedUser);
          setIsLoggedIn(true);
          setUser(parsedUser);
        }
      } catch (error) {
        console.error('Error checking authentication:', error);
        // Đảm bảo xử lý lỗi trong strict mode
        localStorage.removeItem('user');
        setIsLoggedIn(false);
        setUser(null);
      }
    };

    checkAuth();
  }, []);

  const handleLogout = (): void => {
    localStorage.removeItem('user');
    setIsLoggedIn(false);
    setUser(null);
    // Chuyển hướng đến trang đăng nhập hoặc trang chủ
    window.location.href = '/';
  };

  return (
    <header className="bg-background/95 sticky top-0 z-50 w-full border-b px-6 backdrop-blur">
      <div className="container flex h-16 items-center justify-between">
        <Link
          href="/"
          className="flex items-center space-x-2"
        >
          <span className="text-2xl font-bold">Estate Elite</span>
        </Link>

        <NavigationMenu>
          <NavigationMenuList>
            <NavigationMenuItem>
              <Link
                href="/properties"
                className={cn(
                  'group bg-background hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground data-[active]:bg-accent/50 data-[state=open]:bg-accent/50 inline-flex h-10 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors focus:outline-none disabled:pointer-events-none disabled:opacity-50',
                  pathname.startsWith('/properties') ? 'bg-accent/50' : ''
                )}
              >
                Bất động sản
              </Link>
            </NavigationMenuItem>

            <NavigationMenuItem>
              <NavigationMenuTrigger>Dịch vụ</NavigationMenuTrigger>
              <NavigationMenuContent>
                <ul className="grid w-[200px] gap-3">
                  <li>
                    <Link
                      href="/services/buy"
                      className="hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground block space-y-1 rounded-md p-3 leading-none no-underline transition-colors outline-none select-none"
                    >
                      <div className="text-sm font-medium">Mua bất động sản</div>
                    </Link>
                  </li>
                  <li>
                    <Link
                      href="/services/rent"
                      className="hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground block space-y-1 rounded-md p-3 leading-none no-underline transition-colors outline-none select-none"
                    >
                      <div className="text-sm font-medium">Thuê bất động sản</div>
                    </Link>
                  </li>
                </ul>
              </NavigationMenuContent>
            </NavigationMenuItem>

            <NavigationMenuItem>
              <Link
                href="/about"
                className={cn(
                  'group bg-background hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground data-[active]:bg-accent/50 data-[state=open]:bg-accent/50 inline-flex h-10 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors focus:outline-none disabled:pointer-events-none disabled:opacity-50',
                  pathname === '/about' ? 'bg-accent/50' : ''
                )}
              >
                Giới thiệu
              </Link>
            </NavigationMenuItem>
          </NavigationMenuList>
        </NavigationMenu>

        <div className="flex items-center gap-4">
          {isLoggedIn && user ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <div className="flex cursor-pointer items-center gap-2">
                  <span className="hidden text-sm font-medium sm:inline-block">{user.name}</span>
                  <Avatar className="h-8 w-8">
                    <AvatarImage
                      src={user.avatar || '/api/placeholder/32/32'}
                      alt="Avatar"
                    />
                    <AvatarFallback>{user.name.charAt(0) || 'U'}</AvatarFallback>
                  </Avatar>
                </div>
              </DropdownMenuTrigger>
              <DropdownMenuContent
                align="end"
                className="w-56"
              >
                <DropdownMenuItem asChild>
                  <Link href="/profile">Hồ sơ cá nhân</Link>
                </DropdownMenuItem>
                <DropdownMenuItem asChild>
                  <Link href="/favorites">Bất động sản yêu thích</Link>
                </DropdownMenuItem>
                <DropdownMenuItem asChild>
                  <Link href="/my-properties">Bất động sản của tôi</Link>
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem onClick={handleLogout}>Đăng xuất</DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <>
              <Link href="/login">
                <Button variant="outline">Đăng nhập</Button>
              </Link>
              <Link href="/register">
                <Button>Đăng ký</Button>
              </Link>
            </>
          )}
        </div>
      </div>
    </header>
  );
}
