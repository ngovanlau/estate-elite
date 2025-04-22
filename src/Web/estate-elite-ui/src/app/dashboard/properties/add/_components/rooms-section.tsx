import React, { useEffect, useState } from 'react';
import { propertySchema } from './type';
import { UseFormReturn } from 'react-hook-form';
import { z } from 'zod';
import propertyService from '@/services/property-service';
import { Room } from '@/types/response/property-service';
import { InputField } from '@/components/form-fields/input-field';

interface RoomsSectionProps {
  form: UseFormReturn<z.infer<typeof propertySchema>>;
}

export const RoomsSection = ({ form }: RoomsSectionProps) => {
  const [roomTypes, setRoomTypes] = useState<Room[]>([]);

  const fetchRooms = async () => {
    try {
      const response = await propertyService.getRoom();
      if (response.succeeded) {
        setRoomTypes(response.data);

        if (!form.getValues().rooms) {
          const initialRooms = response.data.map((room) => ({
            id: room.id,
            name: room.name,
            quantity: 0,
          }));
          form.setValue('rooms', initialRooms);
        }
      }
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchRooms();
  }, []);

  return (
    <div className="space-y-4">
      <div className="text-lg font-medium">Thông tin thêm</div>
      <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
        {roomTypes.map((roomType, index) => (
          <InputField
            key={roomType.id}
            control={form.control}
            name={`rooms.${index}.quantity`}
            label={roomType.name}
            type="number"
            placeholder="Nhập số lượng"
          />
        ))}
      </div>
    </div>
  );
};
