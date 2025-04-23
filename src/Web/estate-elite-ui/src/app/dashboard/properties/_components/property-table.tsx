// components/property/PropertyTable.tsx
import { Edit, MoreHorizontal, Trash2 } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { formatPrice } from './type';
import { LISTING_TYPE, PROPERTY_STATUS } from '@/lib/enum';
import { OwnerProperty } from '@/types/response/property-response';

interface PropertyTableProps {
  properties: OwnerProperty[];
  onDelete: (id: string) => void;
  onEdit: (property: OwnerProperty) => void;
}

export function PropertyTable({ properties, onDelete, onEdit }: PropertyTableProps) {
  const propertyStatusMap: {
    [listingType in LISTING_TYPE]: {
      [status in PROPERTY_STATUS]: {
        label: string;
        variant: 'outline' | 'secondary' | 'destructive' | 'default';
      };
    };
  } = {
    [LISTING_TYPE.SALE]: {
      [PROPERTY_STATUS.PENDING]: {
        label: 'Chờ duyệt',
        variant: 'outline',
      },
      [PROPERTY_STATUS.ACTIVE]: {
        label: 'Đang bán',
        variant: 'secondary',
      },
      [PROPERTY_STATUS.COMPLETED]: {
        label: 'Đã bán',
        variant: 'destructive',
      },
    },
    [LISTING_TYPE.RENT]: {
      [PROPERTY_STATUS.PENDING]: {
        label: 'Chờ duyệt',
        variant: 'outline',
      },
      [PROPERTY_STATUS.ACTIVE]: {
        label: 'Đang cho thuê', // Changed from "Đang bán" to be more accurate for rentals
        variant: 'destructive',
      },
      [PROPERTY_STATUS.COMPLETED]: {
        label: 'Đã cho thuê', // Changed from "Đã bán" to be more accurate for rentals
        variant: 'destructive',
      },
    },
  };

  const listingTypeMap = {
    [LISTING_TYPE.SALE]: 'Bán',
    [LISTING_TYPE.RENT]: 'Cho thuê',
  };

  const { label, variant } = propertyStatusMap[LISTING_TYPE.SALE][PROPERTY_STATUS.PENDING];

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead className="w-[250px]">Tên BĐS</TableHead>
          <TableHead>Địa chỉ</TableHead>
          <TableHead>Loại giao dịch</TableHead>
          <TableHead className="text-right">Giá</TableHead>
          <TableHead>Trạng thái</TableHead>
          <TableHead>Loại</TableHead>
          <TableHead className="text-right">Diện tích</TableHead>
          <TableHead className="text-center">Thao tác</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {properties.length > 0 ? (
          properties.map((property) => (
            <TableRow key={property.id}>
              <TableCell className="font-medium">{property.title}</TableCell>
              <TableCell className="max-w-[200px] truncate">{property.address}</TableCell>
              <TableCell>{listingTypeMap[property.listingType]}</TableCell>
              <TableCell className="text-right">{formatPrice(property.price)}</TableCell>
              <TableCell>
                <Badge variant={variant}>{label}</Badge>
              </TableCell>
              <TableCell>{property.type}</TableCell>
              <TableCell className="text-right">{property.area} m²</TableCell>
              <TableCell className="text-center">
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button
                      variant="ghost"
                      size="icon"
                    >
                      <MoreHorizontal className="h-4 w-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuLabel>Thao tác</DropdownMenuLabel>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem onClick={() => onEdit(property)}>
                      <Edit className="mr-2 h-4 w-4" /> Chỉnh sửa
                    </DropdownMenuItem>
                    <DropdownMenuItem
                      className="text-red-600"
                      onClick={() => onDelete(property.id)}
                    >
                      <Trash2 className="mr-2 h-4 w-4" /> Xóa
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))
        ) : (
          <TableRow>
            <TableCell
              colSpan={7}
              className="py-6 text-center text-gray-500"
            >
              Không tìm thấy bất động sản nào
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
}
