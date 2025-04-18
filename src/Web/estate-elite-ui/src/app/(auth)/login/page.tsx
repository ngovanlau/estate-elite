// app/login/page.tsx
'use client';

import Image from 'next/image';
import Background from '@/public/images/background-login-register.jpg';
import styles from './styles.module.scss';
import { LoginForm } from './_components/login-form';

export default function LoginPage() {
  return (
    <div className="flex min-h-screen flex-col md:flex-row">
      {/* Left side - Image */}
      <div className="relative hidden md:block md:w-1/2">
        <Image
          src={Background}
          alt="background"
          fill
          sizes="100vw"
          className={styles.image}
        />
      </div>

      {/* Right side - Login form */}
      <div className="flex w-full items-center justify-center p-6 md:w-1/2">
        <div className="w-full max-w-md">
          <LoginForm />
        </div>
      </div>
    </div>
  );
}
