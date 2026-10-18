import { useCallback, useEffect, useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import { calendarSubscriptionApi, type CalendarSubscriptionSummary, type CreatedCalendarSubscription } from "../../../api/calendarSubscriptionApi";
import { useAppSelector } from "../../../store/hooks";

export function CalendarSubscriptionsSection() {
  const { t } = useTranslation("settings");
  const family = useAppSelector((state) => state.household.family);
  const members = useAppSelector((state) => state.household.members);
  const [subscriptions, setSubscriptions] = useState<CalendarSubscriptionSummary[]>([]);
  const [label, setLabel] = useState("");
  const [memberId, setMemberId] = useState("");
  const [created, setCreated] = useState<CreatedCalendarSubscription | null>(null);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [now, setNow] = useState(() => Date.now());
  const tr = useCallback((key: string, fallback: string) => t(`calendarSubscriptions.${key}` as never, fallback), [t]);

  useEffect(() => {
    const timer = window.setInterval(() => setNow(Date.now()), 60_000);
    return () => window.clearInterval(timer);
  }, []);

  const refresh = useCallback(async () => {
    if (!family) return;
    try {
      setSubscriptions(await calendarSubscriptionApi.list(family.familyId));
      setError("");
    } catch {
      setError(tr("loadError", "Could not load calendar subscriptions."));
    }
  }, [family, tr]);

  useEffect(() => {
    // Loading remote data synchronizes component state with the API.
    void refresh();
    // eslint-disable-next-line react-hooks/set-state-in-effect
  }, [refresh]);

  const create = async (event: FormEvent) => {
    event.preventDefault();
    if (!family || !label.trim()) return;
    setBusy(true);
    setError("");
    try {
      const value = await calendarSubscriptionApi.create(family.familyId, label.trim(), memberId || null);
      setCreated(value);
      setLabel("");
      await refresh();
    } catch {
      setError(tr("createError", "Could not create the subscription."));
    } finally {
      setBusy(false);
    }
  };

  const revoke = async (id: string) => {
    setBusy(true);
    try {
      await calendarSubscriptionApi.revoke(id);
      await refresh();
    } catch {
      setError(tr("revokeError", "Could not revoke the subscription."));
    } finally {
      setBusy(false);
    }
  };

  if (!family) return null;

  return (
    <section className="settings-section" aria-labelledby="calendar-subscriptions-title">
      <h2 id="calendar-subscriptions-title" className="settings-section-title">
        {tr("title", "Share DomusMind Plans")}
      </h2>
      <p className="settings-description">
        {tr("description", "Create a read-only calendar feed. It contains Plan titles and timing only and expires after 12 months.")}
      </p>
      <form onSubmit={create} className="calendar-subscription-form">
        <label className="settings-field">
          <span>{tr("label", "Subscription name")}</span>
          <input value={label} maxLength={100} required onChange={(event) => setLabel(event.target.value)} />
        </label>
        <label className="settings-field">
          <span>{tr("scope", "Plans to share")}</span>
          <select value={memberId} onChange={(event) => setMemberId(event.target.value)}>
            <option value="">{tr("householdScope", "Household plans")}</option>
            {members.map((member) => (
              <option key={member.memberId} value={member.memberId}>{member.preferredName || member.name}</option>
            ))}
          </select>
        </label>
        <button className="btn btn-primary" type="submit" disabled={busy || !label.trim()}>
          {busy ? tr("creating", "Creating…") : tr("create", "Create subscription")}
        </button>
      </form>
      {error && <p role="alert" className="calendar-subscription-error">{error}</p>}
      {created && (
        <div className="settings-field-group calendar-subscription-url" role="status">
          <p>{tr("createdOnce", "Copy this private URL now. It will not be shown again.")}</p>
          <input aria-label={tr("privateUrl", "Private calendar URL")} readOnly value={created.url} onFocus={(e) => e.currentTarget.select()} />
          <button className="btn btn-secondary btn-sm" type="button" onClick={() => void navigator.clipboard.writeText(created.url)}>
            {tr("copy", "Copy URL")}
          </button>
          <button className="btn btn-secondary btn-sm" type="button" onClick={() => setCreated(null)}>
            {tr("hideUrl", "Hide URL")}
          </button>
        </div>
      )}
      <div className="settings-list">
        {subscriptions.map((subscription) => {
          const active = !subscription.revokedAtUtc && Date.parse(subscription.expiresAtUtc) > now;
          const member = members.find((item) => item.memberId === subscription.memberId);
          return (
            <article className="calendar-subscription-row" key={subscription.id}>
              <div>
                <strong>{subscription.label}</strong>
                <p className="settings-description">
                  {member ? (member.preferredName || member.name) : tr("householdScope", "Household plans")}
                  {" · "}{tr("expires", "Expires")}: {new Date(subscription.expiresAtUtc).toLocaleDateString()}
                  {" · "}{tr("lastAccess", "Last access")}: {subscription.lastAccessedAtUtc ? new Date(subscription.lastAccessedAtUtc).toLocaleString() : tr("never", "Never")}
                  {" · "}{tr("accessCount", "Reads")}: {subscription.accessCount}
                </p>
              </div>
              {active ? <button className="btn btn-danger btn-sm" disabled={busy} onClick={() => void revoke(subscription.id)} type="button">
                {tr("revoke", "Revoke")}
              </button> : <span>{subscription.revokedAtUtc ? tr("revoked", "Revoked") : tr("expired", "Expired")}</span>}
            </article>
          );
        })}
      </div>
    </section>
  );
}
