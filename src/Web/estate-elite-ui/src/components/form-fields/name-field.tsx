import { User } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { RegisterFormFieldProps } from '@/lib/types';

export const NameField = ({ control }: RegisterFormFieldProps) => (
  <FormField
    control={control}
    name="fullName"
    render={({ field }) => (
      <FormItem>
        <FormLabel>Họ và tên</FormLabel>
        <FormControl>
          <div className="relative">
            <User className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
            <Input
              placeholder="Ngo Van Lau"
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
