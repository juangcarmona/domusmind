import { useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import { AvatarPicker } from "../avatar/AvatarPicker";
import { MEMBER_ROLES, type UnifiedPersonFormValues } from "./memberModalTypes";

interface EditPersonModalProps {
  member: {
    name: string;
    role: string;
    birthDate?: string | null;
    isManager: boolean;
    avatarIconId?: number | null;
    avatarColorId?: number | null;
    avatarInitial?: string;
    preferredName?: string | null;
    primaryPhone?: string | null;
    primaryEmail?: string | null;
    householdNote?: string | null;
  };
  saving: boolean;
  error: string | null;
  onSave: (values: UnifiedPersonFormValues) => void;
  onClose: () => void;
}

export function EditPersonModal({ member, saving, error, onSave, onClose }: EditPersonModalProps) {
  const { t } = useTranslation("settings");
  const tM = (key: string) => t(`household.members.${key}` as never);

  const [name, setName] = useState(member.name);
  const [role, setRole] = useState(member.role);
  const [birthDate, setBirthDate] = useState(member.birthDate ?? "");
  const [isManager, setIsManager] = useState(member.isManager);
  const [preferredName, setPreferredName] = useState(member.preferredName ?? "");
  const [primaryPhone, setPrimaryPhone] = useState(member.primaryPhone ?? "");
  const [primaryEmail, setPrimaryEmail] = useState(member.primaryEmail ?? "");
  const [householdNote, setHouseholdNote] = useState(member.householdNote ?? "");
  const [avatarIconId, setAvatarIconId] = useState<number | null>(member.avatarIconId ?? null);
  const [avatarColorId, setAvatarColorId] = useState<number | null>(member.avatarColorId ?? null);

  const avatarInitial = ((preferredName || name)?.[0] ?? "?").toUpperCase();

  function handleSubmit(e: FormEvent) {
    e.preventDefault();
    onSave({
      name: name.trim(),
      role,
      birthDate: birthDate || "",
      isManager: isManager && role === "Adult",
      preferredName: preferredName.trim(),
      primaryPhone: primaryPhone.trim(),
      primaryEmail: primaryEmail.trim(),
      householdNote: householdNote.trim(),
      avatarIconId,
      avatarColorId,
    });
  }

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div
        className="modal"
        style={{ overflowY: "auto", maxHeight: "90vh" }}
        onClick={(e) => e.stopPropagation()}
      >
        <h2 style={{ marginBottom: "1rem" }}>{tM("editProfileTitle")}</h2>
        <form onSubmit={handleSubmit}>
          {/* ── Avatar ── */}
          <div className="form-group">
            <label className="avatar-picker-label">Avatar</label>
            <AvatarPicker
              initial={avatarInitial}
              iconId={avatarIconId}
              colorId={avatarColorId}
              onIconChange={setAvatarIconId}
              onColorChange={setAvatarColorId}
            />
          </div>

          {/* ── Full name ── */}
          <div className="form-group">
            <label htmlFor="ep-name">{tM("name")}</label>
            <input
              id="ep-name"
              className="form-control"
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              autoFocus
            />
          </div>

          {/* ── Preferred / display name ── */}
          <div className="form-group">
            <label htmlFor="ep-preferred">{tM("preferredName")}</label>
            <input
              id="ep-preferred"
              className="form-control"
              type="text"
              value={preferredName}
              onChange={(e) => setPreferredName(e.target.value)}
              placeholder={tM("preferredNamePlaceholder")}
            />
          </div>

          {/* ── Role + birth date ── */}
          <div className="inline-form" style={{ marginBottom: "0.75rem" }}>
            <div className="form-group" style={{ flex: 1 }}>
              <label htmlFor="ep-role">{tM("role")}</label>
              <select
                id="ep-role"
                className="form-control"
                value={role}
                onChange={(e) => {
                  setRole(e.target.value);
                  if (e.target.value !== "Adult") setIsManager(false);
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
              <label htmlFor="ep-birth">{tM("birthDate")}</label>
              <input
                id="ep-birth"
                className="form-control"
                type="date"
                value={birthDate}
                onChange={(e) => setBirthDate(e.target.value)}
              />
            </div>
          </div>

          {/* ── Manager flag (Adults only) ── */}
          {role === "Adult" && (
            <div className="form-group">
              <label style={{ display: "flex", alignItems: "center", gap: "0.5rem", cursor: "pointer" }}>
                <input
                  type="checkbox"
                  checked={isManager}
                  onChange={(e) => setIsManager(e.target.checked)}
                />
                {tM("isManager")}
              </label>
            </div>
          )}

          {/* ── Contact info ── */}
          <div className="form-group">
            <label htmlFor="ep-phone">{tM("primaryPhone")}</label>
            <input
              id="ep-phone"
              className="form-control"
              type="tel"
              value={primaryPhone}
              onChange={(e) => setPrimaryPhone(e.target.value)}
              autoComplete="off"
            />
          </div>
          <div className="form-group">
            <label htmlFor="ep-email">{tM("primaryEmail")}</label>
            <input
              id="ep-email"
              className="form-control"
              type="email"
              value={primaryEmail}
              onChange={(e) => setPrimaryEmail(e.target.value)}
              autoComplete="off"
            />
          </div>

          {/* ── Household note ── */}
          <div className="form-group">
            <label htmlFor="ep-note">{tM("householdNote")}</label>
            <textarea
              id="ep-note"
              className="form-control"
              value={householdNote}
              onChange={(e) => setHouseholdNote(e.target.value)}
              placeholder={tM("householdNotePlaceholder")}
              rows={3}
              style={{ resize: "vertical" }}
            />
          </div>

          {error && <p className="error-msg">{error}</p>}
          <div className="modal-footer">
            <button type="button" className="btn btn-ghost" onClick={onClose}>
              {tM("cancel")}
            </button>
            <button type="submit" className="btn" disabled={saving}>
              {saving ? tM("saving") : tM("save")}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
