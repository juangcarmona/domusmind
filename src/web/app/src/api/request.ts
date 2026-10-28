import { getStoredToken } from "../lib/tokenStorage";

export { getStoredToken };

const BASE_URL = "";
const BFF_MODE = import.meta.env.VITE_AUTH_MODE === "oidc-bff";
let csrfToken: string | null = null;

export async function getCsrfToken(): Promise<string> {
  if (csrfToken) return csrfToken;
  const response = await fetch("/api/session/csrf", { credentials: "same-origin" });
  if (!response.ok) throw new Error("Session is unavailable");
  const body = await response.json() as { token: string };
  csrfToken = body.token;
  return body.token;
}

export interface ApiError {
  status: number;
  message: string;
}

export async function request<T>(
  path: string,
  options?: RequestInit,
  token?: string | null,
): Promise<T> {
  const resolvedToken = BFF_MODE ? null : token ?? getStoredToken();
  const method = options?.method?.toUpperCase() ?? "GET";
  const csrf = BFF_MODE && !["GET", "HEAD", "OPTIONS"].includes(method)
    ? await getCsrfToken()
    : null;
  const headers: HeadersInit = {
    "Content-Type": "application/json",
    ...(resolvedToken ? { Authorization: `Bearer ${resolvedToken}` } : {}),
    ...(csrf ? { "X-CSRF-TOKEN": csrf } : {}),
    ...options?.headers,
  };
  const res = await fetch(`${BASE_URL}${path}`, { ...options, headers, credentials: "same-origin" });
  if (!res.ok) {
    if (res.status === 401) csrfToken = null;
    let message = res.statusText;
    try {
      const body = await res.json();
      if (body?.error) message = body.error;
    } catch {
      // ignore
    }
    throw { status: res.status, message } as ApiError;
  }
  if (res.status === 204) return undefined as T;
  return res.json() as Promise<T>;
}
