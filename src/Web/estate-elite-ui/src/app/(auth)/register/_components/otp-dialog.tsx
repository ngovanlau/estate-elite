import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import identityService from '@/services/identity-service';
import { Label } from '@radix-ui/react-label';
import { useRouter } from 'next/navigation';
import { useState, useEffect } from 'react';
import toast from 'react-hot-toast';

interface OTPDialogProps {
  userId: string;
  isDialogOpen: boolean;
  handleCloseDialog: () => void;
}

const OtpDialog: React.FC<OTPDialogProps> = ({
  userId,
  isDialogOpen,
  handleCloseDialog,
}: OTPDialogProps) => {
  const [otp, setOtp] = useState<string>('');
  const [isVerifying, setIsVerifying] = useState<boolean>(false);
  const [countdown, setCountdown] = useState<number>(5 * 60);
  const router = useRouter();

  const handleVerifyOtp = async (): Promise<void> => {
    setIsVerifying(true);
    try {
      const response = await identityService.confirm({
        userId,
        code: otp,
      });

      if (response.succeeded && response.data) {
        toast.success('Bạn đã đăng kí tài khoản thành công.');
        router.push('/login');
      }
    } catch (error) {
      console.error('Xác thực thất bại:', error);
    } finally {
      setIsVerifying(false);
    }
  };

  const handleResendOtp = async (): Promise<void> => {
    try {
      await identityService.resendCode(userId);
      setCountdown(300);
    } catch (error) {
      console.error('Lỗi khi gửi lại OTP:', error);
    }
  };

  // Định dạng thời gian (mm:ss)
  const formatTime = (seconds: number): string => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  // Xử lý đếm ngược
  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (countdown > 0) {
      timer = setTimeout(() => setCountdown(countdown - 1), 1000);
    }
    return () => clearTimeout(timer);
  }, [countdown]);

  return (
    <Dialog
      open={isDialogOpen}
      onOpenChange={handleCloseDialog}
    >
      <DialogContent
        className="sm:max-w-[425px]"
        onInteractOutside={(e) => {
          // Ngăn không cho Dialog đóng khi click bên ngoài
          e.preventDefault();
        }}
        onEscapeKeyDown={(e) => e.preventDefault()} // Ngăn đóng bằng phím ESC
      >
        <DialogHeader>
          <DialogTitle>Xác thực tài khoản</DialogTitle>
          <DialogDescription>Vui lòng nhập mã OTP đã được gửi đến email của bạn</DialogDescription>
        </DialogHeader>

        <div className="grid gap-4 py-4">
          <div className="grid gap-2">
            <Label htmlFor="otp">Mã OTP</Label>
            <Input
              id="otp"
              placeholder="Nhập mã OTP"
              value={otp}
              onChange={
                (e: React.ChangeEvent<HTMLInputElement>) =>
                  setOtp(e.target.value.replace(/\D/g, '')) // Chỉ cho phép nhập số
              }
              className="text-center tracking-widest"
              maxLength={6}
              inputMode="numeric"
            />
          </div>
        </div>

        <div className="flex flex-col gap-4">
          <Button
            onClick={handleVerifyOtp}
            disabled={otp.length !== 6 || isVerifying}
            className="w-full bg-blue-600 hover:bg-blue-700"
          >
            {isVerifying ? 'Đang xác thực...' : 'Xác thực'}
          </Button>

          <div className="text-center text-sm">
            Không nhận được mã?{' '}
            {countdown > 0 ? (
              <span className="text-gray-500">Gửi lại sau {formatTime(countdown)}</span>
            ) : (
              <Button
                variant="link"
                className="p-0 text-blue-600 hover:text-blue-800"
                onClick={handleResendOtp}
                disabled={isVerifying}
              >
                Gửi lại mã
              </Button>
            )}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
};

export default OtpDialog;
