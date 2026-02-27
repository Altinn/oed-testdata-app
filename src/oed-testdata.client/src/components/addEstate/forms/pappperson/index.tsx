import {
    Button,
    Textfield,
    Card,
} from "@digdir/designsystemet-react";
import {
    PersonPlusIcon,
    TrashIcon,
} from "@navikt/aksel-icons";
import { HeirFormProps, PersonPapp } from "./../../types";

export default function PappPersonForm({ heir, onRandom, onRemove, errors }: HeirFormProps<PersonPapp>) {
    return (
        <Card key={heir.id} data-color="brand2" variant="tinted">
            <div
                style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fit, minmax(250px, 1fr))",
                    gap: "1rem",
                    alignItems: "start",
                }}
            >
                <Textfield
                    label="Fornavn"
                    value={heir.navn.firstName}
                    error={errors[`${heir.id}-fname`]}
                    required
                />

                <div>
                    <Textfield
                        label="Etternavn"
                        value={heir.navn.lastName}
                        error={errors[`${heir.id}-lname`]}
                        required
                    />
                    <Button
                        type="button"
                        variant="secondary"
                        onClick={() => onRandom(heir.id)}
                        style={{ marginTop: "0.5rem" }}
                    >
                        <PersonPlusIcon />
                        Hent tilfeldig
                    </Button>
                </div>

                <div
                    style={{
                        display: "flex",
                        gap: "0.5rem",
                        alignItems: "end",
                    }}
                >
                    <Textfield
                        label="Fødselsdato (yyyy-mm-dd)"
                        value={heir.dateOfBirth}
                        size={12}
                        error={errors[`${heir.id}-dob`]}
                        maxLength={10}
                        required
                    />

                    <Button
                        type="button"
                        variant="tertiary"
                        data-color="danger"
                        style={{ marginLeft: 'auto' }}
                        onClick={() => onRemove(heir.id)}
                        data-size="md"
                    >
                        <TrashIcon />
                    </Button>
                </div>
            </div>
        </Card>
    );
}
