import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

export const News = () => {
  return (
    <Card>
      <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
        {/* News image would go here */}
      </div>
      <CardHeader>
        <CardTitle className="text-lg">Thị trường bất động sản 2025: Dự báo và xu hướng</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="mb-4 text-sm text-slate-500">
          Phân tích về những thay đổi của thị trường bất động sản trong năm 2025 và các xu hướng
          đáng chú ý...
        </p>
        <p className="text-xs text-slate-400">15/04/2025 • 5 phút đọc</p>
      </CardContent>
    </Card>
  );
};
