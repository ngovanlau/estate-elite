"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import {
	Home,
	Building2,
	Users,
	MessageSquare,
	BarChart3,
	Settings,
	LogOut,
} from "lucide-react";

const navItems = [
	{
		title: "Tổng quan",
		href: "/dashboard",
		icon: Home,
	},
	{
		title: "Bất động sản",
		href: "/dashboard/properties",
		icon: Building2,
	},
	{
		title: "Người dùng",
		href: "/dashboard/users",
		icon: Users,
	},
	{
		title: "Tin nhắn",
		href: "/dashboard/messages",
		icon: MessageSquare,
	},
	{
		title: "Thống kê",
		href: "/dashboard/analytics",
		icon: BarChart3,
	},
	{
		title: "Cài đặt",
		href: "/dashboard/settings",
		icon: Settings,
	},
];

export function Sidebar() {
	const pathname = usePathname();

	return (
		<div className="w-64 bg-gray-50 border-r h-screen sticky top-0 flex flex-col">
			<div className="p-6 border-b">
				<Link href="/" className="flex items-center">
					<span className="font-bold text-xl">BatDongSan</span>
				</Link>
			</div>

			<div className="flex-1 py-6 px-4 space-y-1">
				<p className="text-xs font-semibold text-gray-500 mb-2 px-2">QUẢN LÝ</p>
				{navItems.map((item) => (
					<Link key={item.href} href={item.href}>
						<Button
							variant="ghost"
							className={cn(
								"w-full justify-start",
								pathname === item.href && "bg-gray-200"
							)}>
							<item.icon className="mr-2 h-4 w-4" />
							{item.title}
						</Button>
					</Link>
				))}
			</div>

			<div className="p-4 border-t">
				<Button variant="ghost" className="w-full justify-start text-red-500">
					<LogOut className="mr-2 h-4 w-4" />
					Đăng xuất
				</Button>
			</div>
		</div>
	);
}
