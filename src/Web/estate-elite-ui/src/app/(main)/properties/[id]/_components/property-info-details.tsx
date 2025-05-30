'use client';

import { LISTING_TYPE } from '@/lib/enum';
import { formatCurrency } from '@/lib/utils';
import { PropertyDetails } from '@/types/response/property-response';
import { MapPin } from 'lucide-react';
import PropertyDetailsGallery from './property-details-gallery';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@radix-ui/react-tabs';
import PropertyOverview from './property-overview';
import PropertyUtilities from './property-utilities';
import PropertyLocation from './property-location';
import OwnerCard from './owner-card';
import ActionsCard from './action-card';
import { RentalDialog } from './rental-dialog';
import { useState } from 'react';
import PaymentSuccessDialog from './payment-success-dialog';
import { CaptureOrderResponse } from '@/types/response/payment-response';

type PropertyDetailsProps = {
  details: PropertyDetails;
};

export const PropertyInfoDetails = ({ details: propertyDetails }: PropertyDetailsProps) => {
  const [isRentalModalOpen, setRentalModalOpen] = useState<boolean>(false);
  const [isPaymentSuccessDialogOpen, setIsPaymentSuccessDialogOpen] = useState<boolean>(false);
  const [orderData, setOrderData] = useState<CaptureOrderResponse>();

  const listingTypeMap = {
    [LISTING_TYPE.RENT]: (
      <span className="rounded bg-amber-600 px-2.5 py-1 font-medium text-white">Thuê</span>
    ),
    [LISTING_TYPE.SALE]: (
      <span className="rounded bg-blue-600 px-2.5 py-1 font-medium text-white">Bán</span>
    ),
  };

  const handleRentButtonClick = () => {
    setRentalModalOpen(true);
  };

  const handleClosePaymentSuccessDialog = () => {
    setIsPaymentSuccessDialogOpen(false);
  };

  const handlePaymentSuccess = (orderData: CaptureOrderResponse) => {
    setOrderData(orderData);
    setRentalModalOpen(false);
    setIsPaymentSuccessDialogOpen(true);
  };

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
              {listingTypeMap[propertyDetails.listingType]}
              <div className="text-2xl font-bold text-blue-700">
                {formatCurrency(propertyDetails.price, propertyDetails.currencyUnit)}
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
              <PropertyOverview
                propertyType={propertyDetails.type}
                area={propertyDetails.area}
                rooms={propertyDetails.rooms}
                buildDate={propertyDetails.buildDate}
                description={propertyDetails.description}
              />
            </TabsContent>

            <TabsContent
              value="features"
              className="mt-4"
            >
              <PropertyUtilities utilities={propertyDetails.utilities} />
            </TabsContent>

            <TabsContent
              value="location"
              className="mt-4"
            >
              <PropertyLocation />
            </TabsContent>
          </Tabs>
        </div>

        <div>
          <OwnerCard
            owner={propertyDetails.owner}
            isRental={propertyDetails.listingType === LISTING_TYPE.RENT}
            onRentClick={handleRentButtonClick}
          />

          <ActionsCard />
        </div>
      </div>

      <RentalDialog
        isOpen={isRentalModalOpen}
        onOpenChange={setRentalModalOpen}
        property={propertyDetails}
        handlePaymentSuccess={handlePaymentSuccess}
      />

      <PaymentSuccessDialog
        isOpen={isPaymentSuccessDialogOpen}
        onClose={handleClosePaymentSuccessDialog}
        orderData={orderData}
      />
    </div>
  );
};
