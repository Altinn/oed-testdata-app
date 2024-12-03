import { useState } from "react";
import "./style.css";

export default function LoginDialog() {
  const [username, setUsername] = useState<string>();
  const [password, setPassword] = useState<string>();
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);

    try{
      fetch(`/api/auth/login`, {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': 'Basic ' + btoa(username + ':' + password)
        }
      })
      .then(
        function(response) {
          if (response.status !== 200) {
            console.log('Looks like there was a problem. Status Code: ' +
              response.status);
            return;
          }
          localStorage.setItem("auth", "true");
          location.reload();
        }
      )
      .catch(function(err) {
        console.log('Fetch Error :-S', err);
      });     
    }
    finally {
      setIsLoading(false);
    }
  }

  if (localStorage.getItem("auth") === "true") return null;

  return (
    <div id="login-dialog">
      <h1>LOGIN</h1>
      <form onSubmit={onSubmit}>
        <label>
          Username: 
          <input name="username" type="text" onChange={(e) => setUsername(e.target.value)} disabled={isLoading} />
        </label>
        <label>
          Password: 
          <input name="password" type="password" onChange={(e) => setPassword(e.target.value)} disabled={isLoading} />
        </label>        
        <button type="submit" disabled={isLoading}>{isLoading ? "Loading..." : "Login!"}</button>
      </form>
    </div>
  );
}