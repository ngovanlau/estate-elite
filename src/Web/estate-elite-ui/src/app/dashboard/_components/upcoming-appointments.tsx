import { FC } from 'react';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { upcomingAppointments } from './type';

const UpcomingAppointments: FC = () => {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Upcoming Appointments</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-3">
          {upcomingAppointments.map((appointment) => (
            <div
              key={appointment.id}
              className="flex items-center justify-between rounded-md bg-gray-50 p-3 dark:bg-gray-800"
            >
              <div>
                <p className="font-medium">{appointment.title}</p>
                <p className="text-sm text-gray-500 dark:text-gray-400">
                  {appointment.description}
                </p>
              </div>
              <div className="text-right">
                <p className="text-sm font-medium">{appointment.day}</p>
                <p className="text-sm text-gray-500 dark:text-gray-400">{appointment.time}</p>
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
};

export default UpcomingAppointments;
