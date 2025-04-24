import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  images: {
    domains: ['localhost'], // legacy approach
    remotePatterns: [
      {
        protocol: 'http',
        hostname: 'localhost',
        port: '9000',
        pathname: '/identity-service/backgrounds/**',
      },
    ],
  },
  experimental: {
    externalDir: true, // Cho phép import từ bên ngoài
  },
  // Chế độ Strict Mode của React: Next.js mặc định bật Strict Mode, chủ động render components 2 lần trong môi trường development để phát hiện side effects.
  reactStrictMode: false, // Tắt trong development
  // other Next.js config options...
};

export default nextConfig;
