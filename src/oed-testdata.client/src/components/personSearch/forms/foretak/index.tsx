import {
    Button,
    Textfield,
    Card,
} from "@digdir/designsystemet-react";
import {
    TrashIcon,
    BriefcaseIcon,
} from "@navikt/aksel-icons";
import { Foretak, HeirFormProps } from "./../../types";

export default function ForetakForm({ heir, onFetch, onRandom, onRemove, errors }: HeirFormProps<Foretak>) {

    return (
        <Card key={heir.id} data-color="brand1" variant="tinted">
            <div
                style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fit, minmax(250px, 1fr))",
                    gap: "1rem",
                    alignItems: "start",
                }}
            >
                <Textfield
                    label="Foretaksnavn"
                    value={heir.name}
                    readOnly={true}
                    error={errors[`${heir.id}-name`]}
                    required
                />

                <div>
                    <Textfield
                        label="Tenor org. nr. (9 siffer)"
                        value={heir.organisasjonsNummer}
                        error={errors[`${heir.id}-nin`]}
                        maxLength={9}
                        required
                        onChange={async (e) => {
                            const orgnum = e.target.value.replace(/\D/g, "");
                            await onFetch(heir.id, orgnum);
                        }}
                    />
                    <Button
                        type="button"
                        variant="secondary"
                        onClick={() => onRandom(heir.id)}
                        style={{ marginTop: "0.5rem" }}
                    >
                        <BriefcaseIcon />
                        Hent tilfeldig
                    </Button>
                </div>

                <div
                    style={{
                        display: "flex",
                        gap: "0.5rem",
                        alignItems: "end",
                        marginTop: "2.2rem",
                    }}
                >
                    <Button
                        type="button"
                        variant="tertiary"
                        data-color="danger"
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
