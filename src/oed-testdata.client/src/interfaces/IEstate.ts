import { Person } from "./IPerson";

export interface EstateMetadataPerson {
  nin: string;
  name: string;
}

export interface EstateMetadata {
  persons: EstateMetadataPerson[];
  tags: string[];
}

export interface Estate {
  estateSsn: string;
  estateName: string;
  metadata: EstateMetadata;
  heirs: Person[];
}
