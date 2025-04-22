import React from 'react';
import { LucideIcon } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { FieldPath, FieldValues, Control } from 'react-hook-form';

// Define supported input types
type InputType =
  | 'text'
  | 'email'
  | 'password'
  | 'search'
  | 'tel'
  | 'number'
  | 'date'
  | 'time'
  | 'url';

// Props type with strict typing
export type InputFieldProps<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
  label?: string;
  placeholder?: string;
  type?: InputType;
  icon?: LucideIcon | null;
  required?: boolean;
  disabled?: boolean;
  className?: string;
  description?: string;
  autoComplete?: string;
};

export const InputField = <T extends FieldValues>({
  control,
  name,
  label,
  placeholder = '',
  type = 'text',
  icon: CustomIcon,
  required = false,
  disabled = false,
  className = '',
  description,
  autoComplete,
}: InputFieldProps<T>): React.ReactElement => {
  return (
    <FormField
      control={control}
      name={name}
      render={({ field }) => (
        <FormItem className={className}>
          {label && (
            <FormLabel>
              {label}
              {required && <span className="text-red-500">*</span>}
            </FormLabel>
          )}
          <FormControl>
            <div className="relative">
              {CustomIcon && <CustomIcon className="absolute top-3 left-3 h-4 w-4 text-gray-400" />}
              <Input
                min="0"
                placeholder={placeholder}
                className={CustomIcon ? 'pl-10' : ''}
                disabled={disabled}
                type={type}
                autoComplete={autoComplete}
                {...field}
              />
            </div>
          </FormControl>
          {description && <p className="text-sm text-gray-500">{description}</p>}
          <FormMessage />
        </FormItem>
      )}
    />
  );
};
