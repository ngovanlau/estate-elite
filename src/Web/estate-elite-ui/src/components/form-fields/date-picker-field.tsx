import React from 'react';
import dayjs from 'dayjs';
import { Calendar as CalendarIcon } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import { FieldPath, FieldValues, Control } from 'react-hook-form';

// Props type with strict typing
export type DatePickerFieldProps<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
  label?: string;
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
  className?: string;
  description?: string;
  formatString?: string;
};

export const DatePickerField = <T extends FieldValues>({
  control,
  name,
  label,
  placeholder = 'Pick a date',
  required = false,
  disabled = false,
  className = '',
  description,
  formatString = 'MMM D, YYYY', // Default format: 'Apr 23, 2024'
}: DatePickerFieldProps<T>): React.ReactElement => {
  // Convert dayjs date to JavaScript Date for the Calendar component
  const dayjsToDate = (value: string) => {
    return value ? dayjs(value).toDate() : undefined;
  };

  // Convert JavaScript Date to dayjs for form values
  const dateToFormValue = (date: Date | undefined) => {
    return date ? dayjs(date).toISOString() : undefined;
  };

  return (
    <FormField
      control={control}
      name={name}
      render={({ field }) => {
        // Convert the ISO string stored in form to a Date object for the Calendar
        const dateValue = field.value ? dayjsToDate(field.value) : undefined;

        return (
          <FormItem className={className}>
            {label && (
              <FormLabel>
                {label}
                {required && <span className="text-red-500">*</span>}
              </FormLabel>
            )}
            <FormControl>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant="outline"
                    className={cn(
                      'w-full justify-start text-left font-normal',
                      !field.value && 'text-gray-500',
                      disabled && 'cursor-not-allowed opacity-50'
                    )}
                    disabled={disabled}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {field.value ? (
                      dayjs(field.value).format(formatString)
                    ) : (
                      <span>{placeholder}</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent
                  className="w-auto p-0"
                  align="start"
                >
                  <Calendar
                    mode="single"
                    selected={dateValue}
                    onSelect={(date) => field.onChange(dateToFormValue(date))}
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </FormControl>
            {description && <p className="text-sm text-gray-500">{description}</p>}
            <FormMessage />
          </FormItem>
        );
      }}
    />
  );
};
