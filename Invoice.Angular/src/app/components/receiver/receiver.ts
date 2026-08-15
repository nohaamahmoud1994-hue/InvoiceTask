import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-receiver',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './receiver.html',
  styleUrls: ['./receiver.css']
})
export class Receiver {

  @Input({ required: true })
  form!: FormGroup;

}
