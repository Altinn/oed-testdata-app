import { useState } from "react";
import { Button, Heading, Label, Spinner, DropdownMenu } from "@digdir/designsystemet-react";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import { ArrowCirclepathIcon, PadlockUnlockedIcon, GavelSoundBlockIcon } from "@navikt/aksel-icons";
import { Estate } from "../../interfaces/IEstate";
import { ESTATE_API } from "../../utils/constants";
import { useToast } from "../../context/toastContext";

interface IProps {
  data: Estate;
}

export default function EstateCard({ data }: IProps) {
  const [loadingResetEstate, setLoadingResetEstate] = useState(false);
  const [loadingRemoveRoles, setLoadingRemoveRoles] = useState(false);
  const [loadingIssueProbate, setLoadingIssueProbate] = useState(false);
  const [probateDropdownOpen, setProbateDropdownOpen] = useState(false);
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
    if (confirm(
        "Er du sikker på at du vil utestede skifteattest til dette boet?\n\n" +
        "Denne funksjonen er KUN ment å simulere utsendelse av skifteattest fra DA uten at det på forhånd er sendt inn en skifteerklæring via Digitalt Dødsbo.\n\n" +
        "NB! Skifteattesten vil KUN bli utstedt til den første arvingen i boet.\n\n" +
        "Dersom man heller ønsker en skifteattest som tar hensyn til valg tatt i skifteerklæringen skal dette gjøres ved å sende inn skifteerklæringen.") == false) {
      return;
    }

    try {
      setLoadingIssueProbate(true);
      setProbateDropdownOpen(false);
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
    <article className="card">
      <Heading level={2} size="md" spacing className="card__heading">
        Dødsbo - {data.estateName}
        <CopyToClipboard value={data.estateSsn} />
      </Heading>

      <section className="card__content">
        <Heading level={3} size="sm" spacing>
          Arvinger
        </Heading>
        <ul>
          {data.heirs.map((heir) => {
            const metadata = data?.metadata?.persons?.find(p => p.nin === heir.ssn);
            const relation = heir.relation?.split(":").pop();
            return (
              <li key={heir.ssn}>                
                <div>
                  <Label weight="semibold">{metadata?.name || '<ukjent>'}</Label>
                  <CopyToClipboard value={heir.ssn} />
                </div>
                <div>
                  <Label size="small">{relation}</Label>
                </div>
              </li>
            );
          })}
        </ul>
      </section>

      <div className="card__footer">
        <Button
          variant="secondary"
          onClick={handleRemoveRoles}
          disabled={loadingRemoveRoles}
          aria-disabled={loadingRemoveRoles}
        >
          {loadingRemoveRoles ? (
            <>
              <Spinner title="laster" size="sm" />
              Laster...
            </>
          ) : (
            <>
              <PadlockUnlockedIcon title="fjern roller" fontSize="1.5rem" />
              Fjern roller
            </>
          )}
        </Button>
        <DropdownMenu open={probateDropdownOpen} onClose={() => setProbateDropdownOpen(false)}>
          <DropdownMenu.Trigger 
            variant="secondary" 
            disabled={loadingIssueProbate} 
            onClick={() => setProbateDropdownOpen(!probateDropdownOpen)}
          >
            {loadingIssueProbate ? (
              <Spinner title="laster" size="sm" />
            ) : (
              <GavelSoundBlockIcon title="utestede skifteattest" fontSize="1.5rem" />
            )}
            Skifteattest
          </DropdownMenu.Trigger>
          <DropdownMenu.Content>
            <DropdownMenu.Group>
              <DropdownMenu.Item onClick={() => handleIssueProbate('PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99')}>
                <GavelSoundBlockIcon />
                Privat skifte
              </DropdownMenu.Item>
              <DropdownMenu.Item onClick={() => handleIssueProbate('OFFENTLIG_SKIFTE_ETTER_BEGJARING')}>
                <GavelSoundBlockIcon />
                Offentlig skifte etter begjæring
              </DropdownMenu.Item>
              <DropdownMenu.Item onClick={() => handleIssueProbate('USKIFTE_MED_SAMTYKKE')}>
                <GavelSoundBlockIcon />
                Uskifte med samtykke
              </DropdownMenu.Item>
              <DropdownMenu.Item onClick={() => handleIssueProbate('GJENLEVENDE_EKTEFELLE_I_USKIFTE_IHT_ARVELOVEN_KAP_5')}>
                <GavelSoundBlockIcon />
                Gjenlevende ektefelle i uskifte
              </DropdownMenu.Item>
            </DropdownMenu.Group>            
          </DropdownMenu.Content>
        </DropdownMenu>
        <Button
          variant="secondary"
          color="danger"
          onClick={handleResetEstate}
          disabled={loadingResetEstate}
          aria-disabled={loadingResetEstate}
        >
          {loadingResetEstate ? (
            <>
              <Spinner title="laster" size="xs" />
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
    </article>
  );
}
