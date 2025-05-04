'use client';

import PropertyCard from '@/app/_components/property-card';
import propertyService from '@/services/property-service';
import { Property } from '@/types/response/property-response';
import { PAGE_SIZE } from '@/lib/constant';
import { useInfiniteQuery } from '@tanstack/react-query';
import { Loader2 } from 'lucide-react';
import { InfiniteScroll } from '@/components/infinite-scroll';
import { Button } from '@/components/ui/button';
import { useSearchParams } from 'next/navigation';
import { Suspense } from 'react';

export default function PropertiesPage() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <PropertiesPageContent />
    </Suspense>
  );
}

function PropertiesPageContent() {
  const searchParams = useSearchParams();
  const address = searchParams.get('address');
  const propertyTypeId = searchParams.get('property-type-id');

  const { data, fetchNextPage, hasNextPage, isFetchingNextPage, status } = useInfiniteQuery({
    queryKey: ['properties', address, propertyTypeId],
    queryFn: async ({ pageParam = 1 }) => {
      const response = await propertyService.getProperties({
        pageNumber: pageParam,
        pageSize: PAGE_SIZE,
        lastEntityId: lastEntityId,
        address: address || undefined,
        propertyTypeId: propertyTypeId || undefined,
      });

      if (!response.succeeded) {
        throw new Error('Failed to load properties');
      }

      return {
        properties: response.data,
        currentPage: pageParam,
        totalPages: response.totalPages,
        lastEntityId: response.data[response.data.length - 1]?.id,
      };
    },
    getNextPageParam: (lastPage) => {
      if (lastPage.currentPage < lastPage.totalPages) {
        return lastPage.currentPage + 1;
      }
      return undefined;
    },
    initialPageParam: 1,
  });

  const allProperties = data?.pages.flatMap((page) => page.properties) || [];
  const lastEntityId = data?.pages[data.pages.length - 1]?.lastEntityId;

  // if (status === 'error') {
  //   toast.error(`Lỗi khi tải danh sách bất động sản: ${error.message}`);
  //   return (
  //     <div className="py-10 text-center">Lỗi khi tải danh sách bất động sản. Vui lòng thử lại.</div>
  //   );
  // }

  return (
    <div className="container mx-auto py-8">
      <h1 className="mb-8 text-3xl font-bold">Danh sách bất động sản</h1>

      {status === 'pending' ? (
        <div className="flex justify-center py-10">
          <Loader2 className="text-primary h-8 w-8 animate-spin" />
        </div>
      ) : (
        <>
          <InfiniteScroll
            loadMore={() => fetchNextPage()}
            hasMore={!!hasNextPage}
            isLoading={isFetchingNextPage}
            className="mb-4"
          >
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
              {allProperties.map((property: Property) => (
                <PropertyCard
                  key={property.id}
                  property={property}
                />
              ))}
            </div>
          </InfiniteScroll>

          {/* Manual load more button (optional) */}
          {hasNextPage && !isFetchingNextPage && (
            <div className="flex justify-center py-4">
              <Button
                onClick={() => fetchNextPage()}
                variant="outline"
              >
                Load More
              </Button>
            </div>
          )}
        </>
      )}
    </div>
  );
}
