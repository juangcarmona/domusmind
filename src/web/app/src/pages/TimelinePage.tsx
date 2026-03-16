import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { fetchTimeline } from "../store/timelineSlice";
import { completeTask, cancelTask } from "../store/tasksSlice";
import type { EnrichedTimelineEntry } from "../api/domusmindApi";

const GROUP_LABELS: Record<string, string> = {
  Overdue: "Overdue",
  Today: "Today",
  Tomorrow: "Tomorrow",
  ThisWeek: "This week",
  Later: "Later",
  Undated: "Routines & ongoing",
};

function formatDate(iso: string | null): string {
  if (!iso) return "";
  const d = new Date(iso);
  return d.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
}

function entryTypeDot(type: string) {
  if (type === "CalendarEvent") return "event";
  if (type === "Task") return "task";
  return "routine";
}

function entryTypeLabel(type: string) {
  if (type === "CalendarEvent") return "Plan";
  if (type === "Task") return "Chore";
  return "Routine";
}

function StatusBadge({ status }: { status: string }) {
  const cls = status.toLowerCase();
  return (
    <span className={`entry-status-badge ${cls}`}>
      {status === "Pending" ? "pending" : status.toLowerCase()}
    </span>
  );
}

function TimelineEntry({
  entry,
  memberName,
  onComplete,
  onCancel,
}: {
  entry: EnrichedTimelineEntry;
  memberName: string | null;
  onComplete: (id: string) => void;
  onCancel: (id: string) => void;
}) {
  const isTask = entry.entryType === "Task";
  const isDone =
    entry.status === "Completed" || entry.status === "Cancelled";

  return (
    <div
      className={`entry-card ${entry.isOverdue ? "overdue" : entry.priority === "High" ? "high-priority" : ""}`}
    >
      <div className={`entry-type-dot ${entryTypeDot(entry.entryType)}`} />
      <div className="entry-body">
        <div className="entry-title">{entry.title}</div>
        <div className="entry-meta">
          <span>{entryTypeLabel(entry.entryType)}</span>
          {entry.effectiveDate && (
            <span>
              {" "}
              · {formatDate(entry.effectiveDate)}
            </span>
          )}
          {memberName && (
            <span>
              {" "}
              · {memberName}
            </span>
          )}
          {entry.isUnassigned && isTask && (
            <span style={{ color: "var(--accent)" }}> · unassigned</span>
          )}
        </div>
      </div>
      <div className="entry-actions">
        <StatusBadge status={entry.status} />
        {isTask && !isDone && (
          <>
            <button
              className="btn btn-sm"
              onClick={() => onComplete(entry.entryId)}
              title="Mark done"
              style={{ padding: "0.15rem 0.5rem" }}
            >
              ✓
            </button>
            <button
              className="btn btn-ghost btn-sm"
              onClick={() => onCancel(entry.entryId)}
              title="Cancel"
              style={{ padding: "0.15rem 0.45rem" }}
            >
              ✕
            </button>
          </>
        )}
      </div>
    </div>
  );
}

export function TimelinePage() {
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const { data, status, error } = useAppSelector((s) => s.timeline);
  const [filterType, setFilterType] = useState<string>("");

  const familyId = family?.familyId;

  const memberMap = Object.fromEntries(
    members.map((m) => [m.memberId, m.name]),
  );

  function load() {
    if (!familyId) return;
    dispatch(
      fetchTimeline({
        familyId,
        types: filterType || undefined,
      }),
    );
  }

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [familyId, filterType]);

  async function handleComplete(taskId: string) {
    await dispatch(completeTask(taskId));
    load();
  }

  async function handleCancel(taskId: string) {
    await dispatch(cancelTask(taskId));
    load();
  }

  if (!familyId) return null;

  return (
    <div className="timeline-page">
      <div className="page-header">
        <h1>
          {data
            ? (() => {
                const today = data.groups.find((g) => g.groupKey === "Today");
                return today && today.entries.length > 0
                  ? `Today — ${today.entries.length} item${today.entries.length > 1 ? "s" : ""}`
                  : "Timeline";
              })()
            : "Timeline"}
        </h1>
        <div style={{ display: "flex", gap: "0.5rem", alignItems: "center" }}>
          <select
            className="form-control"
            style={{ width: "auto" }}
            value={filterType}
            onChange={(e) => setFilterType(e.target.value)}
          >
            <option value="">All</option>
            <option value="CalendarEvent">Plans</option>
            <option value="Task">Chores</option>
            <option value="Routine">Routines</option>
          </select>
          <button className="btn btn-ghost btn-sm" onClick={load}>
            ↻
          </button>
        </div>
      </div>

      {status === "loading" && (
        <div className="loading-wrap">Loading timeline…</div>
      )}

      {status === "error" && (
        <div className="card">
          <p className="error-msg">{error}</p>
          <button className="btn btn-ghost btn-sm" onClick={load}>
            Retry
          </button>
        </div>
      )}

      {status === "success" && data && (
        <>
          {data.totalEntries === 0 && (
            <div className="empty-state">
              <p>Nothing here yet.</p>
              <p>
                Add a plan, a chore, or a routine — they will all show up here.
              </p>
            </div>
          )}

          {data.groups.map((group) => (
            <section key={group.groupKey} className="timeline-section">
              <div className="timeline-section-header">
                <h2>{GROUP_LABELS[group.groupKey] ?? group.groupKey}</h2>
                <span
                  className={`timeline-badge ${group.groupKey === "Overdue" ? "overdue" : ""}`}
                >
                  {group.entries.length}
                </span>
              </div>
              <div className="entry-list">
                {group.entries.map((entry) => (
                  <TimelineEntry
                    key={entry.entryId}
                    entry={entry}
                    memberName={
                      entry.assigneeId
                        ? (memberMap[entry.assigneeId] ?? null)
                        : null
                    }
                    onComplete={handleComplete}
                    onCancel={handleCancel}
                  />
                ))}
              </div>
            </section>
          ))}
        </>
      )}
    </div>
  );
}
