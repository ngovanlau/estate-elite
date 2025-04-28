'use client';

import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { RENT_PERIOD } from '@/lib/enum';

type RentalPeriodSelectorProps = {
  quantity: number;
  rentPeriod?: RENT_PERIOD;
  onChange: (period: number) => void;
};

export const RentalPeriodSelector = ({
  quantity,
  rentPeriod,
  onChange,
}: RentalPeriodSelectorProps) => {
  const rentPeriodMap = {
    [RENT_PERIOD.DAY]: 'Ngày',
    [RENT_PERIOD.MONTH]: 'Tháng',
    [RENT_PERIOD.YEAR]: 'Năm',
  };

  return (
    <div className="grid grid-cols-4 items-center gap-4">
      <Label
        htmlFor="rentalValue"
        className="text-right"
      >
        Thời gian
      </Label>
      <Input
        id="rentalValue"
        type="number"
        min="1"
        value={quantity}
        onChange={(e) => onChange(parseInt(e.target.value) || 1)}
        className="col-span-1"
      />
      <div>{rentPeriodMap[rentPeriod || RENT_PERIOD.MONTH]}</div>
    </div>
  );
};
