import { useEffect, useMemo, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import { fetchAreas, assignPrimaryOwner, transferArea, renameArea } from "../../../store/areasSlice";
import { fetchPlans } from "../../../store/plansSlice";
import { fetchRoutines, pauseRoutine, resumeRoutine } from "../../../store/routinesSlice";
import { fetchTimeline } from "../../../store/timelineSlice";
import { completeTask, cancelTask } from "../../../store/tasksSlice";
import { EditEntityModal } from "../../editors/components/EditEntityModal";
import { PlanningAddModal } from "../../planning/components/modals/PlanningAddModal";
import type { PlanningAddModalDefaults } from "../../planning/components/modals/PlanningAddModal";
import { EntityCard } from "../../../components/EntityCard";
import { formatRoutineDays, formatRoutineAssigned } from "../../planning/utils/routineFormatters";
import { AREA_PALETTE, getAreaColor, setAreaColor } from "../utils/areaColors";
import { useDateFormatter } from "../../../hooks/useDateFormatter";

function todayIso(): string {
  const d = new Date();
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}`;
}

// ── Area Detail Page ──────────────────────────────────────────────────────────
//
// Remaining backend gap:
//   HouseholdAreaItem has no `color` field → color lives in localStorage.

export function AreaDetailPage() {
  const { areaId } = useParams<{ areaId: string }>();
  const navigate = useNavigate();
  const { t } = useTranslation("areas");
  const { t: tTasks } = useTranslation("tasks");
  const { t: tPlans } = useTranslation("plans");
  const { t: tRoutines } = useTranslation("routines");
  const { t: tCommon } = useTranslation("common");
  const dispatch = useAppDispatch();
  const { formatDate, formatDateTime } = useDateFormatter();
  const { family, members } = useAppSelector((s) => s.household);
  const { items: areas, status } = useAppSelector((s) => s.areas);
  const { items: planItems, status: plansStatus } = useAppSelector((s) => s.plans);
  const { items: routineItems, status: routinesStatus } = useAppSelector((s) => s.routines);
  const timeline = useAppSelector((s) => s.timeline);
  const familyId = family?.familyId;

  const area = areas.find((a) => a.areaId === areaId);

  // Color is stored in localStorage; initialized from there on mount.
  const [color, setColor] = useState<string>(() =>
    areaId ? getAreaColor(areaId) : AREA_PALETTE[0],
  );
  const [saving, setSaving] = useState(false);
  const [isEditingName, setIsEditingName] = useState(false);
  const [nameInput, setNameInput] = useState("");
  const [renaming, setRenaming] = useState(false);
  const [editTarget, setEditTarget] = useState<{ type: "task" | "routine" | "event"; id: string } | null>(null);
  const [addModal, setAddModal] = useState(false);

  // Area-context creation defaults: pre-select this area + area owner as assignee.
  const creationDefaults = useMemo<PlanningAddModalDefaults>(() => ({
    areaId: areaId,
    assigneeId: area?.primaryOwnerId ?? undefined,
    participantMemberIds: area?.primaryOwnerId ? [area.primaryOwnerId] : undefined,
  }), [areaId, area?.primaryOwnerId]);

  // Tasks: use timelineSlice (same source as EditEntityModal) so edit lookups always succeed.
  const linkedTasks = useMemo(
    () =>
      (timeline.data?.groups.flatMap((g) => g.entries) ?? [])
        .filter((e) => e.entryType === "Task" && e.areaId === areaId && e.status === "Pending"),
    [timeline.data, areaId],
  );
  const tasksLoading = timeline.status === "loading";

  const linkedPlans = planItems.filter((p) => p.areaId === areaId);
  const linkedRoutines = routineItems.filter((r) => r.areaId === areaId);
  const isCustomColor = !AREA_PALETTE.includes(color);

  useEffect(() => {
    if (familyId && status === "idle") dispatch(fetchAreas(familyId));
  }, [familyId, status, dispatch]);

  useEffect(() => {
    if (familyId && plansStatus === "idle") dispatch(fetchPlans({ familyId, from: todayIso() }));
    if (familyId && routinesStatus === "idle") dispatch(fetchRoutines(familyId));
  }, [familyId, plansStatus, routinesStatus, dispatch]);

  // Force a fresh task timeline fetch on every mount so newly created tasks appear.
  // EditEntityModal uses timelineSlice to look up tasks — we must keep this in sync.
  useEffect(() => {
    if (familyId) dispatch(fetchTimeline({ familyId, types: "Task" }));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [familyId]);

  // Member display map (id → display name), used by task + routine cards.
  const memberMap = useMemo<Record<string, string>>(
    () => Object.fromEntries(members.map((m) => [m.memberId, m.preferredName || m.name])),
    [members],
  );

  function refreshLinkedWork() {
    if (!familyId) return;
    dispatch(fetchTimeline({ familyId, types: "Task" }));
    dispatch(fetchPlans({ familyId }));
    dispatch(fetchRoutines(familyId));
  }

  async function handleCompleteTask(taskId: string) {
    await dispatch(completeTask(taskId));
    refreshLinkedWork();
  }

  async function handleCancelTask(taskId: string) {
    await dispatch(cancelTask(taskId));
    refreshLinkedWork();
  }

  async function handlePauseRoutine(routineId: string) {
    if (!familyId) return;
    await dispatch(pauseRoutine({ routineId, familyId }));
  }

  async function handleResumeRoutine(routineId: string) {
    if (!familyId) return;
    await dispatch(resumeRoutine({ routineId, familyId }));
  }

  function handleColorChange(e: React.ChangeEvent<HTMLInputElement>) {
    const c = e.target.value;
    setColor(c);
    if (areaId) setAreaColor(areaId, c);
  }

  function handleSwatchClick(c: string) {
    setColor(c);
    if (areaId) setAreaColor(areaId, c);
  }

  function handleNameClick() {
    setNameInput(area?.name ?? "");
    setIsEditingName(true);
  }

  async function handleNameSave() {
    if (!area || !nameInput.trim() || nameInput.trim() === area.name) {
      setIsEditingName(false);
      return;
    }
    setRenaming(true);
    await dispatch(renameArea({ areaId: area.areaId, name: nameInput.trim() }));
    setRenaming(false);
    setIsEditingName(false);
  }

  function handleNameKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === "Enter") { void handleNameSave(); }
    if (e.key === "Escape") setIsEditingName(false);
  }

  async function handleOwnerChange(e: React.ChangeEvent<HTMLSelectElement>) {
    if (!area || !familyId) return;
    const newId = e.target.value;
    if (!newId) return;
    setSaving(true);
    if (area.primaryOwnerId) {
      await dispatch(transferArea({ areaId: area.areaId, newPrimaryOwnerId: newId, familyId }));
    } else {
      await dispatch(assignPrimaryOwner({ areaId: area.areaId, memberId: newId, familyId }));
    }
    setSaving(false);
  }

  if (!familyId) return null;

  if (status === "loading" && !area) {
    return <div className="loading-wrap">{t("loading")}</div>;
  }

  if (status === "success" && !area) {
    return (
      <div>
        <button
          type="button"
          className="btn btn-ghost btn-sm"
          onClick={() => navigate("/areas")}
          style={{ marginBottom: "1rem" }}
        >
          ← {t("backToAreas")}
        </button>
        <p style={{ color: "var(--muted)" }}>{t("notFound")}</p>
      </div>
    );
  }

  // Areas list not loaded yet and we haven't resolved the area — wait.
  if (!area) return null;

  const hasOwner = !!area.primaryOwnerId;

  return (
    <div className="area-detail-page">
      <button
        type="button"
        className="btn btn-ghost btn-sm"
        onClick={() => navigate("/areas")}
        style={{ marginBottom: "1.25rem" }}
      >
        ← {t("backToAreas")}
      </button>

      {/* ── Header ─────────────────────────────────────────────────────────── */}
      <div className="area-detail-header" style={{ borderLeftColor: color }}>
        <div className="area-detail-color-wrap">
          <div className="area-color-picker">
            {AREA_PALETTE.map((c) => (
              <button
                key={c}
                type="button"
                className={`area-color-swatch${color === c ? " area-color-swatch--active" : ""}`}
                style={{ background: c }}
                onClick={() => handleSwatchClick(c)}
                aria-label={c}
              />
            ))}
            <div
              className={`area-color-custom-trigger${isCustomColor ? " area-color-custom-trigger--active" : ""}`}
              style={isCustomColor ? { background: color } : undefined}
              title={t("customColor")}
            >
              {!isCustomColor && <span className="area-color-custom-label" aria-hidden="true">+</span>}
              <input
                type="color"
                value={color}
                onChange={handleColorChange}
                aria-label={t("customColor")}
              />
            </div>
          </div>
          <span className="area-color-hex">{color.toUpperCase()}</span>
        </div>
        <div className="area-detail-identity">
          {isEditingName ? (
            <input
              className="area-detail-name-input"
              value={nameInput}
              autoFocus
              disabled={renaming}
              onChange={(e) => setNameInput(e.target.value)}
              onBlur={() => { void handleNameSave(); }}
              onKeyDown={handleNameKeyDown}
              aria-label={t("renameHint")}
            />
          ) : (
            <h1
              className="area-detail-name"
              style={{ color, cursor: "pointer" }}
              title={t("renameHint")}
              onClick={handleNameClick}
            >
              {area.name}
            </h1>
          )}
          <p className="area-detail-subtitle">{t("renameHint")}</p>
        </div>
      </div>

      {/* ── Owner ──────────────────────────────────────────────────────────── */}
      <div className="area-detail-section">
        <div className="area-detail-section-header">
          <span className="area-detail-section-title">{t("ownerLabel")}</span>
        </div>
        <select
          className="form-control area-row-select"
          value={area.primaryOwnerId ?? ""}
          disabled={saving}
          onChange={handleOwnerChange}
          aria-label={t("ownerLabel")}
        >
          {!hasOwner && <option value="">{t("noOwner")}</option>}
          {/* Guard: current owner may no longer be in the members list */}
          {hasOwner && !members.some((m) => m.memberId === area.primaryOwnerId) && (
            <option value={area.primaryOwnerId!}>
              {area.primaryOwnerName ?? area.primaryOwnerId}
            </option>
          )}
          {members.map((m) => (
            <option key={m.memberId} value={m.memberId}>
              {m.preferredName || m.name}
            </option>
          ))}
        </select>
        {!hasOwner && (
          <p className="area-detail-hint">{t("noOwnerInstruction")}</p>
        )}
      </div>

      {/* ── Related work ────────────────────────────────────────────────────── */}
      <div className="area-detail-section">
        <div className="area-detail-section-header">
          <span className="area-detail-section-title">{t("relatedWork")}</span>
          <button
            type="button"
            className="btn btn-sm"
            style={{ marginLeft: "auto" }}
            onClick={() => setAddModal(true)}
          >
            + {tCommon("add")}
          </button>
        </div>

        {/* Tasks */}
        {tasksLoading ? (
          <p className="area-related-loading">{tCommon("loading")}</p>
        ) : linkedTasks.length > 0 ? (
          <div style={{ marginBottom: "1.25rem" }}>
            <p className="area-related-group-label">{tTasks("title")}</p>
            <div className="item-list">
              {linkedTasks.map((task) => (
                <EntityCard
                  key={task.entryId}
                  title={task.title}
                  subtitle={
                    <>
                      {task.effectiveDate ? formatDate(task.effectiveDate) : tTasks("noDueDate")}
                      {task.assigneeId && memberMap[task.assigneeId]
                        ? ` · ${memberMap[task.assigneeId]}`
                        : task.isUnassigned
                          ? ` · ${tTasks("unassigned")}`
                          : ""}
                      {task.isOverdue && (
                        <span style={{ color: "var(--danger)" }}> · {tTasks("overdue")}</span>
                      )}
                    </>
                  }
                  accentColor={task.color}
                  isOverdue={task.isOverdue}
                  onClick={() => setEditTarget({ type: "task", id: task.entryId })}
                  actions={
                    <>
                      <button
                        className="btn btn-sm"
                        title={tTasks("markDoneTitle")}
                        onClick={(e) => { e.stopPropagation(); void handleCompleteTask(task.entryId); }}
                      >
                        ✓ {tTasks("done")}
                      </button>
                      <button
                        className="btn btn-ghost btn-sm"
                        title={tCommon("cancel")}
                        onClick={(e) => { e.stopPropagation(); void handleCancelTask(task.entryId); }}
                      >
                        ✕
                      </button>
                    </>
                  }
                />
              ))}
            </div>
          </div>
        ) : null}

        {/* Plans */}
        {linkedPlans.length > 0 && (
          <div style={{ marginBottom: "1.25rem" }}>
            <p className="area-related-group-label">{tPlans("title")}</p>
            <div className="item-list">
              {linkedPlans.map((plan) => (
                <EntityCard
                  key={plan.calendarEventId}
                  title={plan.title}
                  titleStrike={plan.status === "Cancelled"}
                  subtitle={
                    <>
                      {formatDateTime(plan.startTime)}
                      {plan.endTime && ` → ${formatDateTime(plan.endTime)}`}
                      {plan.participants?.length > 0 && (
                        <span> · {plan.participants.map((p) => p.displayName).join(", ")}</span>
                      )}
                    </>
                  }
                  accentColor={plan.color}
                  onClick={() => setEditTarget({ type: "event", id: plan.calendarEventId })}
                />
              ))}
            </div>
          </div>
        )}

        {/* Routines */}
        {linkedRoutines.length > 0 && (
          <div style={{ marginBottom: "1.25rem" }}>
            <p className="area-related-group-label">{tRoutines("title")}</p>
            <div className="item-list">
              {linkedRoutines.map((routine) => {
                const days = formatRoutineDays(routine, tRoutines);
                const assigned = formatRoutineAssigned(routine, memberMap, tRoutines);
                const statusLine = routine.status === "Paused"
                  ? <span style={{ color: "var(--muted)", fontWeight: 600 }}>{tRoutines("paused")}</span>
                  : <span style={{ color: "var(--success)", fontWeight: 600 }}>{tRoutines("active")}</span>;
                return (
                  <EntityCard
                    key={routine.routineId}
                    title={routine.name}
                    subtitle={
                      <>
                        {tRoutines(`frequency${routine.frequency}` as Parameters<typeof tRoutines>[0])}
                        {days ? ` · ${days}` : ""}
                        {routine.time ? ` · ${routine.time.slice(0, 5)}` : ""}
                        {` · ${assigned}`}
                        <span style={{ display: "block", marginTop: "0.2rem" }}>{statusLine}</span>
                      </>
                    }
                    accentColor={routine.color}
                    onClick={() => setEditTarget({ type: "routine", id: routine.routineId })}
                    actions={
                      routine.status === "Active" ? (
                        <button
                          className="btn btn-ghost btn-sm"
                          onClick={(e) => { e.stopPropagation(); void handlePauseRoutine(routine.routineId); }}
                        >
                          {tRoutines("pause")}
                        </button>
                      ) : (
                        <button
                          className="btn btn-sm"
                          onClick={(e) => { e.stopPropagation(); void handleResumeRoutine(routine.routineId); }}
                        >
                          {tRoutines("resume")}
                        </button>
                      )
                    }
                  />
                );
              })}
            </div>
          </div>
        )}

        {/* Empty state — only once all three sections have no items and loading is done */}
        {!tasksLoading &&
          linkedTasks.length === 0 &&
          linkedPlans.length === 0 &&
          linkedRoutines.length === 0 && (
            <div className="area-related-empty">
              <p style={{ fontWeight: 500, marginBottom: "0.4rem", color: "var(--text)" }}>
                {t("relatedWorkEmpty")}
              </p>
              <p className="area-related-hint-text">{t("relatedWorkHint")}</p>
            </div>
          )}
      </div>

      {editTarget && (
        <EditEntityModal
          type={editTarget.type}
          id={editTarget.id}
          onClose={() => setEditTarget(null)}
          onEntitySaved={() => { setEditTarget(null); refreshLinkedWork(); }}
        />
      )}

      {addModal && area && familyId && (
        <PlanningAddModal
          familyId={familyId}
          members={members.map((m) => ({ memberId: m.memberId, name: m.preferredName || m.name }))}
          initialStep="choose"
          defaults={creationDefaults}
          onClose={() => setAddModal(false)}
          onSuccess={() => { setAddModal(false); refreshLinkedWork(); }}
        />
      )}
    </div>
  );
}
