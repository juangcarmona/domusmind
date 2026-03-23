import { useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../../auth/AuthProvider";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import { updateMember } from "../../../store/householdSlice";
import { MembersManagementSection } from "./MembersManagementSection";

const MEMBER_ROLES = ["Adult", "Child", "Pet", "Caregiver"] as const;

export function MembersSettingsSection() {
  const { t } = useTranslation("settings");
  const { user } = useAuth();
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const me = members.find((m) => m.authUserId === user?.userId);
  const tM = (key: string) => t(`household.members.${key}` as never);

  const [isEditing, setIsEditing] = useState(false);
  const [editName, setEditName] = useState("");
  const [editRole, setEditRole] = useState("Adult");
  const [editBirthDate, setEditBirthDate] = useState("");
  const [editIsManager, setEditIsManager] = useState(false);
  const [editSaving, setEditSaving] = useState(false);
  const [editError, setEditError] = useState<string | null>(null);

  function openEdit() {
    if (!me) return;
    setEditName(me.name);
    setEditRole(me.role);
    setEditBirthDate(me.birthDate ?? "");
    setEditIsManager(me.isManager);
    setEditError(null);
    setIsEditing(true);
  }

  async function handleProfileSave(e: FormEvent) {
    e.preventDefault();
    if (!me || !family) return;
    setEditSaving(true);
    setEditError(null);
    const result = await dispatch(
      updateMember({
        familyId: family.familyId,
        memberId: me.memberId,
        name: editName.trim(),
        role: editRole,
        birthDate: editBirthDate || null,
        isManager: editIsManager && editRole === "Adult",
      }),
    );
    setEditSaving(false);
    if (updateMember.fulfilled.match(result)) {
      setIsEditing(false);
    } else {
      setEditError((result.payload as string) ?? tM("updateError"));
    }
  }

  return (
    <>
      {me && (
        <section className="settings-section">
          <h2 className="settings-section-title">{t("membersTab.myProfile")}</h2>
          {isEditing ? (
            <div className="card" style={{ padding: "1rem" }}>
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
                  <button type="button" className="btn btn-ghost" onClick={() => setIsEditing(false)}>
                    {tM("cancel")}
                  </button>
                </div>
              </form>
            </div>
          ) : (
            <div
              className="card"
              style={{
                display: "flex",
                alignItems: "center",
                gap: "1rem",
                padding: "1rem",
                border: "2px solid var(--primary)",
              }}
            >
              <div
                style={{
                  width: 48,
                  height: 48,
                  borderRadius: "50%",
                  background: "color-mix(in srgb, var(--primary) 25%, transparent)",
                  color: "var(--primary)",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  fontWeight: 700,
                  fontSize: "1.2rem",
                  flexShrink: 0,
                }}
              >
                {me.name[0]?.toUpperCase()}
              </div>
              <div style={{ flex: 1, minWidth: 0 }}>
                <div style={{ fontWeight: 700, fontSize: "1.05rem", display: "flex", alignItems: "center", gap: "0.5rem", flexWrap: "wrap" }}>
                  <span>{me.name}</span>
                  {me.isManager && (
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
                    {t("membersTab.youBadge")}
                  </span>
                </div>
                <div style={{ fontSize: "0.85rem", color: "var(--muted)", marginTop: "0.2rem" }}>
                  {t(`household.members.roles.${me.role}` as never, me.role)}
                  {me.birthDate && (
                    <span style={{ marginLeft: "0.75rem" }}>
                      · {new Date(me.birthDate).toLocaleDateString()}
                    </span>
                  )}
                </div>
                <div style={{ fontSize: "0.8rem", color: "var(--muted)", marginTop: "0.15rem" }}>
                  {user?.email}
                </div>
              </div>
              <button
                type="button"
                className="btn btn-ghost btn-sm"
                style={{ flexShrink: 0 }}
                onClick={openEdit}
              >
                {t("membersTab.editProfile")}
              </button>
            </div>
          )}
        </section>
      )}

      <MembersManagementSection />
    </>
  );
}
