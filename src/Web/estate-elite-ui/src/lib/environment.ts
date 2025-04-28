export const environment = {
  identityServiceApi: process.env.NEXT_PUBLIC_IDENTITY_SERVICE_API || '',
  propertyServiceApi: process.env.NEXT_PUBLIC_PROPERTY_SERVICE_API || '',
  paymentServiceApi: process.env.NEXT_PUBLIC_PAYMENT_SERVICE_API || '',

  apiTimeout: process.env.NEXT_PUBLIC_API_TIMEOUT || '10000',

  paypalClientId: process.env.NEXT_PUBLIC_PAYPAL_CLIENT_ID || '',
};
