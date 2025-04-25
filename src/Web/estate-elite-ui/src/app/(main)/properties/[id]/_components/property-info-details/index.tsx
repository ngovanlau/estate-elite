import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { formatCurrency } from '@/lib/utils';
import { Heart, Share2, MapPin, Ruler, BedDouble, Badge } from 'lucide-react';
import PropertyDetailsGallery from '../property-details-gallery';
import Image from 'next/image';
import { LISTING_TYPE } from '@/lib/enum';
import dayjs from 'dayjs';
import { PropertyDetails } from '@/types/response/property-response';
import DefaultAvatar from '@/public/images/default-avatar.png';

type PropertyDetailsProps = {
  details: PropertyDetails;
};

export const PropertyInfoDetails = ({ details: propertyDetails }: PropertyDetailsProps) => {
  return (
    <div
      key={propertyDetails.id}
      className="container mx-auto px-4 py-8"
    >
      <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
        <div className="lg:col-span-2">
          <div className="mb-6">
            <h1 className="mb-2 text-2xl font-bold md:text-3xl">{propertyDetails.title}</h1>
            <div className="mb-4 flex items-center text-gray-600">
              <MapPin className="mr-1 h-4 w-4" />
              <span>{propertyDetails.address}</span>
            </div>

            <div className="flex items-center gap-4">
              <Badge
                className={
                  propertyDetails.listingType === LISTING_TYPE.SALE ? 'bg-blue-600' : 'bg-green-600'
                }
              >
                {propertyDetails.listingType === LISTING_TYPE.SALE ? 'Bán' : 'Cho thuê'}
              </Badge>
              <div className="text-2xl font-bold text-blue-700">
                {formatCurrency(propertyDetails.price)}
                {propertyDetails.listingType === LISTING_TYPE.RENT && '/tháng'}
              </div>
            </div>
          </div>

          <PropertyDetailsGallery images={propertyDetails.images} />

          <Tabs
            defaultValue="overview"
            className="mt-8"
          >
            <TabsList className="grid w-full grid-cols-3">
              <TabsTrigger value="overview">Tổng quan</TabsTrigger>
              <TabsTrigger value="features">Tiện ích</TabsTrigger>
              <TabsTrigger value="location">Vị trí</TabsTrigger>
            </TabsList>

            <TabsContent
              value="overview"
              className="mt-4"
            >
              <Card>
                <CardContent className="pt-6">
                  <div className="mb-6 grid grid-cols-3 gap-4">
                    <div className="flex flex-col items-center rounded-lg bg-gray-50 p-4">
                      <Ruler className="mb-2 h-6 w-6 text-blue-600" />
                      <span className="text-lg font-semibold">{propertyDetails.area}</span>
                      <span className="text-sm text-gray-500">m²</span>
                    </div>

                    {propertyDetails.rooms.map((room, index) => (
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

                  {propertyDetails.buildDate && (
                    <div className="mb-4">
                      <span className="font-semibold">Năm xây dựng:</span>{' '}
                      {dayjs(propertyDetails.buildDate).format('DD/MM/YYYY')}
                    </div>
                  )}

                  <div className="mb-6">
                    <h3 className="mb-2 text-lg font-semibold">Mô tả</h3>
                    <p className="text-gray-700">{propertyDetails.description}</p>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            <TabsContent
              value="features"
              className="mt-4"
            >
              <Card>
                <CardContent className="pt-6">
                  <h3 className="mb-4 text-lg font-semibold">Tiện ích & Đặc điểm</h3>
                  <div className="grid grid-cols-2 gap-y-3 md:grid-cols-3">
                    {propertyDetails.utilities.map((utility, index) => (
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
            </TabsContent>

            <TabsContent
              value="location"
              className="mt-4"
            >
              <Card>
                <CardContent className="pt-6">
                  <h3 className="mb-4 text-lg font-semibold">Vị trí</h3>
                  <div className="flex h-80 items-center justify-center rounded-lg bg-gray-200">
                    <p className="text-gray-600">Bản đồ vị trí bất động sản</p>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>
          </Tabs>
        </div>

        <div>
          <Card className="mb-6">
            <CardContent className="pt-6">
              <div className="mb-4 flex items-center">
                <div className="relative mr-4 h-16 w-16 overflow-hidden rounded-full">
                  <Image
                    src={propertyDetails.owner.avatar || DefaultAvatar}
                    alt={propertyDetails.owner.fullName}
                    fill
                    className="object-cover"
                  />
                </div>
                <div>
                  <h3 className="font-semibold">{propertyDetails.owner.fullName}</h3>
                  {propertyDetails.owner.companyName && (
                    <p className="text-sm text-gray-600">{propertyDetails.owner.companyName}</p>
                  )}
                </div>
              </div>

              <div className="mb-6 space-y-3">
                <p>{propertyDetails.owner.email}</p>
                <p>{propertyDetails.owner.phone}</p>
              </div>

              <div className="space-y-3">
                <Button className="w-full">Liên hệ ngay</Button>
                <Button
                  variant="outline"
                  className="w-full"
                >
                  Đặt lịch xem nhà
                </Button>
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardContent className="pt-6">
              <h3 className="mb-4 font-semibold">Gửi yêu cầu</h3>
              <div className="space-y-4">
                <Button
                  variant="outline"
                  className="flex w-full justify-between"
                >
                  <span>Lưu</span>
                  <Heart className="h-5 w-5" />
                </Button>
                <Button
                  variant="outline"
                  className="flex w-full justify-between"
                >
                  <span>Chia sẻ</span>
                  <Share2 className="h-5 w-5" />
                </Button>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
};
