import { useState } from "react";
import {
  Button,
  Heading,
  Label,
  Spinner,
  Dropdown,
  Tag,
  Card,
  Paragraph,
  Table,
  List,
} from "@digdir/designsystemet-react";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import {
  ArrowCirclepathIcon,
  PadlockUnlockedIcon,
  GavelSoundBlockIcon,
} from "@navikt/aksel-icons";
import { Estate } from "../../interfaces/IEstate";
import { ESTATE_API, RELATIONSHIP_OPTIONS } from "../../utils/constants";
import { useToast } from "../../context/toastContext";

interface IProps {
  data: Estate;
}

export default function EstateCard({ data }: IProps) {
  const [loadingResetEstate, setLoadingResetEstate] = useState(false);
  const [loadingRemoveRoles, setLoadingRemoveRoles] = useState(false);
  const [loadingIssueProbate, setLoadingIssueProbate] = useState(false);
  const { addToast } = useToast();

  const handleResetEstate = async () => {
    try {
      setLoadingResetEstate(true);
      const response = await fetch(ESTATE_API, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ estateSsn: data.estateSsn }),
      });

      /* if auth token expired show toast */
      if (response.status === 401) {
        addToast(
          "Autentiseringstoken har utløpt. Vennligst logg inn igjen.",
          "danger"
        );
        return;
      }
      if (!response.ok) {
        addToast("Noe gikk galt. Prøv igjen", "danger");
      }
      addToast("Dødsboet ble nullstilt.", "success");
    } catch (error) {
      console.error("Error resetting estate:", error);
      addToast("Noe gikk galt. Prøv igjen.", "danger");
    } finally {
      setLoadingResetEstate(false);
    }
  };

  const handleRemoveRoles = async () => {
    const estateUrl = `${ESTATE_API}${data.estateSsn}`;
    try {
      setLoadingRemoveRoles(true);
      const response = await fetch(estateUrl, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ estateSsn: data.estateSsn, status: "FEILFORT" }),
      });
      if (response.status === 401) {
        addToast(
          "Autentiseringstoken har utløpt. Vennligst logg inn igjen.",
          "danger"
        );
        return;
      }
      if (!response.ok) {
        addToast("Noe gikk galt. Prøv igjen", "danger");
      }
      addToast("Rollene ble fjernet.", "success");
    } catch (error) {
      console.error("Error removing roles:", error);
      addToast("Noe gikk galt. Prøv igjen.", "danger");
    } finally {
      setLoadingRemoveRoles(false);
    }
  };

  const handleIssueProbate = async (type: string) => {
    const estateUrl = `${ESTATE_API}${data.estateSsn}`;
    if (
      confirm(
        "Er du sikker på at du vil utestede skifteattest til dette boet?\n\n" +
          "Denne funksjonen er KUN ment å simulere utsendelse av skifteattest fra DA uten at det på forhånd er sendt inn en skifteerklæring via Digitalt Dødsbo.\n\n" +
          "NB! Skifteattesten vil KUN bli utstedt til den første arvingen i boet.\n\n" +
          "Dersom man heller ønsker en skifteattest som tar hensyn til valg tatt i skifteerklæringen skal dette gjøres ved å sende inn skifteerklæringen."
      ) == false
    ) {
      return;
    }

    try {
      setLoadingIssueProbate(true);
      const response = await fetch(estateUrl, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ estateSsn: data.estateSsn, resultatType: type }),
      });
      if (response.status === 401) {
        addToast(
          "Autentiseringstoken har utløpt. Vennligst logg inn igjen.",
          "danger"
        );
        return;
      }
      if (!response.ok) {
        addToast("Noe gikk galt. Prøv igjen", "danger");
      }
      addToast("Skifteattest ble utstedt.", "success");
    } catch (error) {
      console.error("Error issuing probate:", error);
      addToast("Noe gikk galt. Prøv igjen.", "danger");
    } finally {
      setLoadingIssueProbate(false);
    }
  };

  return (
    <Card data-color="brand2">
      <Card.Block>
        <Heading level={2}>Dødsbo - {data.estateName}</Heading>
        <CopyToClipboard value={data.estateSsn} />
        {data.metadata.tags?.length > 0 && (
          <List.Unordered className="tag-list">
            {data.metadata.tags?.map((tag) => (
              <List.Item key={tag}>
                <Tag data-color="warning" variant="outline">
                  {tag}
                </Tag>
              </List.Item>
            ))}
          </List.Unordered>
        )}
      </Card.Block>
      <Card.Block>
        <Table zebra>
          <caption>Arvinger</caption>
          <Table.Body>
            {data.heirs.map((heir) => {
              const metadata = data?.metadata?.persons?.find(
                (p) => p.nin === heir.ssn
              );
              const relation =
                RELATIONSHIP_OPTIONS.find((opt) => opt.value === heir.relation)
                  ?.label || heir.relation;

              return (
                <Table.Row key={heir.ssn}>
                  <Table.Cell
                    className="flex-between"
                    style={{ alignItems: "baseline" }}
                  >
                    <div className="flex-col">
                      <Label weight="semibold">
                        {metadata?.name || "<ukjent>"}
                      </Label>
                      <Paragraph>{relation}</Paragraph>
                    </div>
                    <CopyToClipboard value={heir.ssn} />
                  </Table.Cell>
                </Table.Row>
              );
            })}
          </Table.Body>
        </Table>
      </Card.Block>

      <Card.Block>
        <div className="card__footer">
          <Button
            data-color="accent"
            variant="secondary"
            onClick={handleRemoveRoles}
            disabled={loadingRemoveRoles}
            aria-disabled={loadingRemoveRoles}
          >
            {loadingRemoveRoles ? (
              <>
                <Spinner aria-label="laster" />
                Laster...
              </>
            ) : (
              <>
                <PadlockUnlockedIcon title="fjern roller" fontSize="1.5rem" />
                Fjern roller
              </>
            )}
          </Button>
          <Dropdown.TriggerContext>
            <Dropdown.Trigger data-color="accent" variant="secondary">
              {loadingIssueProbate ? (
                <Spinner aria-label="laster" />
              ) : (
                <GavelSoundBlockIcon
                  title="utestede skifteattest"
                  fontSize="1.5rem"
                />
              )}
              Skifteattest
            </Dropdown.Trigger>
            <Dropdown>
              <Dropdown.List>
                <Dropdown.Item
                  onClick={() =>
                    handleIssueProbate(
                      "PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99"
                    )
                  }
                >
                  <Dropdown.Button>
                    <GavelSoundBlockIcon />
                    Privat skifte
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item
                  onClick={() =>
                    handleIssueProbate("OFFENTLIG_SKIFTE_ETTER_BEGJARING")
                  }
                >
                  <Dropdown.Button>
                    <GavelSoundBlockIcon />
                    Offentlig skifte etter begjæring
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item
                  onClick={() =>
                    handleIssueProbate(
                      "GJENLEVENDE_EKTEFELLE_I_USKIFTE_IHT_ARVELOVEN_KAP_5"
                    )
                  }
                >
                  <Dropdown.Button>
                    <GavelSoundBlockIcon />
                    Gjenlevende ektefelle i uskifte
                  </Dropdown.Button>
                </Dropdown.Item>
              </Dropdown.List>
            </Dropdown>
          </Dropdown.TriggerContext>
          <Button
            data-color="danger"
            variant="secondary"
            onClick={handleResetEstate}
            disabled={loadingResetEstate}
            aria-disabled={loadingResetEstate}
          >
            {loadingResetEstate ? (
              <>
                <Spinner aria-label="laster" />
                Laster...
              </>
            ) : (
              <>
                <ArrowCirclepathIcon title="a11y-title" fontSize="1.5rem" />
                Nullstill bo
              </>
            )}
          </Button>
        </div>
      </Card.Block>
    </Card>
  );
}
