import { useTranslation } from "react-i18next";
import type { WeeklyGridResponse, WeeklyGridCell } from "../../week/types";
import { eventToItem, taskToItem, routineToItem } from "../../week/components/WeeklyGridItem";

interface DayViewProps {
  grid: WeeklyGridResponse | null;
  selectedDate: string; // ISO YYYY-MM-DD
  loading: boolean;
  error: string | null;
}

function DayMemberSection({ name, cell }: { name: string; cell: WeeklyGridCell }) {
  const { t: tWeek } = useTranslation("week");
  const events = cell.events ?? [];
  const tasks = cell.tasks ?? [];
  const routines = cell.routines ?? [];
  const isEmpty = events.length === 0 && tasks.length === 0 && routines.length === 0;

  return (
    <div className="today-summary-member">
      <div className="today-summary-member-name">{name}</div>
      {isEmpty ? (
        <span className="today-summary-empty">{tWeek("todayEmpty")}</span>
      ) : (
        <div className="today-summary-items">
          {events.map((e) => eventToItem(e))}
          {tasks.map((t) => taskToItem(t))}
          {routines.map((r) => routineToItem(r))}
        </div>
      )}
    </div>
  );
}

export function DayView({ grid, selectedDate, loading, error }: DayViewProps) {
  const { t, i18n } = useTranslation("coordination");
  const { t: tWeek } = useTranslation("week");
  const { t: tCommon } = useTranslation("common");

  const todayIso = new Date().toISOString().slice(0, 10);
  const isToday = selectedDate === todayIso;

  const dateLabel = new Date(selectedDate + "T00:00:00").toLocaleDateString(
    i18n.language,
    { weekday: "long", day: "numeric", month: "long" },
  );

  if (loading) {
    return <div className="loading-wrap">{t("loading")}</div>;
  }
  if (error) {
    return <p className="error-msg">{error}</p>;
  }
  if (!grid) {
    return <div className="loading-wrap">{tCommon("loading")}</div>;
  }

  const members = grid.members ?? [];
  const sharedCells = grid.sharedCells ?? [];

  const sharedCell = sharedCells.find((c) => c.date.slice(0, 10) === selectedDate);
  const hasSharedItems = (sharedCell?.routines?.length ?? 0) > 0;

  const memberDays = members.map((member) => ({
    member,
    cell: member.cells.find((c) => c.date.slice(0, 10) === selectedDate) ?? {
      date: selectedDate,
      events: [],
      tasks: [],
      routines: [],
    },
  }));

  const hasAnyContent =
    hasSharedItems ||
    memberDays.some(
      ({ cell }) =>
        (cell.events?.length ?? 0) > 0 ||
        (cell.tasks?.length ?? 0) > 0 ||
        (cell.routines?.length ?? 0) > 0,
    );

  return (
    <div className="today-summary coord-day-panel">
      <div className="today-summary-header">
        <span className="today-summary-label">
          {isToday ? tWeek("today") : t("day.title")}
        </span>
        <span className="today-summary-date">{dateLabel}</span>
      </div>

      {members.length === 0 && (
        <p className="today-summary-empty">{t("day.noMembers")}</p>
      )}

      {members.length > 0 && !hasAnyContent && (
        <p className="today-summary-empty">{t("day.empty")}</p>
      )}

      {members.length > 0 && hasAnyContent && (
        <div className="today-summary-body">
          {hasSharedItems && sharedCell && (
            <DayMemberSection name={t("day.household")} cell={sharedCell} />
          )}
          {memberDays.map(({ member, cell }) => (
            <DayMemberSection key={member.memberId} name={member.name} cell={cell} />
          ))}
        </div>
      )}
    </div>
  );
}

