import { Person } from "./IPerson";

export interface Estate {
  estateSsn: string;
  estateName: string;
  heirs: Person[];
}
