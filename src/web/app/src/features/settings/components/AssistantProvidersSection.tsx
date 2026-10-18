import { useEffect, useState } from "react";
import { assistantProvidersApi, type AssistantProviderRoute, type AssistantProviderTargetInput } from "../../../api/assistantProvidersApi";
import { useAppSelector } from "../../../store/hooks";

type EditableTarget = AssistantProviderTargetInput & { hasCredential: boolean };

const emptyTarget = (position: number): EditableTarget => ({
  id: crypto.randomUUID(), name: ["Primary", "Secondary", "Tertiary"][position],
  hosting: "Local", protocol: "Ollama", endpoint: "",
  model: "llama3.2:3b", apiKey: "", hasCredential: false,
});

export function AssistantProvidersSection() {
  const family = useAppSelector((s) => s.household.family);
  const [route, setRoute] = useState<AssistantProviderRoute>();
  const [targets, setTargets] = useState<EditableTarget[]>([]);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    if (!family?.familyId) return;
    let cancelled = false;
    void assistantProvidersApi.get(family.familyId).then((value) => {
      if (cancelled) return;
      setRoute(value);
      setTargets(value.targets.map((target) => ({ ...target, apiKey: "" })));
    }).catch(() => { if (!cancelled) setError("Assistant provider settings could not be loaded."); });
    return () => { cancelled = true; };
  }, [family?.familyId]);

  if (!family) return null;

  function edit(index: number, changes: Partial<EditableTarget>) {
    setTargets((current) => current.map((target, i) => i === index
      ? { ...target, ...changes }
      : target));
  }

  function move(index: number, offset: number) {
    const next = index + offset;
    if (next < 0 || next >= targets.length) return;
    setTargets((current) => {
      const result = [...current];
      [result[index], result[next]] = [result[next], result[index]];
      return result;
    });
  }

  async function save(event: React.FormEvent) {
    event.preventDefault();
    if (!family) return;
    setBusy(true); setError(""); setSuccess("");
    try {
      const saved = await assistantProvidersApi.replace(family.familyId,
        targets.map(({ hasCredential: _hasCredential, ...target }) => target));
      setRoute(saved);
      setTargets(saved.targets.map((target) => ({ ...target, apiKey: "" })));
      setSuccess("Assistant provider route saved.");
    } catch (reason) {
      setError(typeof reason === "object" && reason && "message" in reason
        ? String(reason.message) : "Assistant provider route could not be saved.");
    } finally { setBusy(false); }
  }

  return <section className="assistant-provider-settings">
    <h2>Assistant providers</h2>
    <p>Targets are tried in order. Hosted targets receive the request text and household information available to the member. Local targets receive that information at the endpoint you configure. The endpoint must be reachable from the DomusMind API; in Docker Compose, Ollama is available at <code>http://ollama:11434</code>.</p>
    {route?.isDeploymentDefault && <p role="note">This route uses the deployment's current provider until you save a household route.</p>}
    {error && <p role="alert" className="assistant-provider-error">{error}</p>}
    {success && <p role="status">{success}</p>}
    {route && !route.canManage ? <ol>{route.targets.map((target) => <li key={target.id}>
      <strong>{target.name}</strong> · {target.hosting} · {target.protocol} · {target.model}
      <div>{target.hosting === "Hosted"
        ? "Assistant request text and authorized household context are sent to this hosted target."
        : "Assistant request text and authorized household context are sent to this local endpoint."}</div>
    </li>)}</ol> : !route ? <p>Loading assistant provider settings…</p> : <form onSubmit={(event) => void save(event)}>
      <ol className="assistant-provider-targets">
        {targets.map((target, index) => <li key={target.id}>
          <fieldset>
            <legend>{index === 0 ? "Primary" : index === 1 ? "Secondary" : "Tertiary"} target</legend>
            <label>Name<input value={target.name} maxLength={60} onChange={(event) => edit(index, { name: event.target.value })} required /></label>
            <label>Hosting<select value={target.hosting} onChange={(event) => edit(index, { hosting: event.target.value as EditableTarget["hosting"] })}>
              <option value="Local">Local</option><option value="Hosted">Hosted</option>
            </select></label>
            <label>Protocol<select value={target.protocol} onChange={(event) => edit(index, { protocol: event.target.value as EditableTarget["protocol"] })}>
              <option value="Ollama">Ollama</option><option value="OpenAICompatible">OpenAI compatible</option>
            </select></label>
            <label>Endpoint<input type="url" value={target.endpoint} maxLength={1000} onChange={(event) => edit(index, { endpoint: event.target.value })} required /></label>
            <label>Model<input value={target.model} maxLength={120} onChange={(event) => edit(index, { model: event.target.value })} required /></label>
            <label>API key{target.hasCredential && <small> A credential is saved. Enter a new value to replace it.</small>}
              <input type="password" autoComplete="new-password" value={target.apiKey} maxLength={4000}
                onChange={(event) => edit(index, { apiKey: event.target.value })} /></label>
            <div className="assistant-provider-order">
              <button type="button" disabled={index === 0} onClick={() => move(index, -1)}>Move up</button>
              <button type="button" disabled={index === targets.length - 1} onClick={() => move(index, 1)}>Move down</button>
              <button type="button" disabled={targets.length <= 1} onClick={() => setTargets((current) => current.filter((_, i) => i !== index))}>Remove</button>
            </div>
          </fieldset>
        </li>)}
      </ol>
      <div className="assistant-provider-actions">
        <button type="button" disabled={targets.length >= 3} onClick={() => setTargets((current) => [...current, emptyTarget(current.length)])}>Add target</button>
        <button type="submit" disabled={busy || targets.length === 0}>{busy ? "Saving…" : "Save provider route"}</button>
      </div>
    </form>}
  </section>;
}
