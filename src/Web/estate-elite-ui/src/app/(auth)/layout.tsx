import Link from "next/link";

export default function AuthLayout({
	children,
}: {
	children: React.ReactNode;
}) {
	return (
		<div className="min-h-screen flex flex-col">
			<header className="py-4 px-6 border-b">
				<Link href="/" className="flex items-center">
					<span className="font-bold text-xl">BatDongSan</span>
				</Link>
			</header>

			<main className="flex-1 flex items-center justify-center">
				<div className="w-full max-w-md p-6">{children}</div>
			</main>

			<footer className="py-4 px-6 border-t text-center text-sm text-gray-500">
				© {new Date().getFullYear()} BatDongSan. Tất cả quyền được bảo lưu.
			</footer>
		</div>
	);
}
