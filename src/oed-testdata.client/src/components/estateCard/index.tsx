import { Button, Heading, Label } from "@digdir/designsystemet-react";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import { ArrowCirclepathIcon } from "@navikt/aksel-icons";
import { Estate } from "../../interfaces/IEstate";

interface IProps {
  data: Estate;
}

export default function EstateCard({ data }: IProps) {
  return (
    <article className="card">
      <Heading level={2} size="md" spacing className="card__heading">
        Dødsbo
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
        <Button color="first" size="sm">
          <ArrowCirclepathIcon title="a11y-title" fontSize="1.5rem" />
          Nullstill
        </Button>
      </div>
    </article>
  );
}
