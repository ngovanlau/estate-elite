import { Header } from "@/components/Layout/Header";
import { Footer } from "@/components/Layout/Footer";

export default function MainLayout({
	children,
}: {
	children: React.ReactNode;
}) {
	return (
		<div className="flex min-h-screen flex-col">
			<Header />
			<main className="flex-1 container mx-auto py-8 px-4">{children}</main>
			<Footer />
		</div>
	);
}
