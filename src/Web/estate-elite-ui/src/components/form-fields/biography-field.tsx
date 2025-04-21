import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { FormFieldProps } from '@/lib/types';
import { FieldPath, FieldValues } from 'react-hook-form';
import { Textarea } from '../ui/textarea';

export const BiographyField = <T extends FieldValues>({ control }: FormFieldProps<T>) => (
  <FormField
    control={control}
    name={'biography' as FieldPath<T>}
    render={({ field }) => (
      <FormItem>
        <FormLabel>Giới thiệu</FormLabel>
        <FormControl>
          <Textarea
            placeholder="Viết một vài dòng về công ty"
            className="resize-none"
            {...field}
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    )}
  />
);
