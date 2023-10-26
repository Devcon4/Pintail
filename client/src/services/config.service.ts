import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfigService {
  httpClient: HttpClient;

  constructor(handler: HttpBackend) {
    this.httpClient = new HttpClient(handler);
  }

  configUrl = 'assets/config/config.json';

  AppConfig = new BehaviorSubject<AppConfig | undefined>(undefined);

  public async load() {
    const res = await lastValueFrom(
      this.httpClient.get<AppConfig>(this.configUrl)
    );
    this.AppConfig.next(res);
  }
}

export type ConfigClients = 'api' | 'ws';

export type ApiClientConfig = {
  host: string;
  prefix: string;
};

export type AppConfig = {
  clients: {
    [key in ConfigClients]: ApiClientConfig;
  };
};
