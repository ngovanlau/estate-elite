import { Mail } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { FormFieldProps } from '@/lib/types';
import { FieldPath, FieldValues } from 'react-hook-form';

export const EmailField = <T extends FieldValues>({ control }: FormFieldProps<T>) => (
  <FormField
    control={control}
    name={'email' as FieldPath<T>}
    render={({ field }) => (
      <FormItem>
        <FormLabel>Email</FormLabel>
        <FormControl>
          <div className="relative">
            <Mail className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
            <Input
              placeholder="name@example.com"
              className="pr-10 pl-10"
              {...field}
            />
          </div>
        </FormControl>
        <FormMessage />
      </FormItem>
    )}
  />
);
