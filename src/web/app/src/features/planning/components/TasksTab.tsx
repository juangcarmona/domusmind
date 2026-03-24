import { useTranslation } from "react-i18next";
import { useDateFormatter } from "../../../hooks/useDateFormatter";
import { EntityCard } from "../../../components/EntityCard";
import type { EnrichedTimelineEntry } from "../../../api/domusmindApi";

interface Props {
  activeTasks: EnrichedTimelineEntry[];
  tasksLoading: boolean;
  /** null = not yet loaded; load triggered by onLoadHistory */
  taskHistory: EnrichedTimelineEntry[] | null;
  taskHistoryLoading: boolean;
  memberMap: Record<string, string>;
  onEdit: (taskId: string) => void;
  onAssign: (task: EnrichedTimelineEntry) => void;
  onComplete: (taskId: string) => void;
  onCancel: (taskId: string) => void;
  onLoadHistory: () => void;
}

export function TasksTab({
  activeTasks,
  tasksLoading,
  taskHistory,
  taskHistoryLoading,
  memberMap,
  onEdit,
  onAssign,
  onComplete,
  onCancel,
  onLoadHistory,
}: Props) {
  const { t } = useTranslation("tasks");
  const { t: tCommon } = useTranslation("common");
  const { formatDate } = useDateFormatter();

  if (tasksLoading) {
    return <div className="loading-wrap">{tCommon("loading")}</div>;
  }

  return (
    <section>
      {activeTasks.length === 0 ? (
        <div className="empty-state">
          <p>{t("empty")}</p>
        </div>
      ) : (
        <div className="item-list" style={{ marginBottom: "1rem" }}>
          {activeTasks.map((task) => (
            <EntityCard
              key={task.entryId}
              title={task.title}
              subtitle={
                <>
                  {task.effectiveDate ? formatDate(task.effectiveDate) : t("noDueDate")}
                  {task.assigneeId && memberMap[task.assigneeId]
                    ? ` · ${memberMap[task.assigneeId]}`
                    : task.isUnassigned
                      ? ` · ${t("unassigned")}`
                      : ""}
                  {task.isOverdue && (
                    <span style={{ color: "var(--danger)" }}> · {t("overdue")}</span>
                  )}
                </>
              }
              accentColor={task.color}
              isOverdue={task.isOverdue}
              onClick={() => onEdit(task.entryId)}
              actions={
                <>
                  <button className="btn btn-ghost btn-sm" onClick={(e) => { e.stopPropagation(); onAssign(task); }} title={t("assignTitle")}>{t("assign")}</button>
                  <button className="btn btn-sm" onClick={(e) => { e.stopPropagation(); onComplete(task.entryId); }} title={t("markDoneTitle")}>✓ {t("done")}</button>
                  <button className="btn btn-ghost btn-sm" onClick={(e) => { e.stopPropagation(); onCancel(task.entryId); }} title={tCommon("cancel")}>✕</button>
                </>
              }
            />
          ))}
        </div>
      )}

      {/* History affordance: show trigger only until history has been loaded */}
      {taskHistory === null && (
        <div className="planning-history-trigger">
          <button
            type="button"
            className="btn btn-ghost btn-sm"
            onClick={onLoadHistory}
            disabled={taskHistoryLoading}
          >
            {taskHistoryLoading ? tCommon("loading") : t("showHistory")}
          </button>
        </div>
      )}

      {taskHistory !== null && (
        <div className="planning-history-section">
          <div className="planning-history-label">{t("history")}</div>
          {taskHistory.length === 0 ? (
            <p className="planning-history-empty">{t("noHistory")}</p>
          ) : (
            <div className="item-list">
              {taskHistory.map((task) => (
                <EntityCard
                  key={task.entryId}
                  title={task.title}
                  titleStrike={task.status === "Completed"}
                  subtitle={task.status.toLowerCase()}
                  accentColor={task.color}
                  dimmed
                  onClick={() => onEdit(task.entryId)}
                />
              ))}
            </div>
          )}
        </div>
      )}
    </section>
  );
}
