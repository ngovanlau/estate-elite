import { z } from 'zod';
import { REGEX } from '@/lib/constant';
import { USER_ROLE } from '@/lib/enum';

export const registerFormSchema = z
  .object({
    fullName: z
      .string()
      .trim()
      .min(3, {
        message: 'Họ tên phải có ít nhất 3 ký tự',
      })
      .max(30, {
        message: 'Họ tên không được vượt quá 30 ký tự',
      }),

    username: z
      .string()
      .min(3, { message: 'Tên đăng nhập phải có ít nhất 3 ký tự' })
      .max(30, { message: 'Tên đăng nhập không được vượt quá 30 ký tự' })
      .regex(REGEX.Username, {
        message: 'Tên đăng nhập chỉ được chứa chữ cái (a-z, A-Z), số (0-9) và dấu gạch dưới (_)',
      }),

    email: z.string().email('Địa chỉ email không hợp lệ'),

    role: z.nativeEnum(USER_ROLE, {
      required_error: 'Vui lòng chọn vai trò của bạn',
    }),

    password: z
      .string()
      .min(6, { message: 'Mật khẩu phải có ít nhất 6 ký tự' })
      .max(256, { message: 'Mật khẩu không được vượt quá 256 ký tự' })
      .regex(REGEX.Password, {
        message: 'Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt',
      }),

    confirmationPassword: z
      .string()
      .min(6, { message: 'Mật khẩu xác nhận phải có ít nhất 6 ký tự' })
      .max(256, { message: 'Mật khẩu xác nhận không được vượt quá 256 ký tự' })
      .regex(REGEX.Password, {
        message:
          'Mật khẩu xác nhận phải chứa ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt',
      }),
  })
  .refine((data) => data.password === data.confirmationPassword, {
    message: 'Mật khẩu xác nhận không khớp với mật khẩu đã nhập',
    path: ['confirmationPassword'],
  });
