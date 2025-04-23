'use client';

import React from 'react';
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
  CardDescription,
} from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { HeartIcon, MapPin, Bed, Bath, Ruler } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';
import { Property } from './property';

interface PropertyCardProps {
  property: Property;
  formatPrice: (price: number, type: 'sale' | 'rent') => string;
}

export function PropertyCard({ property, formatPrice }: PropertyCardProps) {
  return (
    <Link
      href={`/properties/${property.id}`}
      className="group"
    >
      <Card className="group h-full overflow-hidden transition-shadow duration-300 hover:shadow-lg">
        <div className="relative h-48 w-full overflow-hidden">
          <Image
            src={property.image}
            alt={property.title}
            fill
            className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
          {property.featured && (
            <Badge className="absolute top-2 left-2 bg-amber-500">Nổi bật</Badge>
          )}
          <Badge
            className={`absolute top-2 right-2 ${property.type === 'sale' ? 'bg-blue-500' : 'bg-green-500'}`}
          >
            {property.type === 'sale' ? 'Bán' : 'Cho thuê'}
          </Badge>
          <Button
            size="icon"
            variant="ghost"
            className="absolute right-2 bottom-2 h-8 w-8 rounded-full bg-white/70 hover:bg-white"
          >
            <HeartIcon className="h-4 w-4 text-gray-600 hover:text-red-500" />
          </Button>
        </div>

        <CardHeader className="pb-2">
          <CardTitle className="line-clamp-1 text-lg transition-colors group-hover:text-blue-600">
            {property.title}
          </CardTitle>
          <CardDescription className="flex items-center text-sm">
            <MapPin className="mr-1 h-3.5 w-3.5 text-gray-500" />
            {property.location}
          </CardDescription>
        </CardHeader>

        <CardContent className="pb-3">
          <p className="text-lg font-semibold text-blue-600">
            {formatPrice(property.price, property.type)}
          </p>
          <p className="mt-1 line-clamp-2 text-sm text-gray-600">{property.description}</p>
        </CardContent>

        <CardFooter className="border-t pt-3">
          <div className="flex w-full justify-between text-sm text-gray-600">
            {property.category !== 'land' && (
              <>
                <div className="flex items-center">
                  <Bed className="mr-1 h-4 w-4" />
                  {property.bedrooms} phòng ngủ
                </div>
                <div className="flex items-center">
                  <Bath className="mr-1 h-4 w-4" />
                  {property.bathrooms} phòng tắm
                </div>
              </>
            )}
            <div className={`flex items-center ${property.category !== 'land' ? 'ml-auto' : ''}`}>
              <Ruler className="mr-1 h-4 w-4" />
              {property.area} m²
            </div>
          </div>
        </CardFooter>
      </Card>
    </Link>
  );
}
