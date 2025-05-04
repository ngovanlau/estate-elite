import { Check, CalendarClock, CreditCard } from 'lucide-react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { CURRENCY_UNIT, PAYMENT_METHOD } from '@/lib/enum';
import { CaptureOrderResponse } from '@/types/response/payment-response';
import { formatCurrency } from '@/lib/utils';
import dayjs from 'dayjs';

interface PaymentSuccessDialogProps {
  isOpen: boolean;
  onClose: () => void;
  orderData?: CaptureOrderResponse;
}

const paymentMethodMap = {
  [PAYMENT_METHOD.BANK]: 'Chuyển khoản ngân hàng',
  [PAYMENT_METHOD.PAYPAL]: 'Ví điện tử PayPal',
};

export default function PaymentSuccessDialog({
  isOpen,
  onClose,
  orderData,
}: PaymentSuccessDialogProps) {
  return (
    <Dialog
      open={isOpen}
      onOpenChange={onClose}
    >
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-full bg-green-100">
            <Check className="h-6 w-6 text-green-600" />
          </div>
          <DialogTitle className="mt-4 text-center text-lg font-medium text-gray-900">
            Thanh toán thành công
          </DialogTitle>
          <DialogDescription className="text-center text-sm text-gray-500">
            Giao dịch của quý khách đã được xử lý thành công
          </DialogDescription>
        </DialogHeader>

        <div className="mt-4 border-t border-b py-4">
          <dl className="divide-y divide-gray-100">
            <div className="flex justify-between py-2">
              <dt className="text-sm font-medium text-gray-500">Mã giao dịch</dt>
              <dd className="text-sm font-medium text-gray-900">{orderData?.id}</dd>
            </div>
            <div className="flex justify-between py-2">
              <dt className="text-sm font-medium text-gray-500">Số tiền</dt>
              <dd className="text-sm font-medium text-green-600">
                {formatCurrency(
                  orderData?.amount || 0,
                  orderData?.currencyUnit || CURRENCY_UNIT.USD
                )}
              </dd>
            </div>
            <div className="flex justify-between py-2">
              <dt className="text-sm font-medium text-gray-500">Trạng thái</dt>
              <dd>
                <Badge className="bg-green-100 text-green-800 hover:bg-green-100">Hoàn thành</Badge>
              </dd>
            </div>
            <div className="flex justify-between py-2">
              <dt className="text-sm font-medium text-gray-500">Phương thức</dt>
              <dd className="flex items-center text-sm text-gray-900">
                <CreditCard className="mr-1 h-4 w-4 text-gray-400" />
                {paymentMethodMap[orderData?.paymentMethod || PAYMENT_METHOD.BANK]}
              </dd>
            </div>
            <div className="flex justify-between py-2">
              <dt className="text-sm font-medium text-gray-500">Thời gian</dt>
              <dd className="flex items-center text-sm text-gray-900">
                <CalendarClock className="mr-1 h-4 w-4 text-gray-400" />
                {dayjs(orderData?.createdOn).format('DD/MM/YYYY')}
              </dd>
            </div>
          </dl>
        </div>

        <DialogFooter className="sm:justify-center">
          <Button
            onClick={onClose}
            className="w-full sm:w-auto"
          >
            Đóng
          </Button>
          <Button
            variant="outline"
            className="w-full sm:w-auto"
          >
            Xem chi tiết
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
