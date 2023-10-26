import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './modules/app/app.component';
import { appConfig } from './modules/app/app.config';

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
