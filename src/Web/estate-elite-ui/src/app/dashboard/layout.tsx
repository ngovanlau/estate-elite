import { AppSidebar } from '@/components/sidebar';
import { SidebarProvider, SidebarTrigger } from '@/components/ui/sidebar';

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  return (
    <SidebarProvider>
      <AppSidebar />
      <main className="flex-1 p-6">
        <SidebarTrigger />
        {children}
      </main>
    </SidebarProvider>
  );
}
