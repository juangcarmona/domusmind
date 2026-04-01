import { useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import { AvatarPicker } from "../avatar/AvatarPicker";
import type { ProfileFormValues } from "./memberModalTypes";

interface EditProfileModalProps {
  member: {
    preferredName?: string | null;
    primaryPhone?: string | null;
    primaryEmail?: string | null;
    householdNote?: string | null;
    avatarIconId?: number | null;
    avatarColorId?: number | null;
    avatarInitial?: string;
  };
  saving: boolean;
  error: string | null;
  onSave: (values: ProfileFormValues) => void;
  onClose: () => void;
}

export function EditProfileModal({ member, saving, error, onSave, onClose }: EditProfileModalProps) {
  const { t } = useTranslation("settings");
  const tM = (key: string) => t(`household.members.${key}` as never);

  const [preferredName, setPreferredName] = useState(member.preferredName ?? "");
  const [primaryPhone, setPrimaryPhone] = useState(member.primaryPhone ?? "");
  const [primaryEmail, setPrimaryEmail] = useState(member.primaryEmail ?? "");
  const [householdNote, setHouseholdNote] = useState(member.householdNote ?? "");
  const [avatarIconId, setAvatarIconId] = useState<number | null>(member.avatarIconId ?? null);
  const [avatarColorId, setAvatarColorId] = useState<number | null>(member.avatarColorId ?? null);

  const avatarInitial = (member.avatarInitial ?? preferredName?.[0] ?? "?").toUpperCase();

  function handleSubmit(e: FormEvent) {
    e.preventDefault();
    onSave({
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
      <div className="modal" style={{ overflowY: "auto", maxHeight: "90vh" }} onClick={(e) => e.stopPropagation()}>
        <h2 style={{ marginBottom: "1rem" }}>{tM("editProfileTitle")}</h2>
        <form onSubmit={handleSubmit}>
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
          <div className="form-group">
            <label>{tM("preferredName")}</label>
            <input className="form-control" type="text" value={preferredName} onChange={(e) => setPreferredName(e.target.value)} placeholder={tM("preferredNamePlaceholder")} autoFocus />
          </div>
          <div className="form-group">
            <label>{tM("primaryPhone")}</label>
            <input className="form-control" type="tel" value={primaryPhone} onChange={(e) => setPrimaryPhone(e.target.value)} autoComplete="off" />
          </div>
          <div className="form-group">
            <label>{tM("primaryEmail")}</label>
            <input className="form-control" type="email" value={primaryEmail} onChange={(e) => setPrimaryEmail(e.target.value)} autoComplete="off" />
          </div>
          <div className="form-group">
            <label>{tM("householdNote")}</label>
            <textarea className="form-control" value={householdNote} onChange={(e) => setHouseholdNote(e.target.value)} placeholder={tM("householdNotePlaceholder")} rows={3} style={{ resize: "vertical" }} />
          </div>
          {error && <p className="error-msg">{error}</p>}
          <div className="modal-footer">
            <button type="button" className="btn btn-ghost" onClick={onClose}>{tM("cancel")}</button>
            <button type="submit" className="btn" disabled={saving}>{saving ? tM("saving") : tM("save")}</button>
          </div>
        </form>
      </div>
    </div>
  );
}
