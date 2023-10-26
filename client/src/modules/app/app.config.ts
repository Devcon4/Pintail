import {
  APP_INITIALIZER,
  ApplicationConfig,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import BaseUriHandler from '../../services/base-uri.handler';
import { ConfigService } from '../../services/config.service';
import { PaintService } from '../../services/paint.service';
import { routes } from './app.routes';

const startupConfig = (configService: ConfigService) => {
  return () => configService.load();
};

const startupPaint = (paintService: PaintService) => {
  return () => paintService.load();
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    {
      provide: APP_INITIALIZER,
      useFactory: startupConfig,
      deps: [ConfigService],
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: startupPaint,
      deps: [PaintService],
      multi: true,
    },
    provideHttpClient(withInterceptors([BaseUriHandler])),
  ],
};
