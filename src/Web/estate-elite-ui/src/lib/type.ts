import { z } from 'zod';
import { Control } from 'react-hook-form';
import { registerFormSchema } from '../app/(auth)/register/_components/validation';
import { loginFormSchema } from '@/app/(auth)/login/_components/validation';

export type RegisterFormValues = z.infer<typeof registerFormSchema>;
export interface RegisterFormFieldProps {
  control: Control<RegisterFormValues>;
}

export type LoginFormValues = z.infer<typeof loginFormSchema>;
export interface LoginFormFieldProps {
  control: Control<LoginFormValues>;
}
