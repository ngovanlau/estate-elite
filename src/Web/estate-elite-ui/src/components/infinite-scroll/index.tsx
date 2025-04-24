'use client';

import { useEffect, useRef, ReactNode } from 'react';
import { Loader2 } from 'lucide-react';
import { cn } from '@/lib/utils';

type InfiniteScrollProps = {
  loadMore: () => void;
  hasMore: boolean;
  isLoading: boolean;
  children: ReactNode;
  className?: string;
  loader?: ReactNode;
  endMessage?: ReactNode;
};

export function InfiniteScroll({
  loadMore,
  hasMore,
  isLoading,
  children,
  className,
  loader = <Loader2 className="text-primary h-6 w-6 animate-spin" />,
  endMessage = (
    <div className="py-4 text-center text-gray-500">Đã tải hết danh sách bất động sản</div>
  ),
}: InfiniteScrollProps) {
  const observerRef = useRef<IntersectionObserver | null>(null);
  const loadMoreRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (observerRef.current) {
      observerRef.current.disconnect();
    }

    observerRef.current = new IntersectionObserver(
      (entries) => {
        if (entries[0]?.isIntersecting && hasMore && !isLoading) {
          loadMore();
        }
      },
      { threshold: 0.1 }
    );

    if (loadMoreRef.current) {
      observerRef.current.observe(loadMoreRef.current);
    }

    return () => {
      if (observerRef.current) {
        observerRef.current.disconnect();
      }
    };
  }, [loadMore, hasMore, isLoading]);

  return (
    <div className={cn('w-full', className)}>
      {children}

      <div
        ref={loadMoreRef}
        className="flex justify-center py-4"
      >
        {isLoading && loader}
      </div>

      {!hasMore && !isLoading && endMessage}
    </div>
  );
}
