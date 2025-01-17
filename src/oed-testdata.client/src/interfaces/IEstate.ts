import { Person } from "./IPerson";

export interface EstateMetadataPerson {
  nin: string;
  name: string;
}

export interface EstateMetadata {
  persons: EstateMetadataPerson[];
}

export interface Estate {
  estateSsn: string;
  estateName: string;
  metadata: EstateMetadata;
  heirs: Person[];
}
