import { Invoice } from './invoice.model';

export interface InvoiceGetResponse {
  isValid: boolean;
  message: string;
  invoice?: Invoice;
}