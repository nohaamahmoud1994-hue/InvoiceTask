import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Invoice } from '../models/invoice.model';
import { InvoiceGetAllResponse } from '../models/invoice-get-all-response.model';
import { InvoiceGetResponse } from '../models/invoice-get-response.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7082/api/InvoiceApi';


  getAllInvoices(): Observable<InvoiceGetAllResponse> {

    return this.http.get<InvoiceGetAllResponse>(
      this.apiUrl
    );

  }


  getInvoiceById(id: number): Observable<InvoiceGetResponse> {

    return this.http.get<InvoiceGetResponse>(
      `${this.apiUrl}/${id}`
    );

  }


  createInvoice(invoice: Invoice): Observable<Invoice> {

    return this.http.post<Invoice>(
      this.apiUrl,
      invoice
    );

  }


  updateInvoice(id: number, invoice: Invoice): Observable<any> {

    return this.http.put<any>(
      `${this.apiUrl}/${id}`,
      invoice
    );

  }


  deleteInvoice(id: number): Observable<any> {

    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );

  }

}