'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { Button } from '@/components/ui/button';
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
} from '@/components/ui/navigation-menu';
import { cn } from '@/lib/utils';

export function Header() {
  const pathname = usePathname();

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
                legacyBehavior
                passHref
              >
                <NavigationMenuLink
                  className={cn(
                    'group bg-background hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground data-[active]:bg-accent/50 data-[state=open]:bg-accent/50 inline-flex h-10 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors focus:outline-none disabled:pointer-events-none disabled:opacity-50',
                    pathname.startsWith('/properties') ? 'bg-accent/50' : ''
                  )}
                >
                  Bất động sản
                </NavigationMenuLink>
              </Link>
            </NavigationMenuItem>

            <NavigationMenuItem>
              <NavigationMenuTrigger>Dịch vụ</NavigationMenuTrigger>
              <NavigationMenuContent>
                <ul className="grid w-[200px] gap-3">
                  <li>
                    <Link
                      href="/services/buy"
                      passHref
                    >
                      <NavigationMenuLink className="hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground block space-y-1 rounded-md p-3 leading-none no-underline transition-colors outline-none select-none">
                        <div className="text-sm font-medium">Mua bất động sản</div>
                      </NavigationMenuLink>
                    </Link>
                  </li>
                  <li>
                    <Link
                      href="/services/rent"
                      passHref
                    >
                      <NavigationMenuLink className="hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground block space-y-1 rounded-md p-3 leading-none no-underline transition-colors outline-none select-none">
                        <div className="text-sm font-medium">Thuê bất động sản</div>
                      </NavigationMenuLink>
                    </Link>
                  </li>
                </ul>
              </NavigationMenuContent>
            </NavigationMenuItem>

            <NavigationMenuItem>
              <Link
                href="/about"
                passHref
              >
                <NavigationMenuLink
                  className={cn(
                    'group bg-background hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground data-[active]:bg-accent/50 data-[state=open]:bg-accent/50 inline-flex h-10 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors focus:outline-none disabled:pointer-events-none disabled:opacity-50',
                    pathname === '/about' ? 'bg-accent/50' : ''
                  )}
                >
                  Giới thiệu
                </NavigationMenuLink>
              </Link>
            </NavigationMenuItem>
          </NavigationMenuList>
        </NavigationMenu>

        <div className="flex items-center gap-4">
          <Link href="/login">
            <Button variant="outline">Đăng nhập</Button>
          </Link>
          <Link href="/register">
            <Button>Đăng ký</Button>
          </Link>
        </div>
      </div>
    </header>
  );
}
