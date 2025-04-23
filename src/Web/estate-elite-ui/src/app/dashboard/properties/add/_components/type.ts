import { z } from 'zod';
import { UseFormReturn } from 'react-hook-form';
import { District, Province, Ward } from '@/types';
import { LISTING_TYPE, RENT_PERIOD } from '@/lib/enum';

export const MAX_FILE_SIZE = 20 * 1024 * 1024; // 20MB
export const ACCEPTED_IMAGE_TYPES = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp'];

export const RoomValue = z.object({
  id: z.string(),
  name: z.string(),
  quantity: z.number().min(0, 'Số lượng không được âm'),
});

export const propertySchema = z.object({
  title: z.string().min(5, { message: 'Tiêu đề phải có ít nhất 5 ký tự' }),
  description: z.string().min(20, { message: 'Mô tả phải có ít nhất 20 ký tự' }),
  propertyType: z.string({ required_error: 'Vui lòng chọn loại bất động sản' }),
  listingType: z.nativeEnum(LISTING_TYPE, { required_error: 'Vui lòng chọn kiểu giao dịch' }),
  rentPeriod: z.nativeEnum(RENT_PERIOD).optional(),
  price: z.number().min(1, { message: 'Vui lòng nhập giá' }),
  area: z.number().min(1, { message: 'Vui lòng nhập diện tích' }),
  landArea: z.number().min(1, { message: 'Vui lòng nhập diện tích' }),
  buildDate: z.string({ message: 'Vui lòng nhập ngày xây dựng' }),
  address: z.string().min(5, { message: 'Địa chỉ phải có ít nhất 5 ký tự' }),
  province: z.string().min(1, { message: 'Vui lòng chọn thành phố' }),
  district: z.string().min(1, { message: 'Vui lòng chọn quận/huyện' }),
  ward: z.string().min(1, { message: 'Vui lòng chọn phường/xã' }),
  rooms: z.array(RoomValue).optional(),
  utilities: z.array(z.string()).optional(),
  images: z.array(
    z
      .instanceof(File)
      .refine((file) => file.size <= MAX_FILE_SIZE, `Kích thước tệp tối đa là 5MB`)
      .refine(
        (file) => ACCEPTED_IMAGE_TYPES.includes(file.type),
        'Chỉ chấp nhận định dạng .jpg, .jpeg, .png và .webp'
      )
  ),
});

export interface FormComponentProps {
  form: UseFormReturn<z.infer<typeof propertySchema>>;
}

export interface LocationSectionProps extends FormComponentProps {
  provinces: Province[];
  districts: District[];
  wards: Ward[];
  selectedCity?: string;
  selectedDistrict?: string;
  selectedWard?: string;
}

export interface ImageUploadProps extends FormComponentProps {
  previewImages: string[];
  setPreviewImages: React.Dispatch<React.SetStateAction<string[]>>;
}
