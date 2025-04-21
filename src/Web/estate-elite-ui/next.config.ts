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
  // other Next.js config options...
};

export default nextConfig;
