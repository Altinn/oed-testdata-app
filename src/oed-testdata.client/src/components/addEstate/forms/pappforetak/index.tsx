import {
    Button,
    Textfield,
    Card,
} from "@digdir/designsystemet-react";
import {
    TrashIcon,
    BriefcaseIcon,
} from "@navikt/aksel-icons";
import { ForetakPapp, HeirFormProps } from "./../../types";

export default function PappForetakForm({ heir, onRandom, onRemove, errors }: HeirFormProps<ForetakPapp>) {
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
                    label="Organisasjonsnavn"
                    value={heir.organisasjonsNavn}
                    readOnly={false}
                    error={errors[`${heir.id}-name`]}
                    required
                />

                <div>
                    <Textfield
                        label="Registreringsnummer"
                        value={heir.registreringsNummer}
                        error={errors[`${heir.id}-reg`]}
                        maxLength={20}
                        required
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
                    }}
                >
                    <div style={{ flex: 1 }}>
                        <Textfield
                            label="Landskode (3 tegn)"
                            value={heir.countryCode}
                            error={errors[`${heir.id}-cc`]}
                            maxLength={3}
                            size={3}
                            required
                        />
                    </div>
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
