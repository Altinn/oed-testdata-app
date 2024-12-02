import { Button, Heading, Label } from "@digdir/designsystemet-react";
import { IData } from "../../App";
import "./style.css";
import CopyToClipboard from "../copyToClipboard";
import { ArrowCirclepathIcon, TrashIcon } from "@navikt/aksel-icons";

interface IProps {
  data: IData;
}

export default function Card({ data }: IProps) {
  return (
    <article className="card">
      <Heading level={2} size="md" spacing>
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
        <Button color="danger" variant="secondary" size="sm" disabled>
          <TrashIcon title="a11y-title" fontSize="1.5rem" />
          Slett
        </Button>
        <Button color="first" size="sm">
          <ArrowCirclepathIcon title="a11y-title" fontSize="1.5rem" />
          Nullstill
        </Button>
      </div>
    </article>
  );
}
