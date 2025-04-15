'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Home, Building2, Users, MessageSquare, BarChart3, Settings, LogOut } from 'lucide-react';

const navItems = [
  {
    title: 'Tổng quan',
    href: '/dashboard',
    icon: Home,
  },
  {
    title: 'Bất động sản',
    href: '/dashboard/properties',
    icon: Building2,
  },
  {
    title: 'Người dùng',
    href: '/dashboard/users',
    icon: Users,
  },
  {
    title: 'Tin nhắn',
    href: '/dashboard/messages',
    icon: MessageSquare,
  },
  {
    title: 'Thống kê',
    href: '/dashboard/analytics',
    icon: BarChart3,
  },
  {
    title: 'Cài đặt',
    href: '/dashboard/settings',
    icon: Settings,
  },
];

export function Sidebar() {
  const pathname = usePathname();

  return (
    <div className="sticky top-0 flex h-screen w-64 flex-col border-r bg-gray-50">
      <div className="border-b p-6">
        <Link
          href="/"
          className="flex items-center"
        >
          <span className="text-xl font-bold">BatDongSan</span>
        </Link>
      </div>

      <div className="flex-1 space-y-1 px-4 py-6">
        <p className="mb-2 px-2 text-xs font-semibold text-gray-500">QUẢN LÝ</p>
        {navItems.map((item) => (
          <Link
            key={item.href}
            href={item.href}
          >
            <Button
              variant="ghost"
              className={cn('w-full justify-start', pathname === item.href && 'bg-gray-200')}
            >
              <item.icon className="mr-2 h-4 w-4" />
              {item.title}
            </Button>
          </Link>
        ))}
      </div>

      <div className="border-t p-4">
        <Button
          variant="ghost"
          className="w-full justify-start text-red-500"
        >
          <LogOut className="mr-2 h-4 w-4" />
          Đăng xuất
        </Button>
      </div>
    </div>
  );
}
