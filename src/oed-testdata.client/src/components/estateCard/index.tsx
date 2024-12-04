import { useState } from "react";
import { Button, Heading, Label, Spinner } from "@digdir/designsystemet-react";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import { ArrowCirclepathIcon, PadlockUnlockedIcon } from "@navikt/aksel-icons";
import { Estate } from "../../interfaces/IEstate";
import { ESTATE_API } from "../../utils/constants";

interface IProps {
  data: Estate;
}

export default function EstateCard({ data }: IProps) {
  const [loadingResetEstate, setLoadingResetEstate] = useState(false);
  const [loadingRemoveRoles, setLoadingRemoveRoles] = useState(false);

  const handleResetEstate = async () => {
    try {
      setLoadingResetEstate(true);
      await fetch(ESTATE_API, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ estateSsn: data.estateSsn }),
      });
    } catch (error) {
      console.error("Error resetting estate:", error);
    } finally {
      setLoadingResetEstate(false);
    }
  };

  const handleRemoveRoles = async () => {
    const estateUrl = `${ESTATE_API}${data.estateSsn}`;
    try {
      setLoadingRemoveRoles(true);
      await fetch(estateUrl, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ estateSsn: data.estateSsn, status: "FEILFORT" }),
      });
    } catch (error) {
      console.error("Error removing roles:", error);
    } finally {
      setLoadingRemoveRoles(false);
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
            const relation = heir.relation?.split(":").pop();
            return (
              <li key={heir.ssn}>
                <Label>{relation}</Label>
                <CopyToClipboard value={heir.ssn} />
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
              <Spinner variant="interaction" title="loading" size="sm" />
              Laster...
            </>
          ) : (
            <>
              <PadlockUnlockedIcon title="fjern roller" fontSize="1.5rem" />
              Fjern roller
            </>
          )}
        </Button>
        <Button
          variant="secondary"
          color="danger"
          onClick={handleResetEstate}
          disabled={loadingResetEstate}
          aria-disabled={loadingResetEstate}
        >
          {loadingResetEstate ? (
            <>
              <Spinner variant="interaction" title="loading" size="sm" />
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
