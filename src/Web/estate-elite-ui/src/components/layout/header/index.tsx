'use client';

import Link from 'next/link';
import { usePathname, useRouter } from 'next/navigation';
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
import { useAppDispatch, useAppSelector } from '@/lib/hooks';
import { logout, selectIsAuthenticated, selectUser } from '@/redux/slices/auth-slice';

export function Header() {
  const pathname = usePathname();
  const router = useRouter();
  const currentUser = useAppSelector(selectUser);
  const isAuthenticated = useAppSelector(selectIsAuthenticated);
  const dispatch = useAppDispatch();

  // Effect để kiểm tra trạng thái đăng nhập
  // useEffect(() => {
  //   const checkAuth = (): void => {
  //     try {
  //       const storedUser = localStorage.getItem('user');
  //       if (storedUser) {
  //         const parsedUser: User = JSON.parse(storedUser);
  //         setIsLoggedIn(true);
  //         setUser(parsedUser);
  //       }
  //     } catch (error) {
  //       console.error('Error checking authentication:', error);
  //       // Đảm bảo xử lý lỗi trong strict mode
  //       localStorage.removeItem('user');
  //       setIsLoggedIn(false);
  //       setUser(null);
  //     }
  //   };

  //   checkAuth();
  // }, []);

  const handleLogout = (): void => {
    dispatch(logout());
    router.push('/');
  };

  return (
    <header className="bg-background sticky top-0 z-50 w-full border-b px-6 backdrop-blur">
      <div className="flex h-16 w-full items-center justify-between">
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
          {isAuthenticated && currentUser ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <button
                  type="button"
                  className="flex cursor-pointer items-center gap-2"
                >
                  <span className="hidden text-sm font-medium sm:inline-block">
                    {currentUser?.fullName}
                  </span>
                  <Avatar className="h-8 w-8">
                    <AvatarImage
                      src={currentUser?.avatar || '/api/placeholder/32/32'}
                      alt="Avatar"
                    />
                    <AvatarFallback>{currentUser?.fullName.charAt(0) || 'U'}</AvatarFallback>
                  </Avatar>
                </button>
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
                  <Link href="/dashboard">Bảng điều khiển</Link>
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
