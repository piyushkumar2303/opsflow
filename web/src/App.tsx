import { useState } from "react";
import type { LoginResponse } from "./api/auth";
import LoginPage from "./pages/LoginPage";
import "./App.css";

function App() {
  const [session, setSession] = useState<LoginResponse | null>(null);

  return (
    <main className="app-container">
      {session ? (
        <>
          <h1>OpsFlow</h1>
          <p>Signed in as {session.email}</p>

          <button
            type="button"
            className="primary-button"
            onClick={() => setSession(null)}
          >
            Sign out
          </button>
        </>
      ) : (
        <LoginPage onLoginSuccess={setSession} />
      )}
    </main>
  );
}

export default App;
