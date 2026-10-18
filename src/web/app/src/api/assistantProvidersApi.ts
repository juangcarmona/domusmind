import { request } from "./request";

export interface AssistantProviderTarget {
  id: string;
  name: string;
  hosting: "Hosted" | "Local";
  protocol: "Ollama" | "OpenAICompatible";
  endpoint: string;
  model: string;
  hasCredential: boolean;
}

export interface AssistantProviderRoute {
  familyId: string;
  canManage: boolean;
  isDeploymentDefault: boolean;
  targets: AssistantProviderTarget[];
}

export interface AssistantProviderTargetInput extends Omit<AssistantProviderTarget, "hasCredential"> {
  apiKey: string;
}

export const assistantProvidersApi = {
  get: (familyId: string) => request<AssistantProviderRoute>(
    `/api/chats/assistant/providers?familyId=${encodeURIComponent(familyId)}`),
  replace: (familyId: string, targets: AssistantProviderTargetInput[]) =>
    request<AssistantProviderRoute>(`/api/chats/assistant/providers?familyId=${encodeURIComponent(familyId)}`, {
      method: "PUT", body: JSON.stringify({ targets }),
    }),
};
