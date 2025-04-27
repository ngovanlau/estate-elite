import React from 'react';
import { FieldPath, FieldValues, Control } from 'react-hook-form';
import {
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
  FormDescription,
} from '@/components/ui/form';
import { Checkbox } from '@/components/ui/checkbox';

// Props type with strict typing
export type CheckboxFieldProps<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
  label?: string;
  required?: boolean;
  disabled?: boolean;
  className?: string;
  description?: string;
};

export const CheckboxField = <T extends FieldValues>({
  control,
  name,
  label,
  required = false,
  disabled = false,
  className = '',
  description,
}: CheckboxFieldProps<T>): React.ReactElement => {
  return (
    <FormField
      control={control}
      name={name}
      render={({ field }) => (
        <FormItem className={`flex flex-row items-start space-y-0 space-x-3 ${className}`}>
          <FormControl>
            <Checkbox
              checked={field.value}
              onCheckedChange={field.onChange}
              disabled={disabled}
              required={required}
            />
          </FormControl>
          <div className="space-y-1 leading-none">
            {label && (
              <FormLabel>
                {label}
                {required && <span className="text-red-500">*</span>}
              </FormLabel>
            )}
            {description && <FormDescription>{description}</FormDescription>}
          </div>
          <FormMessage />
        </FormItem>
      )}
    />
  );
};
