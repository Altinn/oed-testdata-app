export interface PersonSearch {
  nin: string;
  name: string;
}

export interface HeirSearch extends PersonSearch {
  relation: string;
}
