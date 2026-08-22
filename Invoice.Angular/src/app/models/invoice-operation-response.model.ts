import { Invoice } from './invoice.model';

export interface InvoiceOperationResponse {
  isValid: boolean;
  errors: string[];
  message: string;
  invoice?: Invoice;
}