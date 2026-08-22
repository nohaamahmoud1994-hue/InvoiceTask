import { Invoice } from './invoice.model';

export interface InvoiceGetAllResponse {

  isValid: boolean;

  message: string;

  invoices: Invoice[];

}