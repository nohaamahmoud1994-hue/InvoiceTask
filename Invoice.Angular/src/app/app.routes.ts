import { Routes } from '@angular/router';

import { InvoicePage } from './pagges/invoice/invoice';
import { InvoiceDetails } from './pagges/invoice-details/invoice-details';
import { InvoiceCreate } from './pagges/Create-Invoice/invoice-create';
import { InvoiceEdit } from './pagges/Invoice-Edit/invoice-edit';
import { InvoiceDelete } from './pagges/Invoice-Delete/invoice-delete';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'invoice',
    pathMatch: 'full'
  },

  {
    path: 'invoice',
    component: InvoicePage
  },

  {
    path: 'invoice/details/:id',
    component: InvoiceDetails
  },

  {
    path: 'invoice/create',
    component: InvoiceCreate
  },

  {
    path: 'invoice/edit/:id',
    component: InvoiceEdit
  },

  {
    path: 'invoice/delete/:id',
    component: InvoiceDelete
  }

];