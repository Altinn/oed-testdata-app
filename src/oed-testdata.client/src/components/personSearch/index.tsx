import { PERSON_API } from "../../utils/constants";
import { PersonSearch } from "../../interfaces/IPersonSearch";
import { Button, Textfield } from "@digdir/designsystemet-react";
import { ChangeEvent, useState } from "react";
import { useToast } from "../../context/toastContext";

export default function PersonSearchComp() {
  const [isDeceasedLoading, setIsDeceasedLoading] = useState(false);
  const [isHeirLoading, setIsHeirLoading] = useState(false);
  const [deceasedPeople, setDeceasedPeople] = useState([] as PersonSearch[]);
  const [heirs, setHeirs] = useState([] as PersonSearch[]);
  const [heirInputs, setHeirInputs] = useState<{ [nin: string]: string }>({});
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

  const findRandomDeceased = async () => {
    const res = await sendReq(setIsDeceasedLoading, '?count=1&isDeceased=true');
    setDeceasedPeople(res);
  }

  const findRandomHeir = async () => {
    const res = await sendReq(setIsHeirLoading, '?count=1&isDeceased=false');
    if (!res) return;
    if (Array.isArray(res)) {
      setHeirs(prev => [...prev, ...res]);
      setHeirInputs(prev => ({ ...prev, ...(res.reduce((acc, h) => ({ ...acc, [h.nin]: h.nin }), {})) }));
    } else {
      setHeirs(prev => [...prev, res]);
      setHeirInputs(prev => ({ ...prev, [res.nin]: res.nin }));
    }
  }

  const findHeir = async (value: string, heir: PersonSearch) => {
    const trimmedNin = value.trim();
    if (trimmedNin === "") {
      // Allow clearing the field, do not validate or search
      return;
    }
    if (trimmedNin.length !== 11 || !/^\d{11}$/.test(trimmedNin)) {
      addToast("Ugyldig fødselsnummer. Fødselsnummer må være 11 sifre.", "warning");
      return;
    }
    if (trimmedNin === heir.nin) {
      addToast("Fødselsnummeret er allerede tilknyttet denne arvingen.", "info");
      return;
    }
    if (heirs.some(h => h.nin === trimmedNin)) {
      addToast("Denne personen er allerede lagt til.", "warning");
      setHeirInputs(prev => ({ ...prev, [heir.nin]: heir.nin }));
      return;
    }
    if (trimmedNin === deceasedPeople[0]?.nin) {
      addToast("Arving kan ikke være den avdøde.", "warning");
      return;
    }
    try {
      const res = await sendReq(setIsHeirLoading, `?nin=${trimmedNin}`);
      if (Array.isArray(res) && res.length > 0) {
        setHeirs(prev => prev.map(h => (h.nin === heir.nin ? res[0] : h)));
        setHeirInputs(prev => {
          const newInputs = { ...prev };
          delete newInputs[heir.nin];
          newInputs[res[0].nin] = res[0].nin;
          return newInputs;
        });
      } else {
        addToast("Fant ingen person med dette fødselsnummeret.", "warning");
      }
    } catch (error) {
      console.error("Error fetching heir data:", error);
      addToast("En feil oppstod under henting av data. Vennligst prøv igjen.", "warning");
    }
  };

  return (
    <section>
      <Button onClick={findRandomDeceased} disabled={isDeceasedLoading}>Tilfeldig avdød</Button>
      {deceasedPeople?.length ? (
        <Textfield
          label={deceasedPeople[0].name}
          disabled={isDeceasedLoading}
          size={11}
          value={deceasedPeople[0].nin}
        />
      ) : null}
      <Button onClick={findRandomHeir} disabled={isHeirLoading}>Tilfeldig arving</Button>

      {Array.isArray(heirs) && heirs.length > 0 && (
        <>
          {heirs.map((heir) => (
            <Textfield
              key={heir.nin}
              label={heir.name}
              disabled={isHeirLoading}
              size={11}
              value={heirInputs[heir.nin] ?? heir.nin}
              onChange={e => {
                const val = e.target.value;
                setHeirInputs(prev => ({ ...prev, [heir.nin]: val }));
              }}
              onBlur={e => {
                const val = e.target.value;
                if (val !== heir.nin) {
                  findHeir(val, heir);
                }
              }}
            />
          ))}
        </>
      )}
      <Button>Lag nytt testbo</Button>
    </section>
  );
}