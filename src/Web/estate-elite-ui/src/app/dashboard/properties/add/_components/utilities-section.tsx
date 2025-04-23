import React, { useEffect, useState } from 'react';
import {
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormDescription,
  FormMessage,
} from '@/components/ui/form';
import { Checkbox } from '@/components/ui/checkbox';
import { propertySchema } from './type';
import { UseFormReturn } from 'react-hook-form';
import { z } from 'zod';
import { Utility } from '@/types/response/property-response';
import propertyService from '@/services/property-service';

interface UtilitiesSectionProps {
  form: UseFormReturn<z.infer<typeof propertySchema>>;
}

export const UtilitiesSection = ({ form }: UtilitiesSectionProps) => {
  const [utilities, setUtilities] = useState<Utility[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchUtilities = async () => {
      setIsLoading(true);
      try {
        const response = await propertyService.getUtility();
        if (response.succeeded) {
          setUtilities(response.data);

          // Initialize utilities field if it doesn't exist
          const currentUtilities = form.getValues().utilities || [];
          if (currentUtilities.length === 0) {
            form.setValue('utilities', []);
          }
        }
      } catch (error) {
        console.error('Failed to fetch utilities:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchUtilities();
  }, [form]);

  return (
    <FormField
      control={form.control}
      name="utilities"
      render={({ field }) => (
        <FormItem>
          <div className="mb-4">
            <FormLabel>Tiện ích</FormLabel>
            <FormDescription>Chọn các tiện ích có sẵn cho bất động sản</FormDescription>
          </div>

          {isLoading ? (
            <div className="text-sm text-gray-500">Đang tải tiện ích...</div>
          ) : (
            <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
              {utilities.map((utility) => (
                <FormItem
                  key={utility.id}
                  className="flex flex-row items-start space-y-0 space-x-3"
                >
                  <FormControl>
                    <Checkbox
                      checked={field.value?.includes(utility.id)}
                      onCheckedChange={(checked) => {
                        if (checked) {
                          field.onChange([...(field.value || []), utility.id]);
                        } else {
                          field.onChange(field.value?.filter((id) => id !== utility.id) || []);
                        }
                      }}
                    />
                  </FormControl>
                  <FormLabel className="text-sm font-normal">{utility.name}</FormLabel>
                </FormItem>
              ))}
            </div>
          )}

          <FormMessage />
        </FormItem>
      )}
    />
  );
};
