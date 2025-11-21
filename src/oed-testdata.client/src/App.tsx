import {
  Chip,
  Heading,
  List,
  Paragraph,
  Spinner,
  Switch,
  Tabs,
} from "@digdir/designsystemet-react";
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
      setSelectedTags([...selectedTags.filter((st) => st != tag)]);
    }
  };

  const EstatesPanel = () => {
    return (
      <>
        <Heading
          level={2}
          data-size="xs"
          style={{ marginBottom: "var(--ds-size-2)" }}
        >
          Filtrer på tagger
        </Heading>
        <List.Unordered className="tag-list">
          {uniqueTags?.length > 0 &&
            uniqueTags.map((tag) => (
              <List.Item key={tag}>
                <Chip.Checkbox
                  onClick={() => toggleTag(tag)}
                  name={tag}
                  value={tag}
                  checked={selectedTags.includes(tag)}
                  data-color="warning"
                >
                  {tag}
                </Chip.Checkbox>
              </List.Item>
            ))}
        </List.Unordered>
        <List.Unordered
          className="container__grid"
          style={{ alignItems: "baseline" }}
        >
          {filteredEstates?.map((estate) => {
            return (
              <List.Item key={estate.estateSsn}>
                <EstateCard data={estate} />
              </List.Item>
            );
          })}
        </List.Unordered>
      </>
    );
  };

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

  const uniqueTags = [
    ...new Set(data?.flatMap((estate) => estate.metadata.tags)),
  ]?.sort();
  const filteredEstates = data?.filter((estate) =>
    selectedTags.every((tag) => estate.metadata.tags.includes(tag))
  );

  if (selectedTags.length === 0) {
    filteredEstates?.sort((a, b) => {
      if (a.metadata.tags.length === 0) return -1;
      if (b.metadata.tags.length === 0) return 1;
      return 0;
    });
  }
  const isDevelopment = import.meta.env.MODE === "development";

  return (
    <>
      <Switch
        label="Mørk modus"
        checked={darkMode}
        onChange={handleChange}
        id="dark-mode"
        style={{ padding: "var(--ds-size-4)" }}
      />
      <main>
        <Heading
          level={1}
          data-size="xl"
          style={{ marginBottom: "var(--ds-size-3)" }}
        >
          Digitalt Dødsbo - Testdata
        </Heading>

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
              <Paragraph className="flex-center">
                <Spinner aria-label="Laster inn data..." />
                Laster inn data...
              </Paragraph>
            )}
            <Tabs.Panel value="create-new-estate" id="new-estate-tab">
              <NewEstateForm uniqueTags={uniqueTags} />
            </Tabs.Panel>
            <Tabs.Panel value="estates" id="estates-tab">
              <EstatesPanel />
            </Tabs.Panel>
          </Tabs>
        ) : (
          <EstatesPanel />
        )}
      </main>
    </>
  );
}

export default App;
