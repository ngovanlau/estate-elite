// components/property/PropertyFilterBar.tsx
import { Search, Filter } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';

interface PropertyFilterBarProps {
  searchTerm: string;
  setSearchTerm: (term: string) => void;
  filterStatus: string;
  setFilterStatus: (status: string) => void;
  filterType: string;
  setFilterType: (type: string) => void;
}

export function PropertyFilterBar({
  searchTerm,
  setSearchTerm,
  filterStatus,
  setFilterStatus,
  filterType,
  setFilterType,
}: PropertyFilterBarProps) {
  return (
    <Card className="mb-6">
      <CardContent>
        <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <div className="flex flex-1 items-center space-x-2">
            <Search className="h-4 w-4" />
            <Input
              placeholder="Tìm kiếm theo tên, địa chỉ..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-md"
            />
          </div>
          <div className="flex flex-wrap gap-2">
            <Select
              value={filterStatus}
              onValueChange={setFilterStatus}
            >
              <SelectTrigger className="w-[180px]">
                <SelectValue placeholder="Trạng thái" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Tất cả trạng thái</SelectItem>
                <SelectItem value="Đang bán">Đang bán</SelectItem>
                <SelectItem value="Đang cho thuê">Đang cho thuê</SelectItem>
                <SelectItem value="Đã bán">Đã bán</SelectItem>
                <SelectItem value="Đã cho thuê">Đã cho thuê</SelectItem>
              </SelectContent>
            </Select>
            <Select
              value={filterType}
              onValueChange={setFilterType}
            >
              <SelectTrigger className="w-[180px]">
                <SelectValue placeholder="Loại BĐS" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Tất cả loại</SelectItem>
                <SelectItem value="Căn hộ">Căn hộ</SelectItem>
                <SelectItem value="Nhà phố">Nhà phố</SelectItem>
                <SelectItem value="Biệt thự">Biệt thự</SelectItem>
                <SelectItem value="Đất nền">Đất nền</SelectItem>
              </SelectContent>
            </Select>
            <Button
              variant="outline"
              size="icon"
            >
              <Filter className="h-4 w-4" />
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
