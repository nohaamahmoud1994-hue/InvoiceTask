import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';

import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { InvoiceInfo } from '../../components/invoice-info/invoice-info';
import { Issuer } from '../../components/issuer/issuer';
import { Receiver } from '../../components/receiver/receiver';
import { InvoiceLine } from '../../components/invoice-line/invoice-line';

import { InvoiceService } from '../../services/invoice.service';
import { Invoice } from '../../models/invoice.model';

@Component({
  selector: 'app-invoice-edit',
  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    InvoiceInfo,
    Issuer,
    Receiver,
    InvoiceLine
  ],

  templateUrl: './invoice-edit.html'
})
export class InvoiceEdit implements OnInit {

  private fb = inject(FormBuilder);
  private invoiceService = inject(InvoiceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  invoiceId!: number;

  loading = false;
  errorMessage = '';

  activeTab = 'invoice-info';

  invoiceForm: FormGroup = this.fb.group({

    documentType: [
      { value: 'i', disabled: true }
    ],

    documentTypeVersion: [
      { value: '1.0', disabled: true }
    ],

    taxpayerActivityCode: [
      '',
      Validators.required
    ],

    internalId: [
      '',
      Validators.required
    ],

    issuer: this.fb.group({

      type: ['', Validators.required],
      id: ['', Validators.required],
      name: ['', Validators.required],

      address: this.fb.group({

        branchId: ['', Validators.required],
        country: ['', Validators.required],
        governate: ['', Validators.required],
        regionCity: ['', Validators.required],
        street: ['', Validators.required],
        buildingNumber: ['', Validators.required],

        postalCode: [''],
        floor: [''],
        room: [''],
        landmark: [''],
        additionalInformation: ['']

      })

    }),

    receiver: this.fb.group({

      type: ['', Validators.required],
      id: ['', Validators.required],
      name: ['', Validators.required],

      address: this.fb.group({

        country: ['', Validators.required],
        governate: ['', Validators.required],
        regionCity: ['', Validators.required],
        street: ['', Validators.required],
        buildingNumber: ['', Validators.required]

      })

    }),

    invoiceLines: this.fb.array([])

  });


  get invoiceLines(): FormArray {
    return this.invoiceForm.get('invoiceLines') as FormArray;
  }


  ngOnInit(): void {

    this.invoiceId =
      Number(this.route.snapshot.paramMap.get('id'));

    if (!this.invoiceId) {

      this.errorMessage = 'Invalid invoice id.';
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

          if (response?.isValid && response.invoice) {

            this.invoiceForm.patchValue({

              documentType:
                response.invoice.documentType,

              documentTypeVersion:
                response.invoice.documentTypeVersion,

              taxpayerActivityCode:
                response.invoice.taxpayerActivityCode,

              internalId:
                response.invoice.internalId,

              issuer:
                response.invoice.issuer,

              receiver:
                response.invoice.receiver

            });


            this.invoiceLines.clear();


            if (response.invoice.invoiceLines) {

              response.invoice.invoiceLines.forEach(
                (line: any) => {

                  this.invoiceLines.push(
                    this.createInvoiceLine(line)
                  );

                }
              );

            }

          }
          else {

            this.errorMessage =
              response?.message ||
              'Invoice not found.';

          }

          this.loading = false;

          this.cdr.detectChanges();

        },

        error: (error) => {

          console.error(
            'Load Invoice Error:',
            error
          );

          this.errorMessage =
            'Unable to load invoice.';

          this.loading = false;

          this.cdr.detectChanges();

        }

      });

  }


  createInvoiceLine(line: any): FormGroup {

    return this.fb.group({

      description: [
        line.description || '',
        Validators.required
      ],

      itemType: [
        line.itemType || 'EGS',
        Validators.required
      ],

      itemCode: [
        line.itemCode || '',
        Validators.required
      ],

      unitType: [
        line.unitType || '',
        Validators.required
      ],

      quantity: [
        line.quantity ?? 0,
        [Validators.required, Validators.min(0)]
      ],

      internalCode: [
        line.internalCode || ''
      ],

      unitValue: this.fb.group({

        currencySold: [
          line.unitValue?.currencySold || 'EGP'
        ],

        amountEGP: [
          line.unitValue?.amountEGP ?? 0,
          Validators.required
        ]

      }),

      discount: this.fb.group({

        rate: [
          line.discount?.rate ?? 0
        ],

        amount: [
          line.discount?.amount ?? 0
        ]

      }),

      taxableItems: this.fb.array(

        (line.taxableItems || []).map(
          (tax: any) =>
            this.createTax(tax)
        )

      )

    });

  }


  createTax(tax: any): FormGroup {

    return this.fb.group({

      taxType: [
        tax.taxType || 'T1'
      ],

      rate: [
        tax.rate ?? 0
      ],

      amount: [
        tax.amount ?? 0
      ]

    });

  }


  onSubmit(): void {

    if (this.invoiceForm.invalid) {

      this.invoiceForm.markAllAsTouched();

      return;

    }

    this.loading = true;
    this.errorMessage = '';


    const invoice =
      this.invoiceForm.getRawValue();


    this.invoiceService
      .updateInvoice(
        this.invoiceId,
        invoice
      )
      .subscribe({

        next: (response: any) => {

          console.log(
            'Invoice Updated:',
            response
          );

          this.loading = false;

          /*
           * Update successful
           * Go back to Invoice List
           */
sessionStorage.setItem(
  'invoiceSuccessMessage',
  'Invoice updated successfully.'
);
          this.router.navigate([
            '/invoice'
          ]);

        },

        error: (error) => {

          console.error(
            'Update Invoice Error:',
            error
          );

          this.loading = false;

          this.errorMessage =
            error?.error?.message ||
            'Unable to update invoice.';

          this.cdr.detectChanges();

        }

      });

  }

}