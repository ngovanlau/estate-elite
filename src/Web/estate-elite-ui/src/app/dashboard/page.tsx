'use client';

import { Plus, DownloadIcon } from 'lucide-react';
import { Button } from '@/components/ui/button';

// Data imports
import Header from './_components/header';
import StatsCards from './_components/stats-cards';
import PropertyListings from './_components/property-listings';
import ActivityFeed from './_components/activity-feed';
import QuickActions from './_components/actions';
import UpcomingAppointments from './_components/upcoming-appointments';
import { propertyListings, recentActivities, stats } from './_components/type';

export default function DashboardPage() {
  return (
    <div className="flex min-h-screen bg-gray-50 dark:bg-gray-900">
      {/* Main content */}
      <div className="flex-1">
        {/* Header */}
        <Header />

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
          <StatsCards stats={stats} />

          {/* Main content area */}
          <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
            {/* Property listings */}
            <div className="lg:col-span-2">
              <PropertyListings properties={propertyListings} />
            </div>

            {/* Activity Feed and Quick Actions */}
            <div className="space-y-6">
              {/* Recent Activities */}
              <ActivityFeed activities={recentActivities} />

              {/* Quick Actions */}
              <QuickActions />

              {/* Upcoming Appointments */}
              <UpcomingAppointments />
            </div>
          </div>
        </main>
      </div>
    </div>
  );
}
