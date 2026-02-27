import {
    Button,
    Textfield,
    Card,
    Field,
    Label,
    EXPERIMENTAL_Suggestion as Suggestion,
} from "@digdir/designsystemet-react";
import {
    PersonPlusIcon,
    TrashIcon,
} from "@navikt/aksel-icons";
import { Person, HeirFormProps } from "./../../types";
import { RELATIONSHIP_OPTIONS } from "../../../../utils/constants";

export default function PersonForm({ heir, onFetch, onRandom, onRemove, onRelation, errors }: HeirFormProps<Person>) {
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
                    label="Fullt navn"
                    value={heir.name}
                    readOnly={true}
                    error={errors[`${heir.id}-name`]}
                    required
                />

                <div>
                    <Textfield
                        label="Tenor fødselsnummer (11 siffer)"
                        value={heir.nin}
                        error={errors[`${heir.id}-nin`]}
                        maxLength={11}
                        required
                        onChange={async (e) => {
                            if (!onFetch) return
                            const nin = e.target.value.replace(/\D/g, "");
                            await onFetch(heir.id, nin);
                        }}
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
                    <div style={{ flex: 1 }}>
                        <Field>
                            <Label>Relasjon til avdød</Label>
                            <Suggestion
                                selected={heir.relation?.label || ""}
                                onSelectedChange={(item) => {
                                    if (onRelation) onRelation(heir.id, item)
                                  }
                                }
                            >
                                <Suggestion.Input required />
                                <Suggestion.Clear />
                                <Suggestion.List id="relation-list">
                                    <Suggestion.Empty>Tomt</Suggestion.Empty>
                                    {RELATIONSHIP_OPTIONS.map((option) => (
                                        <Suggestion.Option
                                            key={option.value}
                                            value={option.value}
                                        >
                                            {option.label}
                                        </Suggestion.Option>
                                    ))}
                                </Suggestion.List>
                            </Suggestion>
                        </Field>
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
