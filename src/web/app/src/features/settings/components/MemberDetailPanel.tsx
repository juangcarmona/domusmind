import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import type { FamilyMemberResponse } from "../../../api/domusmindApi";
import type { MemberDetailResponse } from "../../../api/domusmindApi";
import { MemberAvatar } from "./avatar/MemberAvatar";
import { AccessStatusBadge } from "./AccessStatusBadge";
import { EditPersonModal, type UnifiedPersonFormValues } from "./MemberModals";
import { TabBtn } from "./TabBtn";

type DetailTab = "core" | "access" | "contacts" | "notes";

export interface MemberDetailPanelProps {
  member: FamilyMemberResponse;
  detail: MemberDetailResponse | null;
  loadingDetail: boolean;
  isCurrentUserManager: boolean;
  onClose: () => void;
  onGrantAccess: (id: string) => void;
  onRegenPassword: (id: string) => void;
  onDisable: (id: string) => void;
  onEnable: (id: string) => void;
  onSave: (values: UnifiedPersonFormValues) => void;
  saving: boolean;
  error: string | null;
}

export function MemberDetailPanel({
  member,
  detail,
  loadingDetail,
  isCurrentUserManager,
  onClose,
  onGrantAccess,
  onRegenPassword,
  onDisable,
  onEnable,
  onSave,
  saving,
  error,
}: MemberDetailPanelProps) {
  const { t } = useTranslation("settings");
  const tM = (key: string) => t(`household.members.${key}` as never);

  const [activeTab, setActiveTab] = useState<DetailTab>("core");
  const [isEditing, setIsEditing] = useState(false);
  const [pendingSave, setPendingSave] = useState(false);
  const displayName = member.preferredName || member.name;

  // Close the edit modal once the async save resolves without error.
  useEffect(() => {
    if (pendingSave && !saving && !error) {
      setIsEditing(false);
      setPendingSave(false);
    }
  }, [saving, error, pendingSave]);

  return (
    <div style={{ position: "fixed", inset: 0, zIndex: 200, display: "flex", justifyContent: "flex-end" }}>
      <div style={{ position: "absolute", inset: 0, background: "rgba(0,0,0,0.35)" }} onClick={onClose} />
      <div
        style={{
          position: "relative",
          width: "min(420px, 95vw)",
          height: "100%",
          background: "var(--surface, #fff)",
          boxShadow: "-4px 0 24px rgba(0,0,0,0.18)",
          display: "flex",
          flexDirection: "column",
          overflow: "hidden",
        }}
        onClick={(e) => e.stopPropagation()}
      >
        {/* Header */}
        <div style={{ display: "flex", alignItems: "center", gap: "0.75rem", padding: "1rem 1rem 0.75rem", borderBottom: "1px solid var(--border, #eee)" }}>
          <MemberAvatar initial={member.avatarInitial} avatarIconId={member.avatarIconId} avatarColorId={member.avatarColorId} size={44} />
          <div style={{ flex: 1, minWidth: 0 }}>
            <div style={{ fontWeight: 700, fontSize: "1rem", display: "flex", alignItems: "center", gap: "0.4rem", flexWrap: "wrap" }}>
              <span>{displayName}</span>
              {member.isManager && (
                <span style={{ fontSize: "0.68rem", padding: "0.1rem 0.4rem", borderRadius: 4, background: "color-mix(in srgb, var(--primary) 20%, transparent)", color: "var(--primary)" }}>
                  {tM("managerBadge")}
                </span>
              )}
            </div>
            <div style={{ fontSize: "0.82rem", color: "var(--muted)" }}>
              {t(`household.members.roles.${member.role}` as never, member.role)}
            </div>
          </div>
          <button type="button" className="btn btn-ghost btn-sm" onClick={onClose} aria-label={tM("closePanel")}>✕</button>
        </div>

        {/* Tabs */}
        <div style={{ display: "flex", borderBottom: "1px solid var(--border, #eee)", padding: "0 0.5rem" }}>
          <TabBtn id="core" label={tM("profile")} active={activeTab} onSelect={(id) => setActiveTab(id as DetailTab)} />
          <TabBtn id="access" label={tM("access")} active={activeTab} onSelect={(id) => setActiveTab(id as DetailTab)} />
          <TabBtn id="contacts" label={tM("contacts")} active={activeTab} onSelect={(id) => setActiveTab(id as DetailTab)} />
          <TabBtn id="notes" label={tM("notes")} active={activeTab} onSelect={(id) => setActiveTab(id as DetailTab)} />
        </div>

        {/* Body */}
        <div style={{ flex: 1, overflowY: "auto", padding: "1rem" }}>
          {loadingDetail && <p style={{ color: "var(--muted)", fontSize: "0.9rem" }}>Loading…</p>}

          {!loadingDetail && activeTab === "core" && (
            <div>
              <div className="settings-field-group" style={{ marginBottom: "1rem" }}>
                <div className="settings-field">
                  <span className="settings-field-label">{tM("name")}</span>
                  <span className="settings-field-value">{member.name}</span>
                </div>
                {detail?.preferredName && (
                  <div className="settings-field">
                    <span className="settings-field-label">{tM("preferredName")}</span>
                    <span className="settings-field-value">{detail.preferredName}</span>
                  </div>
                )}
                <div className="settings-field">
                  <span className="settings-field-label">{tM("role")}</span>
                  <span className="settings-field-value">{t(`household.members.roles.${member.role}` as never, member.role)}</span>
                </div>
                {member.birthDate && (
                  <div className="settings-field">
                    <span className="settings-field-label">{tM("birthDate")}</span>
                    <span className="settings-field-value">{new Date(member.birthDate).toLocaleDateString()}</span>
                  </div>
                )}
              </div>
              {member.canEdit && (
                <button
                  type="button"
                  className="btn btn-ghost"
                  style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem" }}
                  onClick={() => setIsEditing(true)}
                >
                  {tM("editProfile")}
                </button>
              )}
            </div>
          )}

          {!loadingDetail && activeTab === "access" && (
            <div>
              <div className="settings-field-group" style={{ marginBottom: "1rem" }}>
                <div className="settings-field">
                  <span className="settings-field-label">{tM("access")}</span>
                  <AccessStatusBadge status={member.accessStatus} />
                </div>
                {member.linkedEmail && (
                  <div className="settings-field">
                    <span className="settings-field-label">{tM("email")}</span>
                    <span className="settings-field-value">{member.linkedEmail}</span>
                  </div>
                )}
                {detail?.lastLoginAtUtc && (
                  <div className="settings-field">
                    <span className="settings-field-label">{tM("lastLogin")}</span>
                    <span className="settings-field-value">{new Date(detail.lastLoginAtUtc).toLocaleString()}</span>
                  </div>
                )}
                {member.hasAccount && !detail?.lastLoginAtUtc && (
                  <div className="settings-field">
                    <span className="settings-field-label">{tM("lastLogin")}</span>
                    <span className="settings-field-value" style={{ color: "var(--muted)" }}>{tM("neverLoggedIn")}</span>
                  </div>
                )}
              </div>
              <div style={{ display: "flex", gap: "0.4rem", flexWrap: "wrap" }}>
                {member.canGrantAccess && (
                  <button
                    type="button"
                    className="btn btn-ghost"
                    style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem" }}
                    onClick={() => onGrantAccess(member.memberId)}
                  >
                    {tM("provisionAccess")}
                  </button>
                )}
                {isCurrentUserManager && member.hasAccount && member.accessStatus !== "Disabled" && (
                  <>
                    <button
                      type="button"
                      className="btn btn-ghost"
                      style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem" }}
                      onClick={() => onRegenPassword(member.memberId)}
                    >
                      {tM("regeneratePassword")}
                    </button>
                    <button
                      type="button"
                      className="btn btn-ghost"
                      style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem", color: "#ef4444" }}
                      onClick={() => onDisable(member.memberId)}
                    >
                      {tM("disableAccess")}
                    </button>
                  </>
                )}
                {isCurrentUserManager && member.hasAccount && member.accessStatus === "Disabled" && (
                  <button
                    type="button"
                    className="btn btn-ghost"
                    style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem", color: "#22c55e" }}
                    onClick={() => onEnable(member.memberId)}
                  >
                    {tM("enableAccess")}
                  </button>
                )}
              </div>
            </div>
          )}

          {!loadingDetail && activeTab === "contacts" && (
            <div>
              <div className="settings-field-group" style={{ marginBottom: "1rem" }}>
                <div className="settings-field">
                  <span className="settings-field-label">{tM("primaryPhone")}</span>
                  <span className="settings-field-value">{detail?.primaryPhone || <span style={{ color: "var(--muted)" }}>—</span>}</span>
                </div>
                <div className="settings-field">
                  <span className="settings-field-label">{tM("primaryEmail")}</span>
                  <span className="settings-field-value">{detail?.primaryEmail || <span style={{ color: "var(--muted)" }}>—</span>}</span>
                </div>
              </div>
              {member.canEdit && (
                <button
                  type="button"
                  className="btn btn-ghost"
                  style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem" }}
                  onClick={() => setIsEditing(true)}
                >
                  {tM("editProfile")}
                </button>
              )}
            </div>
          )}

          {!loadingDetail && activeTab === "notes" && (
            <div>
              <div
                style={{
                  background: "color-mix(in srgb, var(--primary) 5%, transparent)",
                  borderRadius: 8,
                  padding: "0.75rem",
                  fontSize: "0.88rem",
                  minHeight: 72,
                  marginBottom: "0.75rem",
                  color: detail?.householdNote ? "inherit" : "var(--muted)",
                }}
              >
                {detail?.householdNote || tM("householdNotePlaceholder")}
              </div>
              {(member.canEdit || isCurrentUserManager) && (
                <button
                  type="button"
                  className="btn btn-ghost"
                  style={{ fontSize: "0.82rem", padding: "0.3rem 0.75rem" }}
                  onClick={() => setIsEditing(true)}
                >
                  {tM("editProfile")}
                </button>
              )}
            </div>
          )}
        </div>
      </div>

      {isEditing && (
        <EditPersonModal
          member={{
            ...member,
            primaryPhone: detail?.primaryPhone ?? null,
            primaryEmail: detail?.primaryEmail ?? null,
            householdNote: detail?.householdNote ?? null,
          }}
          saving={saving}
          error={error}
          onSave={(values) => {
            setPendingSave(true);
            onSave(values);
          }}
          onClose={() => setIsEditing(false)}
        />
      )}
    </div>
  );
}
