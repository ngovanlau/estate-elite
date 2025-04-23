import {
  Building,
  Users,
  Calendar,
  DollarSign,
  ThumbsUp,
  FileText,
  Plus,
  LucideIcon,
} from 'lucide-react';

export interface Property {
  id: string;
  title: string;
  price: number;
  address: string;
  type: string;
  bedrooms?: number;
  bathrooms?: number;
  area: number;
  status: 'active' | 'pending' | 'sold';
  image: string;
}

export interface Activity {
  id: number;
  user: string;
  action: string;
  time: string;
  avatar: string;
}

export interface Stat {
  title: string;
  value: string;
  icon: LucideIcon;
  change: string;
  trend: 'up' | 'down';
}

export interface NavItem {
  title: string;
  icon: LucideIcon;
  href: string;
  variant: 'default' | 'ghost';
  notification?: number;
}

export interface QuickAction {
  title: string;
  icon: LucideIcon;
  variant: 'default' | 'outline';
}

export interface Appointment {
  id: number;
  title: string;
  description: string;
  day: string;
  time: string;
}

// Property listings data
export const propertyListings: Property[] = [
  {
    id: 'qewesadaasdasdas',
    title: 'Luxury Villa in District 2',
    price: 1200000,
    address: '123 Thao Dien, District 2, Ho Chi Minh City',
    type: 'For Sale',
    bedrooms: 4,
    bathrooms: 3,
    area: 250,
    status: 'active',
    image: '/api/placeholder/600/400',
  },
  {
    id: 'sdsqewesadaasdasdas',
    title: 'Modern Apartment in City Center',
    price: 2500,
    address: '456 Le Loi, District 1, Ho Chi Minh City',
    type: 'For Rent',
    bedrooms: 2,
    bathrooms: 2,
    area: 85,
    status: 'pending',
    image: '/api/placeholder/600/400',
  },
  {
    id: 'sdassdasdasdasdasdas',
    title: 'Commercial Space on Main Street',
    price: 4200,
    address: '789 Nguyen Hue, District 1, Ho Chi Minh City',
    type: 'For Rent',
    area: 128,
    status: 'active',
    image: '/api/placeholder/600/400',
  },
  {
    id: 'asdasdasdsdasasdsadsadxc',
    title: 'Townhouse with Garden',
    price: 750000,
    address: '101 An Phu, District 2, Ho Chi Minh City',
    type: 'For Sale',
    bedrooms: 3,
    bathrooms: 2,
    area: 180,
    status: 'sold',
    image: '/api/placeholder/600/400',
  },
];

// Recent activities data
export const recentActivities: Activity[] = [
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
export const stats: Stat[] = [
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

// Navigation items - keeping for reference
export const sidebarNavItems: NavItem[] = [
  {
    title: 'Dashboard',
    icon: Building, // Replaced with Building as a placeholder
    href: '/dashboard',
    variant: 'default',
  },
  {
    title: 'Properties',
    icon: Building,
    href: '/dashboard/properties',
    variant: 'ghost',
  },
  // Add more items as needed
];

// Quick actions
export const quickActions: QuickAction[] = [
  {
    title: 'Add New Property',
    icon: Plus,
    variant: 'default',
  },
  {
    title: 'Manage Clients',
    icon: Users,
    variant: 'outline',
  },
  {
    title: 'Schedule Viewings',
    icon: Calendar,
    variant: 'outline',
  },
  {
    title: 'Generate Reports',
    icon: FileText,
    variant: 'outline',
  },
];

// Upcoming appointments
export const upcomingAppointments: Appointment[] = [
  {
    id: 1,
    title: 'Property Viewing',
    description: 'Luxury Villa in District 2',
    day: 'Today',
    time: '2:00 PM',
  },
  {
    id: 2,
    title: 'Client Meeting',
    description: 'John Smith - Potential Buyer',
    day: 'Tomorrow',
    time: '10:30 AM',
  },
];
