'use client';

import { Card, CardContent } from '@/components/ui/card';
import { Ruler, BedDouble } from 'lucide-react';
import dayjs from 'dayjs';

type Room = {
  name: string;
  quantity: number;
};

type PropertyOverviewProps = {
  area: number;
  rooms: Room[];
  buildDate?: string;
  description: string;
};

const PropertyOverview = ({ area, rooms, buildDate, description }: PropertyOverviewProps) => {
  return (
    <Card>
      <CardContent className="pt-6">
        <div className="mb-6 grid grid-cols-3 gap-4">
          <div className="flex flex-col items-center rounded-lg bg-gray-50 p-4">
            <Ruler className="mb-2 h-6 w-6 text-blue-600" />
            <span className="text-lg font-semibold">{area}</span>
            <span className="text-sm text-gray-500">m²</span>
          </div>

          {rooms.map((room, index) => (
            <div
              key={index}
              className="flex flex-col items-center rounded-lg bg-gray-50 p-4"
            >
              <BedDouble className="mb-2 h-6 w-6 text-blue-600" />
              <span className="text-lg font-semibold">{room.quantity}</span>
              <span className="text-sm text-gray-500">{room.name}</span>
            </div>
          ))}
        </div>

        {buildDate && (
          <div className="mb-4">
            <span className="font-semibold">Năm xây dựng:</span>{' '}
            {dayjs(buildDate).format('DD/MM/YYYY')}
          </div>
        )}

        <div className="mb-6">
          <h3 className="mb-2 text-lg font-semibold">Mô tả</h3>
          <p className="text-gray-700">{description}</p>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertyOverview;
