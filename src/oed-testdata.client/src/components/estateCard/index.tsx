import { Button, Heading, Label, Spinner } from "@digdir/designsystemet-react";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import { ArrowCirclepathIcon } from "@navikt/aksel-icons";
import { Estate } from "../../interfaces/IEstate";
import { ESTATE_API } from "../../utils/constants";
import { useState } from "react";

interface IProps {
  data: Estate;
}

export default function EstateCard({ data }: IProps) {
  const [loading, setLoading] = useState(false);

  const handleResetEstate = async () => {
    setLoading(true);
    const response = await fetch(ESTATE_API, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ estateSsn: data.estateSsn }),
    });

    /* TODO: handle response success or failure with toasts */

    if (!response.ok) {
      console.error("Error resetting estate:", response.statusText);
    }

    setLoading(false);
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
          color="first"
          size="sm"
          onClick={handleResetEstate}
          aria-disabled={loading}
        >
          {loading ? (
            <>
              <Spinner variant="interaction" title="loading" size="sm" />
              Laster...
            </>
          ) : (
            <>
              <ArrowCirclepathIcon title="a11y-title" fontSize="1.5rem" />
              Nullstill
            </>
          )}
        </Button>
      </div>
    </article>
  );
}
