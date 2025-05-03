import { environment } from '@/lib/environment';
import BaseService from './base-service';
import { ApiResponse, PageApiResponse } from '@/types/response/base-response';
import {
  OwnerProperty,
  Property,
  PropertyDetails,
  PropertyType,
  Room,
  Utility,
} from '@/types/response/property-response';
import { CreatePropertyRequest, GetPropertiesRequest } from '@/types/request/property-request';
import { PageRequest } from '@/types/request/base-request';

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
    formData.append('propertyTypeId', request.propertyTypeId);
    if (request.rentPeriod) {
      formData.append('rentPeriod', request.rentPeriod);
    }
    formData.append('area', request.area.toString());
    formData.append('landArea', request.landArea.toString());
    formData.append('buildDate', request.buildDate);
    formData.append('price', request.price.toString());

    // Append địa chỉ (dạng JSON stringify)
    formData.append('address', JSON.stringify(request.address));

    // Append danh sách phòng
    if (request.rooms) {
      formData.append('rooms', JSON.stringify(request.rooms.filter((room) => room.quantity > 0)));
    }

    // Append utilityIds (nhiều giá trị cùng key)
    if (request.utilityIds) {
      formData.append('utilityIds', JSON.stringify(request.utilityIds));
    }

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

  public getOwnerProperties = (request: PageRequest): Promise<PageApiResponse<OwnerProperty[]>> => {
    let url = `/property/owner?pageSize=${request.pageSize}&pageNumber=${request.pageNumber}`;
    if (request.lastEntityId) {
      url += `&lastEntityId=${request.lastEntityId}`;
    }
    return this.instance.get(url);
  };

  public getProperties = (request: GetPropertiesRequest): Promise<PageApiResponse<Property[]>> => {
    let url = `/property?pageSize=${request.pageSize}&pageNumber=${request.pageNumber}`;

    if (request.lastEntityId) {
      url += `&lastEntityId=${request.lastEntityId}`;
    }

    if (request.address) url = `${url}&address=${request.address}`;

    if (request.propertyTypeId) url = `${url}&propertyTypeId=${request.propertyTypeId}`;

    return this.instance.get(url);
  };

  public getPropertyDetails = (id: string): Promise<ApiResponse<PropertyDetails>> => {
    return this.instance.get(`/property/${id}`);
  };
}

const propertyService = new PropertyService();
export default propertyService;
