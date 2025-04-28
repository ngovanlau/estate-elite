import toast from 'react-hot-toast';
import { propertySchema } from './type';
import propertyService from '@/services/property-service';
import { z } from 'zod';
import { CreatePropertyRequest } from '@/types/request/property-request';

export const submitPropertyForm = async (
  values: z.infer<typeof propertySchema>,
  onSuccess: () => void
): Promise<void> => {
  const request: CreatePropertyRequest = {
    title: values.title,
    description: values.description,
    listingType: values.listingType,
    area: values.area,
    landArea: values.landArea,
    buildDate: values.buildDate,
    price: values.price,
    propertyTypeId: values.propertyType,
    address: {
      country: 'Vietnam',
      province: values.province,
      district: values.district,
      ward: values.ward,
      details: values.address,
    },
    utilityIds: values.utilities,
    images: values.images,
  };

  if (values.rentPeriod) {
    request.rentPeriod = values.rentPeriod;
  }

  if (values.rooms) {
    request.rooms = values.rooms.map((room) => ({
      id: room.id,
      quantity: room.quantity,
    }));
  }

  try {
    // In a real app, you would send formData to your API
    const response = await propertyService.createProperty(request);

    if (!response.succeeded) {
      throw new Error('Failed to submit property');
    }

    toast.success('Thông tin bất động sản đã được gửi');
    onSuccess();
  } catch (error) {
    console.error('Error submitting property form:', error);
    toast.error('Có lỗi xảy ra khi gửi thông tin');
  }
};
