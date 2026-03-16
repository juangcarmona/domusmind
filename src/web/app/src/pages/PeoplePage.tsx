import { useEffect, useState, type FormEvent } from "react";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { fetchMembers, addMember } from "../store/householdSlice";

export function PeoplePage() {
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const familyId = family?.familyId;

  const [showForm, setShowForm] = useState(false);
  const [name, setName] = useState("");
  const [role, setRole] = useState("Adult");
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (familyId) dispatch(fetchMembers(familyId));
  }, [familyId, dispatch]);

  async function handleAdd(e: FormEvent) {
    e.preventDefault();
    if (!familyId || !name.trim()) return;
    setSubmitting(true);
    setError(null);
    const result = await dispatch(
      addMember({ familyId, name: name.trim(), role }),
    );
    setSubmitting(false);
    if (addMember.fulfilled.match(result)) {
      setName("");
      setRole("Adult");
      setShowForm(false);
    } else {
      setError(result.payload as string ?? "Failed to add person");
    }
  }

  if (!familyId) return null;

  return (
    <div>
      <div className="page-header">
        <h1>People</h1>
        <button
          className="btn"
          onClick={() => { setShowForm(true); setError(null); }}
        >
          + Add person
        </button>
      </div>

      {showForm && (
        <div className="card">
          <h2>Add person</h2>
          <form onSubmit={handleAdd}>
            <div className="inline-form" style={{ marginBottom: "0.75rem" }}>
              <div className="form-group" style={{ flex: 2 }}>
                <label htmlFor="person-name">Name</label>
                <input
                  id="person-name"
                  className="form-control"
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                  autoFocus
                  placeholder="e.g. Lucas"
                />
              </div>
              <div className="form-group" style={{ flex: 1 }}>
                <label htmlFor="person-role">Role</label>
                <select
                  id="person-role"
                  className="form-control"
                  value={role}
                  onChange={(e) => setRole(e.target.value)}
                >
                  <option value="Adult">Adult</option>
                  <option value="Child">Child</option>
                  <option value="Teen">Teen</option>
                </select>
              </div>
            </div>
            {error && <p className="error-msg">{error}</p>}
            <div style={{ display: "flex", gap: "0.5rem" }}>
              <button type="submit" className="btn" disabled={submitting}>
                {submitting ? "Adding…" : "Add"}
              </button>
              <button
                type="button"
                className="btn btn-ghost"
                onClick={() => setShowForm(false)}
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      {members.length === 0 ? (
        <div className="empty-state">
          <p>No people added yet.</p>
          <p>Add household members to assign chores, plans, and areas.</p>
        </div>
      ) : (
        <div className="item-list">
          {members.map((m) => (
            <div key={m.memberId} className="item-card">
              <div
                style={{
                  width: 36,
                  height: 36,
                  borderRadius: "50%",
                  background:
                    "color-mix(in srgb, var(--primary) 15%, transparent)",
                  color: "var(--primary)",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  fontWeight: 700,
                  fontSize: "0.9rem",
                  flexShrink: 0,
                }}
              >
                {m.name.charAt(0).toUpperCase()}
              </div>
              <div className="item-card-body">
                <div className="item-card-title">{m.name}</div>
                <div className="item-card-subtitle">{m.role}</div>
              </div>
              <span className="pill">{m.role}</span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
