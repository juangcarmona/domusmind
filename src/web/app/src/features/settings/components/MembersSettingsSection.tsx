import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../../auth/AuthProvider";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import { updateMember, updateMemberProfile } from "../../../store/householdSlice";
import { domusmindApi, type MemberDetailResponse } from "../../../api/domusmindApi";
import { EditPersonModal, type UnifiedPersonFormValues } from "./MemberModals";
import { MembersManagementSection } from "./MembersManagementSection";
import { MemberAvatar } from "./avatar/MemberAvatar";

export function MembersSettingsSection() {
  const { t } = useTranslation("settings");
  const { user } = useAuth();
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const me = members.find((m) => m.isCurrentUser);
  const tM = (key: string) => t(`household.members.${key}` as never);

  const [isEditing, setIsEditing] = useState(false);
  const [editDetail, setEditDetail] = useState<MemberDetailResponse | null>(null);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleOpenEdit() {
    if (!me || !family) return;
    setError(null);
    // Load detail so phone/email/note are pre-populated.
    try {
      const detail = await domusmindApi.getMemberDetails(family.familyId, me.memberId);
      setEditDetail(detail);
    } catch {
      setEditDetail(null);
    }
    setIsEditing(true);
  }

  async function handleSave(values: UnifiedPersonFormValues) {
    if (!me || !family) return;
    setSaving(true);
    setError(null);
    const [r1, r2] = await Promise.all([
      dispatch(updateMember({
        familyId: family.familyId,
        memberId: me.memberId,
        name: values.name,
        role: values.role,
        birthDate: values.birthDate || null,
        isManager: values.isManager,
      })),
      dispatch(updateMemberProfile({
        familyId: family.familyId,
        memberId: me.memberId,
        preferredName: values.preferredName || null,
        primaryPhone: values.primaryPhone || null,
        primaryEmail: values.primaryEmail || null,
        householdNote: values.householdNote || null,
        avatarIconId: values.avatarIconId,
        avatarColorId: values.avatarColorId,
      })),
    ]);
    setSaving(false);
    if (updateMember.fulfilled.match(r1) && updateMemberProfile.fulfilled.match(r2)) {
      setIsEditing(false);
      setEditDetail(null);
    } else {
      const payload = !updateMember.fulfilled.match(r1) ? r1.payload : r2.payload;
      setError((payload as string) ?? tM("updateError"));
    }
  }

  // Effective display name: preferredName if set, else name
  const displayName = me?.preferredName || me?.name;
  const avatarInitial = displayName?.[0]?.toUpperCase() ?? "?";

  return (
    <>
      {/* ── Section A: My Profile ─────────────────────────────────────────── */}
      {me && (
        <section className="settings-section">
          <h2 className="settings-section-title">{t("membersTab.myProfile")}</h2>
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
            <MemberAvatar
              initial={avatarInitial}
              avatarIconId={me.avatarIconId}
              avatarColorId={me.avatarColorId}
              size={48}
            />
            <div style={{ flex: 1, minWidth: 0 }}>
              <div style={{ fontWeight: 700, fontSize: "1.05rem", display: "flex", alignItems: "center", gap: "0.5rem", flexWrap: "wrap" }}>
                <span>{displayName}</span>
                {me.name !== displayName && (
                  <span style={{ fontSize: "0.8rem", color: "var(--muted)", fontStyle: "italic" }}>
                    ({me.name})
                  </span>
                )}
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
                {me.linkedEmail ?? user?.email ?? ""}
              </div>
            </div>
            <div style={{ flexShrink: 0 }}>
              <button
                type="button"
                className="btn btn-ghost btn-sm"
                onClick={handleOpenEdit}
              >
                {t("membersTab.editProfile")}
              </button>
            </div>
          </div>

          {isEditing && (
            <EditPersonModal
              member={{
                ...me,
                primaryPhone: editDetail?.primaryPhone ?? null,
                primaryEmail: editDetail?.primaryEmail ?? null,
                householdNote: editDetail?.householdNote ?? null,
              }}
              saving={saving}
              error={error}
              onSave={handleSave}
              onClose={() => { setIsEditing(false); setEditDetail(null); setError(null); }}
            />
          )}
        </section>
      )}

      {/* ── Section B: Household Members ─────────────────────────────────── */}
      <MembersManagementSection />
    </>
  );
}
