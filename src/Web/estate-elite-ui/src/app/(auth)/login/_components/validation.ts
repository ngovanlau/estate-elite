import { z } from 'zod';
import { REGEX } from '@/lib/constant';

export const loginFormSchema = z.object({
  usernameOrEmail: z
    .string()
    .trim()
    .min(3, { message: 'Tên đăng nhập/email phải có ít nhất 3 ký tự' })
    .max(50, { message: 'Tên đăng nhập/email không được vượt quá 50 ký tự' })
    .superRefine((val, ctx) => {
      const isEmail = val.includes('@');

      // Validate email format
      if (isEmail) {
        const emailSchema = z.string().email();
        const result = emailSchema.safeParse(val);
        if (!result.success) {
          ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message: 'Định dạng email không hợp lệ',
          });
        }
      }
      // Validate username format
      else {
        if (!REGEX.Username.test(val)) {
          ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message: 'Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_)',
          });
        }
      }
    }),

  password: z
    .string()
    .trim()
    .min(6, { message: 'Mật khẩu phải có ít nhất 6 ký tự' })
    .max(256, { message: 'Mật khẩu không được vượt quá 256 ký tự' })
    .regex(REGEX.Password, {
      message: 'Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt',
    }),
});
