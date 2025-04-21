'use client';

import { useEffect, useRef } from 'react';
import { Button } from '@/components/ui/button';
import { Camera } from 'lucide-react';
import Image from 'next/image';
import toast from 'react-hot-toast';
import DefaultBackground from '@/public/images/default-background.jpg';
import { useAppDispatch, useAppSelector } from '@/lib/hooks';
import { selectUser, updateUser } from '@/redux/slices/auth-slice';
import identityService from '@/services/identity-service';
import { CurrentUserData } from '@/types/response/identity-response';

export function ProfileHeader() {
  const background = useAppSelector(selectUser)?.background;
  const backgroundInputRef = useRef<HTMLInputElement>(null);
  const currentUser = useAppSelector(selectUser);
  const dispatch = useAppDispatch();

  useEffect(() => {
    console.log('hello');
  }, [currentUser]);

  const handleBackgroundChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];

    if (!file) return;
    if (!file.type.startsWith('image/')) {
      toast.error('Vui lòng chọn tệp ảnh hợp lệ');
      return;
    }
    if (file.size > 20 * 1024 * 1024) {
      toast.error('Kích thước ảnh không được vượt quá 5MB');
      return;
    }

    try {
      const formData = new FormData();
      formData.append('image', file);
      const response = await identityService.uploadBackground(formData);

      if (!response.succeeded || !response.data) {
        toast.error('Cập nhật ảnh bìa thất bại.');
        throw new Error(response.message || 'Cập nhật ảnh bìa thất bại');
      }

      if (currentUser) {
        dispatch(
          updateUser({
            ...currentUser,
            background: response.data,
          } as CurrentUserData)
        );
      }

      toast.success('Ảnh bìa của bạn đã được cập nhật thành công.');
    } catch (error) {
      console.error('Lỗi khi tải lên ảnh bìa:', error);
      toast.error(error instanceof Error ? error.message : 'Đã xảy ra lỗi khi tải lên ảnh bìa');
    }
  };

  return (
    <div className="relative mb-8 h-64 w-full overflow-hidden rounded-xl bg-gray-100">
      <Image
        src={background || DefaultBackground}
        alt="Cover"
        fill
        className="object-cover"
      />
      <Button
        onClick={() => backgroundInputRef.current?.click()}
        variant="secondary"
        size="icon"
        className="absolute right-4 bottom-4 rounded-full"
      >
        <Camera className="h-4 w-4" />
      </Button>
      <input
        ref={backgroundInputRef}
        type="file"
        accept="image/*"
        className="hidden"
        onChange={handleBackgroundChange}
      />
    </div>
  );
}
