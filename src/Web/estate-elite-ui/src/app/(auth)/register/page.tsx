'use client';

import { useState } from 'react';
import Image from 'next/image';
import Background from '@/public/images/background-login-register.jpg';
import styles from './styles.module.scss';
import { RegisterForm } from './_components/register-form';
import OtpDialog from './_components/otp-dialog';
import { registerFormSchema } from './_components/validation';
import { z } from 'zod';

export default function RegisterPage() {
  const [isOtpDialogOpen, setIsOtpDialogOpen] = useState<boolean>(false);

  async function handleRegister(values: z.infer<typeof registerFormSchema>) {
    try {
      console.log(values);
      // Gọi API đăng ký tài khoản ở đây
      // Sau khi đăng ký thành công, mở modal OTP
      setIsOtpDialogOpen(true);
    } catch (error) {
      console.error(error);
    }
  }

  const handleCloseOtpDialog = () => setIsOtpDialogOpen(false);

  return (
    <>
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

        {/* Right side - Registration form */}
        <div className="flex w-full items-center justify-center p-6 md:w-1/2">
          <div className="w-full max-w-fit">
            <RegisterForm onSubmit={handleRegister} />
          </div>
        </div>
      </div>

      {isOtpDialogOpen && (
        <OtpDialog
          isDialogOpen={isOtpDialogOpen}
          handleCloseDialog={handleCloseOtpDialog}
        />
      )}
    </>
  );
}
