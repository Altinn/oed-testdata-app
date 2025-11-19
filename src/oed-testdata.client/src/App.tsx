import { Chip, Heading, Paragraph, Spinner, Switch, Tabs } from "@digdir/designsystemet-react";
import "./App.css";
import EstateCard from "./components/estateCard";
import { ESTATE_API } from "./utils/constants";
import { useFetchData } from "./hooks/fetchData";
import { Estate } from "./interfaces/IEstate";
import LoginDialog from "./components/login";
import { useEffect, useState } from "react";
import { HouseIcon, PlusIcon } from "@navikt/aksel-icons";
import { NewEstateForm } from "./components/personSearch";

function App() {
  const { data, loading } = useFetchData<Estate[]>(ESTATE_API);
  const [selectedTags, setSelectedTags] = useState<string[]>([]);
  const isAuthenticated = localStorage.getItem("auth") === "true";
    const [darkMode, setDarkMode] = useState<boolean>(
    localStorage.getItem("darkMode") === "true"
  );

 useEffect(() => {
    const bodyDiv = document.getElementById("body");
    if (bodyDiv) {
      bodyDiv.setAttribute("data-color-scheme", darkMode ? "dark" : "light");
    }
  }, [darkMode]);
  
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const isDarkMode = event.target.checked;
    const bodyDiv = document.getElementById("body");
    console.log("bodyDiv", bodyDiv);

    if (!bodyDiv) {
      return;
    }

    console.log("isDark", isDarkMode);
    bodyDiv.setAttribute("data-color-scheme", isDarkMode ? "dark" : "light");
    setDarkMode(isDarkMode);
    localStorage.setItem("darkMode", isDarkMode ? "true" : "false");
  };

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

  const uniqueTags = [...new Set(data?.flatMap(estate => estate.metadata.tags))]?.sort();
  const filteredEstates = data?.filter(estate => selectedTags.every(tag => estate.metadata.tags.includes(tag)));

  if (selectedTags.length === 0) {
    filteredEstates?.sort((a, b) => {
      if (a.metadata.tags.length === 0) return -1;
      if (b.metadata.tags.length === 0) return 1;
      return 0;
    });
  }
  const isDevelopment = import.meta.env.MODE === "development";

  return (
    <main>
      <Heading level={1} data-size="xl">
        Digitalt Dødsbo - Testdata
      </Heading>
      <Switch
        label="Mørk modus"
        checked={darkMode}
        onChange={handleChange}
        id="dark-mode"          
      />
      {isDevelopment ? (
        <Tabs defaultValue="estates" style={{ width: "100%" }}>
          <Tabs.List style={{ marginBottom: "var(--ds-size-4)" }}>
            <Tabs.Tab value="estates">
              <HouseIcon /> Testbo
            </Tabs.Tab>
            <Tabs.Tab value="create-new-estate">
              <PlusIcon /> Opprett nytt testbo
            </Tabs.Tab>
          </Tabs.List>

          {loading && (
            <Paragraph data-size="md" className="flex-center">
              <Spinner aria-label="Laster inn data..." />
              Laster inn data...
            </Paragraph>
          )}
          <Tabs.Panel value="create-new-estate" id="new-estate-tab">
            <NewEstateForm uniqueTags={uniqueTags} />
          </Tabs.Panel>
          <Tabs.Panel value="estates" id="estates-tab">
            <ul style={{ display: "flex", flexDirection: "row", gap: ".5rem", flexWrap: "wrap" }}>
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
          </Tabs.Panel>
        </Tabs>
      ) : (
        <>
          <ul style={{ display: "flex", flexDirection: "row", gap: ".5rem", flexWrap: "wrap"  }}>
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
        </>
      )}
    </main>
  );
}

export default App;
