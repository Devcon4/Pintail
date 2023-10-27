import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'workbench/:key',
    loadComponent: () => import('../workbench/bench/bench.component'),
  },
  {
    path: 'home',
    loadComponent: () => import('../home/boards/boards.component'),
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home', pathMatch: 'full' },
];
