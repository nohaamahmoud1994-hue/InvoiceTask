import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-invoice-info',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './invoice-info.html',
  styleUrls: ['./invoice-info.css']
})
export class InvoiceInfo {

  @Input({ required: true })
  form!: FormGroup;

}
