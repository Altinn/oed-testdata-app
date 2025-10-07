import React, { useState } from 'react';
import {
  Button,
  Textfield,
  EXPERIMENTAL_Suggestion as Suggestion,
  Chip,
  Card,
  Paragraph,
  Fieldset,
  Label,
  Field,
  Divider,
  Heading,
} from '@digdir/designsystemet-react';
import { PlusIcon, PersonPlusIcon, TrashIcon, PersonIcon, TagIcon, PersonGroupIcon, PaperplaneIcon } from '@navikt/aksel-icons';
import { ESTATE_API, PERSON_API } from '../../utils/constants';
import { useToast } from '../../context/toastContext';
import './style.css';
import { HeirSearch } from '../../interfaces/IPersonSearch';

interface Person {
  id: string;
  name: string;
  nin: string;
}

interface Heir extends Person {
  relation: SuggestionItem;
}

interface FormData {
  deceased: Person;
  heirs: Heir[];
  tags: string[];
}

interface Props {
  uniqueTags?: string[];
}

type SuggestionItem = {
  value: string;
  label: string;
};

const RELATIONSHIP_OPTIONS = [
  { value: '', label: 'Velg relasjon' },
  { value: 'PART_ANNEN', label: 'Annen part' },
  { value: 'GJENLEV_EKTEFELLE_PARTNER', label: 'Gjenlevende ektefelle/partner' },
  { value: 'GJENLEV_PARTNER', label: 'Gjenlevende partner' },
  { value: 'GJENLEV_SAMBOER', label: 'Gjenlevende samboer' },
  { value: 'BARN', label: 'Barn' },
  { value: 'BARNEBARN', label: 'Barnebarn' },
  { value: 'SAERKULLSBARN', label: 'Særkullsbarn' },
  { value: 'SAERKULLSBARN_BARN', label: 'Særkullsbarns barn' },
  { value: 'FAR', label: 'Far' },
  { value: 'MOR', label: 'Mor' },
  { value: 'SOESKEN', label: 'Søsken' },
  { value: 'SOESKENS_BARN', label: 'Søskens barn' },
  { value: 'SOESKENS_BARNEBARN', label: 'Søskens barnebarn' },
  { value: 'HALV_SOESKEN', label: 'Halvsøsken' },
  { value: 'HALV_SOESKENS_BARN', label: 'Halvsøskens barn' },
  { value: 'FARFAR', label: 'Farfar' },
  { value: 'FARMOR', label: 'Farmor' },
  { value: 'MORFAR', label: 'Morfar' },
  { value: 'MORMOR', label: 'Mormor' },
  { value: 'ONKEL', label: 'Onkel' },
  { value: 'TANTE', label: 'Tante' },
  { value: 'FETTER', label: 'Fetter' },
  { value: 'KUSINE', label: 'Kusine' },
  { value: 'STATEN', label: 'Staten' },
  { value: 'AVDOEDE', label: 'Avdøde' },
  { value: 'FORDRINGSHAVER', label: 'Fordringshaver' },
  { value: 'AVDOEDE_EKTEFELLE_PARTNER', label: 'Avdøde ektefelle/partner' },
  { value: 'TEST_ARVING_FULL', label: 'Test arving (full)' },
  { value: 'TEST_ARVING_BEGR', label: 'Test arving (begrenset)' },
  { value: 'VERGE', label: 'Verge' },
  { value: 'FORELDREVERGE_AKTOER', label: 'Foreldreverge aktør' },
  { value: 'MIDLERTIDIGVERGE_AKTOER', label: 'Midlertidig verge aktør' },
  { value: 'FULLMEKTIG', label: 'Fullmektig' },
  { value: 'PROSFULL', label: 'Prosessfullmektig' },
  { value: 'FREMTIDSFULLMEKTIG', label: 'Fremtidsfullmektig' },
  { value: 'GJENLEV_EKTEFELLE', label: 'Gjenlevende ektefelle' },
  { value: 'MOTTAKER_FULLMAKT_DOEDSBO_LITEN_VERDI', label: 'Mottaker fullmakt dødsbo liten verdi' },
  { value: 'BARNEBARNS_BARN', label: 'Barnebarns barn' },
  { value: 'SOESKENS_BARNEBARNS_BARN', label: 'Søskens barnebarns barn' },
  { value: 'HALV_SOESKENS_BARNEBARN', label: 'Halvsøskens barnebarn' }
];


export function NewEstateForm({ uniqueTags }: Props) {
  const [formData, setFormData] = useState<FormData>({
    deceased: { id: '1', name: '', nin: '' },
    heirs: [],
    tags: []
  });

  const [newTag, setNewTag] = useState('');
  const [showTagInput, setShowTagInput] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});
  const { addToast } = useToast();

  const sendReq = async (
    setLoadingState: (value: boolean) => void,
    queryParams: string) => {
    try {
      setLoadingState(true);
      const response = await fetch(PERSON_API + queryParams, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        }
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
  const fetchHeirNameByNin = async (id: string, nin: string) => {
    if (nin.length === 11) {
      const res = await sendReq(() => { }, `?nin=${nin}&isDeceased=false`);
      if (res && res.length > 0) {
        setFormData(prev => ({
          ...prev,
          heirs: prev.heirs.map(h =>
            h.id === id ? { ...h, name: res[0].name } : h
          )
        }));
      }
    }

    setFormData(prev => ({
      ...prev,
      heirs: prev.heirs.map(h =>
        h.id === id ? { ...h, nin } : h
      )
    }));
  };

  const validateNIN = (nin: string): boolean => {
    return nin.length === 11 && /^\d{11}$/.test(nin);
  };

  const handleDeceasedChange = async (field: 'name' | 'nin', value: string) => {
    if (field === 'nin' && value.length > 11) return;
    if (field === 'nin' && value.length === 11) {
      await fetchDeceasedWithRelations(value, -1);
    }

    setFormData(prev => ({
      ...prev,
      deceased: { ...prev.deceased, [field]: value }
    }));

    // Clear error when user starts typing
    if (errors[`deceased-${field}`]) {
      setErrors(prev => {
        const newErrors = { ...prev };
        delete newErrors[`deceased-${field}`];
        return newErrors;
      });
    }
  };

  const fetchRandomDeceased = async () => {
    const res = await sendReq(() => { }, '?count=1&isDeceased=true');
    if (!res || res.length === 0) {
      addToast("Ingen personer funnet", "warning");
      return;
    }
    setFormData(prev => ({
      ...prev,
      deceased: { id: res[0].id, name: res[0].name, nin: res[0].nin }
    }));
  };

  const fetchDeceasedWithRelations = async (nin?: string | undefined, maxAmountOfChildren?: number | undefined) => {
    const queryParams = `?count=1&isDeceased=true&withRelations=true`;
    const res = await sendReq(() => { }, queryParams + (nin ? `&nin=${nin}` : '') + (maxAmountOfChildren ? `&maxAmountOfChildren=${maxAmountOfChildren}` : ''));
    if (!res || res.length === 0) {
      addToast("Ingen personer funnet", "warning");
      return;
    }
    const deceased = res[0];
    const mappedHeirs = deceased.relations.map((person: HeirSearch, idx: number) => ({
      id: Date.now().toString() + '-' + idx,
      name: person.name,
      nin: person.nin,
      relation:
        RELATIONSHIP_OPTIONS.find(opt =>
          opt.value.toLowerCase() === person.relation.toLowerCase() ||
          opt.label.toLowerCase() === person.relation.toLowerCase() ||
          opt.value.toLowerCase().includes(person.relation.toLowerCase()) ||
          opt.label.toLowerCase().includes(person.relation.toLowerCase())
        )
    }));

    setFormData(prev => ({
      ...prev,
      deceased: { id: deceased.id, name: deceased.name, nin: deceased.nin },
      heirs: mappedHeirs
    }));
  };

  const fetchRandomHeir = async (id: string) => {
    const res = await sendReq(() => { }, '?count=1&isDeceased=false');
    if (!res || res.length === 0) {
      addToast("Ingen personer funnet", "warning");
      return;
    }
    setFormData(prev => ({
      ...prev,
      heirs: prev.heirs.map(person =>
        person.id === id
          ? { ...person, name: res[0].name, nin: res[0].nin }
          : person
      )
    }));
  };

  const addHeir = () => {
    const newPerson: Heir = {
      id: Date.now().toString(),
      name: '',
      nin: '',
      relation: { value: '', label: '' }
    };
    setFormData(prev => ({
      ...prev,
      heirs: [...prev.heirs, newPerson]
    }));
  };

  const updateHeirRelation = (id: string, relationItem: SuggestionItem | undefined) => {
    if (!relationItem) return;
    setFormData(prev => ({
      ...prev,
      heirs: prev.heirs.map(person =>
        person.id === id ? { ...person, relation: relationItem } : person
      )
    }));

    // Clear error when user starts typing
    if (errors[`${id}-relation`]) {
      setErrors(prev => {
        const newErrors = { ...prev };
        delete newErrors[`${id}-relation`];
        return newErrors;
      });
    }
  };

  const removeHeir = (id: string) => {
    setFormData(prev => ({
      ...prev,
      heirs: prev.heirs.filter(person => person.id !== id)
    }));
  };

  const addTag = (tag: string) => {
    if (tag && !formData.tags.includes(tag)) {
      setFormData(prev => ({
        ...prev,
        tags: [...prev.tags, tag]
      }));
    }
    setNewTag('');
    setShowTagInput(false);
  };

  const removeTag = (tagToRemove: string) => {
    setFormData(prev => ({
      ...prev,
      tags: prev.tags.filter(tag => tag !== tagToRemove)
    }));
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    // Validate deceased
    if (!formData.deceased.name.trim()) {
      newErrors['deceased-name'] = 'Navn er påkrevd';
    }
    if (!formData.deceased.nin.trim()) {
      newErrors['deceased-nin'] = 'Fødselsnummer er påkrevd';
    } else if (!validateNIN(formData.deceased.nin)) {
      newErrors['deceased-nin'] = 'Fødselsnummer må være nøyaktig 11 siffer';
    }

    // Validate heirs
    formData.heirs.forEach(person => {
      if (!person.name.trim()) {
        newErrors[`${person.id}-name`] = 'Navn er påkrevd';
      }
      if (!person.nin.trim()) {
        newErrors[`${person.id}-nin`] = 'Fødselsnummer er påkrevd';
      } else if (!validateNIN(person.nin)) {
        newErrors[`${person.id}-nin`] = 'Fødselsnummer må være nøyaktig 11 siffer';
      }
      if (!person.relation) {
        newErrors[`${person.id}-relation`] = 'Relasjon er påkrevd';
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
        heirs: formData.heirs.map(h => ({ ssn: h.nin, relation: h.relation.value, name: h.name })),
        tags: formData.tags
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
          deceased: { id: '1', name: '', nin: '' },
          heirs: [],
          tags: []
        });
      } catch (error) {
        console.error("Error creating new estate:", error);
        addToast("Noe gikk galt. Prøv igjen.", "danger");
      }
    }
  };

  return (
    <div className='estate-form'>
      <Button
        type="button"
        variant="secondary"
        data-size="md"
        onClick={() => fetchDeceasedWithRelations()}
        style={{ margin: '0.5rem 0 2rem 0' }}
      >
        <PersonPlusIcon />
        Hent avdød med relasjoner
      </Button>
      <div style={{ maxWidth: '1200px', margin: '0 auto' }}>
        <form onSubmit={handleSubmit}>
          {/* Deceased Section */}
          <Fieldset>
            <Fieldset.Legend>
              <div style={{ marginBottom: '1rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                <PersonIcon style={{ width: '1.5rem', height: '1.5rem', marginRight: '0.5rem', backgroundColor: 'var(--ds-color-accent-base-contrast-subtle)', color: 'var(--ds-color-text-default)', padding: '0.5rem', borderRadius: '0.5rem' }} />
                <Heading level={2} data-size='md'>
                  Avdød
                </Heading>
              </div>
            </Fieldset.Legend>
            <Divider />
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: '1rem' }}>
              <Textfield
                label="Fullt navn"
                value={formData.deceased.name}
                readOnly={true}
                error={errors['deceased-name']}
              />
              <div>
                <Textfield
                  label="Tenor fødselsnummer (11 siffer)"
                  value={formData.deceased.nin}
                  onChange={(e) => handleDeceasedChange('nin', e.target.value.replace(/\D/g, ''))}
                  error={errors['deceased-nin']}
                  maxLength={11}
                  required
                />
                <Button
                  type="button"
                  variant="secondary"
                  data-size="md"
                  onClick={() => fetchRandomDeceased()}
                  style={{ marginTop: '0.5rem' }}
                >
                  <PersonPlusIcon />
                  Hent tilfeldig
                </Button>
              </div>
            </div>
          </Fieldset>

          {/* Heirs Section */}
          <Fieldset>
            <Fieldset.Legend style={{ width: '100%' }}>
              <div style={{ marginTop: '3rem', marginBottom: '1rem', display: 'flex', alignItems: 'center', justifyContent: 'space-between', width: '100%' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                  <PersonGroupIcon style={{ width: '1.5rem', height: '1.5rem', marginRight: '0.5rem', backgroundColor: 'var(--ds-color-brand1-text-subtle)', color: 'var(--ds-color-background-default)', padding: '0.5rem', borderRadius: '0.5rem' }} />
                  <Heading level={2} data-size='md'>
                    Arvinger
                  </Heading>
                </div>
                <Button
                  type="button"
                  variant="primary"
                  data-size="small"
                  data-color="brand1"
                  onClick={addHeir}
                >
                  <PlusIcon />
                  Legg til arving
                </Button>
              </div>
            </Fieldset.Legend>
            <Divider />
            {formData.heirs.length === 0 ? (
              <div style={{ padding: '2rem', color: 'var(--ds-color-brand1-text-subtle)', textAlign: 'center', display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '1rem' }}>
                <PersonPlusIcon style={{ width: '3rem', height: '3rem' }} />
                <div>
                  <Paragraph data-size='md' style={{ margin: 0, marginBottom: '0.5rem' }}>
                    Ingen arvinger er lagt til ennå
                  </Paragraph>
                  <Paragraph data-size='sm'>
                    Klikk "Legg til arving" for å komme i gang
                  </Paragraph>
                </div>
              </div>
            ) : (
              <>
                {formData.heirs.map((person) => (
                  <Card key={person.id} data-color="brand1" style={{ backgroundColor: 'var(--ds-color-brand1-surface-tinted)', margin: '0.5rem 0' }}>
                    <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '1rem', alignItems: 'start' }}>
                      <Textfield
                        label="Fullt navn"
                        value={person.name}
                        readOnly={true}
                        error={errors[`${person.id}-name`]}
                        required
                      />

                      <div>
                        <Textfield
                          label="Tenor fødselsnummer (11 siffer)"
                          value={person.nin}
                          error={errors[`${person.id}-nin`]}
                          maxLength={11}
                          required
                          onChange={async (e) => {
                            const nin = e.target.value.replace(/\D/g, '');
                            await fetchHeirNameByNin(person.id, nin);
                          }}
                        />
                        <Button
                          type="button"
                          variant="secondary"
                          data-size="small"
                          onClick={() => fetchRandomHeir(person.id)}
                          style={{ marginTop: '0.5rem' }}
                        >
                          <PersonPlusIcon />
                          Hent tilfeldig
                        </Button>
                      </div>

                      <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'end' }}>
                        <div style={{ flex: 1 }}>
                          <Field>
                            <Label>Relasjon til avdød</Label>
                            <Suggestion
                              selected={person.relation.label || ''}
                              onSelectedChange={(item) => updateHeirRelation(person.id, item)}
                            >
                              <Suggestion.Input required />
                              <Suggestion.Clear />
                              <Suggestion.List id="relation-list">
                                <Suggestion.Empty>
                                  Tomt
                                </Suggestion.Empty>
                                {RELATIONSHIP_OPTIONS.map((option) => (
                                  <Suggestion.Option
                                    key={option.value}
                                    value={option.value}
                                  >
                                    {option.label}
                                  </Suggestion.Option>
                                ))}
                              </Suggestion.List>
                            </Suggestion>
                          </Field>
                        </div>
                        <Button
                          type="button"
                          variant="tertiary"
                          data-color="danger"
                          data-size="md"
                          onClick={() => removeHeir(person.id)}
                        >
                          <TrashIcon />
                        </Button>
                      </div>
                    </div>
                  </Card>
                ))}
              </>
            )}
          </Fieldset>

          {/* Tags Section */}
          <Fieldset>
            <Fieldset.Legend style={{ width: '100%' }}>
              <div style={{ marginTop: '3rem', marginBottom: '1rem', display: 'flex', alignItems: 'center', justifyContent: 'space-between', width: '100%' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginRight: '1rem' }}>
                  <TagIcon style={{ width: '1.5rem', height: '1.5rem', marginRight: '0.5rem', backgroundColor: 'var(--ds-color-brand2-text-subtle)', color: 'var(--ds-color-brand2-background-default)', padding: '0.5rem', borderRadius: '0.5rem' }} />
                  <Heading level={2} data-size='md'>
                    Merkelapper
                  </Heading>
                </div>
                <Button
                  type="button"
                  variant="primary"
                  data-size="small"
                  data-color="brand2"
                  onClick={() => setShowTagInput(!showTagInput)}
                >
                  <PlusIcon />
                  Legg til merkelapp
                </Button>
              </div>
            </Fieldset.Legend>
            <Divider />

            <Paragraph data-size="lg" style={{ marginBottom: '1.5rem' }}>
              Velg fra vanlige merkelapper
            </Paragraph>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.5rem' }}>
              {uniqueTags?.filter(tag => !formData.tags.includes(tag)).map((tag) => (
                <Chip.Removable
                  key={tag}
                  onClick={() => addTag(tag)}
                  data-color="brand2"
                >
                  {tag}
                </Chip.Removable>
              ))}
            </div>

            {showTagInput && (
              <div style={{ marginBottom: '1rem' }}>
                <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'end' }}>
                  <div style={{ flex: 1 }}>
                    <Textfield
                      label="Eller legg til egendefinert merkelapp"
                      value={newTag}
                      onChange={(e) => setNewTag(e.target.value)}
                      onKeyPress={(e) => {
                        if (e.key === 'Enter') {
                          e.preventDefault();
                          addTag(newTag);
                        }
                      }}
                    />
                  </div>
                  <Button
                    type="button"
                    variant="primary"
                    data-size="small"
                    data-color="brand2"
                    onClick={() => addTag(newTag)}
                  >
                    Legg til
                  </Button>
                </div>
              </div>
            )}

            <div>
              <Paragraph data-size="lg" style={{ marginBottom: '0.5rem' }}>
                Valgte merkelapper
              </Paragraph>
              {formData.tags.length === 0 ? (
                <Paragraph data-size='sm'>Ingen merkelapper valgt</Paragraph>
              ) : (
                <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.5rem' }}>
                  {formData.tags.map((tag) => (
                    <Chip.Removable
                      key={tag}
                      aria-label={`Fjern merkelapp ${tag}`}
                      data-color="accent"
                      onClick={() => removeTag(tag)}
                    >
                      {tag}
                    </Chip.Removable>
                  ))}
                </div>
              )}
            </div>
          </Fieldset>
          <Divider style={{ marginTop: '2rem' }} />
          {/* Submit Button */}
          <div style={{ display: 'flex', justifyContent: 'center', marginTop: '3rem', marginBottom: '2rem' }}>
            <Button
              type="submit"
              variant="primary"
              data-size="lg"
            >
              <PaperplaneIcon />
              Send inn skjema
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
