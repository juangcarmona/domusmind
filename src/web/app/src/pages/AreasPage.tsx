import { useEffect, useState, type FormEvent } from "react";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import {
  fetchAreas,
  createArea,
  assignPrimaryOwner,
  transferArea,
} from "../store/areasSlice";
import type { HouseholdAreaItem } from "../api/domusmindApi";

function AssignOwnerModal({
  area,
  familyId,
  members,
  onClose,
}: {
  area: HouseholdAreaItem;
  familyId: string;
  members: { memberId: string; name: string }[];
  onClose: () => void;
}) {
  const dispatch = useAppDispatch();
  const [memberId, setMemberId] = useState(area.primaryOwnerId ?? "");
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    if (!memberId) return;
    setSubmitting(true);
    setError(null);

    const isTransfer = !!area.primaryOwnerId && area.primaryOwnerId !== memberId;
    if (isTransfer) {
      const result = await dispatch(transferArea({ areaId: area.areaId, newPrimaryOwnerId: memberId, familyId }));
      setSubmitting(false);
      if (transferArea.fulfilled.match(result)) { onClose(); }
      else { setError((result as { payload?: unknown }).payload as string ?? "Failed"); }
    } else {
      const result = await dispatch(assignPrimaryOwner({ areaId: area.areaId, memberId, familyId }));
      setSubmitting(false);
      if (assignPrimaryOwner.fulfilled.match(result)) { onClose(); }
      else { setError((result as { payload?: unknown }).payload as string ?? "Failed"); }
    }
  }

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()}>
        <h2>
          {area.primaryOwnerId ? "Transfer ownership" : "Assign owner"} — {area.name}
        </h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="owner-select">Person responsible</label>
            <select
              id="owner-select"
              className="form-control"
              value={memberId}
              onChange={(e) => setMemberId(e.target.value)}
              required
              autoFocus
            >
              <option value="">— select person —</option>
              {members.map((m) => (
                <option key={m.memberId} value={m.memberId}>
                  {m.name}
                </option>
              ))}
            </select>
          </div>
          {error && <p className="error-msg">{error}</p>}
          <div className="modal-footer">
            <button type="button" className="btn btn-ghost" onClick={onClose}>
              Cancel
            </button>
            <button type="submit" className="btn" disabled={submitting || !memberId}>
              {submitting ? "Saving…" : "Save"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export function AreasPage() {
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const { items: areas, status, error } = useAppSelector((s) => s.areas);
  const familyId = family?.familyId;

  const [showForm, setShowForm] = useState(false);
  const [areaName, setAreaName] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);
  const [assignTarget, setAssignTarget] = useState<HouseholdAreaItem | null>(null);

  useEffect(() => {
    if (familyId) dispatch(fetchAreas(familyId));
  }, [familyId, dispatch]);

  async function handleCreate(e: FormEvent) {
    e.preventDefault();
    if (!familyId || !areaName.trim()) return;
    setSubmitting(true);
    setFormError(null);
    const result = await dispatch(createArea({ familyId, name: areaName.trim() }));
    setSubmitting(false);
    if (createArea.fulfilled.match(result)) {
      setAreaName("");
      setShowForm(false);
    } else {
      setFormError(result.payload as string ?? "Failed to create area");
    }
  }

  if (!familyId) return null;

  return (
    <div>
      <div className="page-header">
        <h1>Areas</h1>
        <button className="btn" onClick={() => { setShowForm(true); setFormError(null); }}>
          + New area
        </button>
      </div>

      {showForm && (
        <div className="card">
          <h2>Create area</h2>
          <form onSubmit={handleCreate}>
            <div className="form-group">
              <label htmlFor="area-name">Area name</label>
              <input
                id="area-name"
                className="form-control"
                type="text"
                value={areaName}
                onChange={(e) => setAreaName(e.target.value)}
                required
                autoFocus
                placeholder="e.g. Finance, School, Maintenance"
              />
            </div>
            {formError && <p className="error-msg">{formError}</p>}
            <div style={{ display: "flex", gap: "0.5rem" }}>
              <button type="submit" className="btn" disabled={submitting}>
                {submitting ? "Creating…" : "Create"}
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

      {status === "loading" && (
        <div className="loading-wrap">Loading areas…</div>
      )}

      {status === "error" && <p className="error-msg">{error}</p>}

      {status === "success" && areas.length === 0 && (
        <div className="empty-state">
          <p>No areas yet.</p>
          <p>Areas define who is responsible for what in your household.</p>
        </div>
      )}

      {areas.length > 0 && (
        <div className="item-list">
          {areas.map((area) => (
            <div key={area.areaId} className="item-card">
              <div className="item-card-body">
                <div className="item-card-title">{area.name}</div>
                <div className="item-card-subtitle">
                  {area.primaryOwnerName ? (
                    <>Owner: {area.primaryOwnerName}</>
                  ) : (
                    <span style={{ color: "var(--accent)" }}>No owner assigned</span>
                  )}
                  {area.secondaryOwnerIds.length > 0 && (
                    <span>
                      {" "}
                      · {area.secondaryOwnerIds.length} secondary
                    </span>
                  )}
                </div>
              </div>
              <div className="item-card-actions">
                <button
                  className="btn btn-ghost btn-sm"
                  onClick={() => setAssignTarget(area)}
                >
                  {area.primaryOwnerId ? "Transfer" : "Assign"}
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {assignTarget && (
        <AssignOwnerModal
          area={assignTarget}
          familyId={familyId}
          members={members}
          onClose={() => setAssignTarget(null)}
        />
      )}
    </div>
  );
}
