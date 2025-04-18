import { User } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { LoginFormFieldProps } from '@/lib/type';

export const UsernameOrEmailField = ({ control }: LoginFormFieldProps) => (
  <FormField
    control={control}
    name="usernameOrEmail"
    render={({ field }) => (
      <FormItem>
        <FormLabel>Username or Email</FormLabel>
        <FormControl>
          <div className="relative">
            <User className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
            <Input
              placeholder="Username or Email"
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
