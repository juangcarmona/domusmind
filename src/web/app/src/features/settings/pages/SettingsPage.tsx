import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import { fetchSupportedLanguages } from "../../../store/languagesSlice";
import { AccountSettingsSection } from "../components/AccountSettingsSection";
import { HouseholdSettingsSection } from "../components/HouseholdSettingsSection";

export function SettingsPage() {
  const { t } = useTranslation();
  const dispatch = useAppDispatch();
  const languagesStatus = useAppSelector((s) => s.languages.status);

  useEffect(() => {
    if (languagesStatus === "idle") {
      dispatch(fetchSupportedLanguages());
    }
  }, [dispatch, languagesStatus]);

  return (
    <div className="page-wrap">
      <h1 className="page-title">{t("settings.title")}</h1>
      <div className="settings-layout">
        <AccountSettingsSection />
        <HouseholdSettingsSection />
      </div>
    </div>
  );
}
