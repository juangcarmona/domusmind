import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAppDispatch, useAppSelector } from "../../../store/hooks";
import { fetchPlans, cancelEvent } from "../../../store/plansSlice";
import { fetchTimeline } from "../../../store/timelineSlice";
import { fetchRoutines, pauseRoutine, resumeRoutine } from "../../../store/routinesSlice";
import { completeTask, cancelTask, assignTask } from "../../../store/tasksSlice";
import { ConfirmDialog } from "../../../components/ConfirmDialog";
import { PlanningAddModal } from "../components/modals/PlanningAddModal";
import { EditEntityModal } from "../../editors/components/EditEntityModal";
import { AssignTaskModal } from "../components/modals/AssignTaskModal";
import { RoutinesTab } from "../components/RoutinesTab";
import { TasksTab } from "../components/TasksTab";
import { PlansTab } from "../components/PlansTab";
import type { FamilyTimelineEventItem, EnrichedTimelineEntry } from "../../../api/domusmindApi";

type PlanningTab = "routines" | "tasks" | "plans";

export function PlanningPage() {
  const dispatch = useAppDispatch();
  const { family, members } = useAppSelector((s) => s.household);
  const { items: planItems, status: plansStatus } = useAppSelector((s) => s.plans);
  const { data: timeline, status: timelineStatus } = useAppSelector((s) => s.timeline);
  const { items: routineItems, status: routinesStatus } = useAppSelector((s) => s.routines);
  const familyId = family?.familyId;

  const { t: tRoutines } = useTranslation("routines");
  const { t: tTasks } = useTranslation("tasks");
  const { t: tPlans } = useTranslation("plans");
  const { t: tCommon } = useTranslation("common");
  const { t: tNav } = useTranslation("nav");

  const [activeTab, setActiveTab] = useState<PlanningTab>("routines");
  const [addModal, setAddModal] = useState<"plan" | "task" | "routine" | "choose" | null>(null);
  const [cancelTarget, setCancelTarget] = useState<FamilyTimelineEventItem | null>(null);
  const [assignTarget, setAssignTarget] = useState<EnrichedTimelineEntry | null>(null);
  const [editTarget, setEditTarget] = useState<{ type: "routine" | "task" | "event"; id: string } | null>(null);

  const memberMap = Object.fromEntries(members.map((m) => [m.memberId, m.name]));

  function loadPlans() {
    if (familyId) dispatch(fetchPlans(familyId));
  }

  function loadTasks() {
    if (familyId) dispatch(fetchTimeline({ familyId, types: "Task" }));
  }

  function loadRoutines() {
    if (familyId && routinesStatus === "idle") dispatch(fetchRoutines(familyId));
  }

  useEffect(() => {
    loadPlans();
    loadTasks();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [familyId]);

  useEffect(() => {
    loadRoutines();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [familyId, routinesStatus]);

  async function handleCancelPlan() {
    if (!cancelTarget || !familyId) return;
    await dispatch(cancelEvent({ eventId: cancelTarget.calendarEventId, familyId }));
    setCancelTarget(null);
  }

  async function handleCompleteTask(taskId: string) {
    await dispatch(completeTask(taskId));
    loadTasks();
  }

  async function handleCancelTask(taskId: string) {
    await dispatch(cancelTask(taskId));
    loadTasks();
  }

  async function handleAssign(taskId: string, memberId: string) {
    await dispatch(assignTask({ taskId, assigneeId: memberId }));
    loadTasks();
  }

  async function handlePauseRoutine(routineId: string) {
    if (!familyId) return;
    await dispatch(pauseRoutine({ routineId, familyId }));
  }

  async function handleResumeRoutine(routineId: string) {
    if (!familyId) return;
    await dispatch(resumeRoutine({ routineId, familyId }));
  }

  if (!familyId) return null;

  const activePlans = planItems.filter((p) => p.status !== "Cancelled");

  const tasks: EnrichedTimelineEntry[] =
    timeline?.groups.flatMap((g) => g.entries.filter((e) => e.entryType === "Task")) ?? [];

  const activeTasks = tasks.filter((t) => t.status !== "Completed" && t.status !== "Cancelled");
  const doneTasks = tasks.filter((t) => t.status === "Completed" || t.status === "Cancelled");

  const tabs: { key: PlanningTab; label: string }[] = [
    { key: "routines", label: tRoutines("title") },
    { key: "tasks", label: tTasks("title") },
    { key: "plans", label: tPlans("title") },
  ];

  return (
    <div>
      <div className="page-header">
        <h1>{tNav("planning")}</h1>
        <button className="btn" onClick={() => setAddModal("choose")}>
          + {tCommon("add")}
        </button>
      </div>

      <div className="settings-tabs" style={{ marginBottom: "1.5rem" }}>
        {tabs.map((tab) => (
          <button
            key={tab.key}
            type="button"
            className={`settings-tab${activeTab === tab.key ? " active" : ""}`}
            onClick={() => setActiveTab(tab.key)}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {activeTab === "routines" && (
        <RoutinesTab
          routineItems={routineItems}
          routinesStatus={routinesStatus}
          memberMap={memberMap}
          onEdit={(id) => setEditTarget({ type: "routine", id })}
          onPause={handlePauseRoutine}
          onResume={handleResumeRoutine}
        />
      )}

      {activeTab === "tasks" && (
        <TasksTab
          activeTasks={activeTasks}
          doneTasks={doneTasks}
          timelineStatus={timelineStatus}
          memberMap={memberMap}
          onEdit={(id) => setEditTarget({ type: "task", id })}
          onAssign={setAssignTarget}
          onComplete={handleCompleteTask}
          onCancel={handleCancelTask}
        />
      )}

      {activeTab === "plans" && (
        <PlansTab
          activePlans={activePlans}
          plansStatus={plansStatus}
          onEdit={(id) => setEditTarget({ type: "event", id })}
          onCancelPlan={setCancelTarget}
        />
      )}

      {addModal && (
        <PlanningAddModal
          familyId={familyId}
          members={members}
          initialStep={addModal === "choose" ? "choose" : addModal}
          onClose={() => setAddModal(null)}
          onSuccess={() => {
            setAddModal(null);
            loadPlans();
            loadTasks();
            dispatch(fetchRoutines(familyId));
          }}
        />
      )}

      {assignTarget && (
        <AssignTaskModal
          entry={assignTarget}
          members={members}
          onAssign={handleAssign}
          onClose={() => setAssignTarget(null)}
        />
      )}

      {editTarget && (
        <EditEntityModal
          type={editTarget.type}
          id={editTarget.id}
          onClose={() => setEditTarget(null)}
          onEntitySaved={async () => {
            setEditTarget(null);
            loadPlans();
            loadTasks();
            await dispatch(fetchRoutines(familyId));
          }}
        />
      )}

      <ConfirmDialog
        isOpen={!!cancelTarget}
        title={tPlans("cancelEvent")}
        message={tPlans("confirmCancel")}
        confirmLabel={tPlans("yes")}
        onConfirm={handleCancelPlan}
        onCancel={() => setCancelTarget(null)}
      />
    </div>
  );
}
