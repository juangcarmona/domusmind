import type {
  WeeklyGridEventItem,
  WeeklyGridTaskItem,
  WeeklyGridRoutineItem,
} from "../../types";
import { WeeklyGridItem } from "./WeeklyGridItem";

function eventToItem(e: WeeklyGridEventItem, onClick?: () => void, compact?: boolean) {
  const time = e.time ?? undefined; // already HH:mm or null
  const participantNames = e.participants?.map((p) => p.displayName).join(", ");
  return (
    <WeeklyGridItem
      key={e.eventId}
      type="event"
      title={e.title}
      time={time}
      status={e.status}
      color={e.color}
      subtitle={participantNames || undefined}
      compact={compact}
      onClick={onClick}
    />
  );
}

function taskToItem(t: WeeklyGridTaskItem, onClick?: () => void, compact?: boolean) {
  return (
    <WeeklyGridItem
      key={t.taskId}
      type="task"
      title={t.title}
      status={t.status}
      color={t.color}
      compact={compact}
      onClick={onClick}
    />
  );
}

function routineToItem(r: WeeklyGridRoutineItem, onClick?: () => void, compact?: boolean) {
  return (
    <WeeklyGridItem
      key={`routine-${r.routineId}`}
      type="routine"
      title={r.name}
      time={r.time ?? undefined}
      status={r.kind}
      color={r.color}
      compact={compact}
      onClick={onClick}
    />
  );
}

export const weeklyGridItemMappers = {
  eventToItem,
  taskToItem,
  routineToItem,
};
