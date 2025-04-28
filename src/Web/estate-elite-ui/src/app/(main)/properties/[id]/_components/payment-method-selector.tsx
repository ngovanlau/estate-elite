'use client';

import { Label } from '@/components/ui/label';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { PAYMENT_METHOD } from '@/lib/enum';

type PaymentMethodSelectorProps = {
  selected: string;
  onChange: (value: PAYMENT_METHOD) => void;
};

const PaymentMethodSelector = ({ selected, onChange }: PaymentMethodSelectorProps) => {
  return (
    <div className="grid grid-cols-4 items-start gap-4 pt-4">
      <Label className="pt-2 text-right">Thanh toán</Label>
      <RadioGroup
        className="col-span-3"
        value={selected}
        onValueChange={onChange}
      >
        <div className="flex items-center space-x-2">
          <RadioGroupItem
            value={PAYMENT_METHOD.PAYPAL}
            id={PAYMENT_METHOD.PAYPAL}
          />
          <Label htmlFor={PAYMENT_METHOD.PAYPAL}>PayPal</Label>
        </div>
        <div className="flex items-center space-x-2">
          <RadioGroupItem
            value={PAYMENT_METHOD.BANK}
            id={PAYMENT_METHOD.BANK}
          />
          <Label htmlFor={PAYMENT_METHOD.BANK}>Chuyển khoản ngân hàng</Label>
        </div>
      </RadioGroup>
    </div>
  );
};

export default PaymentMethodSelector;
