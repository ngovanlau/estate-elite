import { FC } from 'react';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Bed, Bath, Maximize } from 'lucide-react';
import Image from 'next/image';
import { Property } from './type';

interface PropertyCardProps {
  property: Property;
}

const PropertyCard: FC<PropertyCardProps> = ({ property }) => {
  return (
    <div className="flex flex-col gap-4 border-b border-gray-200 pb-4 md:flex-row dark:border-gray-700">
      <div className="md:w-1/3">
        <Image
          src={property.image}
          alt={property.title}
          className="h-40 w-full rounded-md object-cover"
          width={600}
          height={400}
        />
      </div>
      <div className="md:w-2/3">
        <div className="flex items-center justify-between">
          <h3 className="text-lg font-semibold">{property.title}</h3>
          <Badge
            variant={
              property.status === 'active'
                ? 'default'
                : property.status === 'pending'
                  ? 'outline'
                  : 'secondary'
            }
          >
            {property.status}
          </Badge>
        </div>
        <div className="mt-1 flex items-center">
          <Badge
            variant="outline"
            className="mr-2"
          >
            {property.type}
          </Badge>
          <p className="text-lg font-bold text-blue-600 dark:text-blue-400">{property.price}</p>
        </div>
        <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">{property.address}</p>
        <div className="mt-2 flex items-center gap-3 text-sm">
          {property.bedrooms && (
            <div className="flex items-center gap-1">
              <Bed className="h-4 w-4" />
              <span>{property.bedrooms} beds</span>
            </div>
          )}
          {property.bathrooms && (
            <div className="flex items-center gap-1">
              <Bath className="h-4 w-4" />
              <span>{property.bathrooms} baths</span>
            </div>
          )}
          <div className="flex items-center gap-1">
            <Maximize className="h-4 w-4" />
            <span>{property.area}</span>
          </div>
        </div>
        <div className="mt-3 flex gap-2">
          <Button
            size="sm"
            variant="outline"
          >
            View Details
          </Button>
          <Button size="sm">Contact Agent</Button>
        </div>
      </div>
    </div>
  );
};

export default PropertyCard;
