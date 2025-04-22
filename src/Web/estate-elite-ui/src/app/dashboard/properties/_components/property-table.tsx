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
import { formatPrice, Property } from './type';

interface PropertyTableProps {
  properties: Property[];
  onDelete: (id: string) => void;
  onEdit: (property: Property) => void;
}

export function PropertyTable({ properties, onDelete, onEdit }: PropertyTableProps) {
  const getStatusVariant = (
    status: Property['status']
  ): 'default' | 'secondary' | 'destructive' | 'outline' => {
    switch (status) {
      case 'Đang bán':
        return 'default';
      case 'Đang cho thuê':
        return 'secondary';
      case 'Đã bán':
        return 'destructive';
      case 'Đã cho thuê':
        return 'outline';
      default:
        return 'default';
    }
  };

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead className="w-[250px]">Tên BĐS</TableHead>
          <TableHead>Địa chỉ</TableHead>
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
              <TableCell className="text-right">{formatPrice(property.price)}</TableCell>
              <TableCell>
                <Badge variant={getStatusVariant(property.status)}>{property.status}</Badge>
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
