import { Heading, Paragraph, Spinner } from "@digdir/designsystemet-react";
import "./App.css";
import EstateCard from "./components/estateCard";
import { ESTATE_API } from "./utils/constants";
import { useFetchData } from "./hooks/fetchData";
import { Estate } from "./interfaces/IEstate";
import LoginDialog from "./components/login";

function App() {
  const { data, loading } = useFetchData<Estate[]>(ESTATE_API);
  const isAuthenticated = localStorage.getItem("auth") === "true";

  if (!isAuthenticated) {
    return (
      <main>
        <Heading level={1} size="xl" spacing>
          Digitalt Dødsbo - Testdata
        </Heading>

        <LoginDialog />
      </main>
    );
  }

  return (
    <main>
      <Heading level={1} size="xl" spacing>
        Digitalt Dødsbo - Testdata
      </Heading>

      {loading && (
        <Paragraph size="md" className="flex-center">
          <Spinner title="Laster inn data..." />
          Laster inn data...
        </Paragraph>
      )}

      <ul className="container__grid">
        {data?.map((estate) => {
          return (
            <li key={estate.estateSsn}>
              <EstateCard data={estate} />
            </li>
          );
        })}
      </ul>
    </main>
  );
}

export default App;
