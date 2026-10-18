import { useCallback, useEffect, useState } from "react";
import { useAppSelector } from "../../../store/hooks";
import { inboxApi, type InboxInterpreterInfo, type InboxItem, type InboxProposal } from "../../../api/inboxApi";
import { calendarApi } from "../../../api/calendarApi";
import { listsApi } from "../../../api/listsApi";
import type { SharedListItemDetail, SharedListSummary } from "../../../api/types/listTypes";
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
  const [lists, setLists] = useState<SharedListSummary[]>([]);
  const reload = useCallback(async () => {
    if (familyId) {
      const [inboxItems, timeline] = await Promise.all([
        inboxApi.list(familyId),
        calendarApi.getEvents(familyId),
      ]);
      setItems(inboxItems);
      setPlans(timeline.events);
      setLists((await listsApi.getFamilyLists(familyId)).lists);
    }
  }, [familyId]);
  useEffect(() => {
    if (!familyId) return;
    let cancelled = false;
    void Promise.all([
      inboxApi.list(familyId),
      calendarApi.getEvents(familyId),
      inboxApi.interpreter(familyId),
      listsApi.getFamilyLists(familyId),
    ]).then(([inboxItems, timeline, interpreterInfo, listResponse]) => {
      if (cancelled) return;
      setItems(inboxItems);
      setPlans(timeline.events);
      setInterpreter(interpreterInfo);
      setLists(listResponse.lists);
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
      {interpreter && <div role="note"><p>Inbox sends captured text to the selected household provider for interpretation. Only text you put in Inbox is sent.</p>
        {interpreter.targets.length === 0 ? <p>No provider is configured for this household.</p> : <ul>
          {interpreter.targets.map((target) => <li key={target.id}>{target.name}: {target.hosting} ({target.protocol}, {target.model}){target.hosting === "Hosted" ? " — captured text is sent outside this deployment; no other household context is included." : " — captured text is sent to the configured local endpoint; no other household context is included."}</li>)}
        </ul>}
      </div>}
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
          {item.providerName && <small>Interpreted by {item.providerName} ({item.providerHosting?.toLowerCase()}){item.providerFallbackOccurred ? " after fallback" : ""}.</small>}
          {item.proposals.length === 0 ? <button disabled={busy} onClick={() => void run(() => inboxApi.interpret(item.inboxItemId))}>Interpret note</button> :
            item.proposals.map((proposal) => <ProposalCard key={proposal.proposalId} item={item} proposal={proposal} plans={plans} busy={busy}
            lists={lists} onRun={run} />)}
        </article>) }
    </section>
  </main>;
}

function ProposalCard({ item, proposal, plans, lists, busy, onRun }: { item: InboxItem; proposal: InboxProposal;
  lists: SharedListSummary[];
  plans: FamilyTimelineEventItem[]; busy: boolean;
  onRun: (action: () => Promise<InboxItem>) => Promise<void> }) {
  const [editing, setEditing] = useState(false);
  const [title, setTitle] = useState(proposal.title);
  const proposalTypeLabel = proposal.type === "TimelineNote" ? "Timeline note" : proposal.type;
  const [date, setDate] = useState(proposal.type === "Task" ? proposal.dueDate ?? "" : proposal.date ?? "");
  const [endDate, setEndDate] = useState(proposal.endDate ?? "");
  const [targetPlanId, setTargetPlanId] = useState(proposal.targetPlanId ?? "");
  const [targetListId, setTargetListId] = useState(proposal.targetListId ?? "");
  const [targetItemId, setTargetItemId] = useState(proposal.targetItemId ?? "");
  const [loadedListItems, setLoadedListItems] = useState<{ listId: string; items: SharedListItemDetail[] } | null>(null);
  const [quantity, setQuantity] = useState(proposal.quantity ?? "");
  const [description, setDescription] = useState(proposal.description ?? "");
  const [time, setTime] = useState(proposal.time ?? "");
  const [endTime, setEndTime] = useState(proposal.endTime ?? "");
  const [reminderMinutesBefore, setReminderMinutesBefore] = useState(proposal.reminderMinutesBefore?.toString() ?? "");
  useEffect(() => {
    if (!targetListId || proposal.type !== "ListItemUpdate") return;
    let cancelled = false;
    void listsApi.getSharedListDetail(targetListId).then((detail) => {
      if (!cancelled) setLoadedListItems({ listId: targetListId, items: detail.items });
    }).catch(() => { if (!cancelled) setLoadedListItems({ listId: targetListId, items: [] }); });
    return () => { cancelled = true; };
  }, [targetListId, proposal.type]);
  const listItems = loadedListItems?.listId === targetListId ? loadedListItems.items : [];
  if (proposal.status !== "Pending") return <div className="inbox-proposal"><b>{proposalTypeLabel}: {proposal.title}</b><span>{proposal.status}</span></div>;
  const changes = { title, description: description || null, date: ["Plan", "PlanUpdate", "TimelineNote"].includes(proposal.type) ? date || null : proposal.date,
    time: ["Plan", "PlanUpdate"].includes(proposal.type) ? time || null : proposal.time,
    endDate: ["Plan", "PlanUpdate"].includes(proposal.type) ? endDate || null : proposal.endDate,
    endTime: ["Plan", "PlanUpdate"].includes(proposal.type) ? endTime || null : proposal.endTime,
    dueDate: proposal.type === "Task" ? date || null : proposal.dueDate,
    targetPlanId: ["Reminder", "PlanUpdate"].includes(proposal.type) ? targetPlanId || null : proposal.targetPlanId,
    reminderMinutesBefore: proposal.type === "Reminder" && reminderMinutesBefore ? Number(reminderMinutesBefore) : proposal.reminderMinutesBefore,
    targetListId: ["ListItemAdd", "ListItemUpdate"].includes(proposal.type) ? targetListId || null : proposal.targetListId,
    targetItemId: proposal.type === "ListItemUpdate" ? targetItemId || null : proposal.targetItemId,
    quantity: ["ListItemAdd", "ListItemUpdate"].includes(proposal.type) ? quantity || null : proposal.quantity };
  return <div className="inbox-proposal">
    <h3>{proposalTypeLabel} proposal</h3>
    {editing ? <div className="inbox-refine">
      <label>Title<input value={title} onChange={(e) => setTitle(e.target.value)} /></label>
      {(["Plan", "PlanUpdate", "TimelineNote", "Task"].includes(proposal.type)) && <label>{proposal.type === "TimelineNote" ? "Note date" : proposal.type === "Task" ? "Due date" : "Start date"}<input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></label>}
      {["Plan", "PlanUpdate"].includes(proposal.type) && <>
        <label>Start time<input type="time" value={time} onChange={(e) => setTime(e.target.value)} /></label>
        <label>End date<input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} /></label>
        <label>End time<input type="time" value={endTime} onChange={(e) => setEndTime(e.target.value)} /></label>
      </>}
      {proposal.type === "PlanUpdate" && <label>Plan<select value={targetPlanId} onChange={(e) => setTargetPlanId(e.target.value)}>
        <option value="">Select a plan</option>{plans.map((plan) => <option key={plan.calendarEventId} value={plan.calendarEventId}>{plan.title} ({plan.date})</option>)}
      </select></label>}
      {proposal.type === "Reminder" && <>
        <label>Plan<select value={targetPlanId} onChange={(e) => setTargetPlanId(e.target.value)}>
          <option value="">Select a plan</option>{plans.map((plan) => <option key={plan.calendarEventId} value={plan.calendarEventId}>{plan.title} ({plan.date})</option>)}
        </select></label>
        <label>Minutes before start<input type="number" min="1" value={reminderMinutesBefore} onChange={(e) => setReminderMinutesBefore(e.target.value)} /></label>
      </>}
      {["ListItemAdd", "ListItemUpdate"].includes(proposal.type) && <>
        <label>List<select value={targetListId} onChange={(e) => { setTargetListId(e.target.value); setTargetItemId(""); }}>
          <option value="">Select a list</option>{lists.map((list) => <option key={list.id} value={list.id}>{list.name}</option>)}
        </select></label>
        {proposal.type === "ListItemUpdate" && <label>List item<select value={targetItemId} onChange={(e) => setTargetItemId(e.target.value)}>
          <option value="">Select an item</option>{listItems.map((listItem) => <option key={listItem.itemId} value={listItem.itemId}>{listItem.name}</option>)}
        </select></label>}
        <label>Quantity<input value={quantity} onChange={(e) => setQuantity(e.target.value)} /></label>
        <label>Note<textarea value={description} onChange={(e) => setDescription(e.target.value)} /></label>
      </>}
      {proposal.type === "PlanUpdate" && <label>Description<textarea value={description} onChange={(e) => setDescription(e.target.value)} /></label>}
      <button disabled={busy || !title.trim()} onClick={() => void onRun(async () => { const result = await inboxApi.refine(item.inboxItemId, proposal.proposalId, changes); setEditing(false); return result; })}>Save changes</button>
    </div> : <><b>{proposal.title}</b>{proposal.unresolvedDetails && <p>{proposal.unresolvedDetails}</p>}
      <div className="inbox-actions">
        <button disabled={busy} onClick={() => setEditing(true)}>Refine</button>
        <button disabled={busy} onClick={() => void onRun(() => inboxApi.reject(item.inboxItemId, proposal.proposalId))}>Reject</button>
        <button disabled={busy || (proposal.type === "Plan" && (!proposal.date || !proposal.endDate))
          || (proposal.type === "PlanUpdate" && !proposal.targetPlanId)
          || (proposal.type === "ListItemAdd" && !proposal.targetListId)
          || (proposal.type === "ListItemUpdate" && (!proposal.targetListId || !proposal.targetItemId))
          || (proposal.type === "Reminder" && (!proposal.targetPlanId || !proposal.reminderMinutesBefore))
          || (proposal.type === "TimelineNote" && !proposal.date)}
          title={proposal.type === "Plan" && (!proposal.date || !proposal.endDate) ? "Add start and end dates before approval"
            : proposal.type === "PlanUpdate" && !proposal.targetPlanId ? "Select a Plan before approval"
              : proposal.type === "ListItemAdd" && !proposal.targetListId ? "Select a List before approval"
                : proposal.type === "ListItemUpdate" && (!proposal.targetListId || !proposal.targetItemId) ? "Select a List and item before approval"
            : proposal.type === "Reminder" && (!proposal.targetPlanId || !proposal.reminderMinutesBefore) ? "Select a Plan and reminder offset before approval"
              : proposal.type === "TimelineNote" && !proposal.date ? "Add a date before approval" : undefined}
          onClick={() => void onRun(() => inboxApi.approve(item.inboxItemId, proposal.proposalId))}>Approve</button>
      </div></>}
  </div>;
}
