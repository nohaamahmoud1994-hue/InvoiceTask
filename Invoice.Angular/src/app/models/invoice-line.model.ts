import { Discount } from "./discount.model";
import { TaxableItem } from "./taxable-item.model";
import { Value } from "./value.model";

export interface InvoiceLine {

  description: string;

  itemType: string;

  itemCode: string;

  unitType: string;

  quantity: number;

  unitValue: Value;

  salesTotal: number;

  total: number;

  valueDifference: number;

  totalTaxableFees: number;

  netTotal: number;

  itemsDiscount: number;

  discount?: Discount;

  taxableItems: TaxableItem[];

  internalCode?: string;

}
