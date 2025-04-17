'use client';

import { useState } from 'react';
import {
  Building,
  Home,
  LineChart,
  Users,
  Settings,
  Calendar,
  MessageSquare,
  DollarSign,
  Search,
  Bell,
  ChevronDown,
  ThumbsUp,
  Map,
  FileText,
  Plus,
  Maximize,
  Bath,
  Bed,
  DownloadIcon,
} from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';
import Image from 'next/image';

// Navigation Items for the sidebar
const sidebarNavItems = [
  {
    title: 'Dashboard',
    icon: Home,
    href: '/dashboard',
    variant: 'default',
  },
  {
    title: 'Properties',
    icon: Building,
    href: '/dashboard/properties',
    variant: 'ghost',
  },
  {
    title: 'Analytics',
    icon: LineChart,
    href: '/dashboard/analytics',
    variant: 'ghost',
  },
  {
    title: 'Clients',
    icon: Users,
    href: '/dashboard/clients',
    variant: 'ghost',
  },
  {
    title: 'Calendar',
    icon: Calendar,
    href: '/dashboard/calendar',
    variant: 'ghost',
  },
  {
    title: 'Messages',
    icon: MessageSquare,
    href: '/dashboard/messages',
    variant: 'ghost',
    notification: 5,
  },
  {
    title: 'Map View',
    icon: Map,
    href: '/dashboard/map',
    variant: 'ghost',
  },
  {
    title: 'Settings',
    icon: Settings,
    href: '/dashboard/settings',
    variant: 'ghost',
  },
];

// Property listings data
const propertyListings = [
  {
    id: 1,
    title: 'Luxury Villa in District 2',
    price: '$1,200,000',
    address: '123 Thao Dien, District 2, Ho Chi Minh City',
    type: 'For Sale',
    bedrooms: 4,
    bathrooms: 3,
    area: '250m²',
    status: 'active',
    image: '/api/placeholder/600/400',
  },
  {
    id: 2,
    title: 'Modern Apartment in City Center',
    price: '$2,500/month',
    address: '456 Le Loi, District 1, Ho Chi Minh City',
    type: 'For Rent',
    bedrooms: 2,
    bathrooms: 2,
    area: '85m²',
    status: 'pending',
    image: '/api/placeholder/600/400',
  },
  {
    id: 3,
    title: 'Commercial Space on Main Street',
    price: '$4,200/month',
    address: '789 Nguyen Hue, District 1, Ho Chi Minh City',
    type: 'For Rent',
    area: '120m²',
    status: 'active',
    image: '/api/placeholder/600/400',
  },
  {
    id: 4,
    title: 'Townhouse with Garden',
    price: '$750,000',
    address: '101 An Phu, District 2, Ho Chi Minh City',
    type: 'For Sale',
    bedrooms: 3,
    bathrooms: 2,
    area: '180m²',
    status: 'sold',
    image: '/api/placeholder/600/400',
  },
];

// Recent activities data
const recentActivities = [
  {
    id: 1,
    user: 'John Doe',
    action: 'added a new property listing',
    time: '2 hours ago',
    avatar: 'JD',
  },
  {
    id: 2,
    user: 'Mary Smith',
    action: 'scheduled a viewing for Luxury Villa',
    time: '5 hours ago',
    avatar: 'MS',
  },
  {
    id: 3,
    user: 'Robert Johnson',
    action: 'left a review (4.8/5)',
    time: 'Yesterday',
    avatar: 'RJ',
  },
  {
    id: 4,
    user: 'Lisa Wang',
    action: 'updated Modern Apartment details',
    time: '2 days ago',
    avatar: 'LW',
  },
];

// Stats data
const stats = [
  {
    title: 'Total Properties',
    value: '143',
    icon: Building,
    change: '+12%',
    trend: 'up',
  },
  {
    title: 'Total Clients',
    value: '2,845',
    icon: Users,
    change: '+5.2%',
    trend: 'up',
  },
  {
    title: 'Total Revenue',
    value: '$1.2M',
    icon: DollarSign,
    change: '+18.3%',
    trend: 'up',
  },
  {
    title: 'View Rate',
    value: '78%',
    icon: ThumbsUp,
    change: '-2.5%',
    trend: 'down',
  },
];

export default function DashboardPage() {
  const [isMobileNavOpen, setIsMobileNavOpen] = useState(false);

  return (
    <div className="flex min-h-screen bg-gray-50 dark:bg-gray-900">
      {/* Sidebar for desktop */}
      <aside className="hidden w-64 flex-col border-r border-gray-200 bg-white md:flex dark:border-gray-700 dark:bg-gray-800">
        <div className="p-6">
          <h2 className="text-2xl font-bold text-gray-800 dark:text-white">RealEstate Pro</h2>
          <p className="text-gray-500 dark:text-gray-400">Management Dashboard</p>
        </div>
        <nav className="flex-1 space-y-1 px-3 py-2">
          {sidebarNavItems.map((item) => (
            <Button
              key={item.href}
              variant="default"
              className="h-10 w-full justify-start gap-3 text-base font-normal"
            >
              <item.icon className="h-5 w-5" />
              {item.title}
              {item.notification && (
                <Badge
                  className="ml-auto"
                  variant="destructive"
                >
                  {item.notification}
                </Badge>
              )}
            </Button>
          ))}
        </nav>
        <div className="border-t border-gray-200 p-4 dark:border-gray-700">
          <div className="flex items-center gap-3">
            <Avatar className="h-10 w-10">
              <AvatarImage
                src="/api/placeholder/40/40"
                alt="User"
              />
              <AvatarFallback>AD</AvatarFallback>
            </Avatar>
            <div className="flex flex-col">
              <span className="text-sm font-medium dark:text-white">Alex Dinh</span>
              <span className="text-xs text-gray-500 dark:text-gray-400">Admin</span>
            </div>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button
                  variant="ghost"
                  size="icon"
                  className="ml-auto"
                >
                  <ChevronDown className="h-4 w-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>My Account</DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem>Profile</DropdownMenuItem>
                <DropdownMenuItem>Preferences</DropdownMenuItem>
                <DropdownMenuItem>Sign out</DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </div>
      </aside>

      {/* Mobile sidebar */}
      <Sheet
        open={isMobileNavOpen}
        onOpenChange={setIsMobileNavOpen}
      >
        <SheetContent
          side="left"
          className="w-64 p-0"
        >
          <SheetHeader className="border-b border-gray-200 p-6 dark:border-gray-700">
            <SheetTitle className="text-2xl">RealEstate Pro</SheetTitle>
            <SheetDescription>Management Dashboard</SheetDescription>
          </SheetHeader>
          <div className="py-4">
            <nav className="flex-1 space-y-1 px-3 py-2">
              {sidebarNavItems.map((item) => (
                <SheetClose
                  asChild
                  key={item.href}
                >
                  <Button
                    variant="default"
                    className="h-10 w-full justify-start gap-3 text-base font-normal"
                    onClick={() => setIsMobileNavOpen(false)}
                  >
                    <item.icon className="h-5 w-5" />
                    {item.title}
                    {item.notification && (
                      <Badge
                        className="ml-auto"
                        variant="destructive"
                      >
                        {item.notification}
                      </Badge>
                    )}
                  </Button>
                </SheetClose>
              ))}
            </nav>
          </div>
          <SheetFooter className="border-t border-gray-200 p-4 dark:border-gray-700">
            <div className="flex items-center gap-3">
              <Avatar className="h-10 w-10">
                <AvatarImage
                  src="/api/placeholder/40/40"
                  alt="User"
                />
                <AvatarFallback>AD</AvatarFallback>
              </Avatar>
              <div className="flex flex-col">
                <span className="text-sm font-medium dark:text-white">Alex Dinh</span>
                <span className="text-xs text-gray-500 dark:text-gray-400">Admin</span>
              </div>
            </div>
          </SheetFooter>
        </SheetContent>
      </Sheet>

      {/* Main content */}
      <div className="flex-1">
        {/* Header */}
        <header className="sticky top-0 z-10 border-b border-gray-200 bg-white dark:border-gray-700 dark:bg-gray-800">
          <div className="flex h-16 items-center justify-between px-4 md:px-6">
            <Button
              variant="ghost"
              size="icon"
              className="md:hidden"
              onClick={() => setIsMobileNavOpen(true)}
            >
              <Building className="h-6 w-6" />
              <span className="sr-only">Toggle menu</span>
            </Button>
            <div className="relative ml-auto md:ml-0 md:max-w-md md:flex-1">
              <Search className="absolute top-2.5 left-2.5 h-4 w-4 text-gray-500 dark:text-gray-400" />
              <Input
                type="search"
                placeholder="Search properties, clients..."
                className="pl-8 md:max-w-md"
              />
            </div>
            <div className="ml-2 flex items-center gap-2">
              <Button
                variant="ghost"
                size="icon"
              >
                <Bell className="h-5 w-5" />
              </Button>
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button
                    variant="ghost"
                    size="icon"
                    className="rounded-full md:hidden"
                  >
                    <Avatar className="h-8 w-8">
                      <AvatarImage
                        src="/api/placeholder/32/32"
                        alt="User"
                      />
                      <AvatarFallback>AD</AvatarFallback>
                    </Avatar>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <DropdownMenuLabel>My Account</DropdownMenuLabel>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>Profile</DropdownMenuItem>
                  <DropdownMenuItem>Preferences</DropdownMenuItem>
                  <DropdownMenuItem>Sign out</DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </div>
          </div>
        </header>

        {/* Dashboard content */}
        <main className="p-4 md:p-6">
          {/* Page title */}
          <div className="mb-6 flex items-center justify-between">
            <div>
              <h1 className="text-2xl font-bold text-gray-800 md:text-3xl dark:text-white">
                Dashboard
              </h1>
              <p className="text-gray-500 dark:text-gray-400">
                Welcome back, Alex! Here is what is happening today.
              </p>
            </div>
            <div className="flex gap-2">
              <Button className="hidden md:flex">
                <Plus className="mr-2 h-4 w-4" />
                Add Property
              </Button>
              <Button variant="outline">
                <DownloadIcon className="mr-2 h-4 w-4" />
                Export
              </Button>
            </div>
          </div>

          {/* Stats cards */}
          <div className="mb-6 grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-4">
            {stats.map((stat, index) => (
              <Card key={index}>
                <CardHeader className="flex flex-row items-center justify-between pb-2">
                  <CardTitle className="text-sm font-medium text-gray-500 dark:text-gray-400">
                    {stat.title}
                  </CardTitle>
                  <stat.icon className="h-4 w-4 text-gray-500 dark:text-gray-400" />
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">{stat.value}</div>
                  <p
                    className={`text-xs ${stat.trend === 'up' ? 'text-green-500' : 'text-red-500'}`}
                  >
                    {stat.change} from last month
                  </p>
                </CardContent>
              </Card>
            ))}
          </div>

          {/* Main content area */}
          <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
            {/* Property listings */}
            <div className="lg:col-span-2">
              <Card>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <CardTitle>Property Listings</CardTitle>
                    <Tabs defaultValue="all">
                      <TabsList className="grid w-full grid-cols-4">
                        <TabsTrigger value="all">All</TabsTrigger>
                        <TabsTrigger value="sale">For Sale</TabsTrigger>
                        <TabsTrigger value="rent">For Rent</TabsTrigger>
                        <TabsTrigger value="pending">Pending</TabsTrigger>
                      </TabsList>
                    </Tabs>
                  </div>
                </CardHeader>
                <CardContent>
                  <div className="space-y-4">
                    {propertyListings.map((property) => (
                      <div
                        key={property.id}
                        className="flex flex-col gap-4 border-b border-gray-200 pb-4 md:flex-row dark:border-gray-700"
                      >
                        <div className="md:w-1/3">
                          <Image
                            src={property.image}
                            alt={property.title}
                            className="h-40 w-full rounded-md object-cover"
                            width={600}
                            height={400}
                          />
                        </div>
                        <div className="md:w-2/3">
                          <div className="flex items-center justify-between">
                            <h3 className="text-lg font-semibold">{property.title}</h3>
                            <Badge
                              variant={
                                property.status === 'active'
                                  ? 'default'
                                  : property.status === 'pending'
                                    ? 'outline'
                                    : 'secondary'
                              }
                            >
                              {property.status}
                            </Badge>
                          </div>
                          <div className="mt-1 flex items-center">
                            <Badge
                              variant="outline"
                              className="mr-2"
                            >
                              {property.type}
                            </Badge>
                            <p className="text-lg font-bold text-blue-600 dark:text-blue-400">
                              {property.price}
                            </p>
                          </div>
                          <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                            {property.address}
                          </p>
                          <div className="mt-2 flex items-center gap-3 text-sm">
                            {property.bedrooms && (
                              <div className="flex items-center gap-1">
                                <Bed className="h-4 w-4" />
                                <span>{property.bedrooms} beds</span>
                              </div>
                            )}
                            {property.bathrooms && (
                              <div className="flex items-center gap-1">
                                <Bath className="h-4 w-4" />
                                <span>{property.bathrooms} baths</span>
                              </div>
                            )}
                            <div className="flex items-center gap-1">
                              <Maximize className="h-4 w-4" />
                              <span>{property.area}</span>
                            </div>
                          </div>
                          <div className="mt-3 flex gap-2">
                            <Button
                              size="sm"
                              variant="outline"
                            >
                              View Details
                            </Button>
                            <Button size="sm">Contact Agent</Button>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                  <div className="mt-4 flex justify-center">
                    <Button variant="outline">Load More</Button>
                  </div>
                </CardContent>
              </Card>
            </div>

            {/* Activity Feed and Quick Actions */}
            <div className="space-y-6">
              {/* Recent Activities */}
              <Card>
                <CardHeader>
                  <CardTitle>Recent Activities</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-4">
                    {recentActivities.map((activity) => (
                      <div
                        key={activity.id}
                        className="flex items-start gap-3"
                      >
                        <Avatar className="h-8 w-8">
                          <AvatarFallback>{activity.avatar}</AvatarFallback>
                        </Avatar>
                        <div>
                          <p className="text-sm">
                            <span className="font-medium">{activity.user}</span> {activity.action}
                          </p>
                          <p className="text-xs text-gray-500 dark:text-gray-400">
                            {activity.time}
                          </p>
                        </div>
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>

              {/* Quick Actions */}
              <Card>
                <CardHeader>
                  <CardTitle>Quick Actions</CardTitle>
                </CardHeader>
                <CardContent className="space-y-2">
                  <Button className="w-full justify-start">
                    <Plus className="mr-2 h-4 w-4" />
                    Add New Property
                  </Button>
                  <Button
                    variant="outline"
                    className="w-full justify-start"
                  >
                    <Users className="mr-2 h-4 w-4" />
                    Manage Clients
                  </Button>
                  <Button
                    variant="outline"
                    className="w-full justify-start"
                  >
                    <Calendar className="mr-2 h-4 w-4" />
                    Schedule Viewings
                  </Button>
                  <Button
                    variant="outline"
                    className="w-full justify-start"
                  >
                    <FileText className="mr-2 h-4 w-4" />
                    Generate Reports
                  </Button>
                </CardContent>
              </Card>

              {/* Upcoming Appointments */}
              <Card>
                <CardHeader>
                  <CardTitle>Upcoming Appointments</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    <div className="flex items-center justify-between rounded-md bg-gray-50 p-3 dark:bg-gray-800">
                      <div>
                        <p className="font-medium">Property Viewing</p>
                        <p className="text-sm text-gray-500 dark:text-gray-400">
                          Luxury Villa in District 2
                        </p>
                      </div>
                      <div className="text-right">
                        <p className="text-sm font-medium">Today</p>
                        <p className="text-sm text-gray-500 dark:text-gray-400">2:00 PM</p>
                      </div>
                    </div>
                    <div className="flex items-center justify-between rounded-md bg-gray-50 p-3 dark:bg-gray-800">
                      <div>
                        <p className="font-medium">Client Meeting</p>
                        <p className="text-sm text-gray-500 dark:text-gray-400">
                          John Smith - Potential Buyer
                        </p>
                      </div>
                      <div className="text-right">
                        <p className="text-sm font-medium">Tomorrow</p>
                        <p className="text-sm text-gray-500 dark:text-gray-400">10:30 AM</p>
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
}
