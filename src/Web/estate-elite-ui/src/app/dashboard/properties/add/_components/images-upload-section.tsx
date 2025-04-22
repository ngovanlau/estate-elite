import React from 'react';
import Image from 'next/image';
import { X, ImagePlus } from 'lucide-react';
import { FormDescription } from '@/components/ui/form';
import toast from 'react-hot-toast';
import { ACCEPTED_IMAGE_TYPES, ImageUploadProps, MAX_FILE_SIZE } from './type';

export const ImageUploadSection: React.FC<ImageUploadProps> = ({
  form,
  previewImages,
  setPreviewImages,
}) => {
  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files || files.length === 0) return;

    const imageFiles: File[] = Array.from(files);
    const currentImages = form.getValues('images') || [];

    // Kiểm tra kích thước và loại file
    const validFiles = imageFiles.filter(
      (file) => file.size <= MAX_FILE_SIZE && ACCEPTED_IMAGE_TYPES.includes(file.type)
    );

    if (validFiles.length !== imageFiles.length) {
      toast.error('Một số hình ảnh không hợp lệ');
    }

    // Cập nhật giá trị trong form
    const newImages = [...currentImages, ...validFiles];
    form.setValue('images', newImages);

    // Tạo URL preview cho các hình ảnh
    const newPreviewUrls = validFiles.map((file) => URL.createObjectURL(file));
    setPreviewImages((prev) => [...prev, ...newPreviewUrls]);
  };

  const removeImage = (index: number) => {
    // Xóa khỏi form values
    const currentImages = form.getValues('images') || [];
    const newImages = [...currentImages];
    newImages.splice(index, 1);
    form.setValue('images', newImages);

    // Xóa khỏi preview
    const newPreviews = [...previewImages];
    URL.revokeObjectURL(newPreviews[index]); // Giải phóng bộ nhớ
    newPreviews.splice(index, 1);
    setPreviewImages(newPreviews);
  };

  return (
    <div>
      <div className="mb-4 text-lg font-medium">Hình ảnh bất động sản</div>
      <div className="flex flex-col space-y-4">
        <div className="grid grid-cols-2 gap-4 md:grid-cols-3 lg:grid-cols-4">
          {previewImages.map((url, index) => (
            <div
              key={index}
              className="relative aspect-square overflow-hidden rounded-md border border-gray-200"
            >
              <Image
                src={url}
                alt={`Preview ${index + 1}`}
                fill
                className="object-cover"
              />
              <button
                type="button"
                onClick={() => removeImage(index)}
                className="bg-opacity-50 hover:bg-opacity-70 absolute top-1 right-1 rounded-full bg-black p-1 text-white"
              >
                <X className="h-4 w-4" />
              </button>
            </div>
          ))}

          <label
            htmlFor="image-upload"
            className="relative flex aspect-square cursor-pointer flex-col items-center justify-center rounded-md border border-dashed border-gray-300 text-sm text-gray-500 hover:border-gray-400"
          >
            <ImagePlus className="h-8 w-8" />
            <span className="mt-2">Thêm ảnh</span>
            <input
              id="image-upload"
              type="file"
              multiple
              accept=".jpg,.jpeg,.png,.webp"
              className="sr-only"
              onChange={handleImageUpload}
            />
          </label>
        </div>

        <FormDescription>
          Tải lên tối đa 10 hình ảnh về bất động sản, định dạng .jpg, .jpeg, .png hoặc .webp, kích
          thước tối đa 20MB mỗi ảnh.
        </FormDescription>
      </div>
    </div>
  );
};
