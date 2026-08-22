import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef,
  PLATFORM_ID
} from '@angular/core';

import {
  isPlatformBrowser,
  DatePipe,
  DecimalPipe
} from '@angular/common';

import { RouterLink } from '@angular/router';

import { InvoiceService } from '../../services/invoice.service';
import { Invoice } from '../../models/invoice.model';


@Component({
  selector: 'app-invoice',
  standalone: true,

  imports: [
    RouterLink,
    DatePipe,
    DecimalPipe
  ],

  templateUrl: './invoice.html',
  styleUrl: './invoice.css',
})
export class InvoicePage implements OnInit {

  private invoiceService = inject(InvoiceService);

  private cdr = inject(ChangeDetectorRef);

  private platformId = inject(PLATFORM_ID);


  invoices: Invoice[] = [];

  loading = false;

  errorMessage = '';

  successMessage = '';


  ngOnInit(): void {

    if (isPlatformBrowser(this.platformId)) {

      const message =
        sessionStorage.getItem('invoiceSuccessMessage');

      if (message) {

        this.successMessage = message;

        sessionStorage.removeItem(
          'invoiceSuccessMessage'
        );

      }

      this.loadInvoices();

    }

  }


  loadInvoices(): void {

    this.loading = true;

    this.errorMessage = '';


    this.invoiceService.getAllInvoices().subscribe({

      next: (response: any) => {

        console.log('API RESPONSE:', response);


        if (response && response.isValid) {

          this.invoices =
            response.invoices || [];

        }
        else {

          this.errorMessage =
            response?.message ||
            'Failed to load invoices.';

        }


        this.loading = false;

        this.cdr.detectChanges();

      },


      error: (error: any) => {

        console.error(
          'API Error:',
          error
        );

        this.errorMessage =
          'Unable to connect to the Invoice API.';

        this.loading = false;

        this.cdr.detectChanges();

      }

    });

  }


  getTotalTax(invoice: Invoice): number {

    if (!invoice.taxTotals) {

      return 0;

    }


    return invoice.taxTotals.reduce(

      (total, tax) =>
        total + (tax.amount || 0),

      0

    );

  }

}