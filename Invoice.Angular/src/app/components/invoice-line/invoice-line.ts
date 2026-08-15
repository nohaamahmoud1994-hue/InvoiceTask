import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-invoice-line',
  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule
  ],

  templateUrl: './invoice-line.html',
  styleUrls: ['./invoice-line.css']
})
export class InvoiceLine {

  @Input({ required: true })
  form!: FormGroup;

  private fb = inject(FormBuilder);


  // ==========================================
  // INVOICE LINES
  // ==========================================

  get invoiceLines(): FormArray {
    return this.form.get('invoiceLines') as FormArray;
  }


  // ==========================================
  // TAXABLE ITEMS
  // ==========================================

  getTaxableItems(line: AbstractControl): FormArray {

    return line.get('taxableItems') as FormArray;

  }


  // ==========================================
  // ADD INVOICE LINE
  // ==========================================

  addInvoiceLine(): void {

    const line = this.fb.group({

      description: [
        '',
        Validators.required
      ],

      itemType: [
        'EGS',
        Validators.required
      ],

      itemCode: [
        '',
        Validators.required
      ],

      unitType: [
        '',
        Validators.required
      ],

      quantity: [
        1,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      internalCode: [''],


      // ==========================================
      // UNIT VALUE
      // ==========================================

      unitValue: this.fb.group({

        currencySold: [
          'EGP',
          Validators.required
        ],

        amountEGP: [
          0,
          [
            Validators.required,
            Validators.min(0)
          ]
        ],

        amountSold: [null],

        currencyExchangeRate: [null]

      }),


      // ==========================================
      // DISCOUNT
      // ==========================================

      discount: this.fb.group({

        rate: [
          null,
          [
            Validators.min(0),
            Validators.max(100)
          ]
        ],

        amount: [
          {
            value: 0,
            disabled: true
          }
        ]

      }),


      // ==========================================
      // TAXES
      // ==========================================

      taxableItems: this.fb.array([])

    });


    this.invoiceLines.push(line);

    this.calculateLine(line);
  }


  // ==========================================
  // REMOVE INVOICE LINE
  // ==========================================

  removeInvoiceLine(index: number): void {

    this.invoiceLines.removeAt(index);

  }


  // ==========================================
  // ADD TAX
  // ==========================================

  addTax(line: AbstractControl): void {

    const taxes = this.getTaxableItems(line);


    const tax = this.fb.group({

      taxType: [
        'T1',
        Validators.required
      ],

      rate: [
        0,
        [
          Validators.required,
          Validators.min(0),
          Validators.max(100)
        ]
      ],

      amount: [
        {
          value: 0,
          disabled: true
        }
      ]

    });


    taxes.push(tax);

    this.calculateTaxes(line);
  }


  // ==========================================
  // REMOVE TAX
  // ==========================================

  removeTax(
    line: AbstractControl,
    index: number
  ): void {

    const taxes = this.getTaxableItems(line);

    taxes.removeAt(index);

    this.calculateTaxes(line);
  }


  // ==========================================
  // CALCULATE LINE
  // ==========================================

  calculateLine(line: AbstractControl): void {

    const quantity =
      Number(line.get('quantity')?.value) || 0;

    const amountEGP =
      Number(line.get('unitValue.amountEGP')?.value) || 0;

    const rate =
      Number(line.get('discount.rate')?.value) || 0;


    // Sales Total

    const salesTotal =
      quantity * amountEGP;


    // Discount

    const discountAmount =
      salesTotal * rate / 100;


    line
      .get('discount.amount')
      ?.setValue(
        discountAmount,
        {
          emitEvent: false
        }
      );


    // Taxes

    this.calculateTaxes(line);
  }


  // ==========================================
  // CALCULATE TAXES
  // ==========================================

  calculateTaxes(line: AbstractControl): void {

    const quantity =
      Number(line.get('quantity')?.value) || 0;

    const amountEGP =
      Number(line.get('unitValue.amountEGP')?.value) || 0;

    const discountRate =
      Number(line.get('discount.rate')?.value) || 0;


    // Sales Total

    const salesTotal =
      quantity * amountEGP;


    // Discount

    const discountAmount =
      salesTotal * discountRate / 100;


    // Net Total

    const netTotal =
      salesTotal - discountAmount;


    // Taxes

    const taxes =
      this.getTaxableItems(line);


    taxes.controls.forEach(tax => {

      const taxRate =
        Number(tax.get('rate')?.value) || 0;


      const taxAmount =
        netTotal * taxRate / 100;


      tax
        .get('amount')
        ?.setValue(
          taxAmount,
          {
            emitEvent: false
          }
        );

    });

  }


  // ==========================================
  // VALUE CHANGE
  // ==========================================

  onValueChange(line: AbstractControl): void {

    this.calculateLine(line);

  }

}
