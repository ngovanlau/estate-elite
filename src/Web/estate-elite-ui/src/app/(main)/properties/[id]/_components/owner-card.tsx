'use client';

import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import Image from 'next/image';
import DefaultAvatar from '@/public/images/default-avatar.png';
import { Owner } from '@/types/response/property-response';

type OwnerCardProps = {
  owner: Owner;
  isRental: boolean;
  onRentClick: () => void;
};

const OwnerCard = ({ owner, isRental, onRentClick }: OwnerCardProps) => {
  return (
    <Card className="mb-6">
      <CardContent className="pt-6">
        <div className="mb-4 flex items-center">
          <div className="relative mr-4 h-16 w-16 overflow-hidden rounded-full">
            <Image
              src={owner.avatar || DefaultAvatar}
              alt={owner.fullName}
              fill
              className="object-cover"
            />
          </div>
          <div>
            <h3 className="font-bold">{owner.fullName}</h3>
            {owner.companyName && (
              <p className="text-sm text-gray-600">Công ty: {owner.companyName}</p>
            )}
          </div>
        </div>

        <div className="mb-6 space-y-3 font-semibold">
          <p>Email: {owner.email}</p>
          <p>Số điện thoại: {owner.phone}</p>
        </div>

        <div className="space-y-3">
          <Button className="w-full">Liên hệ ngay</Button>
          <Button
            variant="outline"
            className="w-full"
          >
            Đặt lịch xem nhà
          </Button>
          {isRental && (
            <Button
              className="w-full bg-green-600 hover:bg-green-700"
              onClick={onRentClick}
            >
              Thuê cho tôi
            </Button>
          )}
        </div>
      </CardContent>
    </Card>
  );
};

export default OwnerCard;
