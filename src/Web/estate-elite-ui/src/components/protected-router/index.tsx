'use client';

import { useEffect, ReactNode } from 'react';
import { useRouter } from 'next/navigation';
import { useAppSelector } from '@/lib/hooks';
import { selectIsAuthenticated } from '@/redux/slices/auth-slice';

interface ProtectedRouteProps {
  children: ReactNode;
  fallback?: ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  fallback = <div>Loading...</div>,
}) => {
  const router = useRouter();
  const isAuthenticated = useAppSelector(selectIsAuthenticated);

  useEffect(() => {
    // Chỉ điều hướng nếu chúng ta chắc chắn người dùng chưa được xác thực
    // và chúng ta đang ở phía client
    if (typeof window !== 'undefined' && isAuthenticated === false) {
      router.push('/login');
    }
  }, [isAuthenticated, router]);

  // Hiển thị fallback khi chưa xác thực
  if (!isAuthenticated) {
    return <>{fallback}</>;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
