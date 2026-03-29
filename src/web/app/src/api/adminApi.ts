import { request } from "./request";

// ── Diagnostics ──────────────────────────────────────────────────────────────

export interface AdminSummaryResponse {
  deploymentMode: string;
  householdCount: number;
  userCount: number;
  pendingInvitationCount: number;
  isSystemInitialized: boolean;
}

// ── Households ────────────────────────────────────────────────────────────────

export interface AdminHouseholdSummary {
  familyId: string;
  name: string;
  createdAtUtc: string;
  memberCount: number;
}

export interface AdminHouseholdListResponse {
  items: AdminHouseholdSummary[];
}

// ── Users ─────────────────────────────────────────────────────────────────────

export interface AdminUserSummary {
  userId: string;
  email: string;
  displayName: string | null;
  isDisabled: boolean;
  isOperator: boolean;
  createdAtUtc: string;
  lastLoginAtUtc: string | null;
  linkedFamilyId: string | null;
}

export interface AdminUserListResponse {
  items: AdminUserSummary[];
}

// ── Invitations ───────────────────────────────────────────────────────────────

export interface OperatorInvitationItem {
  id: string;
  email: string;
  note: string | null;
  status: "Pending" | "Accepted" | "Revoked" | "Expired";
  createdAtUtc: string;
  expiresAtUtc: string;
  createdByUserId: string;
}

export interface OperatorInvitationListResponse {
  items: OperatorInvitationItem[];
}

export interface CreateOperatorInvitationRequest {
  email: string;
  note?: string | null;
}

export interface CreateOperatorInvitationResponse {
  id: string;
  email: string;
  token: string;
  expiresAtUtc: string;
}

// ── API client ────────────────────────────────────────────────────────────────

export const adminApi = {
  getSummary: (): Promise<AdminSummaryResponse> =>
    request<AdminSummaryResponse>("/api/admin/summary"),

  getHouseholds: (search?: string): Promise<AdminHouseholdListResponse> =>
    request<AdminHouseholdListResponse>(
      `/api/admin/households${search ? `?search=${encodeURIComponent(search)}` : ""}`
    ),

  getUsers: (search?: string): Promise<AdminUserListResponse> =>
    request<AdminUserListResponse>(
      `/api/admin/users${search ? `?search=${encodeURIComponent(search)}` : ""}`
    ),

  disableUser: (userId: string): Promise<{ userId: string; isDisabled: boolean }> =>
    request(`/api/admin/users/${userId}/disable`, { method: "POST" }),

  enableUser: (userId: string): Promise<{ userId: string; isDisabled: boolean }> =>
    request(`/api/admin/users/${userId}/enable`, { method: "POST" }),

  getInvitations: (): Promise<OperatorInvitationListResponse> =>
    request<OperatorInvitationListResponse>("/api/admin/invitations"),

  createInvitation: (body: CreateOperatorInvitationRequest): Promise<CreateOperatorInvitationResponse> =>
    request<CreateOperatorInvitationResponse>("/api/admin/invitations", {
      method: "POST",
      body: JSON.stringify(body),
    }),

  revokeInvitation: (invitationId: string): Promise<void> =>
    request<void>(`/api/admin/invitations/${invitationId}/revoke`, { method: "POST" }),
};
