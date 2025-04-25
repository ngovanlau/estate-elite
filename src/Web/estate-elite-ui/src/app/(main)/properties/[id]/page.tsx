'use client';

import { useEffect, useState } from 'react';
import { PropertyDetails as PropertyDetailsType } from '@/types/response/property-response';
import propertyService from '@/services/property-service';
import toast from 'react-hot-toast';
import { useParams } from 'next/navigation';
import { PropertyInfoDetails } from './_components/property-info-details';

export default function PropertyDetailPage() {
  const params = useParams();
  const slug = params.id as string;
  const [propertyDetails, setPropertyDetails] = useState<PropertyDetailsType | undefined>();
  const [loading, setLoading] = useState(true);

  const fetchPropertyDetails = async (propertySlug: string) => {
    try {
      setLoading(true);
      const response = await propertyService.getPropertyDetails(propertySlug);
      if (!response.succeeded) {
        toast.error('Không tìm thấy bất động sản này');
        throw new Error('Property not found');
      }

      setPropertyDetails(response.data);
    } catch (error) {
      console.error('Error fetching property details:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (slug) {
      fetchPropertyDetails(slug);
    }
  }, [slug]);

  if (loading) {
    return <div className="flex min-h-screen items-center justify-center">Loading...</div>;
  }

  if (!propertyDetails) {
    return <div className="flex min-h-screen items-center justify-center">Property not found</div>;
  }

  return <PropertyInfoDetails details={propertyDetails} />;
}
