import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ValidationResponse } from '../../models/validation-response.model';

@Component({
  selector: 'app-invoice-result',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './invoice-result.html',
  styleUrls: ['./invoice-result.css']
})
export class InvoiceResult {

  @Input({ required: true })
  result!: ValidationResponse;

}
