import { Eye, Heart, MapPin } from 'lucide-react';
import { LISTING_TYPE } from '@/lib/enum';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import Image from 'next/image';
import { formatCurrency, rentPeriodMap } from '@/lib/utils';
import { Property } from '@/types/response/property-response';
import { useRouter } from 'next/navigation';

type PropertyCardProps = {
  property: Property;
};

const PropertyCard = ({ property }: PropertyCardProps) => {
  const router = useRouter();

  const listingTypeMap = {
    [LISTING_TYPE.RENT]: (
      <span className="rounded bg-amber-600 px-2.5 py-1 text-xs font-medium text-white">Thuê</span>
    ),
    [LISTING_TYPE.SALE]: (
      <span className="rounded bg-blue-600 px-2.5 py-1 text-xs font-medium text-white">Bán</span>
    ),
  };

  const handlePropertyClick = () => {
    router.push(`/properties/${property.id}`);
  };

  return (
    <Card
      onClick={handlePropertyClick}
      className="group cursor-pointer overflow-hidden pt-0 transition-shadow duration-300 hover:shadow-lg"
    >
      <div className="relative">
        <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
          <Image
            src={property.imageUrl}
            alt={property.title}
            fill
            className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        </div>
        <div className="absolute top-3 left-3">{listingTypeMap[property.listingType]}</div>
        <Button
          variant="ghost"
          size="icon"
          className="absolute top-2 right-2 rounded-full bg-white/80 hover:bg-white"
        >
          <Heart className="h-5 w-5" />
        </Button>
      </div>
      <CardHeader className="pb-2">
        <div className="flex justify-between">
          <CardTitle className="line-clamp-1 text-lg transition-colors group-hover:text-blue-600">
            {property.title}
          </CardTitle>
          <div className="flex items-center text-sm text-slate-500">
            <Eye className="mr-1 h-4 w-4" />
            230
          </div>
        </div>
        <CardDescription className="flex items-center text-sm text-slate-500">
          <MapPin className="mr-1 h-4 w-4" />
          <span>{property.address}</span>
        </CardDescription>
      </CardHeader>
      <CardContent className="pb-2">
        <div className="grid grid-cols-3 gap-2 text-sm">
          <div className="rounded bg-slate-100 p-2 text-center">
            <p className="font-medium">
              {' '}
              {formatCurrency(property.price, property.currencyUnit)}
              {property.rentPeriod && `/${rentPeriodMap[property.rentPeriod]}`}
            </p>
            <p className="text-xs text-slate-500">Giá</p>
          </div>
          <div className="rounded bg-slate-100 p-2 text-center">
            <p className="font-medium">{property.area}m²</p>
            <p className="text-xs text-slate-500">Diện tích</p>
          </div>
          <div className="rounded bg-slate-100 p-2 text-center">
            <p className="font-medium">{property.type}</p>
            <p className="text-xs text-slate-500">Loại</p>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertyCard;
