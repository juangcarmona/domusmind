import { useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../../auth/AuthProvider";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import {
  addMember,
  disableMemberAccess,
  provisionMemberAccess,
  regeneratePassword,
  updateMember,
} from "../../../store/householdSlice";

const MEMBER_ROLES = ["Adult", "Child", "Pet", "Caregiver"] as const;
const ADD_MEMBER_ROLES = MEMBER_ROLES.filter((r) => r !== "Caregiver");

function AccessStatusBadge({
  status,
  tM,
}: {
  status: string;
  tM: (key: string) => string;
}) {
  const map: Record<string, { label: string; color: string }> = {
    None: { label: tM("noAccount"), color: "var(--muted)" },
    PasswordChangeRequired: { label: tM("passwordChangeRequired"), color: "#f5a623" },
    Active: { label: tM("accountActive"), color: "#22c55e" },
    Disabled: { label: tM("accountDisabled"), color: "#ef4444" },
  };
  const badge = map[status] ?? map["None"];
  return (
    <span
      style={{
        fontSize: "0.7rem",
        padding: "0.1rem 0.4rem",
        borderRadius: 4,
        background: `color-mix(in srgb, ${badge.color} 18%, transparent)`,
        color: badge.color,
      }}
    >
      {badge.label}
    </span>
  );
}

type EditMode = "profile" | "provisionAccess";

export function MembersManagementSection() {
  const { t } = useTranslation("settings");
  const { user } = useAuth();
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const isCurrentUserManager = members.some(
    (m) => m.authUserId === user?.userId && m.isManager,
  );

  const tM = (key: string) => t(`household.members.${key}` as never);

  // ── Edit state ──────────────────────────────────────────────────────────────
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editMode, setEditMode] = useState<EditMode>("profile");

  // Profile form
  const [editName, setEditName] = useState("");
  const [editRole, setEditRole] = useState("Adult");
  const [editBirthDate, setEditBirthDate] = useState("");
  const [editIsManager, setEditIsManager] = useState(false);
  const [editSaving, setEditSaving] = useState(false);
  const [editError, setEditError] = useState<string | null>(null);

  // Provision access form
  const [provisionEmail, setProvisionEmail] = useState("");
  const [provisionDisplayName, setProvisionDisplayName] = useState("");
  const [provisionSaving, setProvisionSaving] = useState(false);
  const [provisionError, setProvisionError] = useState<string | null>(null);
  const [provisioned, setProvisioned] = useState<{
    email: string;
    temporaryPassword: string;
  } | null>(null);

  // Regenerate password state (per-member, shown inline)
  const [regenMemberId, setRegenMemberId] = useState<string | null>(null);
  const [regenResult, setRegenResult] = useState<string | null>(null);
  const [regenSaving, setRegenSaving] = useState(false);
  const [regenError, setRegenError] = useState<string | null>(null);

  // Disable access state
  const [disableSaving, setDisableSaving] = useState<string | null>(null);
  const [disableError, setDisableError] = useState<{ memberId: string; message: string } | null>(null);

  const [showAddMember, setShowAddMember] = useState(false);
  const [addName, setAddName] = useState("");
  const [addRole, setAddRole] = useState("Adult");
  const [addSaving, setAddSaving] = useState(false);
  const [addError, setAddError] = useState<string | null>(null);

  if (!family) return null;

  const myMemberId = members.find((m) => m.authUserId === user?.userId)?.memberId;

  const roleOrder = (role: string) => {
    switch (role) {
      case "Adult": return 0;
      case "Child": return 1;
      case "Pet": return 2;
      default: return 3;
    }
  };

  const visibleMembers = members
    .filter((m) => m.memberId !== myMemberId)
    .slice()
    .sort((a, b) => {
      const rd = roleOrder(a.role) - roleOrder(b.role);
      if (rd !== 0) return rd;
      const md = (b.isManager ? 1 : 0) - (a.isManager ? 1 : 0);
      if (md !== 0) return md;
      return a.name.localeCompare(b.name);
    });

  // ── Edit helpers ────────────────────────────────────────────────────────────
  function openEdit(memberId: string) {
    const m = members.find((x) => x.memberId === memberId);
    if (!m) return;
    setEditingId(memberId);
    setEditMode("profile");
    setEditName(m.name);
    setEditRole(m.role);
    setEditBirthDate(m.birthDate ?? "");
    setEditIsManager(m.isManager);
    setEditError(null);
    setProvisionEmail("");
    setProvisionDisplayName("");
    setProvisionError(null);
    setProvisioned(null);
  }

  function cancelEdit() {
    setEditingId(null);
    setProvisioned(null);
    setEditError(null);
    setProvisionError(null);
  }

  async function handleProfileSave(e: FormEvent) {
    e.preventDefault();
    if (!editingId) return;
    setEditSaving(true);
    setEditError(null);

    const result = await dispatch(
      updateMember({
        familyId: family!.familyId,
        memberId: editingId,
        name: editName.trim(),
        role: editRole,
        birthDate: editBirthDate || null,
        isManager: editIsManager && editRole === "Adult",
      }),
    );

    setEditSaving(false);
    if (updateMember.fulfilled.match(result)) {
      setEditingId(null);
    } else {
      setEditError((result.payload as string) ?? tM("updateError"));
    }
  }

  async function handleProvisionAccess(e: FormEvent) {
    e.preventDefault();
    if (!editingId) return;
    setProvisionSaving(true);
    setProvisionError(null);

    const result = await dispatch(
      provisionMemberAccess({
        familyId: family!.familyId,
        memberId: editingId,
        email: provisionEmail.trim().toLowerCase(),
        displayName: provisionDisplayName.trim() || null,
      }),
    );

    setProvisionSaving(false);
    if (provisionMemberAccess.fulfilled.match(result)) {
      setProvisioned({
        email: result.payload.email,
        temporaryPassword: result.payload.temporaryPassword,
      });
    } else {
      setProvisionError((result.payload as string) ?? tM("provisionError"));
    }
  }

  async function handleRegenPassword(memberId: string) {
    setRegenMemberId(memberId);
    setRegenResult(null);
    setRegenError(null);
    setRegenSaving(true);

    const result = await dispatch(
      regeneratePassword({ familyId: family!.familyId, memberId }),
    );

    setRegenSaving(false);
    if (regeneratePassword.fulfilled.match(result)) {
      setRegenResult(result.payload.temporaryPassword);
    } else {
      setRegenError((result.payload as string) ?? tM("regenError"));
    }
  }

  async function handleDisableAccess(memberId: string) {
    setDisableSaving(memberId);
    setDisableError(null);

    const result = await dispatch(
      disableMemberAccess({ familyId: family!.familyId, memberId }),
    );

    setDisableSaving(null);
    if (!disableMemberAccess.fulfilled.match(result)) {
      setDisableError({
        memberId,
        message: (result.payload as string) ?? tM("disableError"),
      });
    }
  }

  async function handleAddMember(e: FormEvent) {
    e.preventDefault();
    if (!addName.trim()) return;
    setAddSaving(true);
    setAddError(null);
    const result = await dispatch(
      addMember({
        familyId: family!.familyId,
        name: addName.trim(),
        role: addRole,
      }),
    );
    setAddSaving(false);
    if (addMember.fulfilled.match(result)) {
      setAddName("");
      setAddRole("Adult");
      setShowAddMember(false);
    } else {
      setAddError((result.payload as string) ?? tM("addError"));
    }
  }

  return (
    <section className="settings-section">
      <h2 className="settings-section-title">{tM("title")}</h2>
      {isCurrentUserManager && (
        <div style={{ marginBottom: "0.85rem" }}>
          <button type="button" className="btn" onClick={() => setShowAddMember(true)}>
            + {tM("addMember")}
          </button>
        </div>
      )}

      {showAddMember && (
        <div className="card" style={{ padding: "1rem", marginBottom: "0.75rem" }}>
          <h3 style={{ marginBottom: "0.75rem" }}>{tM("addMember")}</h3>
          <form onSubmit={handleAddMember}>
            <div className="form-group">
              <label>{tM("name")}</label>
              <input
                className="form-control"
                type="text"
                value={addName}
                onChange={(e) => setAddName(e.target.value)}
                required
                autoFocus
              />
            </div>
            <div className="form-group">
              <label>{tM("role")}</label>
              <select
                className="form-control"
                value={addRole}
                onChange={(e) => setAddRole(e.target.value)}
              >
                {ADD_MEMBER_ROLES.map((r) => (
                  <option key={r} value={r}>
                    {t(`household.members.roles.${r}` as never)}
                  </option>
                ))}
              </select>
            </div>
            {addError && <p className="error-msg">{addError}</p>}
            <div style={{ display: "flex", gap: "0.5rem" }}>
              <button type="submit" className="btn" disabled={addSaving}>
                {addSaving ? tM("saving") : tM("addMember")}
              </button>
              <button
                type="button"
                className="btn btn-ghost"
                onClick={() => {
                  setShowAddMember(false);
                  setAddError(null);
                  setAddName("");
                  setAddRole("Adult");
                }}
              >
                {tM("cancel")}
              </button>
            </div>
          </form>
        </div>
      )}

      {visibleMembers.length === 0 ? (
        <p style={{ color: "var(--muted)", fontSize: "0.9rem" }}>{tM("noMembers")}</p>
      ) : (
        <div className="item-list">
          {visibleMembers.map((m) => (
            <div key={m.memberId}>
              {editingId === m.memberId ? (
                <div className="card" style={{ padding: "1rem" }}>
                  {/* Credentials display after successful provisioning */}
                  {provisioned ? (
                    <div>
                      <p style={{ fontWeight: 600, marginBottom: "0.5rem" }}>{tM("credentialsTitle")}</p>
                      <div
                        style={{
                          background: "color-mix(in srgb, var(--warning, #f5a623) 12%, transparent)",
                          border: "1px solid color-mix(in srgb, var(--warning, #f5a623) 40%, transparent)",
                          borderRadius: 8,
                          padding: "0.75rem",
                          marginBottom: "0.75rem",
                          fontSize: "0.85rem",
                        }}
                      >
                        {tM("credentialsSaveWarning")}
                      </div>
                      <div
                        style={{
                          background: "color-mix(in srgb, var(--primary) 8%, transparent)",
                          borderRadius: 8,
                          padding: "0.75rem",
                          fontFamily: "monospace",
                          marginBottom: "1rem",
                        }}
                      >
                        <div>
                          <span style={{ color: "var(--muted)", marginRight: 8 }}>{tM("email")}:</span>
                          <strong>{provisioned.email}</strong>
                        </div>
                        <div>
                          <span style={{ color: "var(--muted)", marginRight: 8 }}>{tM("temporaryPassword")}:</span>
                          <strong>{provisioned.temporaryPassword}</strong>
                          <button
                            type="button"
                            className="btn btn-ghost"
                            style={{ fontSize: "0.7rem", padding: "0.1rem 0.5rem", marginLeft: 8 }}
                            onClick={() => navigator.clipboard?.writeText(provisioned!.temporaryPassword)}
                          >
                            {tM("copy")}
                          </button>
                        </div>
                      </div>
                      <button type="button" className="btn" onClick={cancelEdit}>
                        {tM("done")}
                      </button>
                    </div>
                  ) : (
                    <>
                      {/* Tab bar */}
                      <div style={{ display: "flex", gap: "0.5rem", marginBottom: "1rem", borderBottom: "1px solid var(--border, rgba(255,255,255,0.1))", paddingBottom: "0.5rem" }}>
                        <button
                          type="button"
                          className={`btn btn-ghost${editMode === "profile" ? " active" : ""}`}
                          style={{ fontSize: "0.85rem", padding: "0.25rem 0.6rem", fontWeight: editMode === "profile" ? 700 : undefined }}
                          onClick={() => setEditMode("profile")}
                        >
                          {tM("editTitle")}
                        </button>
                        {isCurrentUserManager && !m.authUserId && (
                          <button
                            type="button"
                            className={`btn btn-ghost${editMode === "provisionAccess" ? " active" : ""}`}
                            style={{ fontSize: "0.85rem", padding: "0.25rem 0.6rem", fontWeight: editMode === "provisionAccess" ? 700 : undefined }}
                            onClick={() => setEditMode("provisionAccess")}
                          >
                            {tM("provisionAccess")}
                          </button>
                        )}
                      </div>

                      {editMode === "profile" && (
                        <form onSubmit={handleProfileSave}>
                          <div className="form-group">
                            <label>{tM("name")}</label>
                            <input
                              className="form-control"
                              type="text"
                              value={editName}
                              onChange={(e) => setEditName(e.target.value)}
                              required
                              autoFocus
                            />
                          </div>
                          <div className="inline-form" style={{ marginBottom: "0.75rem" }}>
                            <div className="form-group" style={{ flex: 1 }}>
                              <label>{tM("role")}</label>
                              <select
                                className="form-control"
                                value={editRole}
                                onChange={(e) => {
                                  setEditRole(e.target.value);
                                  if (e.target.value !== "Adult") setEditIsManager(false);
                                }}
                              >
                                {MEMBER_ROLES.map((r) => (
                                  <option key={r} value={r}>
                                    {t(`household.members.roles.${r}` as never)}
                                  </option>
                                ))}
                              </select>
                            </div>
                            <div className="form-group" style={{ flex: 1 }}>
                              <label>{tM("birthDate")}</label>
                              <input
                                className="form-control"
                                type="date"
                                value={editBirthDate}
                                onChange={(e) => setEditBirthDate(e.target.value)}
                              />
                            </div>
                          </div>
                          {editRole === "Adult" && (
                            <div className="form-group">
                              <label style={{ display: "flex", alignItems: "center", gap: "0.5rem", cursor: "pointer" }}>
                                <input
                                  type="checkbox"
                                  checked={editIsManager}
                                  onChange={(e) => setEditIsManager(e.target.checked)}
                                />
                                {tM("isManager")}
                              </label>
                            </div>
                          )}
                          {editError && <p className="error-msg">{editError}</p>}
                          <div style={{ display: "flex", gap: "0.5rem" }}>
                            <button type="submit" className="btn" disabled={editSaving}>
                              {editSaving ? tM("saving") : tM("save")}
                            </button>
                            <button type="button" className="btn btn-ghost" onClick={cancelEdit}>
                              {tM("cancel")}
                            </button>
                          </div>
                        </form>
                      )}

                      {editMode === "provisionAccess" && (
                        <form onSubmit={handleProvisionAccess}>
                          <p style={{ fontSize: "0.85rem", color: "var(--muted)", marginBottom: "0.75rem" }}>
                            {tM("provisionAccessSubtitle")}
                          </p>
                          <div className="form-group">
                            <label>{tM("email")}</label>
                            <input
                              className="form-control"
                              type="email"
                              value={provisionEmail}
                              onChange={(e) => setProvisionEmail(e.target.value)}
                              required
                              autoFocus
                              autoComplete="off"
                            />
                          </div>
                          <div className="form-group">
                            <label>{tM("displayName")}</label>
                            <input
                              className="form-control"
                              type="text"
                              value={provisionDisplayName}
                              onChange={(e) => setProvisionDisplayName(e.target.value)}
                              placeholder={tM("displayNamePlaceholder")}
                              autoComplete="off"
                            />
                          </div>
                          {provisionError && <p className="error-msg">{provisionError}</p>}
                          <div style={{ display: "flex", gap: "0.5rem" }}>
                            <button type="submit" className="btn" disabled={provisionSaving}>
                              {provisionSaving ? tM("saving") : tM("provisionAccess")}
                            </button>
                            <button type="button" className="btn btn-ghost" onClick={cancelEdit}>
                              {tM("cancel")}
                            </button>
                          </div>
                        </form>
                      )}
                    </>
                  )}
                </div>
              ) : (
                <div className="item-card">
                  <div
                    style={{
                      width: 36,
                      height: 36,
                      borderRadius: "50%",
                      background: "color-mix(in srgb, var(--primary) 15%, transparent)",
                      color: "var(--primary)",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      fontWeight: 700,
                      fontSize: "0.9rem",
                      flexShrink: 0,
                    }}
                  >
                    {m.name[0]?.toUpperCase()}
                  </div>
                  <div style={{ flex: 1, minWidth: 0 }}>
                    <div style={{ fontWeight: 600, display: "flex", alignItems: "center", gap: "0.4rem", flexWrap: "wrap" }}>
                      <span>{m.name}</span>
                      {m.isManager && (
                        <span
                          style={{
                            fontSize: "0.7rem",
                            padding: "0.1rem 0.4rem",
                            borderRadius: 4,
                            background: "color-mix(in srgb, var(--primary) 20%, transparent)",
                            color: "var(--primary)",
                          }}
                        >
                          {tM("managerBadge")}
                        </span>
                      )}
                      {m.authUserId === user?.userId && (
                        <span
                          style={{
                            fontSize: "0.7rem",
                            padding: "0.1rem 0.4rem",
                            borderRadius: 4,
                            background: "color-mix(in srgb, var(--primary) 12%, transparent)",
                            color: "var(--primary)",
                            fontStyle: "italic",
                          }}
                        >
                          {tM("youBadge")}
                        </span>
                      )}
                      <AccessStatusBadge status={m.accessStatus} tM={tM} />
                    </div>
                    <div style={{ fontSize: "0.8rem", color: "var(--muted)" }}>
                      {t(`household.members.roles.${m.role}` as never, m.role)}
                      {m.linkedEmail && (
                        <span style={{ marginLeft: 8 }}>{m.linkedEmail}</span>
                      )}
                    </div>
                    {/* Inline regen result */}
                    {regenMemberId === m.memberId && regenResult && (
                      <div
                        style={{
                          marginTop: "0.5rem",
                          background: "color-mix(in srgb, var(--primary) 8%, transparent)",
                          borderRadius: 6,
                          padding: "0.5rem 0.75rem",
                          fontFamily: "monospace",
                          fontSize: "0.85rem",
                        }}
                      >
                        <span style={{ color: "var(--muted)", marginRight: 8 }}>
                          {tM("newTemporaryPassword")}:
                        </span>
                        <strong>{regenResult}</strong>
                        <button
                          type="button"
                          className="btn btn-ghost"
                          style={{ fontSize: "0.7rem", padding: "0.1rem 0.5rem", marginLeft: 8 }}
                          onClick={() => navigator.clipboard?.writeText(regenResult!)}
                        >
                          {tM("copy")}
                        </button>
                        <div style={{ fontSize: "0.75rem", color: "var(--warning, #f5a623)", marginTop: "0.25rem" }}>
                          {tM("credentialsSaveWarning")}
                        </div>
                      </div>
                    )}
                    {regenMemberId === m.memberId && regenError && (
                      <p className="error-msg" style={{ marginTop: "0.25rem" }}>{regenError}</p>
                    )}
                    {disableError?.memberId === m.memberId && (
                      <p className="error-msg" style={{ marginTop: "0.25rem" }}>{disableError.message}</p>
                    )}
                  </div>
                  <div style={{ display: "flex", gap: "0.4rem", flexShrink: 0, flexWrap: "wrap", justifyContent: "flex-end" }}>
                    {(isCurrentUserManager || m.authUserId === user?.userId) && (
                      <button
                        type="button"
                        className="btn btn-ghost"
                        style={{ fontSize: "0.8rem", padding: "0.25rem 0.6rem" }}
                        onClick={() => openEdit(m.memberId)}
                      >
                        {tM("edit")}
                      </button>
                    )}
                    {/* Admin-only: regenerate password */}
                    {isCurrentUserManager &&
                      m.authUserId &&
                      m.accessStatus !== "Disabled" &&
                      m.authUserId !== user?.userId && (
                        <button
                          type="button"
                          className="btn btn-ghost"
                          style={{ fontSize: "0.8rem", padding: "0.25rem 0.6rem" }}
                          disabled={regenSaving && regenMemberId === m.memberId}
                          onClick={() => handleRegenPassword(m.memberId)}
                        >
                          {regenSaving && regenMemberId === m.memberId
                            ? tM("saving")
                            : tM("regeneratePassword")}
                        </button>
                      )}
                    {/* Admin-only: disable access */}
                    {isCurrentUserManager &&
                      m.authUserId &&
                      m.accessStatus !== "Disabled" &&
                      m.authUserId !== user?.userId && (
                        <button
                          type="button"
                          className="btn btn-ghost"
                          style={{ fontSize: "0.8rem", padding: "0.25rem 0.6rem", color: "#ef4444" }}
                          disabled={disableSaving === m.memberId}
                          onClick={() => handleDisableAccess(m.memberId)}
                        >
                          {disableSaving === m.memberId ? tM("saving") : tM("disableAccess")}
                        </button>
                      )}
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </section>
  );
}
