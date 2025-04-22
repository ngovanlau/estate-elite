// components/property/PropertyFormDialog.tsx
import { useState, useEffect } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Property } from './type';

interface PropertyFormDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSave: (property: Omit<Property, 'id' | 'createdAt'> & { id?: string }) => void;
  editProperty?: Property | null;
}

export function PropertyFormDialog({
  open,
  onOpenChange,
  onSave,
  editProperty = null,
}: PropertyFormDialogProps) {
  const isEditing = !!editProperty;

  const [formData, setFormData] = useState<Omit<Property, 'id' | 'createdAt'> & { id?: string }>({
    title: '',
    address: '',
    price: 0,
    status: 'Đang bán',
    type: 'Căn hộ',
    area: 0,
    bedrooms: 0,
    bathrooms: 0,
  });

  useEffect(() => {
    if (editProperty) {
      setFormData({
        id: editProperty.id,
        title: editProperty.title,
        address: editProperty.address,
        price: editProperty.price,
        status: editProperty.status,
        type: editProperty.type,
        area: editProperty.area,
        bedrooms: editProperty.bedrooms,
        bathrooms: editProperty.bathrooms,
      });
    } else {
      setFormData({
        title: '',
        address: '',
        price: 0,
        status: 'Đang bán',
        type: 'Căn hộ',
        area: 0,
        bedrooms: 0,
        bathrooms: 0,
      });
    }
  }, [editProperty, open]);

  const handleChange = (field: keyof typeof formData, value: string | number) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = () => {
    onSave(formData);
    onOpenChange(false);
  };

  return (
    <Dialog
      open={open}
      onOpenChange={onOpenChange}
    >
      <DialogContent className="sm:max-w-xl">
        <DialogHeader>
          <DialogTitle>
            {isEditing ? 'Chỉnh sửa bất động sản' : 'Thêm bất động sản mới'}
          </DialogTitle>
          <DialogDescription>
            Điền thông tin chi tiết về bất động sản vào form dưới đây.
          </DialogDescription>
        </DialogHeader>
        <div className="grid gap-4 py-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="title">Tiêu đề</Label>
              <Input
                id="title"
                placeholder="Nhập tiêu đề bất động sản"
                value={formData.title}
                onChange={(e) => handleChange('title', e.target.value)}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="address">Địa chỉ</Label>
              <Input
                id="address"
                placeholder="Nhập địa chỉ"
                value={formData.address}
                onChange={(e) => handleChange('address', e.target.value)}
              />
            </div>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="price">Giá</Label>
              <Input
                id="price"
                type="number"
                placeholder="Nhập giá"
                value={formData.price}
                onChange={(e) => handleChange('price', Number(e.target.value))}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="area">Diện tích (m²)</Label>
              <Input
                id="area"
                type="number"
                placeholder="Nhập diện tích"
                value={formData.area}
                onChange={(e) => handleChange('area', Number(e.target.value))}
              />
            </div>
          </div>
          <div className="grid grid-cols-3 gap-4">
            <div className="space-y-2">
              <Label htmlFor="type">Loại bất động sản</Label>
              <Select
                value={formData.type}
                onValueChange={(value) => handleChange('type', value as Property['type'])}
              >
                <SelectTrigger id="type">
                  <SelectValue placeholder="Chọn loại" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="Căn hộ">Căn hộ</SelectItem>
                  <SelectItem value="Nhà phố">Nhà phố</SelectItem>
                  <SelectItem value="Biệt thự">Biệt thự</SelectItem>
                  <SelectItem value="Đất nền">Đất nền</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label htmlFor="bedrooms">Phòng ngủ</Label>
              <Input
                id="bedrooms"
                type="number"
                placeholder="Số phòng ngủ"
                value={formData.bedrooms}
                onChange={(e) => handleChange('bedrooms', Number(e.target.value))}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="bathrooms">Phòng tắm</Label>
              <Input
                id="bathrooms"
                type="number"
                placeholder="Số phòng tắm"
                value={formData.bathrooms}
                onChange={(e) => handleChange('bathrooms', Number(e.target.value))}
              />
            </div>
          </div>
          <div className="space-y-2">
            <Label htmlFor="status">Trạng thái</Label>
            <Select
              value={formData.status}
              onValueChange={(value) => handleChange('status', value as Property['status'])}
            >
              <SelectTrigger id="status">
                <SelectValue placeholder="Chọn trạng thái" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="Đang bán">Đang bán</SelectItem>
                <SelectItem value="Đang cho thuê">Đang cho thuê</SelectItem>
                <SelectItem value="Đã bán">Đã bán</SelectItem>
                <SelectItem value="Đã cho thuê">Đã cho thuê</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
        <DialogFooter>
          <Button
            variant="outline"
            onClick={() => onOpenChange(false)}
          >
            Hủy
          </Button>
          <Button onClick={handleSubmit}>Lưu</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
