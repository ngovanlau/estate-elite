'use client';

import * as React from 'react';
import { BookOpen, Bot, SquareTerminal } from 'lucide-react';

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail,
} from '@/components/ui/sidebar';
import { NavMain } from './nav-man';
import { NavUser } from './nav-user';
import Link from 'next/link';

// This is sample data.
const data = {
  navMain: [
    {
      title: 'Dashboard',
      url: '/dashboard',
      icon: SquareTerminal,
    },
    {
      title: 'Projects',
      url: '/dashboard/projects',
      icon: Bot,
    },
    {
      title: 'Properties',
      url: '/dashboard/properties',
      icon: BookOpen,
    },
  ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar
      collapsible="icon"
      {...props}
    >
      <SidebarHeader>
        <header className="border-b px-6 py-4">
          <Link
            href="/"
            className="flex items-center"
          >
            <span className="text-2xl font-bold">Estate Elite</span>
          </Link>
        </header>
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={data.navMain} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}
