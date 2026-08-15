import { IssuerAddress } from "./issuer-address.model";

export interface Issuer {

  type: string;

  id: string;

  name: string;

  address: IssuerAddress;

}
