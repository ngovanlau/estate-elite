import toast from 'react-hot-toast';
import { PropertyFormValues } from './type';

export const submitPropertyForm = async (
  data: PropertyFormValues,
  onSuccess: () => void
): Promise<void> => {
  try {
    // Create FormData to send both form data and files
    const formData = new FormData();

    // Add form data fields to formData
    Object.entries(data).forEach(([key, value]) => {
      if (key !== 'images' && key !== 'features') {
        // Ensure we're only passing string values
        if (value !== undefined && value !== null) {
          formData.append(key, String(value));
        }
      }
    });

    // Add features with the correct type handling
    if (data.features && data.features.length > 0) {
      data.features.forEach((feature) => {
        formData.append('features', feature);
      });
    }

    // Add image files with the correct type handling
    if (data.images && data.images.length > 0) {
      data.images.forEach((image) => {
        formData.append('images', image);
      });
    }

    // In a real app, you would send formData to your API
    // const response = await fetch('/api/properties', {
    //   method: 'POST',
    //   body: formData,
    // });

    // if (!response.ok) {
    //   throw new Error('Failed to submit property');
    // }

    toast.success('Thông tin bất động sản đã được gửi');
    onSuccess();
  } catch (error) {
    console.error('Error submitting property form:', error);
    toast.error('Có lỗi xảy ra khi gửi thông tin');
  }
};
