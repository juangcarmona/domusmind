import { useTranslation } from "react-i18next";
import { useAuth } from "../../../auth/AuthProvider";
import { useAppSelector } from "../../../store/hooks";
import { MembersManagementSection } from "./MembersManagementSection";

export function MembersSettingsSection() {
  const { t } = useTranslation("settings");
  const { user } = useAuth();
  const members = useAppSelector((s) => s.household.members);

  const me = members.find((m) => m.authUserId === user?.userId);

  return (
    <>
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
            <div>
              <div style={{ fontWeight: 700, fontSize: "1.05rem", display: "flex", alignItems: "center", gap: "0.5rem" }}>
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
                    {t("household.members.managerBadge")}
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
          </div>
        </section>
      )}

      <MembersManagementSection />
    </>
  );
}
