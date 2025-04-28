'use client';

import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Heart, Share2 } from 'lucide-react';

const ActionsCard = () => {
  return (
    <Card>
      <CardContent className="pt-6">
        <h3 className="mb-4 font-semibold">Gửi yêu cầu</h3>
        <div className="space-y-4">
          <Button
            variant="outline"
            className="flex w-full justify-between"
          >
            <span>Lưu</span>
            <Heart className="h-5 w-5" />
          </Button>
          <Button
            variant="outline"
            className="flex w-full justify-between"
          >
            <span>Chia sẻ</span>
            <Share2 className="h-5 w-5" />
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};

export default ActionsCard;
