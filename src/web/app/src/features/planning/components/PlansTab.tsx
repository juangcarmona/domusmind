import { useTranslation } from "react-i18next";
import { useDateFormatter } from "../../../hooks/useDateFormatter";
import type { FamilyTimelineEventItem } from "../../../api/domusmindApi";

interface Props {
  activePlans: FamilyTimelineEventItem[];
  plansStatus: string;
  onEdit: (eventId: string) => void;
  onCancelPlan: (plan: FamilyTimelineEventItem) => void;
}

export function PlansTab({ activePlans, plansStatus, onEdit, onCancelPlan }: Props) {
  const { t } = useTranslation("plans");
  const { t: tCommon } = useTranslation("common");
  const { formatDateTime } = useDateFormatter();

  if (plansStatus === "loading") {
    return <div className="loading-wrap">{tCommon("loading")}</div>;
  }

  if (plansStatus === "success" && activePlans.length === 0) {
    return (
      <div className="empty-state">
        <p>{t("noPlans")}</p>
      </div>
    );
  }

  return (
    <div className="item-list">
      {activePlans.map((plan) => (
        <div
          key={plan.calendarEventId}
          className="item-card"
          style={{ borderLeft: `3px solid ${plan.color}` }}
          onClick={() => onEdit(plan.calendarEventId)}
          role="button"
          tabIndex={0}
          onKeyDown={(e) => {
            if (e.key === "Enter" || e.key === " ") {
              e.preventDefault();
              onEdit(plan.calendarEventId);
            }
          }}
        >
          <div className="item-card-body">
            <div className="item-card-title">{plan.title}</div>
            <div className="item-card-subtitle">
              {formatDateTime(plan.startTime)}
              {plan.endTime && ` → ${formatDateTime(plan.endTime)}`}
              {plan.participants?.length > 0 && (
                <span> · {plan.participants.map((p) => p.displayName).join(", ")}</span>
              )}
            </div>
            <div className="item-card-subtitle" style={{ marginTop: "0.2rem" }}>
              <span className={`entry-status-badge ${plan.status.toLowerCase()}`}>
                {plan.status.toLowerCase()}
              </span>
            </div>
          </div>
          <div className="item-card-actions">
            {plan.status !== "Cancelled" && (
              <button
                className="btn btn-ghost btn-sm"
                onClick={(e) => { e.stopPropagation(); onCancelPlan(plan); }}
              >
                {t("cancelEvent")}
              </button>
            )}
          </div>
        </div>
      ))}
    </div>
  );
}
