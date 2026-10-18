import { request } from "./request";

export interface InboxProposal {
  proposalId: string; type: "Plan" | "Task" | "Reminder" | "TimelineNote"; title: string; description: string | null;
  date: string | null; time: string | null; endDate: string | null; endTime: string | null;
  dueDate: string | null; status: string; unresolvedDetails: string | null; resultId: string | null;
  targetPlanTitle: string | null; targetPlanId: string | null; reminderMinutesBefore: number | null;
}
export interface InboxItem {
  inboxItemId: string; familyId: string; createdByUserId: string; rawText: string;
  createdAtUtc: string; proposals: InboxProposal[];
}
export interface RefineProposal {
  title: string; description: string | null; date: string | null; time: string | null;
  endDate: string | null; endTime: string | null; dueDate: string | null;
  targetPlanId: string | null; reminderMinutesBefore: number | null;
}
export interface InboxInterpreterInfo { provider: string; sendsTextOutsideDeployment: boolean; }

export const inboxApi = {
  interpreter: () => request<InboxInterpreterInfo>("/api/inbox/interpreter"),
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
