
export type MyOrganizationItem = {
  id: string;
  name: string;
  role: number;
};

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getMyOrganizations(
  accessToken: string,
  signal?: AbortSignal
): Promise<MyOrganizationItem[]> {
  const response = await fetch(`${API_BASE_URL}/api/organizations/mine`, {
  method: "GET",
  headers: {
    Authorization: `Bearer ${accessToken}`,
  },
  signal,
});

  if (!response.ok) {
    if (response.status === 401) {
      throw new Error("Your session is invalid or has expired. Please sign in again.");
    }

    throw new Error("Unable to load organizations. Please try again.");
  }

  return await response.json() as MyOrganizationItem[];
}
