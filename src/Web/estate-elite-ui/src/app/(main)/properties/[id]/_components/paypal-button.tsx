'use client';

import paymentService from '@/services/payment-service';
import { OnApproveActions, OnApproveData } from '@paypal/paypal-js';
import { PayPalButtons } from '@paypal/react-paypal-js';
import toast from 'react-hot-toast';

type PaypalButtonProps = {
  propertyId: string;
  rentalPeriod: number;
};

export const PaypalButton = ({ propertyId, rentalPeriod }: PaypalButtonProps) => {
  const onCreateOrder = async (): Promise<string> => {
    try {
      const response = await paymentService.rentProperty({
        propertyId: propertyId,
        rentalPeriod: rentalPeriod,
        returnUrl: `http://localhost:3000/properties/${propertyId}/success`,
        cancelUrl: `http://localhost:3000/properties/${propertyId}/cancel`,
      });

      if (response.succeeded) {
        return response.data?.orderId || '';
      }
    } catch (error) {
      toast.error('Thuê thất bại, vui lòng thử lại sau.');
      throw error;
    }

    return '';
  };

  const onApprove = async (data: OnApproveData, actions: OnApproveActions) => {
    try {
      const response = await fetch(`/api/orders/${data.orderID}/capture`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const orderData = await response.json();
      const errorDetail = orderData?.details?.[0];

      if (errorDetail?.issue === 'INSTRUMENT_DECLINED') {
        return actions.restart();
      } else if (errorDetail) {
        throw new Error(`${errorDetail.description} (${orderData.debug_id})`);
      } else {
        const transaction = orderData.purchase_units[0].payments.captures[0];
        console.log(transaction);
        console.log('Capture result', orderData, JSON.stringify(orderData, null, 2));
      }
    } catch (error) {
      console.error(error);
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
