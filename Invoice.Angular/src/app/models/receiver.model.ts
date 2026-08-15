import { ReceiverAddress } from "./receiver-address.model";

export interface Receiver {

  type: string;

  id?: string;

  name?: string;

  address?: ReceiverAddress;

}
