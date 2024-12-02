import { Heading } from "@digdir/designsystemet-react";
import "./App.css";
import Card from "./components/card";

export interface IPerson {
  ssn: string;
  relation?: string;
}

export interface IData {
  estateSsn: string;
  heirs: IPerson[];
}

const testdata = [
  {
    estateSsn: "24817296595",
    heirs: [
      {
        ssn: "05844498863",
        relation: "MOR",
      },
      {
        ssn: "13814497164",
        relation: "FAR",
      },
      {
        ssn: "13837297278",
        relation: "GJENLEV_PARTNER",
      },
      {
        ssn: "15870897739",
        relation: "BARN",
      },
      {
        ssn: "10839998622",
        relation: "BARN",
      },
    ],
  },
  /* new testdata with only two childs left */
  {
    estateSsn: "24817296597",
    heirs: [
      {
        ssn: "15870897739",
        relation: "BARN",
      },
      {
        ssn: "10839998622",
        relation: "BARN",
      },
    ],
  },
  /* new testdata with 3 child and gjenlevende partner */
  {
    estateSsn: "24817296593",
    heirs: [
      {
        ssn: "15870897739",
        relation: "BARN",
      },
      {
        ssn: "10839998622",
        relation: "BARN",
      },
      {
        ssn: "13837297278",
        relation: "GJENLEV_PARTNER",
      },
    ],
  },
];

function App() {
  return (
    <main>
      <Heading level={1} size="xl" spacing>
        Digitalt Dødsbo - Testdata
      </Heading>

      <ul className="container__flex">
        {testdata.map((data) => {
          return (
            <li key={data.estateSsn}>
              <Card data={data} />
            </li>
          );
        })}
      </ul>
    </main>
  );
}

export default App;
