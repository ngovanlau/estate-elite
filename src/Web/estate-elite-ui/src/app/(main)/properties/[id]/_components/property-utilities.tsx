'use client';

import { Card, CardContent } from '@/components/ui/card';

type PropertyFeaturesProps = {
  utilities: string[];
};

const PropertyUtilities = ({ utilities }: PropertyFeaturesProps) => {
  return (
    <Card>
      <CardContent className="pt-6">
        <h3 className="mb-4 text-lg font-semibold">Tiện ích & Đặc điểm</h3>
        <div className="grid grid-cols-2 gap-y-3 md:grid-cols-3">
          {utilities.map((utility, index) => (
            <div
              key={index}
              className="flex items-center"
            >
              <div className="mr-2 h-2 w-2 rounded-full bg-blue-600"></div>
              <span>{utility}</span>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertyUtilities;
