import { Component, OnInit, inject, ChangeDetectorRef, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { InvoiceService } from '../../services/invoice.service';
import { Invoice } from '../../models/invoice.model';
import { InvoiceGetResponse } from '../../models/invoice-get-response.model';

@Component({
  selector: 'app-invoice-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './invoice-details.html',
  styleUrl: './invoice-details.css'
})
export class InvoiceDetails implements OnInit {

  private invoiceService = inject(InvoiceService);
  private route = inject(ActivatedRoute);
  private cdr = inject(ChangeDetectorRef);
  private platformId = inject(PLATFORM_ID);

  invoice: Invoice | null = null;

  loading = false;
  errorMessage = '';

  ngOnInit(): void {

    if (isPlatformBrowser(this.platformId)) {

      const id = Number(
        this.route.snapshot.paramMap.get('id')
      );

      if (!id) {
        this.errorMessage = 'Invalid invoice id.';
        return;
      }

      this.loadInvoice(id);
    }
  }

  loadInvoice(id: number): void {

    this.loading = true;
    this.errorMessage = '';

    this.invoiceService.getInvoiceById(id).subscribe({

      next: (response: InvoiceGetResponse) => {

        if (
          response &&
          response.isValid &&
          response.invoice
        ) {

          this.invoice = response.invoice;

        } else {

          this.errorMessage =
            response?.message || 'Invoice not found.';
        }

        this.loading = false;

        this.cdr.detectChanges();
      },

      error: (error: any) => {

        console.error('API Error:', error);

        this.errorMessage =
          'Unable to load invoice.';

        this.loading = false;

        this.cdr.detectChanges();
      }

    });
  }


  // Calculate total taxes
  getTotalTaxes(): number {

    if (!this.invoice?.taxTotals) {
      return 0;
    }

    return this.invoice.taxTotals.reduce(
      (total, tax) => total + (tax.amount || 0),
      0
    );
  }


  // Calculate total discount from invoice lines
  getTotalDiscount(): number {

    if (!this.invoice?.invoiceLines) {
      return 0;
    }

    return this.invoice.invoiceLines.reduce(
      (total, line) =>
        total + (line.discount?.amount || 0),
      0
    );
  }

}