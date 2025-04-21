import { Button } from '@/components/ui/button';
import { Camera, CheckCircle, Loader2 } from 'lucide-react';
import { USER_ROLE } from '@/lib/enum';
import { useAppSelector } from '@/lib/hooks';
import { selectUser } from '@/redux/slices/auth-slice';
import { useState } from 'react';

export function VerificationForm() {
  const role = useAppSelector(selectUser)?.role;
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = () => {
    setIsLoading(true);
  };

  return (
    <div className="space-y-6">
      <div className="rounded-lg border border-yellow-100 bg-yellow-50 p-4">
        <h3 className="text-sm font-medium text-yellow-800">Tại sao phải xác minh tài khoản?</h3>
        <ul className="mt-2 space-y-2 text-sm text-yellow-700">
          <li className="flex gap-2">
            <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
            <span>Tăng độ tin cậy và uy tín với khách hàng/đối tác</span>
          </li>
          <li className="flex gap-2">
            <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
            <span>Được ưu tiên hiển thị trong kết quả tìm kiếm</span>
          </li>
          <li className="flex gap-2">
            <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
            <span>Mở khóa các tính năng nâng cao dành cho tài khoản đã xác minh</span>
          </li>
          <li className="flex gap-2">
            <CheckCircle className="mt-0.5 h-4 w-4 flex-shrink-0 text-yellow-500" />
            <span>Tăng cơ hội giao dịch thành công</span>
          </li>
        </ul>
      </div>

      <div className="space-y-4">
        <div>
          <h3 className="text-base font-medium">Để xác minh tài khoản, vui lòng cung cấp:</h3>
          <ul className="mt-2 space-y-2 text-sm">
            <li className="flex items-center gap-2">
              <div className="flex h-5 w-5 items-center justify-center rounded-full bg-gray-200 text-xs">
                1
              </div>
              <span>CMND/CCCD/Hộ chiếu còn hiệu lực</span>
            </li>
            <li className="flex items-center gap-2">
              <div className="flex h-5 w-5 items-center justify-center rounded-full bg-gray-200 text-xs">
                2
              </div>
              <span>Ảnh chân dung rõ nét</span>
            </li>
            {role === USER_ROLE.SELLER && (
              <li className="flex items-center gap-2">
                <div className="flex h-5 w-5 items-center justify-center rounded-full bg-gray-200 text-xs">
                  3
                </div>
                <span>Giấy phép kinh doanh hoặc giấy tờ pháp lý liên quan</span>
              </li>
            )}
          </ul>
        </div>

        <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
          <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
            <Camera className="mb-2 h-8 w-8 text-gray-400" />
            <p className="text-center text-sm text-gray-500">Tải lên mặt trước CMND/CCCD</p>
            <Button
              variant="outline"
              size="sm"
              className="mt-2"
            >
              Chọn file
            </Button>
          </div>

          <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
            <Camera className="mb-2 h-8 w-8 text-gray-400" />
            <p className="text-center text-sm text-gray-500">Tải lên mặt sau CMND/CCCD</p>
            <Button
              variant="outline"
              size="sm"
              className="mt-2"
            >
              Chọn file
            </Button>
          </div>
        </div>

        <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
          <Camera className="mb-2 h-8 w-8 text-gray-400" />
          <p className="text-center text-sm text-gray-500">Tải lên ảnh chân dung rõ nét</p>
          <p className="mt-1 text-center text-xs text-gray-400">
            Chụp ảnh chân dung với góc nhìn thẳng, ánh sáng rõ ràng
          </p>
          <Button
            variant="outline"
            size="sm"
            className="mt-2"
          >
            Chọn file
          </Button>
        </div>

        {role === USER_ROLE.SELLER && (
          <div className="flex min-h-40 flex-col items-center justify-center rounded-lg border border-dashed border-gray-300 p-4">
            <Camera className="mb-2 h-8 w-8 text-gray-400" />
            <p className="text-center text-sm text-gray-500">
              Tải lên giấy phép kinh doanh/giấy tờ pháp lý
            </p>
            <Button
              variant="outline"
              size="sm"
              className="mt-2"
            >
              Chọn file
            </Button>
          </div>
        )}
      </div>

      <div className="rounded-lg border border-blue-100 bg-blue-50 p-4">
        <h3 className="text-sm font-medium text-blue-800">Lưu ý quan trọng:</h3>
        <ul className="mt-2 space-y-1 text-sm text-blue-700">
          <li>• Thông tin của bạn sẽ được bảo mật và chỉ sử dụng cho mục đích xác minh</li>
          <li>• Quy trình xác minh thường mất 1-3 ngày làm việc</li>
          <li>• Bạn sẽ nhận được thông báo qua email khi hoàn tất xác minh</li>
        </ul>
      </div>

      <Button
        className="w-full"
        onClick={handleSubmit}
        disabled={isLoading}
      >
        {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
        Gửi yêu cầu xác minh
      </Button>
    </div>
  );
}
