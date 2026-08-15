import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Invoice } from '../models/invoice.model';
import { ValidationResponse } from '../models/validation-response.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7082/api/InvoiceApi';

  validateInvoice(invoice: Invoice): Observable<ValidationResponse> {
    return this.http.post<ValidationResponse>(
      `${this.apiUrl}/validate`,
      invoice
    );
  }
}
