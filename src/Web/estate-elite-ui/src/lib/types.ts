import { z } from 'zod';
import { Control, FieldValues } from 'react-hook-form';
import { registerFormSchema } from '../app/(auth)/register/_components/_validation';
import { loginFormSchema } from '@/app/(auth)/login/_components/_validation';

// Tạo generic type cho form field props
export interface FormFieldProps<T extends FieldValues> {
  control: Control<T>;
}

// Định nghĩa riêng các type values cho từng form
export type RegisterFormValues = z.infer<typeof registerFormSchema>;
export type LoginFormValues = z.infer<typeof loginFormSchema>;

// (Optional) Có thể export dưới dạng named types nếu cần
export type RegisterFormFieldProps = FormFieldProps<RegisterFormValues>;
export type LoginFormFieldProps = FormFieldProps<LoginFormValues>;
