import { useTranslation } from "react-i18next";
import { EntityCard } from "../../../components/EntityCard";
import { formatRoutineDays, formatRoutineAssigned } from "../utils/routineFormatters";
import type { RoutineListItem } from "../../../api/domusmindApi";

interface Props {
  routineItems: RoutineListItem[];
  routinesStatus: string;
  memberMap: Record<string, string>;
  onEdit: (routineId: string) => void;
  onPause: (routineId: string) => void;
  onResume: (routineId: string) => void;
}

export function RoutinesTab({ routineItems, routinesStatus, memberMap, onEdit, onPause, onResume }: Props) {
  const { t } = useTranslation("routines");
  const { t: tCommon } = useTranslation("common");

  if (routinesStatus === "loading") {
    return <div className="loading-wrap">{tCommon("loading")}</div>;
  }

  if (routineItems.length === 0) {
    return (
      <div className="empty-state">
        <p>{t("empty")}</p>
        <p>{t("emptyHint")}</p>
      </div>
    );
  }

  return (
    <div className="item-list">
      {routineItems.map((routine) => {
        const days = formatRoutineDays(routine, t);
        const assigned = formatRoutineAssigned(routine, memberMap, t);
        const statusLine = routine.status === "Paused"
          ? <span style={{ color: "var(--muted)", fontWeight: 600 }}>{t("paused")}</span>
          : <span style={{ color: "var(--success)", fontWeight: 600 }}>{t("active")}</span>;
        return (
          <EntityCard
            key={routine.routineId}
            title={routine.name}
            subtitle={
              <>
                {t(`frequency${routine.frequency}` as Parameters<typeof t>[0])}
                {days ? ` · ${days}` : ""}
                {routine.time ? ` · ${routine.time.slice(0, 5)}` : ""}
                {` · ${assigned}`}
                <span style={{ display: "block", marginTop: "0.2rem" }}>{statusLine}</span>
              </>
            }
            accentColor={routine.color}
            onClick={() => onEdit(routine.routineId)}
            actions={
              routine.status === "Active" ? (
                <button className="btn btn-ghost btn-sm" onClick={(e) => { e.stopPropagation(); onPause(routine.routineId); }}>{t("pause")}</button>
              ) : (
                <button className="btn btn-sm" onClick={(e) => { e.stopPropagation(); onResume(routine.routineId); }}>{t("resume")}</button>
              )
            }
          />
        );
      })}
    </div>
  );
}
