import { useTranslation } from "react-i18next";
import type { WeeklyGridResponse, WeeklyGridRoutineItem } from "../types";
import { WeekHeader } from "./WeekHeader";
import { WeeklyGridRow } from "./WeeklyGridRow";
import { WeeklyGridItem } from "./WeeklyGridItem";

interface WeeklyGridProps {
  grid: WeeklyGridResponse;
}

function RoutinesRow({ routines = [] }: { routines?: WeeklyGridRoutineItem[] }) {
  const { t } = useTranslation("week");
  if (routines.length === 0) return null;

  return (
    <div className="wg-row wg-row--routines">
      <div className="wg-member-label">
        <span className="wg-member-name">{t("routines")}</span>
      </div>
      <div className="wg-cell wg-cell--routines">
        {routines.map((r) => (
          <WeeklyGridItem
            key={r.routineId}
            type="routine"
            title={r.name}
            time={r.cadence}
            status={r.status}
          />
        ))}
      </div>
    </div>
  );
}

export function WeeklyGrid({ grid }: WeeklyGridProps) {
  const members = grid.members ?? [];

  const days: string[] =
    members.length > 0
      ? (members[0].cells ?? []).map((c) => c.date.slice(0, 10))
      : Array.from({ length: 7 }, (_, i) => {
          const d = new Date(grid.weekStart);
          d.setDate(d.getDate() + i);
          return d.toISOString().slice(0, 10);
        });

  return (
    <div className="weekly-grid">
      <WeekHeader days={days} />
      {members.map((member) => (
        <WeeklyGridRow key={member.memberId} member={member} />
      ))}
    </div>
  );
}
