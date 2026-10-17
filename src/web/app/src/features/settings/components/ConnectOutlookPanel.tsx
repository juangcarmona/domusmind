import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useAppDispatch } from "../../../store/hooks";
import { connectOutlook } from "../../../store/externalCalendarSlice";
import { externalCalendarApi } from "../../../api/externalCalendarApi";
import type { ApiError } from "../../../api/request";

interface Props {
  familyId: string;
  memberId: string;
  onConnected: () => void;
  onCancel: () => void;
}

export const OUTLOOK_CALLBACK_PATH = "/settings/oauth/outlook/callback";

export function ConnectOutlookPanel({ familyId, memberId, onConnected, onCancel }: Props) {
  const { t } = useTranslation("settings");
  const { t: tCommon } = useTranslation("common");
  const dispatch = useAppDispatch();

  const [label, setLabel] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Complete only a callback initiated in this browser tab. The server binds state
  // to the authenticated user, family, member, redirect URI and PKCE verifier.
  useEffect(() => {
    const url = new URL(window.location.href);
    const code = url.searchParams.get("code");
    const state = url.searchParams.get("state");
    const providerError = url.searchParams.get("error");

    if (window.location.pathname !== OUTLOOK_CALLBACK_PATH) return;

    window.history.replaceState({}, "", window.location.pathname);

    if (providerError) {
      if (state) sessionStorage.removeItem(`outlook-oauth:${state}`);
      setError(tCommon("failed"));
      return;
    }

    if (!code || !state) {
      setError(tCommon("failed"));
      return;
    }

    const transactionKey = `outlook-oauth:${state}`;
    const stored = sessionStorage.getItem(transactionKey);
    sessionStorage.removeItem(transactionKey);

    if (!stored) {
      setError(tCommon("failed"));
      return;
    }

    let context: { familyId: string; memberId: string; redirectUri: string };
    try {
      context = JSON.parse(stored) as typeof context;
    } catch {
      setError(tCommon("failed"));
      return;
    }

    setLoading(true);
    dispatch(
      connectOutlook({
        familyId: context.familyId,
        memberId: context.memberId,
        body: { authorizationCode: code, state, redirectUri: context.redirectUri },
      }),
    )
      .unwrap()
      .then(() => onConnected())
      .catch((err: ApiError) => setError(err.message ?? tCommon("failed")))
      .finally(() => setLoading(false));
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  async function handleConnect() {
    setLoading(true);
    setError(null);
    try {
      const redirectUri = `${window.location.origin}${OUTLOOK_CALLBACK_PATH}`;
      const { authUrl } = await externalCalendarApi.getOutlookAuthUrl(familyId, memberId, redirectUri);
      const state = new URL(authUrl).searchParams.get("state");
      if (!state) throw new Error("Microsoft authorization URL did not include state.");

      sessionStorage.setItem(
        `outlook-oauth:${state}`,
        JSON.stringify({ familyId, memberId, redirectUri }),
      );
      // Redirect to Microsoft OAuth
      window.location.href = authUrl;
    } catch (err) {
      setError((err as ApiError).message ?? tCommon("failed"));
      setLoading(false);
    }
  }

  return (
    <div className="connect-outlook-panel">
      <h4 className="connect-outlook-title">{t("connectedCalendars.connectOutlookTitle")}</h4>
      <p className="connect-outlook-description">{t("connectedCalendars.connectOutlookDescription")}</p>

      {error && <p className="form-error">{error}</p>}

      <div className="form-group" style={{ maxWidth: 360 }}>
        <label htmlFor="outlook-label">{t("connectedCalendars.accountLabel")}</label>
        <input
          id="outlook-label"
          type="text"
          className="form-control"
          placeholder={t("connectedCalendars.accountLabelPlaceholder")}
          value={label}
          onChange={(e) => setLabel(e.target.value)}
          disabled={loading}
        />
      </div>

      <div className="connect-outlook-actions">
        <button
          type="button"
          className="btn btn-primary"
          onClick={handleConnect}
          disabled={loading}
        >
          {loading ? tCommon("loading") : t("connectedCalendars.authorizeWithMicrosoft")}
        </button>
        <button
          type="button"
          className="btn btn-secondary"
          onClick={onCancel}
          disabled={loading}
        >
          {tCommon("cancel")}
        </button>
      </div>
    </div>
  );
}
