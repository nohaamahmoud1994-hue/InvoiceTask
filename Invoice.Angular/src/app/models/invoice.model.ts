import { Issuer } from "./issuer.model";
import { Receiver } from "./receiver.model";
import { InvoiceLine } from "./invoice-line.model";
import { TaxTotal } from "./tax-total.model";

export interface Invoice {
  id: number;
  
  issuer: Issuer;

  receiver: Receiver;

  documentType: string;

  documentTypeVersion: string;

  dateTimeIssued: Date;

  taxpayerActivityCode: string;

  internalId: string;

  purchaseOrderReference?: string;

  purchaseOrderDescription?: string;

  salesOrderReference?: string;

  salesOrderDescription?: string;

  proformaInvoiceNumber?: string;

  invoiceLines: InvoiceLine[];

  totalSalesAmount: number;

  totalDiscountAmount: number;

  netAmount: number;

  taxTotals: TaxTotal[];

  extraDiscountAmount: number;

  totalItemsDiscountAmount: number;

  totalAmount: number;

  serviceDeliveryDate?: Date;

}
