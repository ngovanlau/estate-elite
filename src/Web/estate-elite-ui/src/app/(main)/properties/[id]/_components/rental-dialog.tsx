'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from '@/components/ui/dialog';

import { Label } from '@/components/ui/label';
import { RentalPeriodSelector } from './rental-period-selector';
import PaymentMethodSelector from './payment-method-selector';
import { PropertyDetails } from '@/types/response/property-response';
import { PaypalButton } from './paypal-button';
import { PAYMENT_METHOD } from '@/lib/enum';

type RentalDialogProps = {
  isOpen: boolean;
  onOpenChange: (open: boolean) => void;
  property: PropertyDetails;
};

export const RentalDialog = ({ isOpen, onOpenChange, property }: RentalDialogProps) => {
  const [rentalPeriod, setRentalPeriod] = useState(0);
  const [paymentMethod, setPaymentMethod] = useState(PAYMENT_METHOD.PAYPAL);

  const buttonMap = {
    [PAYMENT_METHOD.PAYPAL]: (
      <PaypalButton
        propertyId={property.id}
        rentalPeriod={rentalPeriod}
      />
    ),
    [PAYMENT_METHOD.BANK]: <Button type="submit">Xác nhận</Button>,
  };

  return (
    <Dialog
      open={isOpen}
      onOpenChange={onOpenChange}
    >
      <DialogContent
        className="sm:max-w-md"
        onInteractOutside={(e) => {
          e.preventDefault();
        }}
        onEscapeKeyDown={(e) => e.preventDefault()}
      >
        <DialogHeader>
          <DialogTitle>Thuê bất động sản</DialogTitle>
          <DialogDescription>
            Vui lòng chọn thời gian thuê và phương thức thanh toán
          </DialogDescription>
        </DialogHeader>

        <div className="grid gap-4 py-4">
          <RentalPeriodSelector
            quantity={rentalPeriod}
            rentPeriod={property.rentPeriod}
            onChange={setRentalPeriod}
          />

          <div className="grid grid-cols-4 items-center gap-4">
            <Label
              htmlFor="totalPrice"
              className="text-right"
            >
              Tổng tiền
            </Label>
            <div className="col-span-3 font-semibold">
              {rentalPeriod * property.price} {property.currencyUnit}
            </div>
          </div>

          <PaymentMethodSelector
            selected={paymentMethod}
            onChange={setPaymentMethod}
          />
        </div>

        <DialogFooter>
          <Button
            variant="outline"
            onClick={() => onOpenChange(false)}
          >
            Hủy
          </Button>
          <div>{buttonMap[paymentMethod]}</div>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
