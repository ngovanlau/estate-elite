// app/not-found.tsx
'use client';

import Link from 'next/link';
import { Button } from '@/components/ui/button';
import { Home, ArrowLeft } from 'lucide-react';

export default function NotFound() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-slate-50 px-4">
      <div className="max-w-md text-center">
        <h1 className="text-primary text-6xl font-bold">404</h1>
        <h2 className="mt-4 text-2xl font-semibold tracking-tight text-slate-900">
          Trang không tìm thấy
        </h2>
        <p className="mt-2 text-slate-500">
          Rất tiếc, chúng tôi không thể tìm thấy trang mà bạn đang tìm kiếm.
        </p>

        <div className="mt-8 flex flex-col gap-4 sm:flex-row sm:justify-center">
          <Link href="/">
            <Button
              variant="default"
              className="w-full gap-2 sm:w-auto"
            >
              <Home className="h-4 w-4" />
              <span>Trang chủ</span>
            </Button>
          </Link>
          <Button
            variant="outline"
            className="w-full gap-2 sm:w-auto"
            onClick={() => window.history.back()}
          >
            <ArrowLeft className="h-4 w-4" />
            <span>Quay lại</span>
          </Button>
        </div>
      </div>
    </div>
  );
}
