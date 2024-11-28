import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44321/',
  redirectUri: baseUrl,
  clientId: 'MySeries_App',
  responseType: 'code',
  scope: 'offline_access MySeries',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'MySeries',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44321',
      rootNamespace: 'MySeries',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;