import { User } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { FormFieldProps } from '@/lib/types';
import { FieldPath, FieldValues } from 'react-hook-form';

export const FullNameField = <T extends FieldValues>({ control }: FormFieldProps<T>) => (
  <FormField
    control={control}
    name={'fullName' as FieldPath<T>}
    render={({ field }) => (
      <FormItem>
        <FormLabel>Họ và tên</FormLabel>
        <FormControl>
          <div className="relative">
            <User className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
            <Input
              placeholder=""
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
