import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormGroup,
  ReactiveFormsModule
} from '@angular/forms';

@Component({
  selector: 'app-issuer',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './issuer.html',
  styleUrls: ['./issuer.css']
})
export class Issuer {

  @Input({ required: true })
  form!: FormGroup;

}
