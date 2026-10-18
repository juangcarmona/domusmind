import { useCallback, useEffect, useMemo, useState } from "react";
import { useAuth } from "../../../auth/AuthProvider";
import { useAppSelector } from "../../../store/hooks";
import { chatsApi, type ChatMessage, type ChatThread } from "../../../api/chatsApi";
import type { AssistantProviderRoute } from "../../../api/assistantProvidersApi";
import "./chats.css";

export function ChatsPage() {
  const family = useAppSelector((s) => s.household.family);
  const members = useAppSelector((s) => s.household.members);
  const { user } = useAuth();
  const currentMember = members.find((member) => member.isCurrentUser || member.authUserId === user?.userId);
  const [threads, setThreads] = useState<ChatThread[]>([]);
  const [activeThread, setActiveThread] = useState<ChatThread | null>(null);
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [text, setText] = useState("");
  const [title, setTitle] = useState("");
  const [kind, setKind] = useState<ChatThread["kind"]>("Household");
  const [participants, setParticipants] = useState<string[]>([]);
  const [creating, setCreating] = useState(false);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [assistantNotice, setAssistantNotice] = useState("");
  const [assistantInfo, setAssistantInfo] = useState<AssistantProviderRoute | null>(null);

  const currentMemberId = currentMember?.memberId;
  const otherMembers = useMemo(() => members.filter((member) => member.memberId !== currentMemberId), [members, currentMemberId]);

  const reloadThreads = useCallback(async () => {
    if (!family?.familyId) return;
    const result = await chatsApi.list(family.familyId);
    setThreads(result);
    if (activeThread && !result.some((thread) => thread.chatThreadId === activeThread.chatThreadId)) {
      setActiveThread(null);
      setMessages([]);
    }
  }, [family?.familyId, activeThread]);

  useEffect(() => {
    if (!family?.familyId) return;
    let cancelled = false;
    void chatsApi.list(family.familyId).then((result) => { if (!cancelled) setThreads(result); })
      .catch(() => { if (!cancelled) setError("Conversations could not be loaded."); });
    return () => { cancelled = true; };
  }, [family?.familyId]);

  useEffect(() => {
    let cancelled = false;
    if (!family?.familyId) return;
    void chatsApi.assistant(family.familyId).then((info) => { if (!cancelled) setAssistantInfo(info); });
    return () => { cancelled = true; };
  }, [family?.familyId]);

  useEffect(() => {
    if (!activeThread) { setMessages([]); return; }
    let cancelled = false;
    void chatsApi.messages(activeThread.chatThreadId).then((result) => { if (!cancelled) setMessages(result); })
      .catch(() => { if (!cancelled) setError("Messages could not be loaded."); });
    return () => { cancelled = true; };
  }, [activeThread]);

  async function createThread(event: React.FormEvent) {
    event.preventDefault();
    if (!family?.familyId || !title.trim()) return;
    setBusy(true); setError("");
    try {
      const ids = kind === "Household" ? members.map((member) => member.memberId) : participants;
      const thread = await chatsApi.create(family.familyId, title.trim(), kind, ids);
      setCreating(false); setTitle(""); setParticipants([]); setActiveThread(thread);
      await reloadThreads();
    } catch { setError("The conversation could not be created. Choose at least one other household member."); }
    finally { setBusy(false); }
  }

  async function postMessage(event: React.FormEvent) {
    event.preventDefault();
    if (!activeThread || !text.trim()) return;
    setBusy(true); setError(""); setAssistantNotice("");
    try {
      const response = await chatsApi.post(activeThread.chatThreadId, text.trim());
      setMessages((current) => [...current, response.message, ...(response.assistantResponse ? [response.assistantResponse] : [])]);
      setText("");
      if (response.assistantUnavailable) setAssistantNotice("Your message was saved, but the configured assistant is unavailable. Try again later.");
      await reloadThreads();
    } catch { setError("Your message could not be sent."); }
    finally { setBusy(false); }
  }

  return <main className="chats-page">
    <header className="chats-header"><div><h1>Chats</h1><p>Start a message with <code>@dm</code> to ask about household information or prepare a Plan or List Item proposal for Inbox review.</p></div>
      <button type="button" onClick={() => setCreating((value) => !value)}>New conversation</button></header>
    {assistantInfo && <div className="chats-notice" role="note">
      {assistantInfo.targets.length === 0 ? <p>No assistant provider is configured for this household. A household manager can set one in Settings.</p> : <>
        <p>When you explicitly use <code>@dm</code>, the request and only household information you may access are sent to the selected target. The provider target and any fallback used appear on each answer.</p>
        <ul>{assistantInfo.targets.map((target) => <li key={target.id}>{target.name}: {target.hosting} ({target.protocol}, {target.model}){target.hosting === "Hosted" ? " — data is sent outside this deployment." : " — data is sent to the configured local endpoint."}</li>)}</ul>
      </>}
    </div>}
    {error && <p className="chats-alert" role="alert">{error}</p>}
    {creating && <form className="chats-create" onSubmit={(event) => void createThread(event)}>
      <label>Conversation name<input value={title} maxLength={120} onChange={(event) => setTitle(event.target.value)} required /></label>
      <label>Type<select value={kind} onChange={(event) => setKind(event.target.value as ChatThread["kind"])}>
        <option value="Household">Household</option><option value="Private">Private group</option>
      </select></label>
      {kind === "Private" && <fieldset><legend>Include at least one other member</legend>{otherMembers.map((member) =>
        <label className="chats-member" key={member.memberId}><input type="checkbox" checked={participants.includes(member.memberId)}
          onChange={(event) => setParticipants((current) => event.target.checked ? [...current, member.memberId] : current.filter((id) => id !== member.memberId))} />{member.preferredName || member.name}</label>)}</fieldset>}
      <div className="chats-create-actions"><button type="button" className="chats-secondary" onClick={() => setCreating(false)}>Cancel</button>
        <button disabled={busy || !title.trim() || (kind === "Private" && participants.length === 0)}>Create</button></div>
    </form>}
    <div className="chats-layout">
      <aside className="chats-list" aria-label="Conversations">
        {threads.length === 0 ? <p className="chats-empty">Start a conversation with your household.</p> : threads.map((thread) =>
          <button type="button" key={thread.chatThreadId} className={activeThread?.chatThreadId === thread.chatThreadId ? "chats-thread active" : "chats-thread"}
            onClick={() => { setActiveThread(thread); setAssistantNotice(""); }}><strong>{thread.title}</strong><small>{thread.kind === "Private" ? "Private group" : "Household"}</small></button>)}
      </aside>
      <section className="chats-conversation" aria-label="Selected conversation">
        {!activeThread ? <div className="chats-empty">Select a conversation to read its messages.</div> : <>
          <header><h2>{activeThread.title}</h2><p>{activeThread.kind === "Private" ? "Private group" : "Household conversation"}</p></header>
          <div className="chats-messages" aria-live="polite">
            {messages.length === 0 ? <p className="chats-empty">No messages yet. Say hello to get started.</p> : messages.map((message) =>
              <article className={`chats-message ${message.authorType === "Assistant" ? "assistant" : ""}`} key={message.chatMessageId}>
                <div><strong>{message.authorName}</strong><time dateTime={message.createdAtUtc}>{new Date(message.createdAtUtc).toLocaleString()}</time></div>
                <p>{message.text}</p>
                {message.authorType === "Assistant" && message.providerName && <small className="chats-provider-disclosure">
                  Answered by {message.providerName} ({message.providerHosting?.toLowerCase()}){message.fallbackOccurred ? " after fallback" : ""}.
                </small>}
              </article>)}
          </div>
          {assistantNotice && <p className="chats-notice" role="status">{assistantNotice}</p>}
          <form className="chats-compose" onSubmit={(event) => void postMessage(event)}>
            <label htmlFor="chat-message">Message</label><textarea id="chat-message" value={text} maxLength={4000}
              onChange={(event) => setText(event.target.value)} placeholder="Write a message… Use @dm to ask about household plans, tasks, or responsibilities." />
            <div><span>{text.length}/4000</span><button disabled={busy || !text.trim()}>Send</button></div>
          </form>
        </>}
      </section>
    </div>
  </main>;
}
