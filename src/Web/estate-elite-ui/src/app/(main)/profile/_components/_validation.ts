import { z } from 'zod';

// Schema cho thông tin chung
export const profileFormSchema = z.object({
  fullName: z.string().trim().min(2, {
    message: 'Tên phải có ít nhất 2 ký tự.',
  }),
  email: z.string().trim().email({
    message: 'Email không hợp lệ.',
  }),
  phone: z.string().trim().min(10, {
    message: 'Số điện thoại phải có ít nhất 10 số.',
  }),
  address: z.string().trim().min(5, {
    message: 'Địa chỉ phải có ít nhất 5 ký tự.',
  }),
});

// Schema cho thông tin doanh nghiệp (seller & developer)
export const businessFormSchema = z
  .object({
    companyName: z.string().trim().min(2, {
      message: 'Tên công ty phải có ít nhất 2 ký tự.',
    }),
    taxId: z.string().trim().min(10, {
      message: 'Mã số thuế phải có ít nhất 10 ký tự.',
    }),
    licenseNumber: z
      .string()
      .trim()
      .min(5, {
        message: 'Số giấy phép phải có ít nhất 5 ký tự.',
      })
      .optional(),
    professionalLicense: z
      .string()
      .trim()
      .min(1, {
        message: 'Vui lòng nhập thông tin giấy phép hành nghề.',
      })
      .optional(),
    biography: z
      .string()
      .trim()
      .max(1000, {
        message: 'Tối đa chỉ được 1000 ký tự',
      })
      .optional(),
    establishedYear: z.number().refine(
      (val) => {
        const currentYear = new Date().getFullYear();
        return !isNaN(val) && val > 1900 && val <= currentYear;
      },
      {
        message: 'Năm thành lập không hợp lệ.',
      }
    ),
    acceptsPaypal: z.boolean(),
    paypalEmail: z
      .string()
      .trim()
      .email({
        message: 'Email PayPal không hợp lệ',
      })
      .optional(),

    paypalMerchantId: z
      .string()
      .trim()
      .min(10, {
        message: 'Merchant ID phải có ít nhất 10 ký tự',
      })
      .optional(),
  })
  .refine(
    (data) => {
      if (data.acceptsPaypal) {
        return !!data.paypalEmail && !!data.paypalMerchantId;
      }
      return true;
    },
    {
      message: 'Vui lòng nhập đầy đủ thông tin PayPal khi chọn chấp nhận thanh toán',
      path: ['acceptsPaypal'],
    }
  );

// Schema cho đổi mật khẩu
export const passwordFormSchema = z
  .object({
    currentPassword: z.string().trim().min(8, {
      message: 'Mật khẩu phải có ít nhất 8 ký tự.',
    }),
    newPassword: z.string().trim().min(8, {
      message: 'Mật khẩu mới phải có ít nhất 8 ký tự.',
    }),
    confirmPassword: z.string().trim().min(8, {
      message: 'Mật khẩu xác nhận phải có ít nhất 8 ký tự.',
    }),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: 'Mật khẩu xác nhận không khớp.',
    path: ['confirmPassword'],
  });
