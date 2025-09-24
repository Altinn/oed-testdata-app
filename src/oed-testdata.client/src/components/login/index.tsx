import { useState } from "react";
import "./style.css";
import { AUTH_LOGIN_API } from "../../utils/constants";
import {
  Button,
  ValidationMessage,
  Heading,
  Textfield,
  Card
} from "@digdir/designsystemet-react";
export default function LoginDialog() {
  const [formData, setFormData] = useState({ username: "", password: "" });
  const [isLoading, setIsLoading] = useState(false);
  const [formError, setFormError] = useState(false);

  const onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);

    const { username, password } = formData;

    try {
      fetch(AUTH_LOGIN_API, {
        method: "GET",
        credentials: "include",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: "Basic " + btoa(username + ":" + password),
        },
      })
        .then(function (response) {
          if (response.status === 401) {
            setFormError(true);
            return;
          }

          if (response.status !== 200) {
            console.log(
              "Looks like there was a problem. Status Code: " + response.status
            );
            return;
          }
          localStorage.setItem("auth", "true");
          window.location.reload();
        })
        .catch(function (err) {
          console.log("Fetch Error :-S", err);
        });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <section id="login-dialog">
      <Card>
        <Heading level={2} data-size="md">
          Innlogging
        </Heading>

        <form onSubmit={onSubmit}>
          <Textfield
            label="Brukernavn"
            name="username"
            error={formError}
            onChange={(e) =>
              setFormData({ ...formData, username: e.target.value })
            }
            disabled={isLoading}
          />
          <Textfield
            label="Passord"
            name="password"
            type="password"
            error={formError}
            onChange={(e) =>
              setFormData({ ...formData, password: e.target.value })
            }
            disabled={isLoading}
          />
          {formError && (
            <ValidationMessage>
              Feil brukernavn eller passord. Prøv igjen.
            </ValidationMessage>
          )}
          <Button type="submit">Logg inn</Button>
        </form>
      </Card>
    </section>
  );
}
