import { getCsrfToken } from "./request";

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "";

export interface SetupStatusResponse {
  isInitialized: boolean;
}

export interface InitializeSystemRequest {
  email: string;
  password: string;
  displayName?: string | null;
}

export interface InitializeSystemResponse {
  userId: string;
  email: string;
}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const csrf = import.meta.env.VITE_AUTH_MODE === "oidc-bff" && options?.method === "POST"
    ? await getCsrfToken()
    : null;
  const headers: HeadersInit = {
    "Content-Type": "application/json",
    ...(csrf ? { "X-CSRF-TOKEN": csrf } : {}),
    ...options?.headers,
  };
  const res = await fetch(`${BASE_URL}${path}`, { ...options, headers, credentials: "same-origin" });
  if (!res.ok) {
    let message = res.statusText;
    try {
      const body = await res.json();
      if (body?.message) message = body.message;
    } catch {
      // ignore
    }
    throw { status: res.status, message };
  }
  return res.json() as Promise<T>;
}

export const setupApi = {
  getStatus: (): Promise<SetupStatusResponse> =>
    request<SetupStatusResponse>("/api/setup/status"),

  initialize: (body: InitializeSystemRequest): Promise<InitializeSystemResponse> =>
    request<InitializeSystemResponse>("/api/setup/initialize", {
      method: "POST",
      body: JSON.stringify(body),
    }),
};
