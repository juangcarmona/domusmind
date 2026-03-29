import { useTranslation } from "react-i18next";
import { HouseholdLogo } from "../../../components/HouseholdLogo";
import { useAuth } from "../../../auth/AuthProvider";

export function CloudHostedNoAccessPage() {
  const { t } = useTranslation("onboarding");
  const { logout } = useAuth();

  return (
    <div className="auth-wrap">
      <div className="auth-card">
        <div style={{ textAlign: "center", marginBottom: "1.25rem", color: "var(--primary)" }}>
          <HouseholdLogo size={40} />
        </div>
        <h1 style={{ textAlign: "center", marginBottom: "0.5rem" }}>
          {t("noAccess.title")}
        </h1>
        <p style={{ textAlign: "center", color: "var(--muted, #888)", marginBottom: "1rem", fontSize: "0.9rem" }}>
          {t("noAccess.subtitle")}
        </p>
        <p style={{ textAlign: "center", color: "var(--muted, #888)", marginBottom: "1.5rem", fontSize: "0.9rem" }}>
          {t("noAccess.instruction")}
        </p>
        <button
          type="button"
          className="btn btn-secondary"
          style={{ width: "100%", justifyContent: "center" }}
          onClick={() => void logout()}
        >
          {t("noAccess.signOut")}
        </button>
      </div>
    </div>
  );
}
