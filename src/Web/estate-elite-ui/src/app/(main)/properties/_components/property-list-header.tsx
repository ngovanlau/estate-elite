'use client';

import React from 'react';
import { Button } from '@/components/ui/button';
import { Filter, ArrowUpDown } from 'lucide-react';

interface PropertyListHeaderProps {
  count: number;
}

export function PropertyListHeader({ count }: PropertyListHeaderProps) {
  return (
    <div className="mb-4 flex items-center justify-between">
      <p className="text-gray-600">Hiển thị {count} bất động sản</p>
      <div className="flex items-center gap-2">
        <Button
          variant="outline"
          size="sm"
        >
          <Filter className="mr-2 h-4 w-4" />
          Bộ lọc
        </Button>
        <Button
          variant="outline"
          size="sm"
        >
          <ArrowUpDown className="mr-2 h-4 w-4" />
          Sắp xếp
        </Button>
      </div>
    </div>
  );
}
