import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'FDSService',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44372',
    redirectUri: baseUrl,
    clientId: 'FDSService_App',
    responseType: 'code',
    scope: 'offline_access FDSService',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44321',
      rootNamespace: 'FDSService',
    },
  },
} as Environment;
