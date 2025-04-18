// src/app/(main)/properties/[id]/page.tsx
import { PropertyDetails } from './_components/property-details';

export default async function PropertyDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const { id: slug } = await params;

  const propertyData = {
    id: slug,
    title: 'Căn hộ cao cấp tại trung tâm Quận 1',
    address: 'Đường Nguyễn Huệ, Quận 1, TP. HCM',
    price: 5500000000,
    description: `Căn hộ cao cấp với thiết kế hiện đại, view toàn cảnh thành phố. Tọa lạc tại vị trí đắc địa ngay trung tâm Quận 1, thuận tiện di chuyển đến các điểm giải trí, mua sắm và ăn uống.
Căn hộ được thiết kế với không gian mở, tối ưu ánh sáng tự nhiên. Các phòng đều được trang bị nội thất cao cấp, thiết bị điện tử hiện đại đến từ các thương hiệu hàng đầu.
Khu căn hộ được trang bị đầy đủ tiện ích như: hồ bơi, phòng gym, spa, khu vui chơi trẻ em, nhà hàng, siêu thị, và bảo vệ 24/7.`,
    images: [
      '/api/placeholder/800/600',
      '/api/placeholder/800/600',
      '/api/placeholder/800/600',
      '/api/placeholder/800/600',
      '/api/placeholder/800/600',
    ],
    bedrooms: 3,
    bathrooms: 2,
    area: 120,
    features: [
      'Ban công rộng',
      'Bếp hiện đại',
      'Sàn gỗ cao cấp',
      'Tủ âm tường',
      'Máy lạnh trung tâm',
      'Hệ thống an ninh',
      'Cửa sổ kính cường lực',
      'Nội thất cao cấp',
      'Bãi đậu xe riêng',
      'Thang máy tốc độ cao',
      'Smart home',
      'Lối đi riêng',
    ],
    type: 'sale' as const,
    yearBuilt: 2022,
    agentName: 'Nguyễn Văn A',
    agentPhone: '0123456789',
    agentEmail: 'nguyenvana@example.com',
    agentPhoto: '/api/placeholder/150/150',
  };

  return <PropertyDetails {...propertyData} />;
}
