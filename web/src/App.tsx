import { useState } from "react";
import type { LoginResponse } from "./api/auth";
import LoginPage from "./pages/LoginPage";
import OrganizationsPage from "./pages/OrganizationsPage";
import type { MyOrganizationItem } from "./api/organizations";
import "./App.css";

function App() {
  const [session, setSession] = useState<LoginResponse | null>(null);
  const [selectedOrganization, setSelectedOrganization] =
    useState<MyOrganizationItem | null>(null);

  return (
    <main className="app-container">
      {session ? (
        <>
          <h1>OpsFlow</h1>
          <p>Signed in as {session.email}</p>

          {selectedOrganization ? (
            <section>
              <h2>{selectedOrganization.name}</h2>
              <p>Organization workspace selected.</p>
              <p>Projects workspace will be implemented next.</p>

              <button
                type="button"
                onClick={() => setSelectedOrganization(null)}
              >
                Change organization
              </button>
            </section>
          ) : (
            <OrganizationsPage
              accessToken={session.accessToken}
              onSelectOrganization={setSelectedOrganization}
            />
          )}

          <button
            type="button"
            className="primary-button"
            onClick={() => {
              setSelectedOrganization(null);
              setSession(null);
            }}
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
