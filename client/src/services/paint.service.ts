import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaintService {
  httpClient: HttpClient;

  constructor(handler: HttpBackend) {
    this.httpClient = new HttpClient(handler);
  }

  public async load() {
    const res = await lastValueFrom(
      this.httpClient.get('assets/workers/background-paint.js', {
        responseType: 'blob',
      })
    );
    const fleckBlob = new Blob([res], { type: 'text/javascript' });
    const fleckUrl = URL.createObjectURL(fleckBlob);
    if ((CSS as any).paintWorklet) {
      (CSS as any).paintWorklet.addModule(fleckUrl);
    }
  }
}
