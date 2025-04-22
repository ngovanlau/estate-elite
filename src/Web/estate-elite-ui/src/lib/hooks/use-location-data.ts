import { District, Province, Ward } from '@/types';
import { useEffect, useState } from 'react';
import toast from 'react-hot-toast';

export const useLocationData = () => {
  const [provinces, setProvinces] = useState<Province[]>([]);
  const [districts, setDistricts] = useState<District[]>([]);
  const [wards, setWards] = useState<Ward[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchProvinces = async () => {
      try {
        setIsLoading(true);
        const response = await fetch('/api/provinces');
        if (!response.ok) {
          throw new Error('Failed to fetch provinces');
        }
        const data = await response.json();
        setProvinces(data);
      } catch (error) {
        console.error('Error fetching provinces:', error);
        toast.error('Không thể tải danh sách tỉnh/thành phố');
      } finally {
        setIsLoading(false);
      }
    };

    fetchProvinces();
  }, []);

  return {
    provinces,
    districts,
    wards,
    setDistricts,
    setWards,
    isLoading,
  };
};
