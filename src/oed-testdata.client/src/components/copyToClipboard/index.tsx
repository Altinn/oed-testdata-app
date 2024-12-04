import { useState } from "react";
import { Button, Paragraph, Tooltip } from "@digdir/designsystemet-react";
import "./style.css";
import { FilesIcon } from "@navikt/aksel-icons";

interface IProps {
  value: string;
}

export default function CopyToClipboard({ value }: IProps) {
  const [copied, setCopied] = useState<string | null>(null);
  const isNumeric = /^\d+$/.test(value);

  return (
    <div className="copy-to-clipboard">
      <Paragraph className={isNumeric ? "numeric" : ""}>{value}</Paragraph>
      <Tooltip content="kopiert" open={copied === value} placement="right" >
        <Button
          icon
          color="second"
          variant="tertiary"
          size="small"
          onClick={() => {
            navigator.clipboard.writeText(value);
            setCopied(value);
            setTimeout(() => {
              setCopied(null);
            }, 2000);
          }}
          aria-label="Kopier til utklippstavle"
        >
          <FilesIcon title="a11y-title" fontSize="1.5rem" />
        </Button>
      </Tooltip>
    </div>
  );
}
