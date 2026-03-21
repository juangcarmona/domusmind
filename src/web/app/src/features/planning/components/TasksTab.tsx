import { useTranslation } from "react-i18next";
import { useDateFormatter } from "../../../hooks/useDateFormatter";
import type { EnrichedTimelineEntry } from "../../../api/domusmindApi";

const SECTION_LABEL_STYLE: React.CSSProperties = {
  marginBottom: "0.4rem",
  fontSize: "0.82rem",
  color: "var(--muted)",
  fontWeight: 600,
  textTransform: "uppercase",
  letterSpacing: "0.04em",
};

interface Props {
  activeTasks: EnrichedTimelineEntry[];
  doneTasks: EnrichedTimelineEntry[];
  timelineStatus: string;
  memberMap: Record<string, string>;
  onEdit: (taskId: string) => void;
  onAssign: (task: EnrichedTimelineEntry) => void;
  onComplete: (taskId: string) => void;
  onCancel: (taskId: string) => void;
}

export function TasksTab({
  activeTasks,
  doneTasks,
  timelineStatus,
  memberMap,
  onEdit,
  onAssign,
  onComplete,
  onCancel,
}: Props) {
  const { t } = useTranslation("tasks");
  const { t: tCommon } = useTranslation("common");
  const { formatDate } = useDateFormatter();

  if (timelineStatus === "loading") {
    return <div className="loading-wrap">{tCommon("loading")}</div>;
  }

  return (
    <section>
      {activeTasks.length === 0 ? (
        <div className="empty-state">
          <p>{t("empty")}</p>
        </div>
      ) : (
        <>
          <div style={SECTION_LABEL_STYLE}>
            {t("active")} ({activeTasks.length})
          </div>
          <div className="item-list" style={{ marginBottom: "1rem" }}>
            {activeTasks.map((task) => (
              <div
                key={task.entryId}
                className={`item-card ${task.isOverdue ? "overdue" : ""}`}
                style={{ borderLeft: `3px solid ${task.color}` }}
                onClick={() => onEdit(task.entryId)}
                role="button"
                tabIndex={0}
                onKeyDown={(e) => {
                  if (e.key === "Enter" || e.key === " ") {
                    e.preventDefault();
                    onEdit(task.entryId);
                  }
                }}
              >
                <div className="item-card-body">
                  <div className="item-card-title">{task.title}</div>
                  <div className="item-card-subtitle">
                    {task.effectiveDate ? formatDate(task.effectiveDate) : t("noDueDate")}
                    {task.assigneeId && memberMap[task.assigneeId]
                      ? ` · ${memberMap[task.assigneeId]}`
                      : task.isUnassigned
                        ? ` · ${t("unassigned")}`
                        : ""}
                    {task.isOverdue && (
                      <span style={{ color: "var(--danger)" }}> · {t("overdue")}</span>
                    )}
                  </div>
                </div>
                <div className="item-card-actions">
                  <button
                    className="btn btn-ghost btn-sm"
                    onClick={(e) => { e.stopPropagation(); onAssign(task); }}
                    title={t("assignTitle")}
                  >
                    {t("assign")}
                  </button>
                  <button
                    className="btn btn-sm"
                    onClick={(e) => { e.stopPropagation(); onComplete(task.entryId); }}
                    title={t("markDoneTitle")}
                  >
                    ✓ {t("done")}
                  </button>
                  <button
                    className="btn btn-ghost btn-sm"
                    onClick={(e) => { e.stopPropagation(); onCancel(task.entryId); }}
                    title={tCommon("cancel")}
                  >
                    ✕
                  </button>
                </div>
              </div>
            ))}
          </div>
        </>
      )}

      {doneTasks.length > 0 && (
        <>
          <div style={SECTION_LABEL_STYLE}>
            {t("completedCancelled")}
          </div>
          <div className="item-list">
            {doneTasks.slice(0, 10).map((task) => (
              <div
                key={task.entryId}
                className="item-card"
                style={{ opacity: 0.65, borderLeft: `3px solid ${task.color}` }}
                onClick={() => onEdit(task.entryId)}
                role="button"
                tabIndex={0}
                onKeyDown={(e) => {
                  if (e.key === "Enter" || e.key === " ") {
                    e.preventDefault();
                    onEdit(task.entryId);
                  }
                }}
              >
                <div className="item-card-body">
                  <div
                    className="item-card-title"
                    style={{ textDecoration: task.status === "Completed" ? "line-through" : undefined }}
                  >
                    {task.title}
                  </div>
                  <div className="item-card-subtitle">{task.status.toLowerCase()}</div>
                </div>
              </div>
            ))}
          </div>
        </>
      )}
    </section>
  );
}
