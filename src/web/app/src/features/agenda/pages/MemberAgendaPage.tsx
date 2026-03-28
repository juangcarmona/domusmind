import { useState, useCallback, useEffect } from "react";
import { useParams, useSearchParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useAppSelector } from "../../../store/hooks";
import { weekApi } from "../../today/api/weekApi";
import type { WeeklyGridResponse } from "../../today/types";
import type { ApiError } from "../../../api/domusmindApi";
import { EditEntityModal, type EditableEntityType } from "../../editors/components/EditEntityModal";
import { AgendaHeader, type AgendaView } from "../components/AgendaHeader";
import { MemberDayView } from "../components/MemberDayView";
import { MemberWeekView } from "../components/MemberWeekView";
import { MemberMonthView } from "../components/MemberMonthView";
import {
  toIsoDate,
  addDays,
  addMonths,
  startOfWeek,
} from "../../today/utils/dateUtils";

export function MemberAgendaPage() {
  const { memberId } = useParams<{ memberId: string }>();
  const [searchParams, setSearchParams] = useSearchParams();
  const { t } = useTranslation("agenda");

  const family = useAppSelector((s) => s.household.family);
  const familyId = family?.familyId ?? "";
  const firstDayOfWeek = family?.firstDayOfWeek ?? null;
  const members = useAppSelector((s) => s.household.members);

  // Resolve member identity from household state.
  const householdMember = members.find((m) => m.memberId === memberId);

  // Selected date comes from ?date= query param, defaulting to today.
  const todayIso = toIsoDate(new Date());
  const initialDate = searchParams.get("date") ?? todayIso;
  const [selectedDate, setSelectedDate] = useState<string>(initialDate);
  const [view, setView] = useState<AgendaView>("day");

  // Grid state (reused weekly grid API).
  const [grid, setGrid] = useState<WeeklyGridResponse | null>(null);
  const [gridLoading, setGridLoading] = useState(false);
  const [gridError, setGridError] = useState<string | null>(null);

  // Entity edit modal state.
  const [editTarget, setEditTarget] = useState<{ type: EditableEntityType; id: string } | null>(null);

  const weekStartForSelected = toIsoDate(
    startOfWeek(new Date(selectedDate + "T00:00:00"), firstDayOfWeek),
  );

  const fetchGrid = useCallback(
    async (weekStart: string) => {
      if (!familyId) return;
      setGridLoading(true);
      setGridError(null);
      try {
        const data = await weekApi.getWeeklyGrid(familyId, weekStart);
        setGrid(data);
      } catch (err) {
        const apiErr = err as Partial<ApiError>;
        setGridError(apiErr.message ?? t("error"));
      } finally {
        setGridLoading(false);
      }
    },
    [familyId, t],
  );

  useEffect(() => {
    if (familyId) {
      fetchGrid(weekStartForSelected);
    }
  }, [weekStartForSelected, fetchGrid, familyId]);

  // Keep ?date= in sync with selectedDate so deep links work.
  useEffect(() => {
    setSearchParams({ date: selectedDate }, { replace: true });
  }, [selectedDate, setSearchParams]);

  // Find this member's row in the loaded grid.
  const memberGrid = grid?.members.find((m) => m.memberId === memberId) ?? null;

  // Empty member fallback (no data for the loaded week).
  const emptyMember = { memberId: memberId ?? "", name: householdMember?.name ?? "", role: "", cells: [] };

  // ---- Navigation handlers ----

  function handlePrev() {
    if (view === "day") setSelectedDate(addDays(selectedDate, -1));
    else if (view === "week") setSelectedDate(addDays(selectedDate, -7));
    else setSelectedDate(addMonths(selectedDate, -1));
  }

  function handleNext() {
    if (view === "day") setSelectedDate(addDays(selectedDate, 1));
    else if (view === "week") setSelectedDate(addDays(selectedDate, 7));
    else setSelectedDate(addMonths(selectedDate, 1));
  }

  function handleToday() {
    setSelectedDate(todayIso);
  }

  function handleItemClick(type: "event" | "task" | "routine", id: string) {
    setEditTarget({ type, id });
  }

  /** Called from Week or Month views: select the date and switch to Day view. */
  function handleDayDrill(date: string) {
    setSelectedDate(date);
    setView("day");
  }

  // ---- Render ----

  const memberName = householdMember?.name ?? memberId ?? "";

  if (!familyId) {
    return <div className="loading-wrap">{t("loading")}</div>;
  }

  if (memberId && !householdMember) {
    return (
      <div className="page-content agenda-page">
        <p className="error-msg">{t("memberNotFound")}</p>
      </div>
    );
  }

  return (
    <div className="page-content agenda-page">
      <AgendaHeader
        memberName={memberName}
        selectedDate={selectedDate}
        view={view}
        onViewChange={setView}
        onPrev={handlePrev}
        onNext={handleNext}
        onToday={handleToday}
      />

      <div className="agenda-body">
        {gridLoading && (
          <div className="loading-wrap">{t("loading")}</div>
        )}
        {gridError && (
          <p className="error-msg">{gridError}</p>
        )}

        {!gridLoading && !gridError && (
          <>
            {view === "day" && (
              <MemberDayView
                member={memberGrid ?? emptyMember}
                selectedDate={selectedDate}
                onItemClick={handleItemClick}
              />
            )}
            {view === "week" && (
              <MemberWeekView
                member={memberGrid ?? emptyMember}
                selectedDate={selectedDate}
                onItemClick={handleItemClick}
                onDayClick={handleDayDrill}
              />
            )}
            {view === "month" && (
              <MemberMonthView
                memberId={memberId ?? ""}
                selectedDate={selectedDate}
                firstDayOfWeek={firstDayOfWeek}
                onSelectDay={handleDayDrill}
              />
            )}
          </>
        )}
      </div>

      {editTarget && (
        <EditEntityModal
          type={editTarget.type}
          id={editTarget.id}
          onClose={() => setEditTarget(null)}
          onEntitySaved={async () => {
            setEditTarget(null);
            await fetchGrid(weekStartForSelected);
          }}
        />
      )}
    </div>
  );
}


