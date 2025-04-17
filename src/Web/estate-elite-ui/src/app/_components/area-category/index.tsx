import { Card, CardContent } from '@/components/ui/card';

const AreaCategory = () => {
  return (
    <Card className="overflow-hidden">
      <div className="h-40 bg-slate-200">{/* Area image would go here */}</div>
      <CardContent className="pt-4">
        <h3 className="font-bold">TP. Hồ Chí Minh</h3>
        <p className="text-sm text-slate-500">2,345 bất động sản</p>
      </CardContent>
    </Card>
  );
};

export default AreaCategory;
