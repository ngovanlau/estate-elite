import { Button } from "@/components/ui/button";
import Link from "next/link";

export default function HomePage() {
	return (
		<div className="space-y-16">
			{/* Hero Section */}
			<section className="pt-12 pb-24 text-center">
				<h1 className="text-4xl md:text-6xl font-bold mb-6">
					Tìm Ngôi Nhà Mơ Ước Của Bạn
				</h1>
				<p className="text-xl text-gray-600 mb-10 max-w-3xl mx-auto">
					Khám phá hàng ngàn bất động sản đang chờ đợi bạn trên nền tảng của
					chúng tôi
				</p>

				<div className="w-full max-w-4xl mx-auto bg-white p-4 rounded-lg shadow-lg">
					<div className="grid grid-cols-1 md:grid-cols-4 gap-4">
						<div className="col-span-2">
							<input
								type="text"
								placeholder="Nhập địa điểm, khu vực..."
								className="w-full p-3 border rounded-md"
							/>
						</div>
						<div>
							<select className="w-full p-3 border rounded-md">
								<option value="">Loại bất động sản</option>
								<option value="apartment">Căn hộ</option>
								<option value="house">Nhà phố</option>
								<option value="villa">Biệt thự</option>
							</select>
						</div>
						<div>
							<Button className="w-full p-3 h-12">Tìm kiếm</Button>
						</div>
					</div>
				</div>
			</section>

			{/* Featured Properties */}
			<section>
				<div className="flex items-center justify-between mb-8">
					<h2 className="text-3xl font-bold">Bất Động Sản Nổi Bật</h2>
					<Link href="/properties">
						<Button variant="outline">Xem tất cả</Button>
					</Link>
				</div>

				<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
					{/* Property cards would go here */}
					<div className="bg-white rounded-lg shadow-md overflow-hidden">
						<div className="h-48 bg-gray-300"></div>
						<div className="p-4">
							<h3 className="font-bold text-lg mb-2">
								Căn hộ cao cấp Central Park
							</h3>
							<p className="text-gray-600 mb-2">Quận 1, TP. Hồ Chí Minh</p>
							<p className="font-bold text-lg text-blue-600">4.5 tỷ</p>
						</div>
					</div>

					<div className="bg-white rounded-lg shadow-md overflow-hidden">
						<div className="h-48 bg-gray-300"></div>
						<div className="p-4">
							<h3 className="font-bold text-lg mb-2">Nhà phố Garden Homes</h3>
							<p className="text-gray-600 mb-2">Quận 9, TP. Hồ Chí Minh</p>
							<p className="font-bold text-lg text-blue-600">3.2 tỷ</p>
						</div>
					</div>

					<div className="bg-white rounded-lg shadow-md overflow-hidden">
						<div className="h-48 bg-gray-300"></div>
						<div className="p-4">
							<h3 className="font-bold text-lg mb-2">Biệt thự Ocean View</h3>
							<p className="text-gray-600 mb-2">Vũng Tàu</p>
							<p className="font-bold text-lg text-blue-600">7.8 tỷ</p>
						</div>
					</div>
				</div>
			</section>

			{/* Services */}
			<section className="py-12 bg-gray-50 -mx-4 px-4">
				<div className="container mx-auto">
					<h2 className="text-3xl font-bold text-center mb-12">
						Dịch Vụ Của Chúng Tôi
					</h2>

					<div className="grid grid-cols-1 md:grid-cols-3 gap-8">
						<div className="bg-white p-6 rounded-lg shadow-md text-center">
							<div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
								<span className="text-blue-600 text-2xl">🏠</span>
							</div>
							<h3 className="font-bold text-xl mb-3">Mua Bất Động Sản</h3>
							<p className="text-gray-600">
								Tìm và mua bất động sản phù hợp với nhu cầu và ngân sách của bạn
							</p>
						</div>

						<div className="bg-white p-6 rounded-lg shadow-md text-center">
							<div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
								<span className="text-green-600 text-2xl">🔑</span>
							</div>
							<h3 className="font-bold text-xl mb-3">Thuê Bất Động Sản</h3>
							<p className="text-gray-600">
								Khám phá các lựa chọn thuê nhà với giá cả hợp lý và vị trí thuận
								tiện
							</p>
						</div>

						<div className="bg-white p-6 rounded-lg shadow-md text-center">
							<div className="w-16 h-16 bg-purple-100 rounded-full flex items-center justify-center mx-auto mb-4">
								<span className="text-purple-600 text-2xl">📊</span>
							</div>
							<h3 className="font-bold text-xl mb-3">Đầu Tư Bất Động Sản</h3>
							<p className="text-gray-600">
								Tìm hiểu cơ hội đầu tư bất động sản sinh lời với sự tư vấn
								chuyên nghiệp
							</p>
						</div>
					</div>
				</div>
			</section>
		</div>
	);
}
