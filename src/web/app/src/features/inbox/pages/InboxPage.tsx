import { useCallback, useEffect, useState } from "react";
import { useAppSelector } from "../../../store/hooks";
import { inboxApi, type InboxInterpreterInfo, type InboxItem, type InboxProposal } from "../../../api/inboxApi";
import { calendarApi } from "../../../api/calendarApi";
import type { FamilyTimelineEventItem } from "../../../api/types/calendarTypes";
import "./InboxPage.css";

export function InboxPage() {
  const familyId = useAppSelector((s) => s.household.family?.familyId);
  const [items, setItems] = useState<InboxItem[]>([]);
  const [text, setText] = useState("");
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [interpreter, setInterpreter] = useState<InboxInterpreterInfo>();
  const [plans, setPlans] = useState<FamilyTimelineEventItem[]>([]);
  const reload = useCallback(async () => {
    if (familyId) {
      const [inboxItems, timeline] = await Promise.all([
        inboxApi.list(familyId),
        calendarApi.getEvents(familyId),
      ]);
      setItems(inboxItems);
      setPlans(timeline.events);
    }
  }, [familyId]);
  useEffect(() => {
    if (!familyId) return;
    let cancelled = false;
    void Promise.all([
      inboxApi.list(familyId),
      calendarApi.getEvents(familyId),
      inboxApi.interpreter(),
    ]).then(([inboxItems, timeline, interpreterInfo]) => {
      if (cancelled) return;
      setItems(inboxItems);
      setPlans(timeline.events);
      setInterpreter(interpreterInfo);
    }).catch(() => setError("Inbox could not be loaded."));
    return () => { cancelled = true; };
  }, [familyId]);

  async function capture(event: React.FormEvent) {
    event.preventDefault();
    if (!familyId || !text.trim()) return;
    setBusy(true); setError("");
    try {
      const item = await inboxApi.capture(familyId, text.trim());
      setText("");
      try { await inboxApi.interpret(item.inboxItemId); }
      catch { setError("Your note is saved. The configured interpreter is unavailable; you can retry below."); }
      await reload();
    } catch { setError("Your note could not be saved. Please try again."); }
    finally { setBusy(false); }
  }

  async function run(action: () => Promise<InboxItem>) {
    setBusy(true); setError("");
    try { await action(); await reload(); }
    catch { setError("That action could not be completed. Please review the item and try again."); }
    finally { setBusy(false); }
  }

  return <main className="inbox-page">
    <header><h1>Inbox</h1><p>Capture a household thought. Review each suggestion before it changes household records.</p>
      {interpreter && <p role="note">{interpreter.sendsTextOutsideDeployment
        ? "Inbox text is sent to the configured external interpretation provider."
        : "Inbox text is interpreted by the local provider."}</p>}
    </header>
    <form className="inbox-capture" onSubmit={capture}>
      <label htmlFor="inbox-note">What should your household remember?</label>
      <textarea id="inbox-note" value={text} maxLength={4000} onChange={(e) => setText(e.target.value)} placeholder="Write a note…" />
      <div><span>{text.length}/4000</span><button disabled={busy || !text.trim()}>Save and interpret</button></div>
    </form>
    {error && <p className="inbox-error" role="alert">{error}</p>}
    <section aria-label="Household notes" className="inbox-items">
      {items.length === 0 ? <p className="inbox-empty">Your captured notes will appear here.</p> : items.map((item) =>
        <article className="inbox-item" key={item.inboxItemId}>
          <p className="inbox-raw-note">{item.rawText}</p>
          <time dateTime={item.createdAtUtc}>{new Date(item.createdAtUtc).toLocaleString()}</time>
          {item.proposals.length === 0 ? <button disabled={busy} onClick={() => void run(() => inboxApi.interpret(item.inboxItemId))}>Interpret note</button> :
            item.proposals.map((proposal) => <ProposalCard key={proposal.proposalId} item={item} proposal={proposal} plans={plans} busy={busy}
              onRun={run} />)}
        </article>) }
    </section>
  </main>;
}

function ProposalCard({ item, proposal, plans, busy, onRun }: { item: InboxItem; proposal: InboxProposal;
  plans: FamilyTimelineEventItem[]; busy: boolean;
  onRun: (action: () => Promise<InboxItem>) => Promise<void> }) {
  const [editing, setEditing] = useState(false);
  const [title, setTitle] = useState(proposal.title);
  const proposalTypeLabel = proposal.type === "TimelineNote" ? "Timeline note" : proposal.type;
  const [date, setDate] = useState(proposal.type === "Task" ? proposal.dueDate ?? "" : proposal.date ?? "");
  const [endDate, setEndDate] = useState(proposal.endDate ?? "");
  const [targetPlanId, setTargetPlanId] = useState(proposal.targetPlanId ?? "");
  const [reminderMinutesBefore, setReminderMinutesBefore] = useState(proposal.reminderMinutesBefore?.toString() ?? "");
  if (proposal.status !== "Pending") return <div className="inbox-proposal"><b>{proposalTypeLabel}: {proposal.title}</b><span>{proposal.status}</span></div>;
  const changes = { title, description: proposal.description, date: proposal.type === "Plan" || proposal.type === "TimelineNote" ? date || null : proposal.date,
    time: proposal.time, endDate: proposal.type === "Plan" ? endDate || null : proposal.endDate,
    endTime: proposal.endTime, dueDate: proposal.type === "Task" ? date || null : proposal.dueDate,
    targetPlanId: proposal.type === "Reminder" ? targetPlanId || null : proposal.targetPlanId,
    reminderMinutesBefore: proposal.type === "Reminder" && reminderMinutesBefore ? Number(reminderMinutesBefore) : proposal.reminderMinutesBefore };
  return <div className="inbox-proposal">
    <h3>{proposalTypeLabel} proposal</h3>
    {editing ? <div className="inbox-refine">
      <label>Title<input value={title} onChange={(e) => setTitle(e.target.value)} /></label>
      <label>{proposal.type === "Plan" ? "Start date" : proposal.type === "TimelineNote" ? "Note date" : "Due date"}<input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></label>
      {proposal.type === "Plan" && <label>End date<input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} /></label>}
      {proposal.type === "Reminder" && <>
        <label>Plan<select value={targetPlanId} onChange={(e) => setTargetPlanId(e.target.value)}>
          <option value="">Select a plan</option>{plans.map((plan) => <option key={plan.calendarEventId} value={plan.calendarEventId}>{plan.title} ({plan.date})</option>)}
        </select></label>
        <label>Minutes before start<input type="number" min="1" value={reminderMinutesBefore} onChange={(e) => setReminderMinutesBefore(e.target.value)} /></label>
      </>}
      <button disabled={busy || !title.trim()} onClick={() => void onRun(async () => { const result = await inboxApi.refine(item.inboxItemId, proposal.proposalId, changes); setEditing(false); return result; })}>Save changes</button>
    </div> : <><b>{proposal.title}</b>{proposal.unresolvedDetails && <p>{proposal.unresolvedDetails}</p>}
      <div className="inbox-actions">
        <button disabled={busy} onClick={() => setEditing(true)}>Refine</button>
        <button disabled={busy} onClick={() => void onRun(() => inboxApi.reject(item.inboxItemId, proposal.proposalId))}>Reject</button>
        <button disabled={busy || (proposal.type === "Plan" && (!proposal.date || !proposal.endDate))
          || (proposal.type === "Reminder" && (!proposal.targetPlanId || !proposal.reminderMinutesBefore))
          || (proposal.type === "TimelineNote" && !proposal.date)}
          title={proposal.type === "Plan" && (!proposal.date || !proposal.endDate) ? "Add start and end dates before approval"
            : proposal.type === "Reminder" && (!proposal.targetPlanId || !proposal.reminderMinutesBefore) ? "Select a Plan and reminder offset before approval"
              : proposal.type === "TimelineNote" && !proposal.date ? "Add a date before approval" : undefined}
          onClick={() => void onRun(() => inboxApi.approve(item.inboxItemId, proposal.proposalId))}>Approve</button>
      </div></>}
  </div>;
}
