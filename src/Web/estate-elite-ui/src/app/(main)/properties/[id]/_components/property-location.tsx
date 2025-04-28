'use client';

import { Card, CardContent } from '@/components/ui/card';

const PropertyLocation = () => {
  return (
    <Card>
      <CardContent className="pt-6">
        <h3 className="mb-4 text-lg font-semibold">Vị trí</h3>
        <div className="flex h-80 items-center justify-center rounded-lg bg-gray-200">
          <p className="text-gray-600">Bản đồ vị trí bất động sản</p>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertyLocation;
