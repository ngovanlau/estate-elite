import { environment } from '@/lib/environment';
import BaseService from './base-service';
import { ApiResponse } from '@/types/response/base-response';
import { PropertyType, Room, Utility } from '@/types/response/property-service';
import { CreatePropertyRequest } from '@/types/request/property-request';

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

  public createProperty = (request: CreatePropertyRequest): Promise<ApiResponse<boolean>> => {
    const formData = new FormData();

    // Append các field đơn giản
    formData.append('title', request.title);
    formData.append('description', request.description);
    formData.append('listingType', request.listingType);
    formData.append('rentPeriod', request.rentPeriod);
    formData.append('area', request.area.toString());
    formData.append('landArea', request.landArea.toString());
    formData.append('buildDate', request.buildDate);
    formData.append('price', request.price.toString());
    formData.append('propertyId', request.propertyId);

    // Append địa chỉ (dạng JSON stringify)
    formData.append('address', JSON.stringify(request.address));

    // Append danh sách phòng
    request.rooms.forEach((room, index) => {
      formData.append(`rooms[${index}].id`, room.id);
      formData.append(`rooms[${index}].quantity`, room.quantity);
    });

    // Append utilityIds (nhiều giá trị cùng key)
    request.utilityIds.forEach((id) => {
      formData.append('utilityIds', id);
    });

    // Append hình ảnh
    request.images.forEach((image) => {
      formData.append('images', image); // hoặc `images[${index}]` tùy API
    });

    return this.instance.post('/property', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  };
}

const propertyService = new PropertyService();
export default propertyService;
