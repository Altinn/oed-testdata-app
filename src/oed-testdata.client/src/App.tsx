import { Chip, Heading, Paragraph, Spinner } from "@digdir/designsystemet-react";
import "./App.css";
import EstateCard from "./components/estateCard";
import { ESTATE_API } from "./utils/constants";
import { useFetchData } from "./hooks/fetchData";
import { Estate } from "./interfaces/IEstate";
import LoginDialog from "./components/login";
import { useState } from "react";

function App() {
  const { data, loading } = useFetchData<Estate[]>(ESTATE_API);
  const [selectedTags, setSelectedTags] = useState<string[]>([]);
  const isAuthenticated = localStorage.getItem("auth") === "true";


  const toggleTag = (tag: string) => {
    if (!selectedTags.includes(tag)) {
      setSelectedTags([...selectedTags, tag]);
    } else {
      setSelectedTags([...selectedTags.filter(st => st != tag)])
    }    
  }


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

  const uniqueTags = [...new Set(data?.flatMap(estate => estate.metadata.tags))];
  const filteredEstates = data?.filter(estate => selectedTags.every(tag => estate.metadata.tags.includes(tag)));

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

        {uniqueTags?.length > 0 && 
        <Chip.Group>
          {uniqueTags.map(tag => <Chip.Toggle onClick={() => toggleTag(tag)} name={tag} value={tag} selected={ selectedTags.includes(tag)   /*selectedTags.includes(tag)*/}>{tag}</Chip.Toggle>)}
        </Chip.Group>}

      <ul className="container__grid">
        {filteredEstates?.map((estate) => {
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
