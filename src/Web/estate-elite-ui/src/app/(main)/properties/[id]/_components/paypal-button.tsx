'use client';

import paymentService from '@/services/payment-service';
import { OnApproveData } from '@paypal/paypal-js';
import { PayPalButtons } from '@paypal/react-paypal-js';
import { useState } from 'react';
import toast from 'react-hot-toast';

type PaypalButtonProps = {
  propertyId: string;
  rentalPeriod: number;
};

export const PaypalButton = ({ propertyId, rentalPeriod }: PaypalButtonProps) => {
  const [transactionId, setTransactionId] = useState<string>('');

  const onCreateOrder = async (): Promise<string> => {
    try {
      const response = await paymentService.rentProperty({
        propertyId: propertyId,
        rentalPeriod: rentalPeriod,
        returnUrl: `http://localhost:3000/properties/${propertyId}/success`,
        cancelUrl: `http://localhost:3000/properties/${propertyId}/cancel`,
      });

      if (response.succeeded) {
        setTransactionId(response.data?.transactionId);
        return response.data?.orderId || '';
      }
    } catch (error) {
      toast.error('Thuê thất bại, vui lòng thử lại sau.');
      throw error;
    }

    return '';
  };

  const onApprove = async (data: OnApproveData) => {
    try {
      const response = await paymentService.captureOrder({
        transactionId: transactionId,
        orderId: data.orderID,
      });

      if (!response.succeeded) {
        toast.error('Thuê thất bại, vui lòng thử lại sau.');
        return;
      }

      console.log(response.data);
    } catch (error) {
      toast.error('Thuê thất bại, vui lòng thử lại sau.');
      throw error;
    }
  };

  return (
    <PayPalButtons
      fundingSource="paypal"
      style={{
        shape: 'rect',
        layout: 'vertical',
        color: 'gold',
        label: 'paypal',
      }}
      createOrder={onCreateOrder}
      onApprove={onApprove}
      disabled={rentalPeriod < 1}
    />
  );
};
