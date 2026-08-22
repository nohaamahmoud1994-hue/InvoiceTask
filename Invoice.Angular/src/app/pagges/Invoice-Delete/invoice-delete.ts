import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';

import { InvoiceService } from '../../services/invoice.service';
import { Invoice } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-delete',
  standalone: true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './invoice-delete.html',
})
export class InvoiceDelete implements OnInit {

  private invoiceService = inject(InvoiceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  invoice: Invoice | null = null;

  invoiceId!: number;

  loading = true;

  deleting = false;

  errorMessage = '';


  ngOnInit(): void {

    this.invoiceId =
      Number(this.route.snapshot.paramMap.get('id'));

    if (!this.invoiceId) {

      this.errorMessage =
        'Invalid invoice ID.';

      this.loading = false;

      return;
    }

    this.loadInvoice();

  }


  loadInvoice(): void {

    this.loading = true;

    this.errorMessage = '';

    this.invoiceService
      .getInvoiceById(this.invoiceId)
      .subscribe({

        next: (response: any) => {

          console.log(
            'Invoice Details:',
            response
          );

          if (response && response.isValid) {

            this.invoice =
              response.invoice;

          }
          else {

            this.errorMessage =
              response?.message ||
              'Invoice not found.';

          }

          this.loading = false;

          this.cdr.detectChanges();

        },


        error: (error: any) => {

          console.error(
            'Get Invoice Error:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Unable to load invoice.';

          this.loading = false;

          this.cdr.detectChanges();

        }

      });

  }


  deleteInvoice(): void {

    if (!this.invoiceId || this.deleting) {

      return;

    }


    this.deleting = true;

    this.errorMessage = '';


    this.invoiceService
      .deleteInvoice(this.invoiceId)
      .subscribe({

        next: (response: any) => {

          console.log(
            'Delete Response:',
            response
          );


          if (response && response.isValid) {

            sessionStorage.setItem(
              'invoiceSuccessMessage',
              'Invoice deleted successfully.'
            );

            this.router.navigate([
              '/invoice'
            ]);

          }
          else {

            this.errorMessage =
              response?.message ||
              'Failed to delete invoice.';

            this.deleting = false;

            this.cdr.detectChanges();

          }

        },


        error: (error: any) => {

          console.error(
            'Delete Invoice Error:',
            error
          );


          this.errorMessage =
            error?.error?.message ||
            'Unable to delete invoice.';

          this.deleting = false;

          this.cdr.detectChanges();

        }

      });

  }

}