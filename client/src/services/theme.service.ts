import { Injectable } from '@angular/core';
import { BehaviorSubject, fromEvent, map, tap } from 'rxjs';

export type ThemeType = 'light' | 'dark';
const on = 'var(--on)';
const off = 'var(--off)';

@Injectable({
  providedIn: 'root',
})
export class ThemeState {
  theme = new BehaviorSubject<ThemeType>(
    window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
  );

  isLight = this.theme.pipe(map((theme) => theme === 'light'));
  isDark = this.theme.pipe(map((theme) => theme === 'dark'));

  constructor() {
    fromEvent<MediaQueryListEvent>(
      window.matchMedia('(prefers-color-scheme: dark)'),
      'change'
    )
      .pipe(
        map((event) => (event.matches ? 'dark' : 'light')),
        tap((theme) => this.setTheme(theme))
      )
      .subscribe();
  }

  setTheme(theme: ThemeType) {
    this.theme.next(theme);
  }

  toggleTheme() {
    const isLight = this.theme.getValue() === 'light';
    document.body.style.setProperty('--light', isLight ? off : on);
    document.body.style.setProperty('--dark', isLight ? on : off);
    this.setTheme(isLight ? 'dark' : 'light');
  }
}
