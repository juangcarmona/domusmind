import { request } from "./request";

export interface ChatThread {
  chatThreadId: string;
  familyId: string;
  title: string;
  kind: "Household" | "Private";
  createdByMemberId: string;
  participantMemberIds: string[];
  createdAtUtc: string;
}

export interface ChatMessage {
  chatMessageId: string;
  chatThreadId: string;
  authorMemberId: string | null;
  authorType: "Member" | "Assistant";
  authorName: string;
  text: string;
  replyToMessageId: string | null;
  createdAtUtc: string;
  providerName: string | null;
  providerHosting: "Hosted" | "Local" | null;
  fallbackOccurred: boolean;
}

export interface PostChatMessageResponse {
  message: ChatMessage;
  assistantResponse: ChatMessage | null;
  assistantUnavailable: boolean;
}

export const chatsApi = {
  assistant: (familyId: string) => request<import("./assistantProvidersApi").AssistantProviderRoute>(
    `/api/chats/assistant/providers?familyId=${encodeURIComponent(familyId)}`),
  list: (familyId: string) => request<ChatThread[]>(`/api/chats?familyId=${encodeURIComponent(familyId)}`),
  messages: (threadId: string) => request<ChatMessage[]>(`/api/chats/${threadId}/messages`),
  create: (familyId: string, title: string, kind: ChatThread["kind"], participantMemberIds: string[]) =>
    request<ChatThread>("/api/chats", { method: "POST", body: JSON.stringify({ familyId, title, kind, participantMemberIds }) }),
  post: (threadId: string, text: string) => request<PostChatMessageResponse>(`/api/chats/${threadId}/messages`, {
    method: "POST", body: JSON.stringify({ text }),
  }),
};
