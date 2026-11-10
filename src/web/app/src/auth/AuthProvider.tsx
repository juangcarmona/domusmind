import React, {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { authApi, type MeResponse } from "../api/authApi";
import { getCsrfToken } from "../api/request";
import {
  getStoredToken as getAccessToken,
  getRefreshToken,
  saveTokens,
  clearTokens,
} from "../lib/tokenStorage";

// ── Context ───────────────────────────────────────────────────────────────────

interface AuthState {
  user: MeResponse | null;
  accessToken: string | null;
  isLoading: boolean;
}

interface AuthContextValue extends AuthState {
  login: (email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  refreshToken: () => Promise<boolean>;
  changePassword: (current: string, next: string) => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | null>(null);
const BFF_MODE = import.meta.env.VITE_AUTH_MODE === "oidc-bff";

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [state, setState] = useState<AuthState>({
    user: null,
    accessToken: getAccessToken(),
    isLoading: true,
  });

  // On mount, try to load the current user from the stored token.
  useEffect(() => {
    if (BFF_MODE) {
      clearTokens();
      authApi.me("")
        .then((user) => setState({ user, accessToken: null, isLoading: false }))
        .catch(() => setState({ user: null, accessToken: null, isLoading: false }));
      return;
    }
    const token = getAccessToken();
    if (!token) {
      setState((s) => ({ ...s, isLoading: false }));
      return;
    }

    authApi
      .me(token)
      .then((user) => setState({ user, accessToken: token, isLoading: false }))
      .catch(() => {
        // Access token stale - try refresh
        const rt = getRefreshToken();
        if (!rt) {
          clearTokens();
          setState({ user: null, accessToken: null, isLoading: false });
          return;
        }

        authApi
          .refresh(rt)
          .then(({ accessToken, refreshToken }) => {
            saveTokens(accessToken, refreshToken);
            return authApi.me(accessToken).then((user) =>
              setState({ user, accessToken, isLoading: false })
            );
          })
          .catch(() => {
            clearTokens();
            setState({ user: null, accessToken: null, isLoading: false });
          });
      });
  }, []);

  const login = useCallback(async (email: string, password: string) => {
    if (BFF_MODE) {
      window.location.assign("/api/session/login");
      return;
    }
    const res = await authApi.login({ email, password });
    saveTokens(res.accessToken, res.refreshToken);
    const user = await authApi.me(res.accessToken);
    setState({ user, accessToken: res.accessToken, isLoading: false });
  }, []);

  const logout = useCallback(async () => {
    if (BFF_MODE) {
      const token = await getCsrfToken();
      const form = document.createElement("form");
      form.method = "POST";
      form.action = "/api/session/logout";
      const field = document.createElement("input");
      field.type = "hidden";
      field.name = "__RequestVerificationToken";
      field.value = token;
      form.append(field);
      document.body.append(form);
      form.submit();
      return;
    }
    const rt = getRefreshToken();
    if (rt) {
      try {
        await authApi.logout(rt);
      } catch {
        // best-effort
      }
    }
    clearTokens();
    setState({ user: null, accessToken: null, isLoading: false });
  }, []);

  const refreshToken = useCallback(async (): Promise<boolean> => {
    if (BFF_MODE) {
      try {
        const user = await authApi.me("");
        setState({ user, accessToken: null, isLoading: false });
        return true;
      } catch {
        setState({ user: null, accessToken: null, isLoading: false });
        return false;
      }
    }
    const rt = getRefreshToken();
    if (!rt) return false;

    try {
      const res = await authApi.refresh(rt);
      saveTokens(res.accessToken, res.refreshToken);
      const user = await authApi.me(res.accessToken);
      setState({ user, accessToken: res.accessToken, isLoading: false });
      return true;
    } catch {
      clearTokens();
      setState({ user: null, accessToken: null, isLoading: false });
      return false;
    }
  }, []);

  const changePassword = useCallback(
    async (currentPassword: string, newPassword: string) => {
      if (BFF_MODE) throw new Error("Password changes are managed by the identity provider.");
      const token = state.accessToken;
      if (!token) throw new Error("Not authenticated");
      await authApi.changePassword(token, { currentPassword, newPassword });
      // Tokens invalidated server-side; log out client.
      await logout();
    },
    [state.accessToken, logout]
  );

  return (
    <AuthContext.Provider
      value={{ ...state, login, logout, refreshToken, changePassword }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
