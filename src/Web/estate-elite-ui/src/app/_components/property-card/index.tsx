import { Eye, Heart, MapPin } from 'lucide-react';
import { LISTING_TYPE } from '@/lib/enum';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Button } from '@/components/ui/button';

const PropertyCard = () => {
  const listingTypeMap = {
    [LISTING_TYPE.SALE]: (
      <span className="rounded bg-amber-600 px-2.5 py-1 text-xs font-medium text-white">Thuê</span>
    ),
    [LISTING_TYPE.RENT]: (
      <span className="rounded bg-blue-600 px-2.5 py-1 text-xs font-medium text-white">Bán</span>
    ),
  };

  return (
    <Card className="pt-0">
      <div className="relative">
        <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
          {/* Image would be placed here */}
        </div>
        <div className="absolute top-3 left-3">{listingTypeMap[LISTING_TYPE.SALE]}</div>
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
          <CardTitle className="text-lg">5.2 tỷ</CardTitle>
          <div className="flex items-center text-sm text-slate-500">
            <Eye className="mr-1 h-4 w-4" />
            230
          </div>
        </div>
        <CardDescription className="text-base font-medium">
          Căn hộ 3 phòng ngủ Vinhomes Central Park
        </CardDescription>
      </CardHeader>
      <CardContent className="pb-2">
        <div className="mb-3 flex items-center text-sm text-slate-500">
          <MapPin className="mr-1 h-4 w-4" />
          <span>Quận Bình Thạnh, TP. Hồ Chí Minh</span>
        </div>
        <div className="grid grid-cols-3 gap-2 text-sm">
          <div className="rounded bg-slate-100 p-2 text-center">
            <p className="font-medium">90m²</p>
            <p className="text-xs text-slate-500">Diện tích</p>
          </div>
          <div className="rounded bg-slate-100 p-2 text-center">
            <p className="font-medium">3</p>
            <p className="text-xs text-slate-500">Phòng ngủ</p>
          </div>
          <div className="rounded bg-slate-100 p-2 text-center">
            <p className="font-medium">2</p>
            <p className="text-xs text-slate-500">Phòng tắm</p>
          </div>
        </div>
      </CardContent>
      <CardFooter>
        <Button
          variant="default"
          className="w-full"
        >
          Xem chi tiết
        </Button>
      </CardFooter>
    </Card>
  );
};

export default PropertyCard;
