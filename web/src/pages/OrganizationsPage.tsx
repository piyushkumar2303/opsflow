import { useEffect, useState } from "react";
import {
  getMyOrganizations,
  type MyOrganizationItem,
} from "../api/organizations";

type OrganizationsPageProps = {
  accessToken: string;
  onSelectOrganization: (organization: MyOrganizationItem) => void;
};

function OrganizationsPage({
  accessToken,
  onSelectOrganization,
}: OrganizationsPageProps) {
  const [organizations, setOrganizations] = useState<MyOrganizationItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const controller = new AbortController();

    async function loadOrganizations() {
      try {
        const result = await getMyOrganizations(accessToken, controller.signal);
        setOrganizations(result);
      } catch (err) {
        if (controller.signal.aborted) return;

        setError(
          err instanceof Error ? err.message : "Unable to load organizations.",
        );
      } finally {
        if (!controller.signal.aborted) setIsLoading(false);
      }
    }

    void loadOrganizations();

    return () => controller.abort();
  }, [accessToken]);

  if (isLoading) {
    return <p role="status">Loading organizations...</p>;
  }

  if (error) {
    return (
      <p className="form-error" role="alert">
        {error}
      </p>
    );
  }

  return (
    <section>
      <h2>My Organizations</h2>

      {organizations.length === 0 ? (
        <p>You don't belong to any organizations yet.</p>
      ) : (
        <ul>
          {organizations.map((organization) => (
            <li key={organization.id}>
              <button
                type="button"
                onClick={() => onSelectOrganization(organization)}
              >
                <strong>{organization.name}</strong>
                {" — "}
                {organization.role === 1
                  ? "Admin"
                  : organization.role === 2
                    ? "Manager"
                    : organization.role === 3
                      ? "Engineer"
                      : "Unknown"}
              </button>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}

export default OrganizationsPage;
