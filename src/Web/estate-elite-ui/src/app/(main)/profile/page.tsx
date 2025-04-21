'use client';

import { ProfileHeader } from './_components/profile-header';
import { ProfileSidebar } from './_components/profile-sidebar';
import { TabContent } from './_components/tab-content';

export default function ProfilePage() {
  return (
    <div className="container mx-auto px-4 py-8">
      <ProfileHeader />

      <div className="grid grid-cols-1 gap-8 md:grid-cols-3">
        {/* Sidebar - 1/3 width on medium screens and above */}
          <ProfileSidebar />

        {/* Content - 2/3 width on medium screens and above */}
        <TabContent />
      </div>
    </div>
  );
}
