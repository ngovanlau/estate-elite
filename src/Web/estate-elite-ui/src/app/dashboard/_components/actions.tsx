import { FC } from 'react';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Plus, Users, Calendar, FileText } from 'lucide-react';

const QuickActions: FC = () => {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Quick Actions</CardTitle>
      </CardHeader>
      <CardContent className="space-y-2">
        <Button className="w-full justify-start">
          <Plus className="mr-2 h-4 w-4" />
          Add New Property
        </Button>
        <Button
          variant="outline"
          className="w-full justify-start"
        >
          <Users className="mr-2 h-4 w-4" />
          Manage Clients
        </Button>
        <Button
          variant="outline"
          className="w-full justify-start"
        >
          <Calendar className="mr-2 h-4 w-4" />
          Schedule Viewings
        </Button>
        <Button
          variant="outline"
          className="w-full justify-start"
        >
          <FileText className="mr-2 h-4 w-4" />
          Generate Reports
        </Button>
      </CardContent>
    </Card>
  );
};

export default QuickActions;
