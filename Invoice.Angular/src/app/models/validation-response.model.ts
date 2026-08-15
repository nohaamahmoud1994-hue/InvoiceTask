import { TaxTotal } from './tax-total.model';

export interface ValidationResponse {
  isValid: boolean;
  errors: string[];
  totalSalesAmount: number;
  totalDiscountAmount: number;
  netAmount: number;
  taxTotals: TaxTotal[];
  totalAmount: number;
}
