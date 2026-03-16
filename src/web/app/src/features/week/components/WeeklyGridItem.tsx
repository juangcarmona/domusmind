import type { WeeklyGridEventItem, WeeklyGridTaskItem } from "../types";

type ItemType = "event" | "task" | "routine";

interface WeeklyGridItemProps {
  type: ItemType;
  title: string;
  time?: string;
  status?: string;
}

function typeLabel(type: ItemType): string {
  switch (type) {
    case "event": return "E";
    case "task": return "T";
    case "routine": return "R";
  }
}

export function WeeklyGridItem({ type, title, time, status }: WeeklyGridItemProps) {
  return (
    <div className={`wg-item wg-item--${type}`} title={`${title}${status ? ` (${status})` : ""}`}>
      <span className="wg-item-type">{typeLabel(type)}</span>
      <span className="wg-item-title">{title}</span>
      {time && <span className="wg-item-time">{time}</span>}
    </div>
  );
}

export function eventToItem(e: WeeklyGridEventItem) {
  const time = new Date(e.startTime).toLocaleTimeString(undefined, {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false,
  });
  return (
    <WeeklyGridItem
      key={e.eventId}
      type="event"
      title={e.title}
      time={time}
      status={e.status}
    />
  );
}

export function taskToItem(t: WeeklyGridTaskItem) {
  return (
    <WeeklyGridItem
      key={t.taskId}
      type="task"
      title={t.title}
      status={t.status}
    />
  );
}
