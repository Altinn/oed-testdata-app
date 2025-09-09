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
        <Heading level={1} data-size="xl">
          Digitalt Dødsbo - Testdata
        </Heading>

        <LoginDialog />
      </main>
    );
  }

  const uniqueTags = [...new Set(data?.flatMap(estate => estate.metadata.tags))];
  const filteredEstates = data?.filter(estate => selectedTags.every(tag => estate.metadata.tags.includes(tag)));

  if (selectedTags.length === 0) {
    filteredEstates?.sort((a, b) => {
      if (a.metadata.tags.length === 0) return -1;
      if (b.metadata.tags.length === 0) return 1;
      return 0;
    });
  }
  
  return (
    <main>
      <Heading level={1} data-size="xl">
        Digitalt Dødsbo - Testdata
      </Heading>

      {loading && (
        <Paragraph data-size="md" className="flex-center">
          <Spinner aria-label="Laster inn data..." />
          Laster inn data...
        </Paragraph>
      )}

      <ul style={{display: "flex", flexDirection: "row", gap: ".5rem" }}>
        {uniqueTags?.length > 0 && 
            uniqueTags.map(tag => 
              <Chip.Checkbox
                key={tag}
                onClick={() => toggleTag(tag)} 
                name={tag} 
                value={tag} 
                checked={selectedTags.includes(tag)}>
                {tag}
              </Chip.Checkbox>)
        
        }
      </ul>

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
