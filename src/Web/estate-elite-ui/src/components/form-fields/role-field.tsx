import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { USER_ROLE } from '@/lib/enum';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { RegisterFormFieldProps } from '@/lib/type';

export const RoleField = ({ control }: RegisterFormFieldProps) => (
  <FormField
    control={control}
    name="role"
    render={({ field }) => (
      <FormItem>
        <FormLabel>Bạn là</FormLabel>
        <FormControl>
          <Select
            required
            onValueChange={field.onChange}
            defaultValue={field.value}
          >
            <SelectTrigger className="w-full">
              <SelectValue placeholder="Chọn vai trò của bạn" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value={USER_ROLE.BUYER}>Khách hàng tìm mua/thuê</SelectItem>
              <SelectItem value={USER_ROLE.SELLER}>Chủ sở hữu bất động sản</SelectItem>
            </SelectContent>
          </Select>
        </FormControl>
        <FormMessage />
      </FormItem>
    )}
  />
);
