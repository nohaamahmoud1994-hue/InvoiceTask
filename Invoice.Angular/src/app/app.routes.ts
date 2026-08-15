import { Routes } from '@angular/router';
import { Invoice } from './pagges/invoice/invoice';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'invoice',
    pathMatch: 'full'
  },
  {
    path: 'invoice',
    component: Invoice
  }
];
