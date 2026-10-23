
export type LoginRequest = {
  email: string;
  password: string;
};

export type LoginResponse = {
  userId: string;
  email: string;
  accessToken: string;
};

type ApiError = {
  detail?: string;
};

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function login(request: LoginRequest): Promise<LoginResponse> {
  const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    if (response.status === 401) {
      throw new Error("Invalid email or password.");
    }

    const errorBody = await response.json().catch(() => null) as ApiError | null;
    throw new Error(errorBody?.detail ?? "Login failed. Please try again.");
  }

  return await response.json() as LoginResponse;
}
