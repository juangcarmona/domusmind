import { useEffect, useState } from "react";
import { adminApi, type AdminHouseholdSummary } from "../../../api/adminApi";

export function AdminHouseholdsPage() {
  const [items, setItems] = useState<AdminHouseholdSummary[] | null>(null);
  const [search, setSearch] = useState("");
  const [error, setError] = useState<string | null>(null);

  function load(q?: string) {
    setError(null);
    adminApi.getHouseholds(q || undefined)
      .then((r) => setItems(r.items))
      .catch((e: { message?: string }) => setError(e.message ?? "Failed to load"));
  }

  useEffect(() => { load(); }, []);

  function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    load(search.trim());
  }

  return (
    <div>
      <div className="admin-section-header">
        <h2 className="admin-section-title">Households</h2>
        <form onSubmit={handleSearch} style={{ display: "flex", gap: "0.5rem" }}>
          <input
            className="admin-search"
            placeholder="Search by name…"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
          <button type="submit" className="btn" style={{ fontSize: "0.875rem" }}>Search</button>
        </form>
      </div>
      {error && <p className="error-msg">{error}</p>}
      <div className="admin-card">
        {items === null ? (
          <p style={{ padding: "1rem" }}>Loading…</p>
        ) : items.length === 0 ? (
          <p className="admin-empty">No households found.</p>
        ) : (
          <table className="admin-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Members</th>
                <th>Created</th>
                <th>ID</th>
              </tr>
            </thead>
            <tbody>
              {items.map((h) => (
                <tr key={h.familyId}>
                  <td>{h.name}</td>
                  <td>{h.memberCount}</td>
                  <td>{new Date(h.createdAtUtc).toLocaleDateString()}</td>
                  <td style={{ fontFamily: "monospace", fontSize: "0.75rem", color: "var(--muted, #888)" }}>{h.familyId}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}
