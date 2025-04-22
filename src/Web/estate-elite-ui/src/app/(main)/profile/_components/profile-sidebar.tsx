'use client';

import { useRef } from 'react';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { Building2, Home, Briefcase, Pencil } from 'lucide-react';
import toast from 'react-hot-toast';
import { USER_ROLE } from '@/lib/enum';
import { useAppDispatch, useAppSelector } from '@/lib/hooks';
import { selectUser, updateUser } from '@/redux/slices/auth-slice';
import dayjs from '@/lib/plugins/dayjs';
import identityService from '@/services/identity-service';
import { CurrentUser } from '@/types/response/identity-response';
// import DefaultAvatar from '@/public/images/default-avatar.png';

export function ProfileSidebar() {
  const currentUser = useAppSelector(selectUser);
  const avatarInputRef = useRef<HTMLInputElement>(null);
  const avatar = currentUser?.avatar;
  const role = currentUser?.role || USER_ROLE.BUYER;
  const dispatch = useAppDispatch();

  async function handleAvatarChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];

    // Validate file
    if (!file) return;
    if (!file.type.startsWith('image/')) {
      toast.error('Vui lòng chọn tệp ảnh hợp lệ');
      return;
    }
    if (file.size > 20 * 1024 * 1024) {
      toast.error('Kích thước ảnh không được vượt quá 20MB');
      return;
    }

    try {
      const formData = new FormData();
      formData.append('image', file);
      const response = await identityService.uploadAvatar(formData);

      if (!response.succeeded) {
        toast.error('Cập nhật ảnh avatar thất bại');
        throw new Error('Upload failed');
      }

      dispatch(
        updateUser({
          ...currentUser,
          avatar: response?.data,
        } as CurrentUser)
      );

      toast.success('Ảnh avatar của bạn đã được cập nhật thành công.');
    } catch (error) {
      console.error('Upload error:', error);
      toast.error('Có lỗi xảy ra khi cập nhật avatar.');
      throw error; // Ném lỗi để component cha có thể bắt nếu cần
    }
  }

  // Render nhãn theo loại người dùng
  const roleMap = {
    [USER_ROLE.BUYER]: {
      name: 'Khách hàng tìm mua/thuê',
      icon: <Home className="h-4 w-4" />,
    },
    [USER_ROLE.SELLER]: {
      name: 'Chủ sở hữu bất động sản',
      icon: <Briefcase className="h-4 w-4" />,
    },
    [USER_ROLE.ADMIN]: {
      name: 'Quản trị viên',
      icon: <Building2 className="h-4 w-4" />,
    },
  };

  return (
    <div className="md:col-span-1">
      <Card className="sticky top-4">
        <CardHeader className="pb-2">
          <CardTitle>Hồ sơ cá nhân</CardTitle>
          <CardDescription>Quản lý thông tin tài khoản của bạn</CardDescription>
        </CardHeader>
        <CardContent className="flex flex-col items-center pt-6">
          <div className="relative mb-6">
            <Avatar className="h-24 w-24">
              <AvatarImage
                src={avatar}
                alt="Avatar"
              />
              <AvatarFallback>NA</AvatarFallback>
            </Avatar>
            <Button
              onClick={() => avatarInputRef.current?.click()}
              variant="secondary"
              size="icon"
              className="absolute right-0 bottom-0 rounded-full"
            >
              <Pencil className="h-4 w-4" />
            </Button>
            <input
              ref={avatarInputRef}
              type="file"
              accept="image/*"
              className="hidden"
              onChange={handleAvatarChange}
            />
          </div>

          <h3 className="text-lg font-medium">{currentUser?.fullName}</h3>
          <p className="text-sm text-gray-500">{currentUser?.email}</p>

          <div className="mt-2 flex items-center gap-1">
            <Badge
              variant="outline"
              className="flex items-center gap-1"
            >
              {roleMap[role].icon} <span>{roleMap[role].name}</span>
            </Badge>
            {/* TODO
          {isVerified && (
            <Badge className="flex items-center gap-1 bg-green-500">
              <CheckCircle className="h-3 w-3" />
              <span>Đã xác minh</span>
            </Badge>
          )} */}
          </div>

          <div className="mt-6 w-full">
            <Separator className="my-4" />
            <div className="flex items-center justify-between text-sm">
              <span className="text-gray-500">ID người dùng:</span>
              <span className="font-medium">#{currentUser?.id}</span>
            </div>
            <div className="mt-2 flex items-center justify-between text-sm">
              <span className="text-gray-500">Ngày tham gia:</span>
              <span className="font-medium">
                {dayjs(currentUser?.createdOn).tz('Asia/Ho_Chi_Minh').format('DD/MM/YYYY')}
              </span>
            </div>
            {/* TODO
          {role === USER_ROLE.SELLER && (
            <div className="mt-2 flex items-center justify-between text-sm">
              <span className="text-gray-500">Đánh giá:</span>
              <span className="flex items-center font-medium">
                <Award className="mr-1 h-4 w-4 text-yellow-500" />
                4.8/5
              </span>
            </div>
          )} */}
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
