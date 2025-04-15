import Link from 'next/link';

export function Footer() {
  return (
    <footer className="w-full border-t bg-gray-100 py-6">
      <div className="container mx-auto px-4">
        <div className="grid grid-cols-1 gap-8 md:grid-cols-4">
          <div>
            <h3 className="mb-4 text-lg font-bold">BatDongSan</h3>
            <p className="text-gray-600">
              Nền tảng mua bán, cho thuê bất động sản hàng đầu Việt Nam
            </p>
          </div>

          <div>
            <h4 className="mb-4 font-semibold">Liên kết nhanh</h4>
            <ul className="space-y-2">
              <li>
                <Link
                  href="/properties"
                  className="text-gray-600 hover:text-gray-900"
                >
                  Tìm bất động sản
                </Link>
              </li>
              <li>
                <Link
                  href="/services"
                  className="text-gray-600 hover:text-gray-900"
                >
                  Dịch vụ
                </Link>
              </li>
              <li>
                <Link
                  href="/about"
                  className="text-gray-600 hover:text-gray-900"
                >
                  Về chúng tôi
                </Link>
              </li>
            </ul>
          </div>

          <div>
            <h4 className="mb-4 font-semibold">Dịch vụ</h4>
            <ul className="space-y-2">
              <li>
                <Link
                  href="/services/buy"
                  className="text-gray-600 hover:text-gray-900"
                >
                  Mua bất động sản
                </Link>
              </li>
              <li>
                <Link
                  href="/services/rent"
                  className="text-gray-600 hover:text-gray-900"
                >
                  Thuê bất động sản
                </Link>
              </li>
              <li>
                <Link
                  href="/services/sell"
                  className="text-gray-600 hover:text-gray-900"
                >
                  Bán bất động sản
                </Link>
              </li>
            </ul>
          </div>

          <div>
            <h4 className="mb-4 font-semibold">Liên hệ</h4>
            <address className="text-gray-600 not-italic">
              <p>123 Đường ABC, Quận XYZ</p>
              <p>Thành phố HCM, Việt Nam</p>
              <p className="mt-2">Email: info@batdongsan.example</p>
              <p>Điện thoại: (123) 456-7890</p>
            </address>
          </div>
        </div>

        <div className="mt-8 border-t border-gray-200 pt-6">
          <p className="text-center text-gray-500">
            © {new Date().getFullYear()} BatDongSan. Tất cả quyền được bảo lưu.
          </p>
        </div>
      </div>
    </footer>
  );
}
