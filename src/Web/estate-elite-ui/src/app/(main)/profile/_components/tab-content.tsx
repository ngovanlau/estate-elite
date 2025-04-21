import { useState } from 'react';
import { USER_ROLE } from '@/lib/enum';
import { GeneralInfoTab } from './general-info-tab';
import { BusinessInfoTab } from './business-info-tab';
import { PasswordTab } from './password-tab';
import { VerificationForm } from './verification-form';
import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { useAppSelector } from '@/lib/hooks';
import { selectUser } from '@/redux/slices/auth-slice';

export function TabContent() {
  const [activeTab, setActiveTab] = useState('general');
  const role = useAppSelector(selectUser)?.role;
  // Mock form submissions
  //   const handleProfileSubmit = (values: z.infer<typeof profileFormSchema>) => {
  //     console.log('Profile form submitted:', values);
  //     toast.success('Thông tin cá nhân đã được cập nhật thành công.');
  //   };

  //   const handleBusinessSubmit = (values: z.infer<typeof businessFormSchema>) => {
  //     console.log('Business form submitted:', values);
  //     toast.success('Thông tin doanh nghiệp đã được cập nhật thành công.');
  //   };

  //   const handlePasswordSubmit = (values: z.infer<typeof passwordFormSchema>) => {
  //     console.log('Password form submitted:', values);
  //     toast.success('Mật khẩu đã được cập nhật thành công.');
  //   };

  //   const handlePasswordReset = () => {
  //     console.log('Password reset requested');
  //     toast.success('Link đặt lại mật khẩu đã được gửi đến email của bạn.');
  //   };

  //   const handleVerificationSubmit = () => {
  //     console.log('Verification submitted');
  //     toast.success('Yêu cầu xác minh đã được gửi. Chúng tôi sẽ xem xét trong 1-3 ngày làm việc.');
  //   };

  //   const handleLicenseUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
  //     const file = e.target?.files?.[0];
  //     if (file) {
  //       console.log('License file uploaded:', file.name);
  //       toast.success('Tải lên giấy phép thành công.');
  //     }
  //   };

  // Default values for forms
  //   const profileDefaultValues = {
  //     name: 'Nguyễn Văn A',
  //     email: 'nguyenvana@example.com',
  //     phone: '0987654321',
  //     address: 'Hà Nội, Việt Nam',
  //     role: userRole.toLowerCase(),
  //     bio: 'Đây là một số thông tin giới thiệu về tôi.',
  //   };

  //   const businessDefaultValues = {
  //     companyName: 'Công ty ABC',
  //     taxId: '0123456789',
  //     website: 'https://example.com',
  //     establishedYear: '2020',
  //     businessType: 'company',
  //   };

  //   const passwordDefaultValues = {
  //     currentPassword: '',
  //     newPassword: '',
  //     confirmPassword: '',
  //   };

  // Render specific form based on active tab
  const renderTabContent = () => {
    switch (activeTab) {
      case 'general':
        return <GeneralInfoTab />;
      case 'business':
        return <BusinessInfoTab />;
      case 'password':
        return <PasswordTab />;
      case 'verification':
        return <VerificationForm />;
      default:
        return null;
    }
  };

  return (
    <div className="space-y-3 md:col-span-2">
      <Tabs
        value={activeTab}
        onValueChange={setActiveTab}
        className="flex w-full flex-col space-y-1"
      >
        <TabsList>
          <TabsTrigger
            value="general"
            className={`w-full justify-start ${activeTab === 'general' ? 'bg-accent' : ''}`}
          >
            Thông tin chung
          </TabsTrigger>

          {role === USER_ROLE.SELLER && (
            <TabsTrigger
              value="business"
              className={`w-full justify-start ${activeTab === 'business' ? 'bg-accent' : ''}`}
            >
              Thông tin doanh nghiệp
            </TabsTrigger>
          )}

          <TabsTrigger
            value="password"
            className={`w-full justify-start ${activeTab === 'password' ? 'bg-accent' : ''}`}
          >
            Đổi mật khẩu
          </TabsTrigger>

          {/* TODO
            {!isVerified && (
              <TabsTrigger
                value="verification"
                className={`w-full justify-start ${
                  activeTab === 'verification' ? 'bg-accent' : ''
                }`}
                onClick={() => setActiveTab('verification')}
              >
                Xác minh tài khoản
              </TabsTrigger>
            )} */}
        </TabsList>
      </Tabs>
      {renderTabContent()}
    </div>
  );
}
