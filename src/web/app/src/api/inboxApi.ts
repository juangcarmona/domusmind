import { request } from "./request";

export interface InboxProposal {
  proposalId: string; type: "Plan" | "PlanUpdate" | "ListItemAdd" | "ListItemUpdate" | "Task" | "Reminder" | "TimelineNote"; title: string; description: string | null;
  date: string | null; time: string | null; endDate: string | null; endTime: string | null;
  dueDate: string | null; status: string; unresolvedDetails: string | null; resultId: string | null;
  targetPlanTitle: string | null; targetPlanId: string | null; reminderMinutesBefore: number | null;
  targetListId: string | null; targetItemId: string | null; quantity: string | null;
}
export interface InboxItem {
  inboxItemId: string; familyId: string; createdByUserId: string; rawText: string;
  createdAtUtc: string; proposals: InboxProposal[];
  sourceChatMessageId: string | null; providerName: string | null; providerHosting: "Hosted" | "Local" | null;
  providerFallbackOccurred: boolean;
}
export interface RefineProposal {
  title: string; description: string | null; date: string | null; time: string | null;
  endDate: string | null; endTime: string | null; dueDate: string | null;
  targetPlanId: string | null; reminderMinutesBefore: number | null;
  targetListId: string | null; targetItemId: string | null; quantity: string | null;
}
export interface InboxInterpreterInfo {
  familyId: string; canManage: boolean; isDeploymentDefault: boolean;
  targets: { id: string; name: string; hosting: "Hosted" | "Local"; protocol: string; model: string; hasCredential: boolean }[];
}

export const inboxApi = {
  interpreter: (familyId: string) => request<InboxInterpreterInfo>(`/api/inbox/interpreter?familyId=${encodeURIComponent(familyId)}`),
  list: (familyId: string) => request<InboxItem[]>(`/api/inbox?familyId=${encodeURIComponent(familyId)}`),
  capture: (familyId: string, text: string) => request<InboxItem>("/api/inbox", {
    method: "POST", body: JSON.stringify({ familyId, text }),
  }),
  interpret: (id: string) => request<InboxItem>(`/api/inbox/${id}/interpret`, { method: "POST" }),
  refine: (itemId: string, proposalId: string, body: RefineProposal) =>
    request<InboxItem>(`/api/inbox/${itemId}/proposals/${proposalId}/refine`, { method: "POST", body: JSON.stringify(body) }),
  approve: (itemId: string, proposalId: string) =>
    request<InboxItem>(`/api/inbox/${itemId}/proposals/${proposalId}/approve`, { method: "POST" }),
  reject: (itemId: string, proposalId: string) =>
    request<InboxItem>(`/api/inbox/${itemId}/proposals/${proposalId}/reject`, { method: "POST" }),
};
