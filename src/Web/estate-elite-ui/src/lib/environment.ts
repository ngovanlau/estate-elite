export const environment = {
  identityServiceApi: process.env.NEXT_PUBLIC_IDENTITY_SERVICE_API || '',
  propertyServiceApi: process.env.NEXT_PUBLIC_PROPERTY_SERVICE_API || '',

  apiTimeout: process.env.NEXT_PUBLIC_API_TIMEOUT || '10000',
};
