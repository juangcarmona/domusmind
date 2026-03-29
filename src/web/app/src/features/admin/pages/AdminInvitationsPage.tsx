import { useEffect, useState, type FormEvent } from "react";
import { adminApi, type OperatorInvitationItem, type CreateOperatorInvitationResponse } from "../../../api/adminApi";

function statusBadgeClass(status: string) {
  switch (status.toLowerCase()) {
    case "pending": return "admin-badge--pending";
    case "accepted": return "admin-badge--accepted";
    case "revoked": return "admin-badge--revoked";
    case "expired": return "admin-badge--expired";
    default: return "";
  }
}

export function AdminInvitationsPage() {
  const [items, setItems] = useState<OperatorInvitationItem[] | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [email, setEmail] = useState("");
  const [note, setNote] = useState("");
  const [creating, setCreating] = useState(false);
  const [created, setCreated] = useState<CreateOperatorInvitationResponse | null>(null);
  const [createError, setCreateError] = useState<string | null>(null);

  function load() {
    setError(null);
    adminApi.getInvitations()
      .then((r) => setItems(r.items))
      .catch((e: { message?: string }) => setError(e.message ?? "Failed to load"));
  }

  useEffect(() => { load(); }, []);

  async function handleCreate(e: FormEvent) {
    e.preventDefault();
    if (!email.trim()) return;
    setCreating(true);
    setCreateError(null);
    setCreated(null);
    try {
      const result = await adminApi.createInvitation({ email: email.trim(), note: note.trim() || null });
      setCreated(result);
      setEmail("");
      setNote("");
      load();
    } catch (e) {
      const err = e as { message?: string };
      setCreateError(err.message ?? "Failed to create invitation");
    } finally {
      setCreating(false);
    }
  }

  async function handleRevoke(id: string) {
    try {
      await adminApi.revokeInvitation(id);
      load();
    } catch (e) {
      const err = e as { message?: string };
      setError(err.message ?? "Failed to revoke");
    }
  }

  return (
    <div>
      <div className="admin-section-header">
        <h2 className="admin-section-title">Operator Invitations</h2>
      </div>

      {/* Create form */}
      <div className="admin-card" style={{ padding: "1rem", marginBottom: "1rem" }}>
        <h3 style={{ fontSize: "0.875rem", fontWeight: 600, marginBottom: "0.75rem" }}>Issue new invitation</h3>
        <form onSubmit={(e) => void handleCreate(e)} style={{ display: "flex", gap: "0.5rem", flexWrap: "wrap", alignItems: "flex-end" }}>
          <div className="form-group" style={{ marginBottom: 0 }}>
            <label style={{ fontSize: "0.8rem", marginBottom: "0.25rem", display: "block" }}>Email</label>
            <input
              className="form-control"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              style={{ width: "220px" }}
              placeholder="user@example.com"
            />
          </div>
          <div className="form-group" style={{ marginBottom: 0 }}>
            <label style={{ fontSize: "0.8rem", marginBottom: "0.25rem", display: "block" }}>Note (optional)</label>
            <input
              className="form-control"
              type="text"
              value={note}
              onChange={(e) => setNote(e.target.value)}
              style={{ width: "200px" }}
              placeholder="Internal note"
            />
          </div>
          <button type="submit" className="btn" disabled={creating} style={{ fontSize: "0.875rem" }}>
            {creating ? "Issuing…" : "Issue invitation"}
          </button>
        </form>
        {createError && <p className="error-msg" style={{ marginTop: "0.5rem" }}>{createError}</p>}
        {created && (
          <div style={{ marginTop: "0.75rem" }}>
            <p style={{ fontSize: "0.8rem", marginBottom: "0.25rem", color: "var(--text)" }}>
              Invitation issued for <strong>{created.email}</strong>. Share this token with the recipient:
            </p>
            <div className="admin-token-box">{created.token}</div>
            <p style={{ fontSize: "0.75rem", color: "var(--muted)" }}>
              Expires: {new Date(created.expiresAtUtc).toLocaleString()}
            </p>
          </div>
        )}
      </div>

      {error && <p className="error-msg">{error}</p>}

      <div className="admin-card">
        {items === null ? (
          <p style={{ padding: "1rem" }}>Loading…</p>
        ) : items.length === 0 ? (
          <p className="admin-empty">No invitations yet.</p>
        ) : (
          <table className="admin-table">
            <thead>
              <tr>
                <th>Email</th>
                <th>Status</th>
                <th>Issued</th>
                <th>Expires</th>
                <th>Note</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {items.map((inv) => (
                <tr key={inv.id}>
                  <td>{inv.email}</td>
                  <td>
                    <span className={`admin-badge ${statusBadgeClass(inv.status)}`}>{inv.status}</span>
                  </td>
                  <td style={{ fontSize: "0.75rem", color: "var(--muted)" }}>
                    {new Date(inv.createdAtUtc).toLocaleDateString()}
                  </td>
                  <td style={{ fontSize: "0.75rem", color: "var(--muted)" }}>
                    {new Date(inv.expiresAtUtc).toLocaleDateString()}
                  </td>
                  <td style={{ fontSize: "0.75rem", color: "var(--muted)" }}>{inv.note ?? "—"}</td>
                  <td>
                    {inv.status === "Pending" && (
                      <button
                        type="button"
                        className="btn btn-secondary"
                        style={{ fontSize: "0.75rem", padding: "0.2rem 0.5rem" }}
                        onClick={() => void handleRevoke(inv.id)}
                      >
                        Revoke
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
