import React, { useState } from "react";
import {
  Button,
  Textfield,
  Chip,
  Dropdown,
  Paragraph,
  Fieldset,
  Divider,
  Heading,
  Avatar,
  List,
  SuggestionItem,
} from "@digdir/designsystemet-react";
import {
  PlusIcon,
  PersonPlusIcon,
  PersonIcon,
  TagIcon,
  PersonGroupIcon,
  PaperplaneIcon,
  Buildings3Icon,
  PersonCheckmarkIcon,
} from "@navikt/aksel-icons";
import {
  ESTATE_API,
  PERSON_API,
  COMPANY_API,
  RELATIONSHIP_OPTIONS,
} from "../../utils/constants";
import { useToast } from "../../context/toastContext";
import "./style.css";
import { HeirSearch } from "../../interfaces/IPersonSearch";
import { Foretak, Heir, Person } from "./types";
import ForetakForm from "./forms/foretak";
import PersonForm from "./forms/person";

interface FormData {
  deceased: Person;
  heirs: Heir[];
  tags: string[];
}

interface Props {
  uniqueTags?: string[];
}

export function NewEstateForm({ uniqueTags }: Props) {
  const [newHeirDropdownOpen, setNewHeirDropdownOpen] = useState(false);
  const [formData, setFormData] = useState<FormData>({
    deceased: { type: "Person", id: "1", name: "", nin: "", mottakerOriginalSkifteattest: false, role: "" },
    heirs: [],
    tags: [],
  });

  const [newTag, setNewTag] = useState("");
  const [showTagInput, setShowTagInput] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});
  const { addToast } = useToast();

  const sendCompanyReq = async (
    setLoadingState: (value: boolean) => void,
    queryParams: string
  ) => {
    try {
      setLoadingState(true);
      const response = await fetch(COMPANY_API + queryParams, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.status === 401) {
        addToast(
          "Autentiseringstoken har utløpt. Vennligst logg inn igjen.",
          "danger"
        );
        return;
      }
      if (!response.ok) {
        addToast("Noe gikk galt. Prøv igjen", "danger");
      }

      return await response.json();
    } catch (error) {
      console.error("Error resetting estate:", error);
      addToast("Noe gikk galt. Prøv igjen.", "danger");
    } finally {
      setLoadingState(false);
    }
  };

  const sendPersonReq = async (
    setLoadingState: (value: boolean) => void,
    queryParams: string
  ) => {
    try {
      setLoadingState(true);
      const response = await fetch(PERSON_API + queryParams, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.status === 401) {
        addToast(
          "Autentiseringstoken har utløpt. Vennligst logg inn igjen.",
          "danger"
        );
        return;
      }
      if (!response.ok) {
        addToast("Noe gikk galt. Prøv igjen", "danger");
      }

      return await response.json();
    } catch (error) {
      console.error("Error resetting estate:", error);
      addToast("Noe gikk galt. Prøv igjen.", "danger");
    } finally {
      setLoadingState(false);
    }
  };

  // Fetches person by nin and sets name for the heir
  const fetchHeirNameByNin = async (id: string, nin: string, isDeceased?: boolean) => {
    if (nin.length === 11) {
      const res = await sendPersonReq(() => {}, `?nin=${nin}&isDeceased=${isDeceased}`);
      if (res && res.length > 0) {
        setFormData((prev) => ({
          ...prev,
          heirs: prev.heirs.map((h) =>
            h.id === id ? { ...h, name: res[0].name } : h
          ),
        }));
      }
    }

    setFormData((prev) => ({
      ...prev,
      heirs: prev.heirs.map((h) => (h.id === id ? { ...h, nin } : h)),
    }));
  };

  // Fetches company by org num and sets name for the heir
  const fetchHeirNameByOrgNum = async (id: string, orgnum: string) => {
    if (orgnum.length === 9) {
      const res = await sendCompanyReq(() => {}, `?id=${orgnum}`);
      if (res && res.length > 0) {
        setFormData((prev) => ({
          ...prev,
          heirs: prev.heirs.map((h) =>
            h.id === id ? { ...h, name: res[0].name } : h
          ),
        }));
      }
    }

    setFormData((prev) => ({
      ...prev,
      heirs: prev.heirs.map((h) => (h.id === id ? { ...h, orgnum } : h)),
    }));
  };

  const validateNIN = (nin: string): boolean => {
    return nin.length === 11 && /^\d{11}$/.test(nin);
  };

  const handleDeceasedChange = async (field: "name" | "nin", value: string) => {
    if (field === "nin" && value.length > 11) return;
    if (field === "nin" && value.length === 11) {
      await fetchDeceasedWithRelations(value, 0);
    }

    setFormData((prev) => ({
      ...prev,
      deceased: { ...prev.deceased, [field]: value },
    }));

    // Clear error when user starts typing
    if (errors[`deceased-${field}`]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[`deceased-${field}`];
        return newErrors;
      });
    }
  };

  const fetchRandomDeceased = async () => {
    const res = await sendPersonReq(() => {}, "?count=1&isDeceased=true");
    if (!res || res.length === 0) {
      addToast("Ingen personer funnet", "warning");
      return;
    }
    setFormData((prev) => ({
        ...prev,
        deceased: { type: "Person", id: res[0].id, name: res[0].name, nin: res[0].nin, mottakerOriginalSkifteattest: false, role: "" },
    }));
  };

  const fetchDeceasedWithRelations = async (
    nin?: string | undefined,
    maxAmountOfChildren?: number | undefined
  ) => {
    const queryParams = `?count=1&isDeceased=true&withRelations=true`;
    const res = await sendPersonReq(() => {},
        queryParams + (nin ? `&nin=${nin}` : "") + (maxAmountOfChildren ? `&maxAmountOfChildren=${maxAmountOfChildren}` : ""));
    if (!res || res.length === 0) {
      addToast("Ingen personer funnet", "warning");
      return;
    }
    const deceased = res[0];
    const mappedHeirs = deceased.relations.map(
      (person: HeirSearch, idx: number) => ({
        id: Date.now().toString() + "-" + idx,
        name: person.name,
        nin: person.nin,
        type: "Person",
        relation: RELATIONSHIP_OPTIONS.find(
          (opt) =>
            opt.value.toLowerCase() === person.relation.toLowerCase() ||
            opt.label.toLowerCase() === person.relation.toLowerCase() ||
            opt.value.toLowerCase().includes(person.relation.toLowerCase()) ||
            opt.label.toLowerCase().includes(person.relation.toLowerCase())
        ),
      })
    );

    setFormData((prev) => ({
        ...prev,
        deceased: { type: "Person", role: "", id: deceased.id, name: deceased.name, nin: deceased.nin, mottakerOriginalSkifteattest: false },
      heirs: mappedHeirs,
    }));
  };

  const fetchRandomPersonHeir = async (id: string) => {
    const res = await sendPersonReq(() => {}, "?count=1&isDeceased=false");
    if (!res || res.length === 0) {
      addToast("Ingen personer funnet", "warning");
      return;
    }
    setFormData((prev) => ({
      ...prev,
      heirs: prev.heirs.map((person) =>
        person.id === id
          ? { ...person, name: res[0].name, nin: res[0].nin }
          : person
      ),
    }));
  };

  const fetchRandomCompanyHeir = async (id: string) => {
    const res = await sendCompanyReq(() => {}, "?count=1");
    if (!res || res.length === 0) {
      addToast("Ingen foretak funnet", "warning");
      return;
    }
    setFormData((prev) => ({
      ...prev,
      heirs: prev.heirs.map((company) =>
        company.id === id
          ? { ...company, name: res[0].name, organisasjonsNummer: res[0].orgNum }
          : company
      ),
    }));
  };

  const handleAddHeirForm = async (type: 'Person' | 'PersonPapp' | 'Foretak' | 'ForetakPapp') => {
    switch (type) {
      case 'Person':
        addPersonHeir();
        break;
      case 'Foretak':
        addCompanyHeir();
        break;
      case 'PersonPapp':
      case 'ForetakPapp':
        addToast("Kommer snart", "warning");
        break;
    }
  }

  const addCompanyHeir = () => {
    const newCompany: Foretak = {
      type: "Foretak",
      relation: { value: "TEST_ARVING_FULL", label: "" },
      name: "",
      id: Date.now().toString(),
      organisasjonsNummer: "",
      mottakerOriginalSkifteattest: false,
    };
    setFormData((prev) => ({
      ...prev,
      heirs: [...prev.heirs, newCompany],
    }));
  };
 
  const addPersonHeir = () => {
    const newPerson: Person = {
      id: Date.now().toString(),
      type: "Person",
      role: "",
      mottakerOriginalSkifteattest: false,
      name: "",
      nin: "",
      relation: { value: "", label: "" },
    };
    setFormData((prev) => ({
      ...prev,
      heirs: [...prev.heirs, newPerson],
    }));
  };

  const updateHeirRelation = (
    id: string,
    relationItem: SuggestionItem | undefined
  ) => {
    if (!relationItem) return;
    setFormData((prev) => ({
      ...prev,
      heirs: prev.heirs.map((heir) =>
        heir.id === id ? { ...heir, relation: relationItem } : heir
      ),
    }));

    // Clear error when user starts typing
    if (errors[`${id}-relation`]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[`${id}-relation`];
        return newErrors;
      });
    }
  };

  const removeHeir = (id: string) => {
    setFormData((prev) => ({
      ...prev,
      heirs: prev.heirs.filter((heir) => heir.id !== id),
    }));
  };

  const addTag = (tag: string) => {
    if (tag && !formData.tags.includes(tag)) {
      setFormData((prev) => ({
        ...prev,
        tags: [...prev.tags, tag],
      }));
    }
    setNewTag("");
    setShowTagInput(false);
  };

  const removeTag = (tagToRemove: string) => {
    setFormData((prev) => ({
      ...prev,
      tags: prev.tags.filter((tag) => tag !== tagToRemove),
    }));
  };

  const toggleTag = (tag: string) => {
    if (formData.tags.includes(tag)) {
      removeTag(tag);
    } else {
      addTag(tag);
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    // Validate deceased
    if (!formData.deceased.name.trim()) {
      newErrors["deceased-name"] = "Navn er påkrevd";
    }
    if (!formData.deceased.nin.trim()) {
      newErrors["deceased-nin"] = "Fødselsnummer er påkrevd";
    } else if (!validateNIN(formData.deceased.nin)) {
      newErrors["deceased-nin"] = "Fødselsnummer må være nøyaktig 11 siffer";
    }

    // Validate heirs
    formData.heirs.forEach((heir) => {
        switch (heir.type) {
          case "Person":
            if (!heir.name.trim()) {
              newErrors[`${heir.id}-name`] = "Navn er påkrevd";
            }
            if (!heir.nin.trim()) {
              newErrors[`${heir.id}-nin`] = "Fødselsnummer er påkrevd";
            } else if (!validateNIN(heir.nin)) {
              newErrors[`${heir.id}-nin`] = "Fødselsnummer må være nøyaktig 11 siffer";
            }
            if (!heir.relation) {
              newErrors[`${heir.id}-relation`] = "Relasjon er påkrevd";
            }
            break;

          case "Foretak":
            if (!heir.organisasjonsNummer.trim()) {
              newErrors[`${heir.id}-orgnum`] = "Organisasjonsnummer er påkrevd";
            } else if (!/^\d{9}$/.test(heir.organisasjonsNummer)) {
              newErrors[`${heir.id}-orgnum`] = "Organisasjonsnummer må være nøyaktig 9 siffer";
            }
            break;

          case "PersonPapp":
            if (!heir.dateOfBirth.trim()) {
              newErrors[`${heir.id}-dob`] = "Fødselsdato er påkrevd";
            } else if (!/^(\d{4})-(\d{2})-(\d{2})$/.test(heir.dateOfBirth)) {
              newErrors[`${heir.id}-dob`] = "Fødselsdato må være i formatet 'YYYY-MM-DD'";
            }
            if (!heir.navn) {
              newErrors[`${heir.id}-name`] = "Navn er påkrevd";
            }
            break;

          case "ForetakPapp":
            if (!heir.organisasjonsNavn) {
              newErrors[`${heir.id}-name`] = "Navn er påkrevd";
            }
            if (!heir.registreringsNummer) {
              newErrors[`${heir.id}-reg`] = "Registreringsnummer er påkrevd";
            }
            if (!heir.countryCode) {
              newErrors[`${heir.id}-cc`] = "Landskode er påkrevd";
            }
            break;
      }
    });

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (validateForm()) {
      const newEstate = {
        estateSsn: formData.deceased.nin,
        deceasedName: formData.deceased.name,
        heirs: formData.heirs.map((heir) => 
        {
            switch (heir.type) {
                case "Person":
                    return {
                        name: heir.name,
                        type: heir.type,
                        ssn: heir.nin,
                        relation: heir.relation?.value,
                    }
                case "Foretak":
                    return {
                        name: heir.name,
                        type: heir.type,
                        orgnum: heir.organisasjonsNummer,
                        relation: heir.relation?.value,
                    }
                default:
                    addToast(heir.type + " ikke implementert", "danger")
             }
        }),
        tags: formData.tags,
      };
      try {
        const response = await fetch(ESTATE_API + "add", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(newEstate),
        });

        /* if auth token expired show toast */
        if (response.status === 401) {
          addToast(
            "Autentiseringstoken har utløpt. Vennligst logg inn igjen.",
            "danger"
          );
          return;
        }
        if (!response.ok) {
          addToast("Noe gikk galt. Prøv igjen", "danger");
          return;
        }
        addToast("Dødsboet ble opprettet.", "success");
        setFormData({
          deceased: { type: "Person", id: "1", name: "", nin: "", role: "", mottakerOriginalSkifteattest: false },
          heirs: [],
          tags: [],
        });
      } catch (error) {
        console.error("Error creating new estate:", error);
        addToast("Noe gikk galt. Prøv igjen.", "danger");
      }
    }
  };

  const renderForm = <T extends Heir>(heir: T) => {
    switch (heir.type) {
        case "Foretak":
            return (
                <ForetakForm
                    heir={heir}
                    errors={errors}
                    onFetch={fetchHeirNameByOrgNum}
                    onRandom={fetchRandomCompanyHeir}
                    onRemove={removeHeir}
                />)
        case "Person":
            return (
                <PersonForm
                    heir={heir}
                    errors={errors}
                    onFetch={fetchHeirNameByNin}
                    onRandom={fetchRandomPersonHeir}
                    onRemove={removeHeir}
                    onRelation={updateHeirRelation}
                />)
        default:
            return (<em>No renderer for {heir.type}</em>)
    }
  }

  return (
    <div className="estate-form" data-size="md">
      <Button
        type="button"
        variant="secondary"
        onClick={() => fetchDeceasedWithRelations()}
        style={{ marginBottom: "var(--ds-size-8)" }}
      >
        <PersonPlusIcon />
        Hent avdød med relasjoner
      </Button>
      <main>
        <form onSubmit={handleSubmit}>
          {/* Deceased Section */}
          <Fieldset>
            <Fieldset.Legend
              className="flex-center"
              style={{ marginBottom: "var(--ds-size-2)" }}
            >
              <Avatar aria-label="Avdød" variant="square">
                <PersonIcon />
              </Avatar>
              <Heading level={2} data-size="md">
                Avdød
              </Heading>
            </Fieldset.Legend>

            <div
              style={{
                display: "grid",
                gridTemplateColumns: "repeat(auto-fit, minmax(300px, 1fr))",
                gap: "1rem",
              }}
            >
              <Textfield
                label="Fullt navn"
                value={formData.deceased.name}
                readOnly={true}
                error={errors["deceased-name"]}
              />
              <div>
                <Textfield
                  label="Tenor fødselsnummer (11 siffer)"
                  value={formData.deceased.nin}
                  onChange={(e) =>
                    handleDeceasedChange(
                      "nin",
                      e.target.value.replace(/\D/g, "")
                    )
                  }
                  error={errors["deceased-nin"]}
                  maxLength={11}
                  required
                />
                <Button
                  type="button"
                  variant="secondary"
                  onClick={() => fetchRandomDeceased()}
                  style={{ marginTop: "var(--ds-size-2)" }}
                >
                  <PersonPlusIcon />
                  Hent tilfeldig
                </Button>
              </div>
            </div>
          </Fieldset>
          <Divider />

          {/* Heirs Section */}
          <Fieldset>
            <Fieldset.Legend
              className="flex-center"
              style={{ marginBottom: "var(--ds-size-2)", width: "100%" }}
            >
              <Avatar aria-label="Avdød" data-color="brand2" variant="square">
                <PersonGroupIcon />
              </Avatar>
              <Heading level={2} data-size="md">
                Arvinger
              </Heading>
              <div style={{ marginLeft: "auto" }}>
                  <Dropdown.TriggerContext>
                    <Dropdown.Trigger 
                      data-color="brand2"
                      onClick={() => setNewHeirDropdownOpen(!newHeirDropdownOpen)}
                    >
                      <PlusIcon title="Legg til arving" fontSize="1.5rem" />
                      Legg til arving
                    </Dropdown.Trigger>
                    <Dropdown
                      open={newHeirDropdownOpen} 
                      onClose={() => setNewHeirDropdownOpen(false)}
                    >
                      <Dropdown.List data-color="brand2">
                        <Dropdown.Item onClick={() => {
                          setNewHeirDropdownOpen(false);
                          handleAddHeirForm("Person");
                        }}>
                          <Dropdown.Button>
                            <PersonIcon />
                            Person
                          </Dropdown.Button>
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => {
                          setNewHeirDropdownOpen(false);
                          handleAddHeirForm("PersonPapp");
                        }}>
                          <Dropdown.Button>
                            <PersonCheckmarkIcon />
                            Person (papp)
                          </Dropdown.Button>
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => {
                          setNewHeirDropdownOpen(false);
                          handleAddHeirForm("Foretak");
                        }}>
                          <Dropdown.Button>
                            <Buildings3Icon  />
                            Foretak
                          </Dropdown.Button>
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => {
                          setNewHeirDropdownOpen(false);
                          handleAddHeirForm("ForetakPapp");
                        }}>
                          <Dropdown.Button>
                            <Buildings3Icon  />
                            Foretak (papp)
                          </Dropdown.Button>
                        </Dropdown.Item>
                      </Dropdown.List>
                    </Dropdown>
                  </Dropdown.TriggerContext>
              </div>
            </Fieldset.Legend>

            {formData.heirs.length === 0 ? (
              <div
                className="flex-col"
                style={{
                  padding: "2rem",
                  color: "var(--ds-color-brand2-text-subtle)",
                  textAlign: "center",
                  alignItems: "center",
                }}
              >
                <PersonPlusIcon style={{ width: "3rem", height: "3rem" }} />
                <div>
                  <Paragraph>Ingen arvinger er lagt til ennå</Paragraph>
                  <Paragraph>
                    Klikk "Legg til arving" for å komme i gang
                  </Paragraph>
                </div>
              </div>
            ) : (
              <section className="flex-col" style={{ gap: "var(--ds-size-6)" }}>
                {formData.heirs.map((heir) => renderForm(heir))}
              </section>
            )}
          </Fieldset>
          <Divider />

          {/* Tags Section */}
          <Fieldset data-color="warning">
            <Fieldset.Legend
              className="flex-center"
              style={{ marginBottom: "var(--ds-size-2)", width: "100%" }}
            >
              <Avatar aria-label="Merkelapper" variant="square">
                <TagIcon />
              </Avatar>
              <Heading level={2} data-size="md">
                Merkelapper
              </Heading>
              <Button
                onClick={() => setShowTagInput(!showTagInput)}
                style={{ marginLeft: "auto" }}
              >
                <PlusIcon />
                Legg til merkelapp
              </Button>
            </Fieldset.Legend>

            <Heading
              data-size="xs"
              style={{ marginBottom: "var(--ds-size-3)" }}
            >
              Velg fra vanlige merkelapper
            </Heading>
            <List.Unordered className="tag-list">
              {uniqueTags?.map((tag) => (
                <List.Item key={tag}>
                  <Chip.Checkbox
                    onClick={() => toggleTag(tag)}
                    data-color="warning"
                    value={tag}
                    checked={formData.tags.includes(tag)}
                  >
                    {tag}
                  </Chip.Checkbox>
                </List.Item>
              ))}
            </List.Unordered>

            {showTagInput && (
              <div className="flex-between" style={{ alignItems: "flex-end" }}>
                <Textfield
                  label="Eller legg til egendefinert merkelapp"
                  value={newTag}
                  onChange={(e) => setNewTag(e.target.value)}
                  style={{ flexGrow: 1 }}
                />

                <Button data-color="warning" onClick={() => addTag(newTag)}>
                  Legg til
                </Button>
              </div>
            )}
          </Fieldset>
          <Divider />

          <Button
            type="submit"
            data-size="lg"
            style={{ margin: "var(--ds-size-18) auto 0" }}
          >
            <PaperplaneIcon />
            Send inn skjema
          </Button>
        </form>
      </main>
    </div>
  );
}
