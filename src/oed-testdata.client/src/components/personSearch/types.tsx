type SuggestionItem = {
    value: string;
    label: string;
};

type CommonHeirFields = {
    type: string;
    id: string;
    mottakerOriginalSkifteattest: boolean;
    relation?: SuggestionItem;
    role: string;
};

export type Heir =
    | Person
    | Foretak
    | PersonPapp
    | ForetakPapp;

export type Person = {
    type: 'Person';
    nin: string;
    name: string;
} & (CommonHeirFields)

export type Foretak = {
    type: 'Foretak';
    organisasjonsNummer: string;
    name: string;
} & (CommonHeirFields)

export type PersonPapp = {
    type: 'PersonPapp';
    dateOfBirth: string;
    navn: {
        firstName: string;
        lastName: string;
    }
} & (CommonHeirFields)

export type ForetakPapp = {
    type: 'ForetakPapp';
    organisasjonsNavn: string;
    registreringsNummer: string;
    countryCode: string;
    role: string;
} & (CommonHeirFields)

export interface HeirFormProps<T extends Heir> {
    heir: T;
    onFetch: (id: string, num: string) => void;
    onRandom: (id: string) => void;
    onRemove: (id: string) => void;
    onRelation?: (id: string, item: SuggestionItem | undefined) => void;
    errors: Record<string, string>;
}

