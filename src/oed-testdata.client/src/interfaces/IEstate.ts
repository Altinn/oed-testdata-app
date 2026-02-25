import { Heir } from "./IHeir";

// Not only a person, could also be an organization. Using this interface to support both cases, without changing too much of the existing code.
export interface EstateMetadataPerson {
  nin: string;
  orgNum: string;
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
  heirs: Heir[];
}