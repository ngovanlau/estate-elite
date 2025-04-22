import { environment } from '@/lib/environment';
import BaseService from './base-service';
import { ApiResponse } from '@/types/response/base-response';
import { PropertyType, Room, Utility } from '@/types/response/property-service';

class PropertyService extends BaseService {
  public constructor() {
    super(environment.propertyServiceApi + '/api/');
  }

  public getPropertyType = (): Promise<ApiResponse<PropertyType[]>> => {
    return this.instance.get('/propertytype');
  };

  public getUtility = (): Promise<ApiResponse<Utility[]>> => {
    return this.instance.get('/utility');
  };

  public getRoom = (): Promise<ApiResponse<Room[]>> => {
    return this.instance.get('/room');
  };
}

const propertyService = new PropertyService();
export default propertyService;
