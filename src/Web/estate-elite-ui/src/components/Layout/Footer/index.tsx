import Link from "next/link";

export function Footer() {
	return (
		<footer className="w-full py-6 bg-gray-100 border-t">
			<div className="container mx-auto px-4">
				<div className="grid grid-cols-1 md:grid-cols-4 gap-8">
					<div>
						<h3 className="font-bold text-lg mb-4">BatDongSan</h3>
						<p className="text-gray-600">
							Nền tảng mua bán, cho thuê bất động sản hàng đầu Việt Nam
						</p>
					</div>

					<div>
						<h4 className="font-semibold mb-4">Liên kết nhanh</h4>
						<ul className="space-y-2">
							<li>
								<Link
									href="/properties"
									className="text-gray-600 hover:text-gray-900">
									Tìm bất động sản
								</Link>
							</li>
							<li>
								<Link
									href="/services"
									className="text-gray-600 hover:text-gray-900">
									Dịch vụ
								</Link>
							</li>
							<li>
								<Link
									href="/about"
									className="text-gray-600 hover:text-gray-900">
									Về chúng tôi
								</Link>
							</li>
						</ul>
					</div>

					<div>
						<h4 className="font-semibold mb-4">Dịch vụ</h4>
						<ul className="space-y-2">
							<li>
								<Link
									href="/services/buy"
									className="text-gray-600 hover:text-gray-900">
									Mua bất động sản
								</Link>
							</li>
							<li>
								<Link
									href="/services/rent"
									className="text-gray-600 hover:text-gray-900">
									Thuê bất động sản
								</Link>
							</li>
							<li>
								<Link
									href="/services/sell"
									className="text-gray-600 hover:text-gray-900">
									Bán bất động sản
								</Link>
							</li>
						</ul>
					</div>

					<div>
						<h4 className="font-semibold mb-4">Liên hệ</h4>
						<address className="not-italic text-gray-600">
							<p>123 Đường ABC, Quận XYZ</p>
							<p>Thành phố HCM, Việt Nam</p>
							<p className="mt-2">Email: info@batdongsan.example</p>
							<p>Điện thoại: (123) 456-7890</p>
						</address>
					</div>
				</div>

				<div className="mt-8 pt-6 border-t border-gray-200">
					<p className="text-center text-gray-500">
						© {new Date().getFullYear()} BatDongSan. Tất cả quyền được bảo lưu.
					</p>
				</div>
			</div>
		</footer>
	);
}
