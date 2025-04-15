"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { Button } from "@/components/ui/button";
import {
	NavigationMenu,
	NavigationMenuContent,
	NavigationMenuItem,
	NavigationMenuLink,
	NavigationMenuList,
	NavigationMenuTrigger,
} from "@/components/ui/navigation-menu";
import { cn } from "@/lib/utils";

export function Header() {
	const pathname = usePathname();

	return (
		<header className="sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur">
			<div className="container flex h-16 items-center justify-between">
				<Link href="/" className="flex items-center space-x-2">
					<span className="font-bold text-xl">BatDongSan</span>
				</Link>

				<NavigationMenu>
					<NavigationMenuList>
						<NavigationMenuItem>
							<Link href="/properties" legacyBehavior passHref>
								<NavigationMenuLink
									className={cn(
										"group inline-flex h-10 w-max items-center justify-center rounded-md bg-background px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-accent/50 data-[state=open]:bg-accent/50",
										pathname.startsWith("/properties") ? "bg-accent/50" : ""
									)}>
									Bất động sản
								</NavigationMenuLink>
							</Link>
						</NavigationMenuItem>

						<NavigationMenuItem>
							<NavigationMenuTrigger>Dịch vụ</NavigationMenuTrigger>
							<NavigationMenuContent>
								<ul className="grid w-[200px] gap-3 p-4">
									<li>
										<Link href="/services/buy" legacyBehavior passHref>
											<NavigationMenuLink className="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground">
												<div className="text-sm font-medium">
													Mua bất động sản
												</div>
											</NavigationMenuLink>
										</Link>
									</li>
									<li>
										<Link href="/services/rent" legacyBehavior passHref>
											<NavigationMenuLink className="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground">
												<div className="text-sm font-medium">
													Thuê bất động sản
												</div>
											</NavigationMenuLink>
										</Link>
									</li>
								</ul>
							</NavigationMenuContent>
						</NavigationMenuItem>

						<NavigationMenuItem>
							<Link href="/about" legacyBehavior passHref>
								<NavigationMenuLink
									className={cn(
										"group inline-flex h-10 w-max items-center justify-center rounded-md bg-background px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-accent/50 data-[state=open]:bg-accent/50",
										pathname === "/about" ? "bg-accent/50" : ""
									)}>
									Giới thiệu
								</NavigationMenuLink>
							</Link>
						</NavigationMenuItem>
					</NavigationMenuList>
				</NavigationMenu>

				<div className="flex items-center gap-4">
					<Link href="/login">
						<Button variant="outline">Đăng nhập</Button>
					</Link>
					<Link href="/register">
						<Button>Đăng ký</Button>
					</Link>
				</div>
			</div>
		</header>
	);
}
