import Link from 'next/link';

export default function AuthLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="flex min-h-screen flex-col">
      <header className="border-b px-6 py-4">
        <Link
          href="/"
          className="flex items-center"
        >
          <span className="text-2xl font-bold">Estate Elite</span>
        </Link>
      </header>

      <main className="flex flex-1 items-center justify-center">
        <div className="w-full p-6">{children}</div>
      </main>

      <footer className="border-t px-6 py-4 text-center text-sm text-gray-500">
        © {new Date().getFullYear()} BatDongSan. Tất cả quyền được bảo lưu.
      </footer>
    </div>
  );
}
