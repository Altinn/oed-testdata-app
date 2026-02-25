import { useState } from "react";
import {
  Heading,
  Label,
  Spinner,
  Dropdown,
  Tag,
  Card,
  Paragraph,
  Table,
  List,
  Divider,
} from "@digdir/designsystemet-react";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import {
  ArrowCirclepathIcon,
  PadlockUnlockedIcon,
  GavelSoundBlockIcon,
  ClockIcon,
  ClockDashedIcon,
} from "@navikt/aksel-icons";
import { Estate } from "../../interfaces/IEstate";
import { ESTATE_API, RELATIONSHIP_OPTIONS } from "../../utils/constants";
import { useToast } from "../../context/toastContext";
import { addDays, dateOnlyString } from "../../utils/dateUtils";

interface IProps {
  data: Estate;
}

export default function EstateCard({ data }: IProps) {
  const [resetDropdownOpen, setResetDropdownOpen] = useState(false);
  const [probateDropdownOpen, setProbateDropdownOpen] = useState(false);
  const [accessDateDropdownOpen, setAccessDateDropdownOpen] = useState(false);

  const [loadingResetEstate, setLoadingResetEstate] = useState(false);
  const [loadingIssueProbate, setLoadingIssueProbate] = useState(false);
  const [loadingChangeAccesDate, setLoadingChangeAccesDate] = useState(false);
  const { addToast } = useToast();

  const handleResetEstate = async (accessDateOffsetDays: number | null) => {
    try {
      setLoadingResetEstate(true);

      const accessDate = accessDateOffsetDays !== null
        ? dateOnlyString(addDays(new Date(), accessDateOffsetDays))
        : null;

      const response = await fetch(ESTATE_API, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ 
          estateSsn: data.estateSsn,
          tilgangsdatoDigitaltDodsbo: accessDate,
        }),
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
      setLoadingResetEstate(true);
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
      setLoadingResetEstate(false);
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

  const handleChangeAccessDate = async (offsetDays: number | null) => {
    const estateUrl = `${ESTATE_API}${data.estateSsn}`;
    try {
      setLoadingChangeAccesDate(true);

      const accessDate = offsetDays !== null
        ? dateOnlyString(addDays(new Date(), offsetDays))
        : null;

      const response = await fetch(estateUrl, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ 
          estateSsn: data.estateSsn, 
          tilgangsdatoDigitaltDodsbo: accessDate  
        }),
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

    } catch (error) {
      console.error("Error changing access date:", error);
      addToast("Noe gikk galt. Prøv igjen.", "danger");
    } finally {
      setLoadingChangeAccesDate(false);
    }
  }
  
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
                  (p) => (heir.ssn && p.nin === heir.ssn)
                      || (heir.orgNum && p.orgNum === heir.orgNum)
              );
              const relation = RELATIONSHIP_OPTIONS.find((opt) => opt.value === heir.relation)?.label || heir.relation;
              const ssnOrOrgNum = heir.ssn || heir.orgNum
              
              return (
                <Table.Row key={ssnOrOrgNum}>
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
                    <CopyToClipboard value={ssnOrOrgNum} />
                  </Table.Cell>
                </Table.Row>
              );
            })}
          </Table.Body>
        </Table>
      </Card.Block>
      <Card.Block>
        <div className="card__footer">
          <Dropdown.TriggerContext>
            <Dropdown.Trigger 
              data-color="accent" 
              variant="secondary" 
              onClick={() => setProbateDropdownOpen(!probateDropdownOpen)}
            >
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
            <Dropdown 
              open={probateDropdownOpen} 
              onClose={() => setProbateDropdownOpen(false)}
            >
              <Dropdown.List>
                <Dropdown.Item
                  onClick={() => {
                    setProbateDropdownOpen(false);
                    handleIssueProbate(
                      "PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99"
                    )
                  }}
                >
                  <Dropdown.Button>
                    <GavelSoundBlockIcon />
                    Privat skifte
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item
                  onClick={() => {
                    setProbateDropdownOpen(false);
                    handleIssueProbate("OFFENTLIG_SKIFTE_ETTER_BEGJARING")
                  }}
                >
                  <Dropdown.Button>
                    <GavelSoundBlockIcon />
                    Offentlig skifte etter begjæring
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item
                  onClick={() => {
                    setProbateDropdownOpen(false);
                    handleIssueProbate(
                      "GJENLEVENDE_EKTEFELLE_I_USKIFTE_IHT_ARVELOVEN_KAP_5"
                    )
                  }}
                >
                  <Dropdown.Button>
                    <GavelSoundBlockIcon />
                    Gjenlevende ektefelle i uskifte
                  </Dropdown.Button>
                </Dropdown.Item>
              </Dropdown.List>
            </Dropdown>
          </Dropdown.TriggerContext>
          <Dropdown.TriggerContext>
            <Dropdown.Trigger 
              data-color="accent"
              variant="secondary"
              disabled={ loadingChangeAccesDate }
              onClick={() => setAccessDateDropdownOpen(!accessDateDropdownOpen)}
            >
              { loadingChangeAccesDate ? (
                <Spinner aria-label="laster" />
              ) : (
                <ClockIcon title="endre tilgansdato" fontSize="1.5rem" />
              )}
              Tilgangsdato
            </Dropdown.Trigger>
            <Dropdown
              open={accessDateDropdownOpen} 
              onClose={() => setAccessDateDropdownOpen(false)}
            >
              <Dropdown.List>
                <Dropdown.Item onClick={() => {
                  setAccessDateDropdownOpen(false);
                  handleChangeAccessDate(0);
                }}>
                  <Dropdown.Button>
                    <ClockDashedIcon />
                    I dag
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item onClick={() => {
                  setAccessDateDropdownOpen(false);
                  handleChangeAccessDate(1);
                }}>
                  <Dropdown.Button>
                    <ClockDashedIcon />
                    I morgen
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item onClick={() => {
                  setAccessDateDropdownOpen(false);
                  handleChangeAccessDate(5);
                }}>
                  <Dropdown.Button>
                    <ClockDashedIcon />
                    + 5d
                  </Dropdown.Button>
                </Dropdown.Item>
                <Divider />
                <Dropdown.Item onClick={() => {
                  setAccessDateDropdownOpen(false);
                  handleChangeAccessDate(-1);
                }}>
                  <Dropdown.Button>
                    <ClockDashedIcon />
                    I går
                  </Dropdown.Button>
                </Dropdown.Item>
              </Dropdown.List>
            </Dropdown>
          </Dropdown.TriggerContext>
          <Dropdown.TriggerContext>
            <Dropdown.Trigger 
              data-color="danger"
              variant="secondary"
              disabled={ loadingResetEstate }
              onClick={() => setResetDropdownOpen(!resetDropdownOpen)}
            >
              { loadingResetEstate ? (
                <Spinner aria-label="laster" />
              ) : (
                <ArrowCirclepathIcon title="a11y-title" fontSize="1.5rem" />
              )}
              Nullstill
            </Dropdown.Trigger>
            <Dropdown
              open={resetDropdownOpen} 
              onClose={() => setResetDropdownOpen(false)}
            >
              <Dropdown.List>
                <Dropdown.Item
                  onClick={() => { 
                    setResetDropdownOpen(false);
                    handleResetEstate(null); 
                  }}
                >
                  <Dropdown.Button>
                    <ArrowCirclepathIcon />
                    Nullstill
                  </Dropdown.Button>
                </Dropdown.Item>
                <Dropdown.Item
                  onClick={() => {
                    setResetDropdownOpen(false);
                    handleResetEstate(5);
                  }}
                >
                  <Dropdown.Button>
                    <ClockIcon />
                    Nullstill - tilgang +5d
                  </Dropdown.Button>
                </Dropdown.Item>
                <Divider />
                <Dropdown.Item
                  onClick={() => {
                    setResetDropdownOpen(false);
                    handleRemoveRoles();
                  }}
                >
                  <Dropdown.Button>
                    <PadlockUnlockedIcon title="fjern roller" fontSize="1.5rem" />
                    Fjern roller
                  </Dropdown.Button>
                </Dropdown.Item>
              </Dropdown.List>
            </Dropdown>
          </Dropdown.TriggerContext>
          {/* <Button
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
                Nullstill
              </>
            )}
          </Button> */}          
        </div>
      </Card.Block>
    </Card>
  );
}
