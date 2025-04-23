export interface Property {
  id: string;
  title: string;
  description: string;
  price: number;
  location: string;
  type: 'sale' | 'rent';
  category: 'apartment' | 'house' | 'villa' | 'land';
  bedrooms: number;
  bathrooms: number;
  area: number;
  image: string;
  featured: boolean;
}

export const sampleProperties: Property[] = [
  {
    id: '1',
    title: 'Căn hộ cao cấp Vinhomes Central Park',
    description: 'Căn hộ 3 phòng ngủ view sông và công viên tuyệt đẹp',
    price: 5500000000,
    location: 'Quận Bình Thạnh, TP.HCM',
    type: 'sale',
    category: 'apartment',
    bedrooms: 3,
    bathrooms: 2,
    area: 110,
    image: '/api/placeholder/800/500',
    featured: true,
  },
  {
    id: '2',
    title: 'Nhà phố liền kề The Sun Avenue',
    description: 'Nhà phố thiết kế hiện đại, nội thất cao cấp',
    price: 35000000,
    location: 'Quận 2, TP.HCM',
    type: 'rent',
    category: 'house',
    bedrooms: 4,
    bathrooms: 3,
    area: 180,
    image: '/api/placeholder/800/500',
    featured: false,
  },
  {
    id: '3',
    title: 'Biệt thự nghỉ dưỡng Phú Quốc',
    description: 'Biệt thự view biển với hồ bơi riêng',
    price: 12000000000,
    location: 'Phú Quốc, Kiên Giang',
    type: 'sale',
    category: 'villa',
    bedrooms: 5,
    bathrooms: 4,
    area: 350,
    image: '/api/placeholder/800/500',
    featured: true,
  },
  {
    id: '4',
    title: 'Đất nền Bảo Lộc',
    description: 'Đất nền view đồi, khí hậu mát mẻ quanh năm',
    price: 2300000000,
    location: 'Bảo Lộc, Lâm Đồng',
    type: 'sale',
    category: 'land',
    bedrooms: 0,
    bathrooms: 0,
    area: 500,
    image: '/api/placeholder/800/500',
    featured: false,
  },
  {
    id: '5',
    title: 'Căn hộ studio Masteri An Phú',
    description: 'Căn hộ studio hiện đại, tiện nghi',
    price: 8000000,
    location: 'Quận 2, TP.HCM',
    type: 'rent',
    category: 'apartment',
    bedrooms: 1,
    bathrooms: 1,
    area: 45,
    image: '/api/placeholder/800/500',
    featured: false,
  },
  {
    id: '6',
    title: 'Nhà phố Thảo Điền',
    description: 'Nhà phố khu compound an ninh 24/7',
    price: 45000000,
    location: 'Quận 2, TP.HCM',
    type: 'rent',
    category: 'house',
    bedrooms: 4,
    bathrooms: 4,
    area: 200,
    image: '/api/placeholder/800/500',
    featured: true,
  },
];
