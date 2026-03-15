import { useState, type FormEvent } from "react";
import { useAuth } from "../auth/AuthProvider";
import type { ApiError } from "../api/authApi";

interface Props {
  onSuccess?: () => void;
  onGoToLogin?: () => void;
}

export function LoginPage({ onSuccess, onGoToLogin }: Props) {
  const { login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      await login(email, password);
      onSuccess?.();
    } catch (err) {
      setError((err as ApiError).message ?? "Login failed.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={styles.card}>
      <h2>Sign in</h2>
      <form onSubmit={handleSubmit} style={styles.form}>
        <label>
          Email
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={styles.input}
          />
        </label>
        <label>
          Password
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            style={styles.input}
          />
        </label>
        {error && <p style={styles.error}>{error}</p>}
        <button type="submit" disabled={loading} style={styles.button}>
          {loading ? "Signing in…" : "Sign in"}
        </button>
      </form>
      {onGoToLogin && (
        <p>
          No account?{" "}
          <button style={styles.link} onClick={onGoToLogin}>
            Register
          </button>
        </p>
      )}
    </div>
  );
}

const styles = {
  card: { maxWidth: 360, margin: "60px auto", padding: 24, border: "1px solid #ccc", borderRadius: 8 } as const,
  form: { display: "flex", flexDirection: "column" as const, gap: 12 },
  input: { display: "block", width: "100%", marginTop: 4, padding: "6px 8px", boxSizing: "border-box" as const },
  error: { color: "crimson", margin: 0 } as const,
  button: { padding: "8px 16px", cursor: "pointer" } as const,
  link: { background: "none", border: "none", color: "#0066cc", cursor: "pointer", padding: 0 } as const,
};
