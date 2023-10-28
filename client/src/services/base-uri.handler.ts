import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ConfigService } from './config.service';

const BaseUriHandler: HttpInterceptorFn = (req, next) => {
  const configService = inject(ConfigService);
  const appClients = configService.AppConfig.getValue()?.clients;

  if (!appClients) {
    throw new Error('No clients found in app config; cannot set base uri.');
  }

  for (const [clientName, clientConfig] of Object.entries(appClients)) {
    if (req.url.startsWith(`@${clientName}`)) {
      const removedClientName = req.url.replace(`@${clientName}/`, '');
      const prefixedUrl = `${clientConfig.prefix ?? '.'}/${removedClientName}`;

      req = req.clone({
        url: new URL(prefixedUrl, clientConfig.host).href,
      });
      break;
    }
  }

  return next(req);
};

export default BaseUriHandler;
