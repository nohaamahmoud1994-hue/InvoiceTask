import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

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
import { InvoiceResult } from '../../components/invoice-result/invoice-result';

import { InvoiceService } from '../../services/invoice.service';
import { ValidationResponse } from '../../models/validation-response.model';

@Component({
  selector: 'app-invoice',
  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    InvoiceInfo,
    Issuer,
    Receiver,
    InvoiceLine,
    InvoiceResult
  ],

  templateUrl: './invoice.html',
  styleUrls: ['./invoice.css']
})
export class Invoice {

  activeTab = 'invoice-info';

  private fb = inject(FormBuilder);
  private invoiceService = inject(InvoiceService);
  private cdr = inject(ChangeDetectorRef);

  validationResult: ValidationResponse | null = null;

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

      type: [
        '',
        Validators.required
      ],

      id: [
        '',
        Validators.required
      ],

      name: [
        '',
        Validators.required
      ],

      address: this.fb.group({

        branchId: [
          '',
          Validators.required
        ],

        country: [
          '',
          Validators.required
        ],

        governate: [
          '',
          Validators.required
        ],

        regionCity: [
          '',
          Validators.required
        ],

        street: [
          '',
          Validators.required
        ],

        buildingNumber: [
          '',
          Validators.required
        ],

        postalCode: [''],
        floor: [''],
        room: [''],
        landmark: [''],
        additionalInformation: ['']

      })

    }),

    receiver: this.fb.group({

      type: [
        '',
        Validators.required
      ],

      id: [
        '',
        Validators.required
      ],

      name: [
        '',
        Validators.required
      ],

      address: this.fb.group({

        country: [
          '',
          Validators.required
        ],

        governate: [
          '',
          Validators.required
        ],

        regionCity: [
          '',
          Validators.required
        ],

        street: [
          '',
          Validators.required
        ],

        buildingNumber: [
          '',
          Validators.required
        ]

      })

    }),

    invoiceLines: this.fb.array([])

  });


  get invoiceLines(): FormArray {

    return this.invoiceForm.get(
      'invoiceLines'
    ) as FormArray;

  }


  onSubmit(): void {

    if (this.invoiceForm.invalid) {

      this.invoiceForm.markAllAsTouched();

      return;
    }

    const invoice =
      this.invoiceForm.getRawValue();

    this.invoiceService
      .validateInvoice(invoice)
      .subscribe({

        next: (response: ValidationResponse) => {

          console.log(
            'Validation Response:',
            response
          );

          this.validationResult = response;

          // Force Angular to update the view
          this.cdr.detectChanges();

        },

        error: (error) => {

          console.error(
            'Validation Error:',
            error
          );

          if (error.error) {

            this.validationResult =
              error.error;

            // Force Angular to update the view
            this.cdr.detectChanges();

          }

        }

      });

  }

}
