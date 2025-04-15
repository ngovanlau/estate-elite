// pages/index.tsx
import React from 'react';
import { Search, MapPin, Home, Building, ArrowRight, Heart, Eye, Phone, Mail } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Input } from '@/components/ui/input';

export default function HomePage() {
  return (
    <div className="flex min-h-screen flex-col">
      {/* Header */}
      <header className="sticky top-0 z-50 w-full border-b bg-white">
        <div className="container flex h-16 items-center justify-between">
          <div className="flex items-center gap-6">
            {/* <a href="/" className="flex items-center gap-2 font-bold text-xl">
							<Home className="h-6 w-6" />
							<span>RealEstate Pro</span>
						</a> */}
            <nav className="hidden gap-6 md:flex">
              <a
                href="/buy"
                className="text-sm font-medium hover:underline"
              >
                Mua
              </a>
              <a
                href="/rent"
                className="text-sm font-medium hover:underline"
              >
                Thuê
              </a>
              <a
                href="/sell"
                className="text-sm font-medium hover:underline"
              >
                Bán
              </a>
              <a
                href="/agents"
                className="text-sm font-medium hover:underline"
              >
                Môi giới
              </a>
              <a
                href="/news"
                className="text-sm font-medium hover:underline"
              >
                Tin tức
              </a>
            </nav>
          </div>
          <div className="flex items-center gap-4">
            <Button
              variant="ghost"
              size="sm"
            >
              Đăng nhập
            </Button>
            <Button size="sm">Đăng ký</Button>
            <Button
              variant="outline"
              size="sm"
              className="hidden md:flex"
            >
              <MapPin className="mr-2 h-4 w-4" />
              Đăng tin
            </Button>
          </div>
        </div>
      </header>

      {/* Hero Section */}
      <section className="relative h-[500px] overflow-hidden bg-slate-900">
        <div className="absolute inset-0 bg-gradient-to-r from-slate-900 to-slate-800/50">
          {/* Background image would be set in CSS */}
        </div>
        <div className="relative z-10 container flex h-full flex-col items-center justify-center text-center text-white">
          <h1 className="mb-6 text-4xl font-bold md:text-5xl">Tìm ngôi nhà mơ ước của bạn</h1>
          <p className="mb-8 max-w-2xl text-lg md:text-xl">
            Khám phá hàng ngàn bất động sản để mua và thuê trên nền tảng của chúng tôi
          </p>

          {/* Search Bar */}
          <div className="w-full max-w-4xl rounded-lg bg-white p-4 text-slate-900">
            <Tabs
              defaultValue="buy"
              className="w-full"
            >
              <TabsList className="mb-4 grid w-full grid-cols-2">
                <TabsTrigger value="buy">Mua</TabsTrigger>
                <TabsTrigger value="rent">Thuê</TabsTrigger>
              </TabsList>
              <TabsContent
                value="buy"
                className="mt-0"
              >
                <div className="flex flex-col gap-3 md:flex-row">
                  <div className="relative flex flex-1">
                    <MapPin className="absolute top-3 left-3 h-5 w-5 text-slate-400" />
                    <Input
                      placeholder="Tìm kiếm theo khu vực, dự án..."
                      className="flex-1 pl-10"
                    />
                  </div>
                  <Select>
                    <SelectTrigger className="w-full md:w-[180px]">
                      <SelectValue placeholder="Loại nhà" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="apartment">Căn hộ</SelectItem>
                      <SelectItem value="house">Nhà phố</SelectItem>
                      <SelectItem value="villa">Biệt thự</SelectItem>
                      <SelectItem value="land">Đất nền</SelectItem>
                    </SelectContent>
                  </Select>
                  <Select>
                    <SelectTrigger className="w-full md:w-[180px]">
                      <SelectValue placeholder="Ngân sách" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="1">Dưới 1 tỷ</SelectItem>
                      <SelectItem value="2">1 - 3 tỷ</SelectItem>
                      <SelectItem value="3">3 - 5 tỷ</SelectItem>
                      <SelectItem value="4">5 - 10 tỷ</SelectItem>
                      <SelectItem value="5">Trên 10 tỷ</SelectItem>
                    </SelectContent>
                  </Select>
                  <Button className="w-full md:w-auto">
                    <Search className="mr-2 h-4 w-4" />
                    Tìm kiếm
                  </Button>
                </div>
              </TabsContent>
              <TabsContent
                value="rent"
                className="mt-0"
              >
                <div className="flex flex-col gap-3 md:flex-row">
                  <div className="relative flex flex-1">
                    <MapPin className="absolute top-3 left-3 h-5 w-5 text-slate-400" />
                    <Input
                      placeholder="Tìm kiếm theo khu vực, dự án..."
                      className="flex-1 pl-10"
                    />
                  </div>
                  <Select>
                    <SelectTrigger className="w-full md:w-[180px]">
                      <SelectValue placeholder="Loại nhà" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="apartment">Căn hộ</SelectItem>
                      <SelectItem value="house">Nhà phố</SelectItem>
                      <SelectItem value="villa">Biệt thự</SelectItem>
                      <SelectItem value="office">Văn phòng</SelectItem>
                    </SelectContent>
                  </Select>
                  <Select>
                    <SelectTrigger className="w-full md:w-[180px]">
                      <SelectValue placeholder="Giá thuê" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="1">Dưới 5 triệu</SelectItem>
                      <SelectItem value="2">5 - 10 triệu</SelectItem>
                      <SelectItem value="3">10 - 20 triệu</SelectItem>
                      <SelectItem value="4">20 - 50 triệu</SelectItem>
                      <SelectItem value="5">Trên 50 triệu</SelectItem>
                    </SelectContent>
                  </Select>
                  <Button className="w-full md:w-auto">
                    <Search className="mr-2 h-4 w-4" />
                    Tìm kiếm
                  </Button>
                </div>
              </TabsContent>
            </Tabs>
          </div>
        </div>
      </section>

      {/* Featured Properties */}
      <section className="bg-slate-50 py-16">
        <div className="container">
          <div className="mb-8 flex items-center justify-between">
            <h2 className="text-2xl font-bold">Bất động sản nổi bật</h2>
            <Button variant="outline">
              Xem tất cả
              <ArrowRight className="ml-2 h-4 w-4" />
            </Button>
          </div>

          <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
            {/* Property Card 1 */}
            <Card>
              <div className="relative">
                <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
                  {/* Image would be placed here */}
                </div>
                <div className="absolute top-3 left-3">
                  <span className="rounded bg-blue-600 px-2.5 py-1 text-xs font-medium text-white">
                    Bán
                  </span>
                </div>
                <Button
                  variant="ghost"
                  size="icon"
                  className="absolute top-2 right-2 rounded-full bg-white/80 hover:bg-white"
                >
                  <Heart className="h-5 w-5" />
                </Button>
              </div>
              <CardHeader className="pb-2">
                <div className="flex justify-between">
                  <CardTitle className="text-lg">5.2 tỷ</CardTitle>
                  <div className="flex items-center text-sm text-slate-500">
                    <Eye className="mr-1 h-4 w-4" />
                    230
                  </div>
                </div>
                <CardDescription className="text-base font-medium">
                  Căn hộ 3 phòng ngủ Vinhomes Central Park
                </CardDescription>
              </CardHeader>
              <CardContent className="pb-2">
                <div className="mb-3 flex items-center text-sm text-slate-500">
                  <MapPin className="mr-1 h-4 w-4" />
                  <span>Quận Bình Thạnh, TP. Hồ Chí Minh</span>
                </div>
                <div className="grid grid-cols-3 gap-2 text-sm">
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">90m²</p>
                    <p className="text-xs text-slate-500">Diện tích</p>
                  </div>
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">3</p>
                    <p className="text-xs text-slate-500">Phòng ngủ</p>
                  </div>
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">2</p>
                    <p className="text-xs text-slate-500">Phòng tắm</p>
                  </div>
                </div>
              </CardContent>
              <CardFooter>
                <Button
                  variant="default"
                  className="w-full"
                >
                  Xem chi tiết
                </Button>
              </CardFooter>
            </Card>

            {/* Property Card 2 */}
            <Card>
              <div className="relative">
                <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
                  {/* Image would be placed here */}
                </div>
                <div className="absolute top-3 left-3">
                  <span className="rounded bg-amber-600 px-2.5 py-1 text-xs font-medium text-white">
                    Thuê
                  </span>
                </div>
                <Button
                  variant="ghost"
                  size="icon"
                  className="absolute top-2 right-2 rounded-full bg-white/80 hover:bg-white"
                >
                  <Heart className="h-5 w-5" />
                </Button>
              </div>
              <CardHeader className="pb-2">
                <div className="flex justify-between">
                  <CardTitle className="text-lg">25 triệu/tháng</CardTitle>
                  <div className="flex items-center text-sm text-slate-500">
                    <Eye className="mr-1 h-4 w-4" />
                    187
                  </div>
                </div>
                <CardDescription className="text-base font-medium">
                  Nhà phố nội thất cao cấp Thảo Điền
                </CardDescription>
              </CardHeader>
              <CardContent className="pb-2">
                <div className="mb-3 flex items-center text-sm text-slate-500">
                  <MapPin className="mr-1 h-4 w-4" />
                  <span>Quận 2, TP. Hồ Chí Minh</span>
                </div>
                <div className="grid grid-cols-3 gap-2 text-sm">
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">120m²</p>
                    <p className="text-xs text-slate-500">Diện tích</p>
                  </div>
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">4</p>
                    <p className="text-xs text-slate-500">Phòng ngủ</p>
                  </div>
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">3</p>
                    <p className="text-xs text-slate-500">Phòng tắm</p>
                  </div>
                </div>
              </CardContent>
              <CardFooter>
                <Button
                  variant="default"
                  className="w-full"
                >
                  Xem chi tiết
                </Button>
              </CardFooter>
            </Card>

            {/* Property Card 3 */}
            <Card>
              <div className="relative">
                <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
                  {/* Image would be placed here */}
                </div>
                <div className="absolute top-3 left-3">
                  <span className="rounded bg-blue-600 px-2.5 py-1 text-xs font-medium text-white">
                    Bán
                  </span>
                </div>
                <Button
                  variant="ghost"
                  size="icon"
                  className="absolute top-2 right-2 rounded-full bg-white/80 hover:bg-white"
                >
                  <Heart className="h-5 w-5" />
                </Button>
              </div>
              <CardHeader className="pb-2">
                <div className="flex justify-between">
                  <CardTitle className="text-lg">12.8 tỷ</CardTitle>
                  <div className="flex items-center text-sm text-slate-500">
                    <Eye className="mr-1 h-4 w-4" />
                    342
                  </div>
                </div>
                <CardDescription className="text-base font-medium">
                  Biệt thự view sông Saigon Pearl
                </CardDescription>
              </CardHeader>
              <CardContent className="pb-2">
                <div className="mb-3 flex items-center text-sm text-slate-500">
                  <MapPin className="mr-1 h-4 w-4" />
                  <span>Quận Bình Thạnh, TP. Hồ Chí Minh</span>
                </div>
                <div className="grid grid-cols-3 gap-2 text-sm">
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">250m²</p>
                    <p className="text-xs text-slate-500">Diện tích</p>
                  </div>
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">5</p>
                    <p className="text-xs text-slate-500">Phòng ngủ</p>
                  </div>
                  <div className="rounded bg-slate-100 p-2 text-center">
                    <p className="font-medium">4</p>
                    <p className="text-xs text-slate-500">Phòng tắm</p>
                  </div>
                </div>
              </CardContent>
              <CardFooter>
                <Button
                  variant="default"
                  className="w-full"
                >
                  Xem chi tiết
                </Button>
              </CardFooter>
            </Card>
          </div>
        </div>
      </section>

      {/* Area Categories */}
      <section className="bg-white py-16">
        <div className="container">
          <h2 className="mb-8 text-2xl font-bold">Bất động sản theo khu vực</h2>

          <div className="grid grid-cols-2 gap-6 md:grid-cols-4">
            {/* Area Card 1 */}
            <Card className="overflow-hidden">
              <div className="h-40 bg-slate-200">{/* Area image would go here */}</div>
              <CardContent className="pt-4">
                <h3 className="font-bold">TP. Hồ Chí Minh</h3>
                <p className="text-sm text-slate-500">2,345 bất động sản</p>
              </CardContent>
            </Card>

            {/* Area Card 2 */}
            <Card className="overflow-hidden">
              <div className="h-40 bg-slate-200">{/* Area image would go here */}</div>
              <CardContent className="pt-4">
                <h3 className="font-bold">Hà Nội</h3>
                <p className="text-sm text-slate-500">1,892 bất động sản</p>
              </CardContent>
            </Card>

            {/* Area Card 3 */}
            <Card className="overflow-hidden">
              <div className="h-40 bg-slate-200">{/* Area image would go here */}</div>
              <CardContent className="pt-4">
                <h3 className="font-bold">Đà Nẵng</h3>
                <p className="text-sm text-slate-500">943 bất động sản</p>
              </CardContent>
            </Card>

            {/* Area Card 4 */}
            <Card className="overflow-hidden">
              <div className="h-40 bg-slate-200">{/* Area image would go here */}</div>
              <CardContent className="pt-4">
                <h3 className="font-bold">Nha Trang</h3>
                <p className="text-sm text-slate-500">621 bất động sản</p>
              </CardContent>
            </Card>
          </div>
        </div>
      </section>

      {/* Services */}
      <section className="bg-slate-900 py-16 text-white">
        <div className="container">
          <h2 className="mb-8 text-center text-2xl font-bold">Dịch vụ của chúng tôi</h2>

          <div className="grid grid-cols-1 gap-8 md:grid-cols-3">
            <div className="p-6 text-center">
              <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-blue-600/20">
                <Home className="h-8 w-8 text-blue-400" />
              </div>
              <h3 className="mb-2 text-xl font-bold">Mua bất động sản</h3>
              <p className="text-slate-300">
                Tìm kiếm và mua ngôi nhà mơ ước với sự hỗ trợ từ đội ngũ chuyên gia của chúng tôi.
              </p>
              <Button
                variant="link"
                className="mt-4 text-blue-400"
              >
                Tìm hiểu thêm
              </Button>
            </div>

            <div className="p-6 text-center">
              <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-amber-600/20">
                <Building className="h-8 w-8 text-amber-400" />
              </div>
              <h3 className="mb-2 text-xl font-bold">Cho thuê bất động sản</h3>
              <p className="text-slate-300">
                Dễ dàng tìm kiếm căn hộ, nhà phố hoặc văn phòng cho thuê phù hợp với nhu cầu của
                bạn.
              </p>
              <Button
                variant="link"
                className="mt-4 text-amber-400"
              >
                Tìm hiểu thêm
              </Button>
            </div>

            <div className="p-6 text-center">
              <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-green-600/20">
                <MapPin className="h-8 w-8 text-green-400" />
              </div>
              <h3 className="mb-2 text-xl font-bold">Đăng tin bất động sản</h3>
              <p className="text-slate-300">
                Bạn muốn bán hoặc cho thuê? Đăng tin ngay để tiếp cận hàng ngàn khách hàng tiềm
                năng.
              </p>
              <Button
                variant="link"
                className="mt-4 text-green-400"
              >
                Tìm hiểu thêm
              </Button>
            </div>
          </div>
        </div>
      </section>

      {/* Recent News */}
      <section className="bg-slate-50 py-16">
        <div className="container">
          <div className="mb-8 flex items-center justify-between">
            <h2 className="text-2xl font-bold">Tin tức bất động sản</h2>
            <Button variant="outline">
              Xem tất cả
              <ArrowRight className="ml-2 h-4 w-4" />
            </Button>
          </div>

          <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
            {/* News Card 1 */}
            <Card>
              <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
                {/* News image would go here */}
              </div>
              <CardHeader>
                <CardTitle className="text-lg">
                  Thị trường bất động sản 2025: Dự báo và xu hướng
                </CardTitle>
              </CardHeader>
              <CardContent>
                <p className="mb-4 text-sm text-slate-500">
                  Phân tích về những thay đổi của thị trường bất động sản trong năm 2025 và các xu
                  hướng đáng chú ý...
                </p>
                <p className="text-xs text-slate-400">15/04/2025 • 5 phút đọc</p>
              </CardContent>
            </Card>

            {/* News Card 2 */}
            <Card>
              <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
                {/* News image would go here */}
              </div>
              <CardHeader>
                <CardTitle className="text-lg">
                  5 dự án chung cư cao cấp đáng chú ý tại TP.HCM
                </CardTitle>
              </CardHeader>
              <CardContent>
                <p className="mb-4 text-sm text-slate-500">
                  Điểm qua những dự án chung cư cao cấp đang được quan tâm nhiều nhất tại TP.HCM
                  trong thời gian gần đây...
                </p>
                <p className="text-xs text-slate-400">10/04/2025 • 8 phút đọc</p>
              </CardContent>
            </Card>

            {/* News Card 3 */}
            <Card>
              <div className="aspect-video overflow-hidden rounded-t-lg bg-slate-200">
                {/* News image would go here */}
              </div>
              <CardHeader>
                <CardTitle className="text-lg">
                  Hướng dẫn đầu tư bất động sản cho người mới bắt đầu
                </CardTitle>
              </CardHeader>
              <CardContent>
                <p className="mb-4 text-sm text-slate-500">
                  Những kiến thức cơ bản và lời khuyên hữu ích dành cho những người mới tham gia vào
                  thị trường đầu tư bất động sản...
                </p>
                <p className="text-xs text-slate-400">08/04/2025 • 12 phút đọc</p>
              </CardContent>
            </Card>
          </div>
        </div>
      </section>

      {/* Partners */}
      <section className="bg-white py-16">
        <div className="container">
          <h2 className="mb-8 text-center text-2xl font-bold">Đối tác của chúng tôi</h2>

          <div className="grid grid-cols-2 gap-8 md:grid-cols-5">
            {/* Partner logos would go here */}
            <div className="flex h-16 items-center justify-center rounded bg-slate-100"></div>
            <div className="flex h-16 items-center justify-center rounded bg-slate-100"></div>
            <div className="flex h-16 items-center justify-center rounded bg-slate-100"></div>
            <div className="flex h-16 items-center justify-center rounded bg-slate-100"></div>
            <div className="flex h-16 items-center justify-center rounded bg-slate-100"></div>
          </div>
        </div>
      </section>

      {/* Contact */}
      <section className="bg-slate-50 py-16">
        <div className="container max-w-4xl">
          <Card>
            <CardHeader className="text-center">
              <CardTitle className="text-2xl">Liên hệ với chúng tôi</CardTitle>
              <CardDescription>
                Bạn cần được tư vấn thêm? Đội ngũ chuyên gia của chúng tôi luôn sẵn sàng hỗ trợ
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                <div>
                  <div className="mb-6 flex items-center gap-4">
                    <div className="flex h-12 w-12 items-center justify-center rounded-full bg-blue-100">
                      <Phone className="h-6 w-6 text-blue-600" />
                    </div>
                    <div>
                      <p className="font-medium">Điện thoại</p>
                      <p className="text-slate-500">+84 28 1234 5678</p>
                    </div>
                  </div>
                  <div className="mb-6 flex items-center gap-4">
                    <div className="flex h-12 w-12 items-center justify-center rounded-full bg-blue-100">
                      <Mail className="h-6 w-6 text-blue-600" />
                    </div>
                    <div>
                      <p className="font-medium">Email</p>
                      <p className="text-slate-500">contact@realestatepro.vn</p>
                    </div>
                  </div>
                  <div className="flex items-center gap-4">
                    <div className="flex h-12 w-12 items-center justify-center rounded-full bg-blue-100">
                      <MapPin className="h-6 w-6 text-blue-600" />
                    </div>
                    <div>
                      <p className="font-medium">Địa chỉ</p>
                      <p className="text-slate-500">123 Nguyễn Huệ, Quận 1, TP.HCM</p>
                    </div>
                  </div>
                </div>
                <div className="space-y-4">
                  <Input placeholder="Họ và tên" />
                  <Input
                    placeholder="Email"
                    type="email"
                  />
                  <Input
                    placeholder="Số điện thoại"
                    type="tel"
                  />
                  <Input
                    placeholder="Nội dung"
                    className="h-24"
                  />
                  <Button className="w-full">Gửi tin nhắn</Button>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-slate-900 pt-16 pb-8 text-slate-300">
        <div className="container">
          <div className="mb-16 grid grid-cols-1 gap-8 md:grid-cols-4">
            <div>
              <div className="mb-4 flex items-center gap-2 text-xl font-bold text-white">
                <Home className="h-6 w-6" />
                <span>RealEstate Pro</span>
              </div>
              <p className="mb-4">
                Nền tảng hàng đầu về mua bán và cho thuê bất động sản tại Việt Nam.
              </p>
              <div className="flex gap-4">
                <Button
                  variant="ghost"
                  size="icon"
                  className="rounded-full"
                >
                  <span className="sr-only">Facebook</span>
                  {/* Facebook icon */}
                </Button>
                <Button
                  variant="ghost"
                  size="icon"
                  className="rounded-full"
                >
                  <span className="sr-only">Youtube</span>
                  {/* Youtube icon */}
                </Button>
                <Button
                  variant="ghost"
                  size="icon"
                  className="rounded-full"
                >
                  <span className="sr-only">Instagram</span>
                  {/* Instagram icon */}
                </Button>
              </div>
            </div>
            <div>
              <h3 className="mb-4 font-bold text-white">Về chúng tôi</h3>
              <ul className="space-y-2">
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Giới thiệu
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Liên hệ
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Tuyển dụng
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Tin tức
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Điều khoản dịch vụ
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Chính sách bảo mật
                  </a>
                </li>
              </ul>
            </div>{' '}
            <div>
              <h3 className="mb-4 font-bold text-white">Dịch vụ</h3>
              <ul className="space-y-2">
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Mua bất động sản
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Thuê bất động sản
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Bán bất động sản
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Môi giới
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Định giá
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="hover:text-white"
                  >
                    Tư vấn đầu tư
                  </a>
                </li>
              </ul>
            </div>
            <div>
              <h3 className="mb-4 font-bold text-white">Nhận thông tin</h3>
              <p className="mb-4">Đăng ký để nhận thông tin mới nhất về thị trường bất động sản.</p>
              <div className="flex gap-2">
                <Input
                  placeholder="Email của bạn"
                  className="border-slate-700 bg-slate-800"
                />
                <Button>Đăng ký</Button>
              </div>
            </div>
          </div>

          <div className="border-t border-slate-800 pt-8 text-center text-sm">
            <p>© 2025 RealEstate Pro. Tất cả quyền được bảo lưu.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
