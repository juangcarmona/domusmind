import { request } from "./request";

export interface CalendarSubscriptionSummary {
  id: string;
  label: string;
  memberId: string | null;
  createdAtUtc: string;
  expiresAtUtc: string;
  revokedAtUtc: string | null;
  lastAccessedAtUtc: string | null;
  accessCount: number;
}

export interface CreatedCalendarSubscription {
  id: string;
  label: string;
  memberId: string | null;
  expiresAtUtc: string;
  url: string;
}

export const calendarSubscriptionApi = {
  list: (familyId: string) => request<CalendarSubscriptionSummary[]>(
    `/api/calendar/subscriptions?familyId=${encodeURIComponent(familyId)}`,
  ),
  create: (familyId: string, label: string, memberId: string | null) =>
    request<CreatedCalendarSubscription>("/api/calendar/subscriptions", {
      method: "POST",
      body: JSON.stringify({ familyId, label, memberId }),
    }),
  revoke: (id: string) => request<void>(`/api/calendar/subscriptions/${id}`, { method: "DELETE" }),
};
